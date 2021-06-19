using UnityEngine.Events;

namespace RealbizGames.Ads
{
    [System.Serializable]
    public class OnRewardedVideoAdOpenedEvent : UnityEvent { }

    [System.Serializable]
    public class OnRewardedVideoAdClosedEvent : UnityEvent { }
    [System.Serializable]
    public class OnRewardedVideoAvailabilityChangedEvent : UnityEvent<bool> { }
    [System.Serializable]
    public class OnRewardedVideoAdStartedEvent : UnityEvent { }
    [System.Serializable]
    public class OnRewardedVideoAdEndedEvent : UnityEvent { }
    [System.Serializable]
    public class OnRewardedVideoAdRewardedEvent : UnityEvent<RewardedAdDTO> { }
    [System.Serializable]
    public class OnRewardedVideoAdShowFailedEvent : UnityEvent<RewardedFailedToShowDTO> { }
    [System.Serializable]
    public class OnRewardedVideoAdClickedEvent : UnityEvent { }

    public class RewardedNotification
    {
        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        public readonly OnRewardedVideoAdOpenedEvent onRewardedVideoAdOpenedEvent = new OnRewardedVideoAdOpenedEvent();
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        public readonly OnRewardedVideoAdClosedEvent onRewardedVideoAdClosedEvent = new OnRewardedVideoAdClosedEvent();
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        public readonly OnRewardedVideoAvailabilityChangedEvent onRewardedVideoAvailabilityChangedEvent = new OnRewardedVideoAvailabilityChangedEvent();
        // ----------------------------------------------------------------------------------------
        // Note: the events below are not available for all supported rewarded video ad networks. 
        // Check which events are available per ad network you choose to include in your build. 
        // We recommend only using events which register to ALL ad networks you include in your build. 
        // ----------------------------------------------------------------------------------------

        //Invoked when the video ad starts playing. 
        public readonly OnRewardedVideoAdStartedEvent onRewardedVideoAdStartedEvent = new OnRewardedVideoAdStartedEvent();

        // ----------------------------------------------------------------------------------------
        // Note: the events below are not available for all supported rewarded video ad networks. 
        // Check which events are available per ad network you choose to include in your build. 
        // We recommend only using events which register to ALL ad networks you include in your build. 
        // ----------------------------------------------------------------------------------------
        //Invoked when the video ad finishes playing. 
        public readonly OnRewardedVideoAdEndedEvent onRewardedVideoAdEndedEvent = new OnRewardedVideoAdEndedEvent();

        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for 
        // the callback from the  ironSource server.
        //@param - placement - placement object which contains the reward data
        public readonly OnRewardedVideoAdRewardedEvent onRewardedVideoAdRewardedEvent = new OnRewardedVideoAdRewardedEvent();
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        public readonly OnRewardedVideoAdShowFailedEvent onRewardedVideoAdShowFailedEvent = new OnRewardedVideoAdShowFailedEvent();
        public readonly OnRewardedVideoAdClickedEvent onRewardedVideoAdClickedEvent = new OnRewardedVideoAdClickedEvent();
    }
}