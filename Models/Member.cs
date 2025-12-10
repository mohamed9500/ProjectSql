using System.ComponentModel.DataAnnotations;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string First_name { get; set; }

        [Required]
        [StringLength(50)]
        public string Last_name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public int? MembershipID { get; set; }

        // Navigation Properties
        public Membership Membership { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}

