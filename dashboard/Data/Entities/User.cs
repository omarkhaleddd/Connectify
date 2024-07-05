namespace dashboard.Entities
{
    public class User
    {
        public User(){}
        public User(int id, string name, string email, string status,string image)  
        {
            Id = id;
            Name = name;
            Email = email;
            Status = status;
            Image = image;
        }
        public int Id { get; set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
    }
}
