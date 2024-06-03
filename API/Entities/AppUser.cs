using System.ComponentModel.DataAnnotations;

namespace API.Entities; 

public class AppUser 
{ 
    // EF needs to be public 
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }


}