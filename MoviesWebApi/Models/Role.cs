namespace MoviesWebApi.Models
{
    public class Role
    {
        [MaxLength(50)]
        public string RoleName { get; set; }
    }
}
