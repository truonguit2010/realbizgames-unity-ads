
namespace RealbizGames.Ads
{
    public class AdNotificationCenter
    {
        private static AdNotificationCenter _instance;

        public static AdNotificationCenter Instance {
            get {
                if (_instance == null) {
                    _instance = new AdNotificationCenter();
                }
                return _instance;
            }
        }

        public BannerNotification BannerNotification { get => _bannerNotification; set => _bannerNotification = value; }
        public InterstitialNotification InterstitialNotification { get => _interstitialNotification; set => _interstitialNotification = value; }
        public RewardedNotification RewardedNotification { get => _rewardedNotification; set => _rewardedNotification = value; }

        private BannerNotification _bannerNotification;

        private InterstitialNotification _interstitialNotification;

        private RewardedNotification _rewardedNotification;

        private AdNotificationCenter() {
            _bannerNotification = new BannerNotification();
            _interstitialNotification = new InterstitialNotification();
            _rewardedNotification = new RewardedNotification();
        }


        
    }
}