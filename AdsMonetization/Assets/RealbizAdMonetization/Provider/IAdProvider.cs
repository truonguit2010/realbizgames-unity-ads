
namespace RealbizGames.Ads
{
    public interface IAdProvider
    {
        void Init();

        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd();

        bool isInterstitialAdAvailable();

        void ShowBanner();

        void Update();

        void Destroy();

        System.DateTime lastVideoAdCloseTime { get; }
    }
}