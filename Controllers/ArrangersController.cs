using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
	}
}
