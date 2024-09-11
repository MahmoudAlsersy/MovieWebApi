namespace MoviesWebApi.Models
{
    public class AuthModel
    {
        public string Massege {  get; set; }
        public string Token { get; set; }
        public bool ISAuthenticate { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Expairon { get; set; }
    }
}
