using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorld.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace KubeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingController : ControllerBase
    {
        private readonly ILogger<GreetingController> _logger;
        private readonly IClusterClient _clusterClient;

        public GreetingController(
            IClusterClient clusterClient,
            ILogger<GreetingController> logger)
        {
            _clusterClient = clusterClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery]string greeting)
        {
            _logger.LogInformation("<= SayHello: " + greeting);
            var friend = _clusterClient.GetGrain<IHello>(0);
            var response = await friend.SayHello(greeting);
            _logger.LogInformation("=> SayHello: " + response);
            return response;
        }
    }
}