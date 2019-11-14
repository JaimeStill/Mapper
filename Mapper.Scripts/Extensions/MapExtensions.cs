using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Mapper.Scripts.Extensions
{
    public static class MapExtensions
    {
        public static List<StateData> GetStates() => new List<StateData>
        {
            new StateData { Fips = "01", Name = "alabama", Display = "Alabama" },
            new StateData { Fips = "02", Name = "alaska", Display = "Alaska" },
            new StateData { Fips = "04", Name = "arizona", Display = "Arizona" },
            new StateData { Fips = "05", Name = "arkansas", Display = "Arkansas" },
            new StateData { Fips = "06", Name = "california", Display = "California" },
            new StateData { Fips = "08", Name = "colorado", Display = "Colorado" },
            new StateData { Fips = "09", Name = "connecticut", Display = "Connecticut" },
            new StateData { Fips = "10", Name = "delaware", Display = "Delware" },
            new StateData { Fips = "11", Name = "washington-dc", Display = "Washington D.C." },
            new StateData { Fips = "12", Name = "florida", Display = "Florida" },
            new StateData { Fips = "13", Name = "georgia", Display = "Georgia" },
            new StateData { Fips = "15", Name = "hawaii", Display = "Hawaii" },
            new StateData { Fips = "16", Name = "idaho", Display = "Idaho" },
            new StateData { Fips = "17", Name = "illinois", Display = "Illinois" },
            new StateData { Fips = "18", Name = "indiana", Display = "Indiana" },
            new StateData { Fips = "19", Name = "iowa", Display = "Iowa" },
            new StateData { Fips = "20", Name = "kansas", Display = "Kansas" },
            new StateData { Fips = "21", Name = "kentucky", Display = "Kentucky" },
            new StateData { Fips = "22", Name = "louisiana", Display = "Louisiana" },
            new StateData { Fips = "23", Name = "maine", Display = "Maine" },
            new StateData { Fips = "24", Name = "maryland", Display = "Maryland" },
            new StateData { Fips = "25", Name = "massachusetts", Display = "Massachusetts" },
            new StateData { Fips = "26", Name = "michigan", Display = "Michigan" },
            new StateData { Fips = "27", Name = "minnesota", Display = "Minnesota" },
            new StateData { Fips = "28", Name = "mississippi", Display = "Mississippi" },
            new StateData { Fips = "29", Name = "missouri", Display = "Missouri" },
            new StateData { Fips = "30", Name = "montana", Display = "Montana" },
            new StateData { Fips = "31", Name = "nebraska", Display = "Nebraska" },
            new StateData { Fips = "32", Name = "nevada", Display = "Nevada" },
            new StateData { Fips = "33", Name = "new-hampshire", Display = "New Hampshire" },
            new StateData { Fips = "34", Name = "new-jersey", Display = "New Jersey" },
            new StateData { Fips = "35", Name = "new-mexico", Display = "New Mexico" },
            new StateData { Fips = "36", Name = "new-york", Display = "New York" },
            new StateData { Fips = "37", Name = "north-carolina", Display = "North Carolina" },
            new StateData { Fips = "38", Name = "north-dakota", Display = "North Dakota" },
            new StateData { Fips = "39", Name = "ohio", Display = "Ohio" },
            new StateData { Fips = "40", Name = "oklahoma", Display = "Oklahoma" },
            new StateData { Fips = "41", Name = "oregon", Display = "Oregon" },
            new StateData { Fips = "42", Name = "pennsylvania", Display = "Pennsylvania" },
            new StateData { Fips = "44", Name = "rhode-island", Display = "Rhode Island" },
            new StateData { Fips = "45", Name = "south-carolina", Display = "South Carolina" },
            new StateData { Fips = "46", Name = "south-dakota", Display = "South Dakota" },
            new StateData { Fips = "47", Name = "tennessee", Display = "Tennessee" },
            new StateData { Fips = "48", Name = "texas", Display = "Texas" },
            new StateData { Fips = "49", Name = "utah", Display = "Utah" },
            new StateData { Fips = "50", Name = "vermont", Display = "Vermont" },
            new StateData { Fips = "51", Name = "virginia", Display = "Virginia" },
            new StateData { Fips = "53", Name = "washington", Display = "Washington" },
            new StateData { Fips = "54", Name = "west-virginia", Display = "West Virginia" },
            new StateData { Fips = "55", Name = "wisconsin", Display = "Wisconsin" },
            new StateData { Fips = "56", Name = "wyoming", Display = "Wyoming" },
            new StateData { Fips = "72", Name = "puerto-rico", Display = "Puerto Rico" }
        };

        public static async Task<ConsoleOutput> CreateMap(this MapOptions options, IWebHostEnvironment env)
        {
            var guid = Guid.NewGuid();
            options.Path = $"{env.ContentRootPath}/wwwroot/mapping/";
            options.OutPath = $"{env.ContentRootPath}/wwwroot/mapping/data/{guid.ToString()}/";
            options.UnzipMap();

            var result = await options.CreateMap();

            if (!result.HasError)
            {
                result.Result = await File.ReadAllTextAsync($"{options.OutPath}{options.Data.Name}.json");
            }

            if (Directory.Exists(options.OutPath))
            {
                Directory.Delete(options.OutPath, true);
            }

            return result;
        }

        public static async Task<Command> GenerateMapScript(this MapOptions model)
        {
            var script = await (".Scripts.Create-Map.ps1").GetTextFromEmbeddedResource();

            Command createMap = new Command(script, true);
            createMap.Parameters.Add("fips", model.Data.Fips);
            createMap.Parameters.Add("path", model.Path);
            createMap.Parameters.Add("outPath", model.OutPath);
            createMap.Parameters.Add("width", model.Width);
            createMap.Parameters.Add("height", model.Height);

            return createMap;
        }

        static async Task<ConsoleOutput> CreateMap(this MapOptions model)
        {
            var script = await model.GenerateMapScript();
            var output = await script.ExecuteCommand();

            if (!output.HasError)
            {
                output.Result = $"Data for {model.Data.Display} successfully created";
            }

            return output;
        }

        static void DeleteOutPath(this MapOptions options)
        {
            if (Directory.Exists(options.OutPath))
            {
                Directory.Delete(options.OutPath, true);
            }
        }

        static void UnzipMap(this MapOptions options)
        {
            var shape = $"cb_2014_{options.Data.Fips}_tract_500k";
            ZipFile.ExtractToDirectory($"{options.Path}maps/{shape}.zip", $"{options.Path}maps/{shape}");
        }
    }
}