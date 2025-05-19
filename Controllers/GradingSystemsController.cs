using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PianoGradeAPI.Controllers {
	[Route("[controller]")]
	[ApiController]
	public class GradingSystemsController : ControllerBase {
		private GradesContext gradesContext;

		public GradingSystemsController(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}

		public List<string> GetGradingSystems() {
			return gradesContext.Grades.Select(g => g.GradingSystem).Distinct().ToList();
		}
	}
}
