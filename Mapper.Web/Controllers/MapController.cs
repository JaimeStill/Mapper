using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mapper.Scripts;
using Microsoft.AspNetCore.Hosting;
using static Mapper.Scripts.Extensions.MapExtensions;

namespace Mapper.Web.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private IWebHostEnvironment env;

        public MapController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet("[action]")]
        public List<StateData> GetStateData() => GetStates();

        [HttpGet("[action]")]
        public async Task<ConsoleOutput> TestMap()
        {
            var options = new MapOptions
            {
                Data = new StateData
                {
                    Fips = "02",
                    Name = "alaska",
                    Display = "Alaska"
                },
                Width = 960,
                Height = 960
            };

            return await options.CreateMap(env);
        }

        [HttpPost("[action]")]
        public async Task<ConsoleOutput> CreateMap([FromBody]MapOptions options) => await options.CreateMap(env);
    }
}