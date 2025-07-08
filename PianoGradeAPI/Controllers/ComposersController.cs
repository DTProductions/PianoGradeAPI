using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Dtos;
using System.Linq;
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
        public List<GetComposerDto> GetComposer([FromQuery] string? name, [FromQuery] int? sinceId, [FromQuery] int? limit) {
			IQueryable<Composer> query = gradesContext.Composers;
			if (sinceId != null) {
				query = query.OrderBy(c => c.Id).Where(c => c.Id > sinceId);
			}

			if (limit != null) {
				query = query.Take((int)limit);
			}

			if (name != null) {
                query = query.Where(c => c.Name.ToUpper().Contains(name.ToUpper()));
			}

			List<GetComposerDto> composers = query.Select(c=> new GetComposerDto() {
                Id = c.Id,
                Name = c.Name,
                Era = c.Era,
                Nationality = c.Nationality
            }).ToList();

            logger.LogInformation("Retrieving composer info...");
            return composers;
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<ActionResult<GetComposerDto>> GetComposerById(int id) {
			GetComposerDto? composer = await gradesContext.Composers.Where(c => c.Id == id).Select(c => new GetComposerDto() {
                Id = c.Id,
                Name = c.Name,
                Era = c.Era,
                Nationality = c.Nationality
            }).FirstOrDefaultAsync();

            if(composer == null) {
                return NotFound();
            }

            return Ok(composer);
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

        [HttpPut]
        public async Task<ActionResult> UpdateComposer([FromBody] UpdateComposerDto updateComposerDto) {
            Composer? composerToUpdate = await gradesContext.Composers.FirstOrDefaultAsync(c => c.Id == updateComposerDto.Id);
            if(composerToUpdate == null) {
                return UnprocessableEntity();
            }

            composerToUpdate.Name = updateComposerDto.Name;
            composerToUpdate.Era = updateComposerDto.Era;
            composerToUpdate.Nationality = updateComposerDto.Nationality;

            gradesContext.Composers.Update(composerToUpdate);
            await gradesContext.SaveChangesAsync();

            return NoContent();
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
