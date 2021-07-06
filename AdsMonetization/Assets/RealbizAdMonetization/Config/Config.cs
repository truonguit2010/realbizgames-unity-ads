
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

        public BannerAdConfig BannerAdConfig { get => _bannerAdConfig; }
        public InterstitialAdConfig InterstitialAdConfig { get => _interstitialAdConfig; }
        public RewardedAdConfig RewardedAdConfig { get => _rewardedAdConfig; }
        public BackToGameAdConfig BackToGameAdConfig { get => _backToGameAdConfig; }
        public ISConfig IsConfig { get => _isConfig; set => _isConfig = value; }

        private BannerAdConfig _bannerAdConfig;
        private InterstitialAdConfig _interstitialAdConfig;
        private RewardedAdConfig _rewardedAdConfig;
        private BackToGameAdConfig _backToGameAdConfig;

        private ISConfig _isConfig;

        public Provider provider = Provider.IronSource;

        private Config() {
            _bannerAdConfig = new BannerAdConfig();
            _interstitialAdConfig = new InterstitialAdConfig();
            _rewardedAdConfig = new RewardedAdConfig();
            _backToGameAdConfig = new BackToGameAdConfig();
            _isConfig = new ISConfig();
        }


    }
}