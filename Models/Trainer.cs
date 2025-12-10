using System.ComponentModel.DataAnnotations;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerID { get; set; }

        [Required]
        [StringLength(50)]
        public string? First_name { get; set; }

        [Required]
        [StringLength(50)]
        public string? Last_name { get; set; }

        [Required]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string? Specialty { get; set; }

        // Navigation Property
        public ICollection<Class> Classes { get; set; }
    }
}
