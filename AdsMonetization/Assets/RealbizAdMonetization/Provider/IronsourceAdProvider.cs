using System;
using UnityEngine;

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
            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(Config.DefaultInstance.IsConfig.platfromId);
            IronSource.Agent.shouldTrackNetworkState(true);

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

        public void HideBanner()
        {
            bannerAd.HideBanner();
        }

        public void ShowInterstitialAd(InterstitialDTO dto)
        {
            interstitialAd.ShowInterstitial(dto);
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