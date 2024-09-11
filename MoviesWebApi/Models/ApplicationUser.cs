using Microsoft.AspNetCore.Identity;

namespace MoviesWebApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FristName {  get; set; }
        public string LastName {  get; set; }
    }
}
