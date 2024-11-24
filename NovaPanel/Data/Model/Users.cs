namespace NovaPanel.Data.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // This will be stored as an MD5 hash in the DB
        public string Email { get; set; }
        public DateTime RegDate { get; set; }
        public int Level { get; set; }
        public string Record { get; set; }
    }

}
