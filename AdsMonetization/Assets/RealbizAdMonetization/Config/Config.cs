
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
        public ISConfig IsConfig { get => _isConfig; set => _isConfig = value; }

        private BannerAdConfig _bannerAdConfig;
        private InterstitialAdConfig _interstitialAdConfig;
        private RewardedAdConfig _rewardedAdConfig;
        private BackToGameAdConfig _backToGameAdConfig;

        private ISConfig _isConfig;

        public Provider provider = Provider.IronSource;

        private Config() {
            BannerAdConfig = new BannerAdConfig();
            InterstitialAdConfig = new InterstitialAdConfig();
            RewardedAdConfig = new RewardedAdConfig();
            _backToGameAdConfig = new BackToGameAdConfig();
            _isConfig = new ISConfig();
        }


    }
}