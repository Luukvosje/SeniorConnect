namespace Seniorconnect_Luuk_deVos.Model
{
    public class ActivityModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int maxUsers { get; set; }
        public int price { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int createdByUserId { get; set; }
        public bool joined { get; set; }
    }
}
