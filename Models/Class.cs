using System.ComponentModel.DataAnnotations;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        public DateTime Schedule { get; set; }

        [Required]
        public int TrainerID { get; set; }

        // Navigation properties
        public Trainer? Trainer { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
