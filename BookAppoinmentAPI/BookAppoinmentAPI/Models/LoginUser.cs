using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.Models
{
    public class LoginUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId {  get; set; }

        [Required]
        public string Username {  get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailId {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash {  get; set; }
        [Required]
        public string Role {  get; set; }

        [Required]
        public Employee Employee { get; set; }
        public int EmployeeId {  get; set; }
    }
}
