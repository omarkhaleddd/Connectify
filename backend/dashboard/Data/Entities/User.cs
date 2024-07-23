namespace dashboard.Entities
{
    public class User
    {
        public User(){}
        public User(string id, string displayName)  
        {
            Id = id;
            DisplayName = displayName;
        }
        public string Id { get; set;}
        public string DisplayName { get; set; }
    }
}
