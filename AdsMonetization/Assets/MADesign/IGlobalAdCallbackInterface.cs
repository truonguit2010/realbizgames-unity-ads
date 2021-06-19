
namespace MADesign
{
    public interface IGlobalAdCallbackInterface
    {
        // Banner callback
        void Banner_OnAdFailedToLoad_1(string provider, string message);//1

        void Banner_OnAdLoaded_2(string provider);//2

        void Banner_OnAdOpening_3(string provider);//3

        void Banner_OnPaidEvent_4(string provider, string CurrencyCode, long Value);// 4

        void Banner_OnAdLeavingApplication_5(string provider);// 5

        // Interstitial callbacks
        void Interstitial_OnAdLoaded_1(string provider);

        void Interstitial_OnAdFailedToLoad_2(string provider, string message, string adid);

        void Interstitial_OnAdClosed_3(string provider);

        void Interstitial_OnAdOpening_4(string provider);

        void Interstitial_OnAdPaidEvent_5(string provider, string CurrencyCode, long Value);

        void Interstitial_OnAdLeavingApplication_6(string provider);

        // RewardedAd callbacks
        void RewardedAd_OnAdLoaded_1(string provider);

        void RewardedAd_OnAdFailedToLoad_2(string provider, string message, string adId);

        void RewardedAd_OnAdFailedToShow_3(string provider, string message, string adId, string errorCode);

        void RewardedAd_OnAdOpening_4(string provider);

        void RewardedAd_OnUserEarnedReward_5(string provider);

        void RewardedAd_OnAdClosed_6(string provider);

        void RewardedAd_OnPaidEvent_7(string provider, string CurrencyCode, long Value);

    }
}