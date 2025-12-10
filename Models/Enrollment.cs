using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int ClassID { get; set; }

        [Required]
        public DateTime EnrolledOn { get; set; }

        // Navigation Properties
        public Member? Member { get; set; }
        public Class Class { get; set; }
    }
}
