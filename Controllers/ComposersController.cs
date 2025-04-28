using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace PianoGradeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComposersController : ControllerBase
    {

        private readonly ILogger<ComposersController> _logger;

        public ComposersController(ILogger<ComposersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string GetComposerInfo([FromQuery] string name)
        {
            _logger.LogInformation("Retrieving composer info...");
            return name;
        }
    }
}
