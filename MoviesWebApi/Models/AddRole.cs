namespace MoviesWebApi.Models
{
    public class AddRole
    {
        [MaxLength(50)]
        public string UserId { get; set; }
        [MaxLength(50)]
        public string Roleid { get; set; }
    }
}
