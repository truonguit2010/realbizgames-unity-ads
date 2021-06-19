
namespace RealbizGames.Ads
{
    public class Config
    {
        private static Config _defaultInstance;

        public static Config DefaultInstance {
            get {
                if (_defaultInstance == null) {
                    _defaultInstance = new Config();
                }
                return _defaultInstance;
            }
        }

        public BannerAdConfig BannerAdConfig { get => _bannerAdConfig; set => _bannerAdConfig = value; }
        public InterstitialAdConfig InterstitialAdConfig { get => _interstitialAdConfig; set => _interstitialAdConfig = value; }
        public RewardedAdConfig RewardedAdConfig { get => _rewardedAdConfig; set => _rewardedAdConfig = value; }
        public BackToGameAdConfig BackToGameAdConfig { get => _backToGameAdConfig; set => _backToGameAdConfig = value; }

        private BannerAdConfig _bannerAdConfig;
        private InterstitialAdConfig _interstitialAdConfig;
        private RewardedAdConfig _rewardedAdConfig;
        private BackToGameAdConfig _backToGameAdConfig;

        private Config() {
            BannerAdConfig = new BannerAdConfig();
            InterstitialAdConfig = new InterstitialAdConfig();
            RewardedAdConfig = new RewardedAdConfig();
            _backToGameAdConfig = new BackToGameAdConfig();
        }


    }
}