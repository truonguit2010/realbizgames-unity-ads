
namespace RealbizGames.Ads
{
    public interface IAdProvider
    {
        void Init();

        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd();

        void ShowBanner();

        void Update();

        void Destroy();
    }
}