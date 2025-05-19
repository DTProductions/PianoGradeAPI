using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace PianoGradeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComposersController : ControllerBase
    {

        private readonly ILogger<ComposersController> logger;
        private GradesContext gradesContext;

        public ComposersController(ILogger<ComposersController> logger, GradesContext gradesContext)
        {
            this.logger = logger;
			this.gradesContext = gradesContext;
		}

        [HttpGet]
        public List<Composer> GetComposerInfoByName([FromQuery] string name)
        {
            List<Composer> composers = gradesContext.Composers.Where(c=>c.Name == name).ToList();
            logger.LogInformation("Retrieving composer info...");
            return composers;
        }
    }
}
