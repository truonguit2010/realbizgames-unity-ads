

namespace RealbizGames.Ads
{
    public class ISInterstitialAdController : IInterstitialAd
    {
        public void Destroy()
        {
            //Invoked when the Interstitial is Ready to shown after load function is called
            IronSourceEvents.onInterstitialAdReadyEvent -= onInterstitialAdReadyEvent;
            //Invoked when the initialization process has failed.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onInterstitialAdLoadFailedEvent -= onInterstitialAdLoadFailedEvent;
            //Invoked right before the Interstitial screen is about to open.
            IronSourceEvents.onInterstitialAdShowSucceededEvent -= onInterstitialAdShowSucceededEvent;
            //Invoked when the ad fails to show.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onInterstitialAdShowFailedEvent -= onInterstitialAdShowFailedEvent;
            // Invoked when end user clicked on the interstitial ad
            IronSourceEvents.onInterstitialAdClickedEvent -= onInterstitialAdClickedEvent;
            //Invoked when the Interstitial Ad Unit has opened
            IronSourceEvents.onInterstitialAdOpenedEvent -= onInterstitialAdOpenedEvent;
            //Invoked when the interstitial ad closed and the user goes back to the application screen.
            IronSourceEvents.onInterstitialAdClosedEvent -= onInterstitialAdClosedEvent;
        }

        public void Init()
        {
            
            //Invoked when the Interstitial is Ready to shown after load function is called
            IronSourceEvents.onInterstitialAdReadyEvent += onInterstitialAdReadyEvent;
            //Invoked when the initialization process has failed.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onInterstitialAdLoadFailedEvent += onInterstitialAdLoadFailedEvent;
            //Invoked right before the Interstitial screen is about to open.
            IronSourceEvents.onInterstitialAdShowSucceededEvent += onInterstitialAdShowSucceededEvent;
            //Invoked when the ad fails to show.
            //@param description - string - contains information about the failure.
            IronSourceEvents.onInterstitialAdShowFailedEvent += onInterstitialAdShowFailedEvent;
            // Invoked when end user clicked on the interstitial ad
            IronSourceEvents.onInterstitialAdClickedEvent += onInterstitialAdClickedEvent;
            //Invoked when the Interstitial Ad Unit has opened
            IronSourceEvents.onInterstitialAdOpenedEvent += onInterstitialAdOpenedEvent;
            //Invoked when the interstitial ad closed and the user goes back to the application screen.
            IronSourceEvents.onInterstitialAdClosedEvent += onInterstitialAdClosedEvent;

        }

        public void ShowInterstitial()
        {
            IronSource.Agent.showInterstitial();
        }

        public void Update()
        {
            
        }

        // -------------------------------------------------------------
        #region Interstitial Ads callbacks
        private void onInterstitialAdReadyEvent()
        {
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdReadyEvent.Invoke();
        }

        private void onInterstitialAdLoadFailedEvent(IronSourceError e)
        {
            InterstitialFailedToLoadDTO dto = new InterstitialFailedToLoadDTO(code: e.getCode().ToString(), message: e.getDescription());
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdLoadFailedEvent.Invoke(dto);
        }

        private void onInterstitialAdClosedEvent()
        {
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdClosedEvent.Invoke();
        }

        private void onInterstitialAdShowSucceededEvent()
        {
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdShowSucceededEvent.Invoke();
        }

        private void onInterstitialAdClickedEvent()
        {
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdClickedEvent.Invoke();
        }

        private void onInterstitialAdOpenedEvent()
        {
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdOpenedEvent.Invoke();
        }
        
        private void onInterstitialAdShowFailedEvent(IronSourceError e) {
            InterstitialFailedToShowDTO dto = new InterstitialFailedToShowDTO(code: e.getCode().ToString(), message: e.getDescription());
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdShowFailedEvent.Invoke(dto);
        }
        #endregion
    }
}
