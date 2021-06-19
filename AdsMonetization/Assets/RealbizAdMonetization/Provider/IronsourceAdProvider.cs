using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealbizGames.Ads
{
    public class IronsourceAdProvider : IAdProvider
    {
        private IBannerAd bannerAd;
        private IInterstitialAd interstitialAd;
        private IRewardedAd rewardedAd;

        public void Destroy()
        {
            bannerAd.Destroy();
            interstitialAd.Destroy();
            rewardedAd.Destroy();
        }

        public void Init()
        {
            bannerAd = new ISBannerAdController();
            interstitialAd = new ISInterstitialAdController();
            rewardedAd = new ISRewardedAdController();

            bannerAd.Init();
            interstitialAd.Init();
            rewardedAd.Init();
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