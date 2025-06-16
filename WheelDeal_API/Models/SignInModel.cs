using System.ComponentModel.DataAnnotations;

namespace WheelDeal_API.Models
{
    public class SignInModel
    {

        [Required,EmailAddress]
        [Key]
           public string? Email { get; set; }
        [Required]
           public string? Password { get; set; }


    }
}
