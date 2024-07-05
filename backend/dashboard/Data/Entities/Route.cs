namespace dashboard.Entities
{
    public class Route
    {
        public string Href { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Route[] Children { get; set; }
    }
}
