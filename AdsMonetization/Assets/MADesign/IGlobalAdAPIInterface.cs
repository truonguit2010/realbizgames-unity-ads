
namespace MADesign
{

    public interface IGlobalAdAPIInterface
    {
        void InitAdNetwork(IGlobalAdCallbackInterface aa = null);

        // Banner method
        void RequestBanner_1();//1
        void ShowBanner_2();//2
        void HideBanner_3();//3
        bool IsRequestingBanner_4();

        // Interstitial methods
        bool IsInterstitialAdsReady_1();
        void ShowInterstitial_2(string placement = "");
        void DestroyInterstitialAd_3();
        void RequestInterstitial_4(bool force = false, bool showWhenLoaded = false);
        bool IsRequestingInterstitial_5();

        // Rewarded methods
        bool IsRewardedAdsReady_1();
        void RequestRewardVideo_2(bool force = false);
        void ShowRewardVideo_3(string placement = "");
        void DestroyRewardedAd_4();
        bool IsRequestingRewarded_5();
    }
}