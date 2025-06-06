﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		[HttpPut]
		public async Task<ActionResult> UpdateArranger([FromBody] UpdateArrangerDto updateArrangerDto) {
			Arranger? arrangerToUpdate = await gradesContext.Arrangers.FirstOrDefaultAsync(a => a.Id == updateArrangerDto.Id);
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
			Arranger? arrangerToDelete = await gradesContext.Arrangers.Include(a => a.Pieces).FirstOrDefaultAsync(a => a.Id == id);
			if (arrangerToDelete == null) {
				return UnprocessableEntity();
			}

			gradesContext.Arrangers.Remove(arrangerToDelete);
			await gradesContext.SaveChangesAsync();
			return NoContent();
		}
	}
}
