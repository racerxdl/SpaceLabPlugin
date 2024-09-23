namespace SpaceLabAPI.Models
{
    public class Faction
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string PublicNote { get; set; }
        public string FounderName { get; set; }
        public string[] Members { get; set; }
    }
}
