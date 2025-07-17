using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Contexts;
using PianoGradeAPI.Dtos;
using PianoGradeAPI.Entities;

namespace PianoGradeAPI.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class ArrangersController : ControllerBase {
		private GradesContext gradesContext;

		public ArrangersController(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}


		[HttpGet]
		public List<GetArrangerDto> GetArrangers(string? name, [FromQuery] int? sinceId, [FromQuery] int? limit) {
			IQueryable<ArrangerEntity> query = gradesContext.Arrangers;
			if (sinceId != null) {
				query = query.OrderBy(a => a.Id).Where(a => a.Id > sinceId);
			}

			if (limit != null) {
				query = query.Take((int)limit);
			}

			if (name != null) {
				query = query.Where(a => a.Name.ToUpper().Contains(name.ToUpper()));
			}

			List<GetArrangerDto> arrangers = query.Select(a => new GetArrangerDto() {
				Id = a.Id,
				Name = a.Name
			}).ToList();

			return arrangers;
		}

		[HttpGet]
		[Route("{id?}")]
		public async Task<ActionResult<GetArrangerDto>> GetArrangerById(int id) {
			GetArrangerDto? arranger = await gradesContext.Arrangers.Where(a => a.Id == id).Select(a => new GetArrangerDto() {
				Id = a.Id,
				Name = a.Name
			}).FirstOrDefaultAsync();

			if(arranger == null) {
				return NotFound();
			}

			return Ok(arranger);
		}

		[HttpPost]
		public async Task<ActionResult> InsertArranger([FromBody] InsertArrangerDto insertArrangerDto) {
			ArrangerEntity arrangerToAdd = new ArrangerEntity() {
				Name = insertArrangerDto.Name
			};

			gradesContext.Arrangers.Add(arrangerToAdd);
			await gradesContext.SaveChangesAsync();
			return Created();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateArranger([FromBody] UpdateArrangerDto updateArrangerDto) {
			ArrangerEntity? arrangerToUpdate = await gradesContext.Arrangers.FirstOrDefaultAsync(a => a.Id == updateArrangerDto.Id);
			if (arrangerToUpdate == null) {
				return UnprocessableEntity();
			}

			arrangerToUpdate.Name = updateArrangerDto.Name;

			gradesContext.Arrangers.Update(arrangerToUpdate);
			await gradesContext.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteArranger([FromQuery] int id) {
			ArrangerEntity? arrangerToDelete = await gradesContext.Arrangers.Include(a => a.Pieces).FirstOrDefaultAsync(a => a.Id == id);
			if (arrangerToDelete == null) {
				return UnprocessableEntity();
			}

			gradesContext.Arrangers.Remove(arrangerToDelete);
			await gradesContext.SaveChangesAsync();
			return NoContent();
		}
	}
}
