using System;
using System.Text.RegularExpressions;

namespace Business.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public DateTimeOffset Created { get; set; }

        public Course() { }

        // TODO - As copying between layer models gets more frequent use Automapper
        public Course(Data.Models.Course course)
        {
            Id = course.Id;
            Title = course.Title;
            Description = course.Description;
            Created = course.Created;

            // The CourseCode field is the initial value of course title with no spaces and uppercased followed by when it was added in the format “yyyyMMdd”
            Code = Regex.Replace(Title.ToUpperInvariant(), @"\s+", "") + Created.ToString("yyyyMMdd");
        }
    }
}
