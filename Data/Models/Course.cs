using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
