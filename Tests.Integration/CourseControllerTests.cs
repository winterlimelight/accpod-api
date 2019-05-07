using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Business.Commands;

namespace Tests.Integration
{
    public class CourseControllerTests
    {
        private readonly TestClient _ctx = new TestClient();

        [Fact]
        public async Task CourseController_AddCourse_Basic()
        {
            // TODO use EF test server to verify correct data in store and that returned GET url includes correct id.

            var req = new AddCourseRequest { Title = "Nice title", Description = "A description", IsPublic = false };
            var result = await _ctx.Client.PostAsync($"course", new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"));

            // TODO Location header missing the Guid
            // TODO use EF test server

        }

        [Fact]
        public void CourseController_AddCourse_RequiredFields()
        {

            // TODO use dynamic object to have parameters missed.
        }

        [Fact]
        public void CourseController_AddCourse_TitleLength()
        {

        }

        [Fact]
        public void CourseController_GetCourses()
        {
            // empty request

            // normal request
        }
    }
}
