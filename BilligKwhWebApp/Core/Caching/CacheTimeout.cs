namespace BilligKwhWebApp.Core.Caching
{
    public sealed class CacheTimeout
    {
        public const int FifteenMinutes = 15;
        public const int Brief = 30;
        public const int Normal = 60;
        public const int TwoHours = 120;
        public const int ThreeHours = 180;
        public const int FourHours = 240;
        public const int FiveHours = 300;
        public const int SixHours = 360;
        public const int EightHours = 480;
        public const int TenHours = 600;
        public const int TwelveHours = 720;
        public const int Day = 1440;
        public const int VeryLong = 21600;
        public const int NeverExpire = int.MaxValue;
    }
}
