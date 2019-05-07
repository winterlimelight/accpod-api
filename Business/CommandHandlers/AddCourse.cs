using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business.CommandHandlers
{
    public class AddCourse : ICommandHandler<Commands.AddCourseRequest>
    {
        // TODO private readonly ILogger _logger;
        private readonly Data.SqliteContext _ctx;

        public AddCourse(/* TODO ILogger logger, */ Data.SqliteContext context)
        {
            // TODO _logger = logger;
            _ctx = context;
        }

        public async Task<Guid?> Handle(Commands.AddCourseRequest request)
        {
            DateTimeOffset created = DateTimeOffset.UtcNow;

            // The CourseCode field is the initial value of course title with no spaces and uppercased followed by when it was added in the format “yyyyMMdd”
            string code = Regex.Replace(request.Title.ToUpperInvariant(), @"\s+", "") + created.ToString("yyyyMMdd");

            Guid id = Guid.NewGuid();
            _ctx.Courses.Add(new Data.Models.Course
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                IsPublic = request.IsPublic,
                Code = code,
                Created = created
            });
            await _ctx.SaveChangesAsync();
            return id;
        }
   }
}