using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace PianoGradeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComposersController : ControllerBase
    {

        private readonly ILogger<ComposersController> _logger;
        private GradesContext _gradesContext;

        public ComposersController(ILogger<ComposersController> logger, GradesContext gradesContext)
        {
            _logger = logger;
			_gradesContext = gradesContext;
		}

        [HttpGet]
        public List<Composer> GetComposerInfoByName([FromQuery] string name)
        {
            List<Composer> composers = _gradesContext.Composers.Where(c=>c.Name == name).ToList();
            _logger.LogInformation("Retrieving composer info...");
            return composers;
        }
    }
}
