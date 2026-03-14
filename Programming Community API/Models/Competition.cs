namespace Programming_Community_API.Models
{
    public class Competition
    {
        public int competition_id { get; set; }
        public string title { get; set; }
        public string password { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int? created_by { get; set; }
        public int? rounds { get; set; }
    }
}