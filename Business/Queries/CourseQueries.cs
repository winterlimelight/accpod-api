using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Business.Queries
{
    public interface ICourseQueries
    {
        Task<IEnumerable<Models.Course>> GetAll();
    }

    public class CourseQueries : ICourseQueries
    {
        private readonly Data.SqliteContext _ctx;

        public CourseQueries(Data.SqliteContext context)
        {
            _ctx = context;
        }

        public async Task<IEnumerable<Models.Course>> GetAll()
        {
            return await _ctx.Courses
                .Select(c => new Models.Course(c))
                .OrderBy(c => c.Title)
                .ToListAsync();
        }
    }
}
