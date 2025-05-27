using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PianoGradeAPI.Dtos;

namespace PianoGradeAPI.Controllers {
	[Route("[controller]")]
	[ApiController]
	public class ArrangersController : ControllerBase {
		private GradesContext gradesContext;

		public ArrangersController(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}


		[HttpGet]
		public List<string> GetArrangers(string? query) {
			IQueryable<Arranger> arrangers = gradesContext.Arrangers;
			if(query != null) {
				arrangers = arrangers.Where(a => a.Name.Contains(query));
			}

			return arrangers.Select(a => a.Name).ToList();
		}

		[HttpPost]
		public async Task<ActionResult> InsertArranger([FromBody] InsertArrangerDto insertArrangerDto) {
			Arranger arrangerToAdd = new Arranger() {
				Name = insertArrangerDto.Name
			};

			gradesContext.Arrangers.Add(arrangerToAdd);
			await gradesContext.SaveChangesAsync();
			return Created();
		}
	}
}
