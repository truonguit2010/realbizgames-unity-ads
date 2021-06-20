
namespace RealbizGames.Ads
{
    public interface IAdProvider
    {
        void Init();

        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd(InterstitialDTO dto);

        bool isInterstitialAdAvailable();

        void ShowBanner();

        void HideBanner();

        void Update();

        void Destroy();

        System.DateTime lastVideoAdCloseTime { get; }
    }
}