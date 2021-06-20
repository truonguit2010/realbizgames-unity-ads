using System;

namespace RealbizGames.Ads
{
    public class ISInterstitialAdController : IInterstitialAd
    {

        private InterstitialAdConfig config;

        private double interstitialUpdateIntervalCounter;
        private DateTime lastRequestInterstitialAdTime = DateTime.Now;

        private DateTime _lastInterstitialAdClosedTime = DateTime.Now;

        public DateTime lastInterstitialAdClosedTime => _lastInterstitialAdClosedTime;

        private InterstitialDTO interstitialDTO;

        public ISInterstitialAdController(InterstitialAdConfig config)
        {
            this.config = config;
        }

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


            lastRequestInterstitialAdTime = DateTime.Now;
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

        public void ShowInterstitial(InterstitialDTO dto)
        {
            this.interstitialDTO = dto;
            IronSource.Agent.showInterstitial();
        }

        public bool isAvailableAd()
        {
            return IronSource.Agent.isInterstitialReady();
        }

        public void Update()
        {
            interstitialUpdateIntervalCounter = DateTime.Now.Subtract(lastRequestInterstitialAdTime).TotalSeconds;
            if (interstitialUpdateIntervalCounter >= config.reloadIntervalSeconds || interstitialUpdateIntervalCounter < 0)
            {
                lastRequestInterstitialAdTime = DateTime.Now;
                IronSource.Agent.loadInterstitial();
            }
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
            _lastInterstitialAdClosedTime = DateTime.Now;
            AdNotificationCenter.Instance.InterstitialNotification.onInterstitialAdClosedEvent.Invoke(this.interstitialDTO);
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
