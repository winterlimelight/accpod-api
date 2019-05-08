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

        // Nullable bool is used so that we can detect when the property was ignored. Without it a missing field would default to false
        [Required]
        public bool? IsPublic { get; set; }
    }
}
