namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string MinimumMinutesBeforeAskingService{ get; set; }
        public string HourPriceWalkUSD { get; set; }
        public string MinimumUpdateSheduleHours { get; set; }
        public string MaximumUpdateSheduleHours { get; set; }

    }
}