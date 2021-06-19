
using System;
using UnityEngine;

namespace MADesign
{
    public class MAIronSourceController : MonoBehaviour, IGlobalAdAPIInterface
    {
        const string TAG = "IronSourceController";

        [SerializeField]
        private string ironsourceAndroidKey = "d4f8bf65";

        [SerializeField]
        private string ironsourceIosKey = "d37c0d75";

        public string ironSourceAppId
        {
            get
            {
#if UNITY_IOS
            return ironsourceIosKey;
#elif UNITY_ANDROID
            return ironsourceAndroidKey;
#else
                return "unexpected_platform";
#endif
            }
        }

        public string ironsourceBannerAdId
        {
            get
            {
                return "no_banner_id";
            }
        }

        public string ironsourceInterstitialAdId
        {
            get
            {
                return "no_interstitial_id";
            }
        }

        private string ironsourceRewardedAdId
        {
            get
            {
                return "no_rewarded_id";
            }
        }

        public IGlobalAdCallbackInterface globalAdCallbackInterface = null;

        public void InitAdNetwork(IGlobalAdCallbackInterface aa)
        {
            Debug.LogFormat("{0} InitAdNetwork {1}", TAG, ironSourceAppId);

            this.globalAdCallbackInterface = aa;

            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(ironSourceAppId);
            IronSource.Agent.shouldTrackNetworkState(true);

            AddIronSourceCallbackListeners(); 
        }


        private void AddIronSourceCallbackListeners()
        {
            // Add Banner Events

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

            // Add Interstitial Events

            //Invoked when the Interstitial is Ready to shown after load function is called
            IronSourceEvents.onInterstitialAdReadyEvent += Interstitial_OnAdLoaded_1;
            //Invoked when the initialization process has failed.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onInterstitialAdLoadFailedEvent += Interstitial_OnAdFailedToLoad_2;
            //Invoked right before the Interstitial screen is about to open.
            IronSourceEvents.onInterstitialAdShowSucceededEvent += Interstitial_OnAdOpening_4;
            //Invoked when the ad fails to show.
            //@param description - string - contains information about the failure.
            //IronSourceEvents.onInterstitialAdShowFailedEvent += Inter;
            // Invoked when end user clicked on the interstitial ad
            //IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            //Invoked when the Interstitial Ad Unit has opened
            //IronSourceEvents.onInterstitialAdOpenedEvent += Interstitial;
            //Invoked when the interstitial ad closed and the user goes back to the application screen.
            IronSourceEvents.onInterstitialAdClosedEvent += Interstitial_OnAdClosed_3;

            //----------------------------------------------------------------------------------------
            //----------------------------------------------------------------------------------------
            //Add Rewarded Video Events
            //----------------------------------------------------------------------------------------
            //----------------------------------------------------------------------------------------

            //Invoked when the RewardedVideo ad view has opened.
            //Your Activity will lose focus. Please avoid performing heavy 
            //tasks till the video ad will be closed.
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedAd_OnAdOpening_4;
            //Invoked when the RewardedVideo ad view is about to be closed.
            //Your activity will now regain its focus.
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedAd_OnAdClosed_6;
            //Invoked when there is a change in the ad availability status.
            //@param - available - value will change to true when rewarded videos are available. 
            //You can then show the video by calling showRewardedVideo().
            //Value will change to false when no videos are available.
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedAd_OnAdLoaded_1;
            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------

            //Invoked when the video ad starts playing. 
            //IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;

            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------
            //Invoked when the video ad finishes playing. 
            // IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedAd_OnUserEndReward_8;

            //Invoked when the user completed the video and should be rewarded. 
            //If using server-to-server callbacks you may ignore this events and wait for 
            // the callback from the  ironSource server.
            //@param - placement - placement object which contains the reward data
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedAd_OnUserEarnedReward_5;
            //Invoked when the Rewarded Video failed to show
            //@param description - string - contains information about the failure.
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedAd_OnAdFailedToShow_3;
            // IronSourceEvents.onRewardedVideoAdClickedEvent += Rewarded;

        }

        // -------------------------------------------------------------------------
        // BANNER
        // -------------------------------------------------------------------------
        public void RequestBanner_1()
        {
            //Debug.LogFormat("{0} RequestBanner_1: {1}", TAG, ironsourceBannerAdId);
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        public void ShowBanner_2()
        {
            IronSource.Agent.displayBanner();
        }

        public void HideBanner_3()
        {
            IronSource.Agent.hideBanner();
            IronSource.Agent.destroyBanner();
        }

        public bool IsRequestingBanner_4()
        {
            return false;
        }

        // -------------------------------------------------------------------------
        // INTERSTITIAL
        // -------------------------------------------------------------------------
        public bool IsInterstitialAdsReady_1()
        {
            return IronSource.Agent.isInterstitialReady();
        }


        public void ShowInterstitial_2(string placement = "")
        {
            IronSource.Agent.showInterstitial();
        }

        public void DestroyInterstitialAd_3()
        {
            // 
        }

        public void RequestInterstitial_4(bool force = false, bool showWhenLoaded = false)
        {
            IronSource.Agent.loadInterstitial();
        }

        public bool IsRequestingInterstitial_5()
        {
            return false;
        }

        // -------------------------------------------------------------------------
        // REWARDED
        // -------------------------------------------------------------------------

        public bool IsRewardedAdsReady_1()
        {
            return IronSource.Agent.isRewardedVideoAvailable();
        }

        public void RequestRewardVideo_2(bool force = false)
        {
            // Ironsource không cần request.
        }

        public void ShowRewardVideo_3(string placement = "")
        {
            IronSource.Agent.showRewardedVideo();
        }

        public void DestroyRewardedAd_4()
        {
            // Không load thì cũng không cần request.
        }

        public bool IsRequestingRewarded_5()
        {
            return false;
        }

        #region Banner Callback
        private void Banner_OnAdFailedToLoad_1(IronSourceError error)
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Banner_OnAdFailedToLoad_1(MAAdController.PROVIDER_IRONSOURCE, error.getDescription());
            }
        }

        private void Banner_OnAdLoaded_2()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Banner_OnAdLoaded_2(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void Banner_OnAdOpening_3()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Banner_OnAdOpening_3(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void Banner_OnPaidEvent_4()
        {
            if (this.globalAdCallbackInterface != null)
            {
                //globalAdCallbackInterface.Banner_OnPaidEvent_4(AdController.PROVIDER_IRONSOURCE, e.AdValue.CurrencyCode, e.AdValue.Value);
            }
        }

        private void Banner_OnAdLeavingApplication_5()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Banner_OnAdLeavingApplication_5(MAAdController.PROVIDER_IRONSOURCE);
            }
        }
        #endregion

        #region Interstitial Ads callbacks
        private void Interstitial_OnAdLoaded_1()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Interstitial_OnAdLoaded_1(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void Interstitial_OnAdFailedToLoad_2(IronSourceError e)
        {
            Debug.Log("@LOG Interstitial_OnAdFailedToLoad_2 e:" + e.getDescription());

            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Interstitial_OnAdFailedToLoad_2(MAAdController.PROVIDER_IRONSOURCE, e.getDescription(), ironsourceInterstitialAdId);
            }
        }

        private void Interstitial_OnAdClosed_3()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Interstitial_OnAdClosed_3(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void Interstitial_OnAdOpening_4()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Interstitial_OnAdOpening_4(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void Interstitial_OnAdPaidEvent_5()
        {
            if (this.globalAdCallbackInterface != null)
            {
                //globalAdCallbackInterface.Interstitial_OnAdPaidEvent_5(AdController.PROVIDER_IRONSOURCE, e.AdValue.CurrencyCode, e.AdValue.Value);
            }
        }

        private void Interstitial_OnAdLeavingApplication_6(object sender, EventArgs e)
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.Interstitial_OnAdLeavingApplication_6(MAAdController.PROVIDER_IRONSOURCE);
            }
        }
        #endregion

        #region Rewarded Ads Callbacks
        private void RewardedAd_OnAdLoaded_1(bool isAvailableToShow)
        {
            if (this.globalAdCallbackInterface != null)
            {
                if (isAvailableToShow)
                {
                    globalAdCallbackInterface.RewardedAd_OnAdLoaded_1(MAAdController.PROVIDER_IRONSOURCE);
                }
                //else {
                //    RewardedAd_OnAdFailedToLoad_2("not available");
                //}
            }
        }

        private void RewardedAd_OnAdFailedToLoad_2(string message)
        {

            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.RewardedAd_OnAdFailedToLoad_2(MAAdController.PROVIDER_IRONSOURCE, message, ironsourceRewardedAdId);
            }
        }

        private void RewardedAd_OnAdFailedToShow_3(IronSourceError e)
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.RewardedAd_OnAdFailedToShow_3(MAAdController.PROVIDER_IRONSOURCE, e.getDescription(), ironsourceRewardedAdId, "" + e.getErrorCode());
            }
        }

        private void RewardedAd_OnAdOpening_4()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.RewardedAd_OnAdOpening_4(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void RewardedAd_OnUserEarnedReward_5(IronSourcePlacement ironSourcePlacement)
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.RewardedAd_OnUserEarnedReward_5(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void RewardedAd_OnAdClosed_6()
        {
            if (this.globalAdCallbackInterface != null)
            {
                globalAdCallbackInterface.RewardedAd_OnAdClosed_6(MAAdController.PROVIDER_IRONSOURCE);
            }
        }

        private void RewardedAd_OnPaidEvent_7()
        {
            if (this.globalAdCallbackInterface != null)
            {
                //globalAdCallbackInterface.RewardedAd_OnPaidEvent_7(AdController.PROVIDER_IRONSOURCE, e.AdValue.CurrencyCode, e.AdValue.Value);
            }
        }
        #endregion
    }
}
