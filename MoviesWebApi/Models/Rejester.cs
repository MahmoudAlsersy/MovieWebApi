namespace MoviesWebApi.Models
{
    public class Rejester
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Email {  get; set; }
        [MaxLength(150)]
        public string Passwored { get; set; }
        [MaxLength (50)]
        public string UserName { get; set; }

    }
}
