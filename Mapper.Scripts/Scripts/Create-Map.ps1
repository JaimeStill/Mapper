[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [string]$fips = "01",
    [Parameter(Mandatory = $true)]
    [string]$path = '',
    [Parameter(Mandatory = $true)]
    [string]$outPath = '',
    [Parameter()]
    [int]$width = 960,
    [Parameter()]
    [int]$height = 960
)

$shape = "cb_2014_$($fips)_tract_500k"
$census = "cb_2014_$($fips)_tract_DP02_0001E"

if (Test-Path -Path "$($path)maps/$($shape)" -PathType Container) {
    if (Test-Path -Path "$($path)census/$($census).json" -PathType Leaf) {
        if (!(Test-Path "$($path)temp" -PathType Container)) {
            New-Item -Path $path -Name "temp" -ItemType "directory" -Force | Out-Null
        }

        if (!(Test-Path -Path "$outPath" -PathType Container)) {
            New-Item -Path $outPath -ItemType "directory" -Force | Out-Null
        }

        $state = (Get-Content "$($path)data.json" | ConvertFrom-Json).states | Where-Object { $_.fips -eq $fips }

        if (!($state)) {
            Write-Error "$($fips) does not have associated state data"
            Exit
        }

        shp2json "$($path)maps/$($shape)/$($shape).shp" | `
            geoproject "d3.$($state.projection).fitSize([$width, $height], d)" | `
            ndjson-split "d.features" | `
            ndjson-map "d.id = d.properties.GEOID.slice(2), d" > `
            "$($path)temp/$($state.name).ndjson"

        ndjson-cat "$($path)census/$($census).json" | `
            cmd /c "ndjson-split `"d.slice(1)`"" | `
            ndjson-map "{id: d[2] + d[3], DP02_0001E: +d[0]}" > `
            "$($path)temp/$($state.name)-census.ndjson"

        ndjson-join "d.id" "$($path)temp/$($state.name).ndjson" "$($path)temp/$($state.name)-census.ndjson" | `
            ndjson-map "d[0].properties = {density: Math.floor(d[1].DP02_0001E / d[0].properties.ALAND * 2589975.2356)}, d[0]" | `
            geo2topo -n tracts=- | `
            toposimplify -p 1 -f | `
            topoquantize 1e5 | `
            topomerge -k "d.id.slice(0, 3)" counties=tracts | `
            topomerge --mesh -f "a !== b" counties=counties | `
            topo2geo tracts=- > `
            "$($outPath)$($state.name).json"

        Remove-Item "$($path)maps/$($shape)" -Force -Recurse
        Remove-Item "$($path)temp" -Force -Recurse
    } else {
        Write-Error "$($fips) does not have associated census data"
        Exit
    }
} else {
    Write-Error "$($fips) does not have associate map data"
    Exit
}