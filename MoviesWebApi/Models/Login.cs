namespace MoviesWebApi.Models
{
    public class Login
    {
        [MaxLength(50)]
        public string Email {  get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
