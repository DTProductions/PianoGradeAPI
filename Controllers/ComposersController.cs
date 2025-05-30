using Microsoft.AspNetCore.Mvc;
using PianoGradeAPI.Dtos;
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

		[HttpPost]
		public async Task<ActionResult> InsertComposer([FromBody] InsertComposerDto insertComposerDto) {
			Composer composerToAdd = new Composer() {
				Name = insertComposerDto.Name,
				Era = insertComposerDto.Era,
				Nationality = insertComposerDto.Nationality
			};

			gradesContext.Composers.Add(composerToAdd);
			await gradesContext.SaveChangesAsync();
			return Created();
		}
	}
}
