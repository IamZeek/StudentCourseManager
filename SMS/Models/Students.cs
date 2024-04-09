using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Students
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string Address3 { get; set; }
        public string? Courses { get; set; }
    }
}
