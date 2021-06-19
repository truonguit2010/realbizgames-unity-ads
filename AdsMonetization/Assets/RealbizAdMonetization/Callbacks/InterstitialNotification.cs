using UnityEngine.Events;

namespace RealbizGames.Ads
{
    [System.Serializable]
    public class OnInterstitialAdReadyEvent : UnityEvent { };

    [System.Serializable]
    public class OnInterstitialAdLoadFailedEvent : UnityEvent<InterstitialFailedToLoadDTO> { };

    [System.Serializable]
    public class OnInterstitialAdShowSucceededEvent : UnityEvent { };

    [System.Serializable]
    public class OnInterstitialAdShowFailedEvent : UnityEvent<InterstitialFailedToShowDTO> { };

    [System.Serializable]
    public class OnInterstitialAdClickedEvent : UnityEvent { };

    [System.Serializable]
    public class OnInterstitialAdOpenedEvent : UnityEvent { };

    [System.Serializable]
    public class OnInterstitialAdClosedEvent : UnityEvent { };

    public class InterstitialNotification
    {
        /// <summary>
        /// Invoked when the Interstitial is Ready to shown after load function is called
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdReadyEvent onInterstitialAdReadyEvent = new OnInterstitialAdReadyEvent();

        /// <summary>
        /// Invoked when the initialization process has failed.
        /// @param description - string - contains information about the failure.
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdLoadFailedEvent onInterstitialAdLoadFailedEvent = new OnInterstitialAdLoadFailedEvent();

        /// <summary>
        /// Invoked right before the Interstitial screen is about to open.
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdShowSucceededEvent onInterstitialAdShowSucceededEvent = new OnInterstitialAdShowSucceededEvent();

        /// <summary>
        /// Invoked when the ad fails to show.
        /// @param description - string - contains information about the failure.
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdShowFailedEvent onInterstitialAdShowFailedEvent = new OnInterstitialAdShowFailedEvent();

        /// <summary>
        /// Invoked when end user clicked on the interstitial ad
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdClickedEvent onInterstitialAdClickedEvent = new OnInterstitialAdClickedEvent();

        /// <summary>
        /// Invoked when the Interstitial Ad Unit has opened
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdOpenedEvent onInterstitialAdOpenedEvent = new OnInterstitialAdOpenedEvent();

        /// <summary>
        /// Invoked when the interstitial ad closed and the user goes back to the application screen.
        /// </summary>
        /// <returns></returns>
        public readonly OnInterstitialAdClosedEvent onInterstitialAdClosedEvent = new OnInterstitialAdClosedEvent();
    }
}
