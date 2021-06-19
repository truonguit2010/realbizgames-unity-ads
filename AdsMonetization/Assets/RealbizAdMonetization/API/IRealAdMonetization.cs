
namespace RealbizGames.Ads
{
    public interface IRealAdMonetization
    {
        void Init();
        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd();

        void ShowBanner();

        void Update();

        void OnApplicationResume();

        void Destroy();
    }
}