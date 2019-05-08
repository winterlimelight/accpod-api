using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business.CommandHandlers
{
    public class AddCourse : ICommandHandler<Commands.AddCourseRequest>
    {
        private readonly Data.SqliteContext _ctx;

        public AddCourse(Data.SqliteContext context)
        {
            _ctx = context;
        }

        public async Task<Guid?> Handle(Commands.AddCourseRequest request)
        {
            Guid id = Guid.NewGuid();
            _ctx.Courses.Add(new Data.Models.Course
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                IsPublic = request.IsPublic.Value,
                Created = DateTimeOffset.UtcNow
            });
            await _ctx.SaveChangesAsync();
            return id;
        }
   }
}