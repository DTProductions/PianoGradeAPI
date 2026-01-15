using Microsoft.AspNetCore.Mvc.Testing;
using PianoGradeAPI;
using PianoGradeAPI.Contexts;
using PianoGradeAPI.Dtos;
using PianoGradeAPI.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Tests {

	public class ComposerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime {

		private CustomWebApplicationFactory<Program> webApplicationFactory;
		private HttpClient client;
		private GradesContext gradesContext;
		private IServiceScope scope;

		public ComposerIntegrationTests(CustomWebApplicationFactory<Program> webApplicationFactory) {
			this.webApplicationFactory = webApplicationFactory;
			client = webApplicationFactory.CreateClient();
			scope = webApplicationFactory.Services.CreateScope();
			gradesContext = scope.ServiceProvider.GetRequiredService<GradesContext>();
		}

		[Fact]
		public async void AllComposersReturned() {
			HttpResponseMessage response = await client.GetAsync("/composers");
			Assert.True(response.StatusCode == HttpStatusCode.OK);

			List<GetComposerDto> returnedComposers = await response.Content.ReadFromJsonAsync<List<GetComposerDto>>();

			GetComposerDto? john = returnedComposers.Where(c=> c.Name == "john" && c.Era == "baroque" && c.Nationality == "austrian").FirstOrDefault();
			Assert.NotNull(john);

			GetComposerDto? mary = returnedComposers.Where(c => c.Name == "mary" && c.Era == "romantic" && c.Nationality == "german").FirstOrDefault();
			Assert.NotNull(mary);

			GetComposerDto? alice = returnedComposers.Where(c => c.Name == "alice" && c.Era == "modern" && c.Nationality == "italian").FirstOrDefault();
			Assert.NotNull(alice);

			GetComposerDto? michael = returnedComposers.Where(c => c.Name == "michael" && c.Era == "romantic" && c.Nationality == "french").FirstOrDefault();
			Assert.NotNull(michael);
		}

		[Fact]
		public async void QueryByFullNameReturnsMatchingComposers() {
			string query = "john";
			HttpResponseMessage response = await client.GetAsync("/composers?name=" + query);

			Assert.True(response.StatusCode == HttpStatusCode.OK);

			List<GetComposerDto> returnedComposers = await response.Content.ReadFromJsonAsync<List<GetComposerDto>>();

			Assert.True(returnedComposers.Count > 0);
			Assert.True(returnedComposers.Where(c => c.Name == query).Count() == returnedComposers.Count);
		}

		[Fact]
		public async void QueryByPartialNameReturnsMatchingComposers() {
			string query = "a";
			HttpResponseMessage response = await client.GetAsync("/composers?name=" + query);

			Assert.True(response.StatusCode == HttpStatusCode.OK);

			List<GetComposerDto> returnedComposers = await response.Content.ReadFromJsonAsync<List<GetComposerDto>>();

			Assert.True(returnedComposers.Count > 0);
			Assert.True(returnedComposers.Where(c => c.Name.Contains(query)).Count() == returnedComposers.Count);
		}

		[Fact]
		public async void ComposersQueryIsLimited() {
			int limit = 1;
			HttpResponseMessage response = await client.GetAsync("/composers?limit=" + limit);

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			List<GetComposerDto> returnedComposers = await response.Content.ReadFromJsonAsync<List<GetComposerDto>>();

			Assert.Equal(limit, returnedComposers.Count);
		}

		[Fact]
		public async void QueryByIdReturnsMatchingComposer() {
			HttpResponseMessage allComposersresponse = await client.GetAsync("/composers");
			Assert.Equal(HttpStatusCode.OK, allComposersresponse.StatusCode);

			List<GetComposerDto> allComposers = await allComposersresponse.Content.ReadFromJsonAsync<List<GetComposerDto>>();
			Assert.NotEmpty(allComposers);

			foreach (GetComposerDto composer in allComposers) {
				HttpResponseMessage composerByIdResponse = await client.GetAsync("/composers/" + composer.Id);
				Assert.Equal(HttpStatusCode.OK, composerByIdResponse.StatusCode);

				GetComposerDto returnedComposer = await composerByIdResponse.Content.ReadFromJsonAsync<GetComposerDto>();
				Assert.Equal(composer.Id, returnedComposer.Id);
				Assert.Equal(composer.Name, returnedComposer.Name);
				Assert.Equal(composer.Nationality, returnedComposer.Nationality);
				Assert.Equal(composer.Era, returnedComposer.Era);
			}
		}

		public async Task InitializeAsync() {
			await DatabaseSeeder.Seed(gradesContext);
		}

		public async Task DisposeAsync() {
			await DatabaseSeeder.Clear(gradesContext);
			scope.Dispose();
		}
	}
}