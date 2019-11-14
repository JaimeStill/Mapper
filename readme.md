# Choropleth Generator

* [Overview](#overview)
* [Build](#build)
* [Infrastructure](#infrastructure)
    * [Mapper.Scripts](#mapperscripts)
    * [Mapper.Web](#mapperweb)

[![choropleth-generator](https://imgur.com/ylb4RME.gif)](https://imgur.com/ylb4RME)

## Overview
[Back to Top](#choropleth-generator)

> This work is based on [Mike Bostock](https://github.com/mbostock/)'s [Command-Line Cartography](https://medium.com/@mbostock/command-line-cartography-part-1-897aa8f8ca2c) articles on Medium.

This repository represents my progress towards automating map-based visualizations for a .NET Core / Angular app stack. It builds on the initial efforts I've made in two other repositories.

[mapping-research](https://github.com/JaimeStill/mapping-research) explored using a Windows command line to generate an [equivalent map of Texas](https://github.com/JaimeStill/mapping-research/blob/master/texas/texas-choropleth.md).

[ps-mapping](https://github.com/JaimeStill/ps-mapping) explored automating the process for any state using any compatible [scheme](https://github.com/d3/d3-scale-chromatic).

## Build
[Back to Top](#choropleth-generator)

In order to build this repository, you will need the following installed:

* [.NET Core SDK](https://dotnet.microsoft.com/download)
* [Node.js - Current](https://nodejs.org/en/)
* [Yarn](https://yarnpkg.com/lang/en/)
* [PowerShell Core](https://github.com/PowerShell/PowerShell#get-powershell)

Additionally, you will need to globally install yarn packages on your machine using the following command:

```
yarn global add shapefile d3 d3-geo-projection d3-scale-chromatic ndjson-cli topojson-client topojson-server topojson-simplify
```

## Infrastructure
[Back to Top](#choropleth-generator)

The core files necessary for generating maps are located in [Mapper.Web/wwwroot/mapping](https://github.com/JaimeStill/Mapper/tree/master/Mapper.Web/wwwroot/mapping). These files are used by a PowerShell script, executed via the [Microsoft.PowerShell.SDK](https://github.com/PowerShell/PowerShell/tree/master/src/Microsoft.PowerShell.SDK) to generate the GeoJSON necessary to render the map.

### [Mapper.Scripts](https://github.com/JaimeStill/Mapper/tree/master/Mapper.Scripts)
[Back to Top](#choropleth-generator)

Contains classes, extension methods, and a PowerShell script for generating maps.

**Key Files**  

* [Create-Map.ps1](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Scripts/Scripts/Create-Map.ps1)
* [ScriptExtensions.cs](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Scripts/Extensions/ScriptExtensions.cs)
* [MapExtensions.cs](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Scripts/Extensions/MapExtensions.cs)

### [Mapper.Web](https://github.com/JaimeStill/Mapper/tree/master/Mapper.Web)
[Back to Top](#choropleth-generator)

<span>ASP.NET</span> Core / Angular SPA

**Key Files**  

* [MapController.cs](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Web/Controllers/MapController.cs)
* [map.service.ts](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Web/ClientApp/src/app/services/map.service.ts)
* [home.component.ts](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Web/ClientApp/src/app/routes/home/home.component.ts)
* [home.component.html](https://github.com/JaimeStill/Mapper/blob/master/Mapper.Web/ClientApp/src/app/routes/home/home.component.html)
