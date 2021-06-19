using System;

namespace RealbizGames.Ads
{
    public class IronsourceAdProvider : IAdProvider
    {
        private IBannerAd bannerAd;
        private IInterstitialAd interstitialAd;
        private IRewardedAd rewardedAd;

        public DateTime lastVideoAdCloseTime => interstitialAd.lastInterstitialAdClosedTime;

        public void Destroy()
        {
            bannerAd.Destroy();
            interstitialAd.Destroy();
            rewardedAd.Destroy();
        }

        public void Init()
        {
            bannerAd = new ISBannerAdController(Config.DefaultInstance.BannerAdConfig);
            interstitialAd = new ISInterstitialAdController(Config.DefaultInstance.InterstitialAdConfig);
            rewardedAd = new ISRewardedAdController(Config.DefaultInstance.RewardedAdConfig);

            bannerAd.Init();
            interstitialAd.Init();
            rewardedAd.Init();
        }

        public bool isInterstitialAdAvailable()
        {
            return interstitialAd.isAvailableAd();
        }

        public void ShowBanner()
        {
            bannerAd.ShowBanner();
        }

        public void ShowInterstitialAd()
        {
            interstitialAd.ShowInterstitial();
        }

        public void ShowRewardedAd(RewardedAdDTO dto)
        {
            rewardedAd.ShowRewardedAd(dto);
        }

        public void Update()
        {
            bannerAd.Update();
            interstitialAd.Update();
            rewardedAd.Update();
        }
    }
}