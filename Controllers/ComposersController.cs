using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpDelete]
        public async Task<ActionResult> DeleteComposer([FromQuery] int id) {
            Composer? composerToDelete = await gradesContext.Composers.Include(c => c.Pieces).FirstOrDefaultAsync(c => c.Id == id);
            if(composerToDelete == null) {
                return UnprocessableEntity();
            }

            gradesContext.Composers.Remove(composerToDelete);
            await gradesContext.SaveChangesAsync();
            return NoContent();
        }
	}
}
