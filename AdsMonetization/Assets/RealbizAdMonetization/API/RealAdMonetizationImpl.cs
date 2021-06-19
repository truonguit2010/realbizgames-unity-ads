using UnityEngine;
using System;

namespace RealbizGames.Ads
{
    public class RealAdMonetizationImpl : IRealAdMonetization
    {
        const string TAG = "RealAdMonetizationImpl";

        private DateTime _back2GameTime = DateTime.Now;

        private IAdProvider provider;

        public void Destroy()
        {
            provider.Destroy();
        }

        public void Init()
        {
            provider = new IronsourceAdProvider();
            provider.Init();

            _back2GameTime = DateTime.Now;
        }

        public void OnApplicationResume()
        {
            if (Config.DefaultInstance.BackToGameAdConfig.enable) {
                double interval = DateTime.Now.Subtract(_back2GameTime).TotalSeconds;
                if (interval >= Config.DefaultInstance.BackToGameAdConfig.restrictIntervalSeconds) {
                    if (provider.isInterstitialAdAvailable()) {
                        _back2GameTime = DateTime.Now;
                        ShowInterstitialAd();
                    }
                }
            }
        }

        public void ShowBanner()
        {
            provider.ShowBanner();
        }

        public void ShowInterstitialAd()
        {
            double interval = DateTime.Now.Subtract(provider.lastVideoAdCloseTime).TotalSeconds;
            if (interval >= Config.DefaultInstance.InterstitialAdConfig.restrictIntervalSeconds) {
                provider.ShowInterstitialAd();
            } else {
                Debug.LogFormat("{0} - ShowInterstitialAd Ignore by restrictIntervalSeconds {1}", TAG, Config.DefaultInstance.InterstitialAdConfig.restrictIntervalSeconds);
            }
        }

        public void ShowRewardedAd(RewardedAdDTO dto)
        {
            provider.ShowRewardedAd(dto);
        }

        public void Update()
        {
            provider.Update();
        }
    }
}