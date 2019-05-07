using System.ComponentModel.DataAnnotations;

namespace Business.Commands
{
    public class AddCourseRequest : ICommand
    {
        [Required]
        [StringLength(199, ErrorMessage = "Title must be less than 200 characters")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}
