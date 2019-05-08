using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;
using Business.Commands;

namespace Tests.Integration
{
    public class CourseControllerTests //: IClassFixture<CustomWebApplicationFactory<Api.Startup>>
    {
        private readonly CustomWebApplicationFactory<Api.Startup> _factory;

        public CourseControllerTests()
        {
            _factory = new CustomWebApplicationFactory<Api.Startup>();
        }

        [Fact]
        public async Task CourseController_AddCourse_Basic()
        {
            var client = _factory.CreateClient();

            // create course
            var req = new AddCourseRequest { Title = "Nice title", Description = "A description", IsPublic = false };
            var result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"));

            // response fields
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            string quotedCourseId = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Guid courseId = Guid.Parse(quotedCourseId.Replace("\"", ""));
            Assert.EndsWith($"Course?id=" + courseId, result.Headers.Location.ToString()); // includes uri of created course 

            // Verify database contents
            Data.Models.Course dbCourse = _factory.DbContext.Courses.Single();
            Assert.Equal(courseId, dbCourse.Id);
            Assert.Equal(req.Title, dbCourse.Title);
            Assert.Equal(req.Description, dbCourse.Description);
            Assert.Equal(req.IsPublic, dbCourse.IsPublic);
            Assert.True(DateTimeOffset.UtcNow - dbCourse.Created < TimeSpan.FromSeconds(5));

            // Verify readback
            var fetchedCourseBody = GetAllCoursesBody(await client.GetAsync("course")).Single();
            Assert.Equal(courseId, fetchedCourseBody.Id);
            Assert.Equal(req.Title, fetchedCourseBody.Title);
            Assert.Equal(req.Description, fetchedCourseBody.Description);
            // The CourseCode field is the initial of course title with no spaces and uppercased followed by when it was added in the format “yyyyMMdd”
            Assert.Equal("NICETITLE" + DateTimeOffset.UtcNow.ToString("yyyyMMdd"), fetchedCourseBody.Code);
            Assert.True(DateTimeOffset.UtcNow - fetchedCourseBody.Created < TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task CourseController_AddCourse_RequiredFields()
        {
            var client = _factory.CreateClient();

            // No fields
            var noFieldsReq = new { };
            var result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(noFieldsReq), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var details = JsonConvert.DeserializeObject<ValidationProblemDetails>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            Assert.Equal("One or more validation errors occurred.", details.Title);
            Assert.Equal(3, details.Errors.Count);

            // Missing title
            var noTitleReq = new { Description = "A description", IsPublic = false };
            AssertMissingField("Title", await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(noTitleReq), Encoding.UTF8, "application/json")));

            // Missing description
            var noDescriptionReq = new { Title = "Nice title", IsPublic = true };
            AssertMissingField("Description", await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(noDescriptionReq), Encoding.UTF8, "application/json")));

            // Missing isPublic
            var noIsPublicReq = new { Title = "Nice title", Description = "A description" };
            AssertMissingField("IsPublic", await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(noIsPublicReq), Encoding.UTF8, "application/json")));
        }

        [Fact]
        public async Task CourseController_AddCourse_FieldContent()
        {
            var client = _factory.CreateClient();

            // Title must be less than 200 characters.
            var chars199 = new { Description = "A description", IsPublic = false, Title = new string('a', 199) };
            var result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(chars199), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

            var chars200 = new { Description = "A description", IsPublic = false, Title = new string('a', 200) };
            result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(chars200), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var details = JsonConvert.DeserializeObject<ValidationProblemDetails>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            var error = details.Errors.Single();
            Assert.Equal("Title", error.Key);
            Assert.Equal("Title must be less than 200 characters", error.Value.Single());
        }

        [Fact]
        public async Task CourseController_GetCourses()
        {
            var client = _factory.CreateClient();

            // empty request
            Assert.Empty(GetAllCoursesBody(await client.GetAsync("course")));

            // normal request
            var req = new AddCourseRequest { Title = "def", Description = "A description", IsPublic = false };
            var result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"));
            var body = GetAllCoursesBody(await client.GetAsync("course"));
            Assert.Single(body);
            Assert.Equal("def", body.Single().Title);

            // multiple-entries request
            req = new AddCourseRequest { Title = "abc", Description = "A description", IsPublic = false };
            result = await client.PostAsync("course", new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"));
            body = GetAllCoursesBody(await client.GetAsync("course"));
            Assert.Equal(2, body.Length);

            // test The list is ordered by course title
            Assert.Equal("abc", body.First().Title);
            Assert.Equal("def", body.Last().Title);
        }

        private void AssertMissingField(string name, HttpResponseMessage result)
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var details = JsonConvert.DeserializeObject<ValidationProblemDetails>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            Assert.Equal("One or more validation errors occurred.", details.Title);
            var error = details.Errors.Single();
            Assert.Equal(name, error.Key);
            Assert.Equal($"The {name} field is required.", error.Value.Single());
        }

        private Business.Models.Course[] GetAllCoursesBody(HttpResponseMessage fetchedCourse)
        {
            string fetchedBody = fetchedCourse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Business.Models.Course[]>(fetchedBody);
        }
    }
}
