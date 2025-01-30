namespace Travel_Planner.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Itinerary> Itineraries { get; set; }
    }
}
