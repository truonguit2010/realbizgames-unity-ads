using System;
using UnityEngine;

namespace RealbizGames.Ads
{
    public class ISBannerAdController : IBannerAd
    {
        public const string TAG = "ISBannerAdController";
        
        private BannerAdConfig config;

        private DateTime lastRequestBannerAdTime = DateTime.Now;
        private double bannerUpdateIntervalCounter;

        public ISBannerAdController(BannerAdConfig config)
        {
            this.config = config;
        }

        public void Init()
        {
            //Invoked when the banner loading process has failed.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onBannerAdLoadFailedEvent += Banner_OnAdFailedToLoad_1;
            //Invoked once the banner has loaded
            IronSourceEvents.onBannerAdLoadedEvent += Banner_OnAdLoaded_2;
            // Invoked when end user clicks on the banner ad
            IronSourceEvents.onBannerAdClickedEvent += Banner_OnAdOpening_3;
            //Notifies the presentation of a full screen content following user click
            //IronSourceEvents.onBannerAdScreenPresentedEvent += Banner_OnAdOpening_3;
            //Notifies the presented screen has been dismissed
            //IronSourceEvents.onBannerAdScreenDismissedEvent += bann;
            //Invoked when the user leaves the app
            IronSourceEvents.onBannerAdLeftApplicationEvent += Banner_OnAdLeavingApplication_5;

            lastRequestBannerAdTime = DateTime.Now;
        }

        public void ShowBanner()
        {
            Debug.LogFormat("{0} - ShowBanner", TAG);
            IronSource.Agent.displayBanner();
        }

        public void HideBanner()
        {
            Debug.LogFormat("{0} - HideBanner", TAG);

            IronSource.Agent.hideBanner();
            IronSource.Agent.destroyBanner();
        }

        public void Update()
        {
            if (config.enable)
            {
                bannerUpdateIntervalCounter = DateTime.Now.Subtract(lastRequestBannerAdTime).TotalSeconds;
                if (bannerUpdateIntervalCounter >= config.reloadIntervalSeconds || bannerUpdateIntervalCounter < 0)
                {
                    // Có trường hợp nhỏ hơn 0 khi user cheat time.

                    // Update remote config data theo banner interval.
                    lastRequestBannerAdTime = DateTime.Now;
                    IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, config.isBottom ? IronSourceBannerPosition.BOTTOM : IronSourceBannerPosition.TOP);
                    Debug.LogFormat("{0} - LoadBanner {1}", TAG, config.enable);
                }
            }
        }

        public void Destroy()
        {
            //Invoked when the banner loading process has failed.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onBannerAdLoadFailedEvent -= Banner_OnAdFailedToLoad_1;
            //Invoked once the banner has loaded
            IronSourceEvents.onBannerAdLoadedEvent -= Banner_OnAdLoaded_2;
            // Invoked when end user clicks on the banner ad
            IronSourceEvents.onBannerAdClickedEvent -= Banner_OnAdOpening_3;
            //Notifies the presentation of a full screen content following user click
            //IronSourceEvents.onBannerAdScreenPresentedEvent += Banner_OnAdOpening_3;
            //Notifies the presented screen has been dismissed
            //IronSourceEvents.onBannerAdScreenDismissedEvent += bann;
            //Invoked when the user leaves the app
            IronSourceEvents.onBannerAdLeftApplicationEvent -= Banner_OnAdLeavingApplication_5;
        }

        #region Banner Callback
        private void Banner_OnAdFailedToLoad_1(IronSourceError error)
        {
            BannerFailedToLoadDTO dto = new BannerFailedToLoadDTO(code: error.getCode().ToString(), message: error.getDescription());
            AdNotificationCenter.Instance.BannerNotification.NotifyLoadFailed(dto);
        }

        private void Banner_OnAdLoaded_2()
        {
            AdNotificationCenter.Instance.BannerNotification.NotifyLoadedEvent();
        }

        private void Banner_OnAdOpening_3()
        {
            AdNotificationCenter.Instance.BannerNotification.NotifyAdClickEvent();
        }

        private void Banner_OnAdLeavingApplication_5()
        {
            AdNotificationCenter.Instance.BannerNotification.NotifyLeftApplicationEvent();
        }



        #endregion
    }
}