using UnityEngine;
using System;

namespace RealbizGames.Ads
{
    public class RealAdMonetizationImpl : IRealAdMonetization
    {
        const string TAG = "RealAdMonetizationImpl";

        private static RealAdMonetizationImpl _defaultInstance;

        public static RealAdMonetizationImpl DefaultInstance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new RealAdMonetizationImpl();
                }
                return _defaultInstance;
            }
        }

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

        public void ShowAppOpenAd(InterstitialDTO dto)
        {
            if (Config.DefaultInstance.BackToGameAdConfig.enable)
            {
                double interval = DateTime.Now.Subtract(_back2GameTime).TotalSeconds;
                if (interval >= Config.DefaultInstance.BackToGameAdConfig.restrictIntervalSeconds)
                {
                    if (provider.isInterstitialAdAvailable())
                    {
                        _back2GameTime = DateTime.Now;
                        ShowInterstitialAd(dto);
                    }
                }
            }
        }

        public void ShowBanner()
        {
            if (Config.DefaultInstance.BannerAdConfig.enable)
            {
                provider.ShowBanner();
            }
            else
            {
                Debug.LogFormat("{0} - ShowBanner is not enable", TAG);
            }
        }

        public void HideBanner()
        {
            provider.HideBanner();
        }

        public void ShowInterstitialAd(InterstitialDTO dto)
        {
            if (Config.DefaultInstance.InterstitialAdConfig.enable)
            {
                double interval = DateTime.Now.Subtract(provider.lastVideoAdCloseTime).TotalSeconds;
                if (interval >= Config.DefaultInstance.InterstitialAdConfig.restrictIntervalSeconds)
                {
                    provider.ShowInterstitialAd(dto);
                }
                else
                {
                    Debug.LogFormat("{0} - ShowInterstitialAd Ignore by restrictIntervalSeconds {1}", TAG, Config.DefaultInstance.InterstitialAdConfig.restrictIntervalSeconds);
                }
            }
            else
            {
                Debug.LogFormat("{0} - ShowInterstitialAd is not enable", TAG);
            }
        }

        public void ShowRewardedAd(RewardedAdDTO dto)
        {
            provider.ShowRewardedAd(dto);
        }

        public void Update()
        {
            if (provider != null)
            {
                provider.Update();
            }
        }

    }
}