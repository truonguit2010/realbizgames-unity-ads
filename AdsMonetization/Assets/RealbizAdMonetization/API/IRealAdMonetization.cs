
namespace RealbizGames.Ads
{
    public interface IRealAdMonetization
    {
        void Init();
        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd(InterstitialDTO dto);

        void ShowBanner();

        void HideBanner();

        void Update();

        void ShowAppOpenAd(InterstitialDTO dto);

        void Destroy();
    }
}