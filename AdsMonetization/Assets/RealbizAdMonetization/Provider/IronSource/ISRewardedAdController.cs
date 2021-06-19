
namespace RealbizGames.Ads
{
    public class ISRewardedAdController : IRewardedAd
    {
        private RewardedAdDTO rewardedAdDTO;
        private bool isCallRewarded = false;
        private bool isCallClose = false;

        public void Init()
        {
            //Invoked when the RewardedVideo ad view has opened.
            //Your Activity will lose focus. Please avoid performing heavy 
            //tasks till the video ad will be closed.
            IronSourceEvents.onRewardedVideoAdOpenedEvent += onRewardedVideoAdOpenedEvent;
            //Invoked when the RewardedVideo ad view is about to be closed.
            //Your activity will now regain its focus.
            IronSourceEvents.onRewardedVideoAdClosedEvent += onRewardedVideoAdClosedEvent;
            //Invoked when there is a change in the ad availability status.
            //@param - available - value will change to true when rewarded videos are available. 
            //You can then show the video by calling showRewardedVideo().
            //Value will change to false when no videos are available.
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += onRewardedVideoAvailabilityChangedEvent;
            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------

            //Invoked when the video ad starts playing. 
            IronSourceEvents.onRewardedVideoAdStartedEvent += onRewardedVideoAdStartedEvent;

            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------
            //Invoked when the video ad finishes playing. 
            IronSourceEvents.onRewardedVideoAdEndedEvent += onRewardedVideoAdEndedEvent;

            //Invoked when the user completed the video and should be rewarded. 
            //If using server-to-server callbacks you may ignore this events and wait for 
            // the callback from the  ironSource server.
            //@param - placement - placement object which contains the reward data
            IronSourceEvents.onRewardedVideoAdRewardedEvent += onRewardedVideoAdRewardedEvent;
            //Invoked when the Rewarded Video failed to show
            //@param description - string - contains information about the failure.
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += onRewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += onRewardedVideoAdClickedEvent;
        }

        public void ShowRewardedAd(RewardedAdDTO dto)
        {
            resetRewardCheckingState();
            this.rewardedAdDTO = dto;
            IronSource.Agent.showRewardedVideo();
        }

        public void Update()
        {
            if (isCallRewarded && isCallClose) {
                RewardedAdDTO dto = rewardedAdDTO;
                resetRewardCheckingState();
                AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdRewardedEvent.Invoke(dto);
            }
        }

        private void resetRewardCheckingState() {
            isCallRewarded = false;
            isCallClose = false;
            rewardedAdDTO = null;
        }

        public void Destroy()
        {
            //Invoked when the RewardedVideo ad view has opened.
            //Your Activity will lose focus. Please avoid performing heavy 
            //tasks till the video ad will be closed.
            IronSourceEvents.onRewardedVideoAdOpenedEvent -= onRewardedVideoAdOpenedEvent;
            //Invoked when the RewardedVideo ad view is about to be closed.
            //Your activity will now regain its focus.
            IronSourceEvents.onRewardedVideoAdClosedEvent -= onRewardedVideoAdClosedEvent;
            //Invoked when there is a change in the ad availability status.
            //@param - available - value will change to true when rewarded videos are available. 
            //You can then show the video by calling showRewardedVideo().
            //Value will change to false when no videos are available.
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= onRewardedVideoAvailabilityChangedEvent;
            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------

            //Invoked when the video ad starts playing. 
            IronSourceEvents.onRewardedVideoAdStartedEvent -= onRewardedVideoAdStartedEvent;

            // ----------------------------------------------------------------------------------------
            // Note: the events below are not available for all supported rewarded video ad networks. 
            // Check which events are available per ad network you choose to include in your build. 
            // We recommend only using events which register to ALL ad networks you include in your build. 
            // ----------------------------------------------------------------------------------------
            //Invoked when the video ad finishes playing. 
            IronSourceEvents.onRewardedVideoAdEndedEvent -= onRewardedVideoAdEndedEvent;

            //Invoked when the user completed the video and should be rewarded. 
            //If using server-to-server callbacks you may ignore this events and wait for 
            // the callback from the  ironSource server.
            //@param - placement - placement object which contains the reward data
            IronSourceEvents.onRewardedVideoAdRewardedEvent -= onRewardedVideoAdRewardedEvent;
            //Invoked when the Rewarded Video failed to show
            //@param description - string - contains information about the failure.
            IronSourceEvents.onRewardedVideoAdShowFailedEvent -= onRewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent -= onRewardedVideoAdClickedEvent;
        }

        // ---------------------------------------------------------------------------------
        #region Rewarded Ads Callbacks
        private void onRewardedVideoAvailabilityChangedEvent(bool isAvailableToShow)
        {
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAvailabilityChangedEvent.Invoke(isAvailableToShow);
        }

        private void onRewardedVideoAdShowFailedEvent(IronSourceError e)
        {
            RewardedFailedToShowDTO dto = new RewardedFailedToShowDTO(code: e.getErrorCode().ToString(), message: e.getDescription());
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdShowFailedEvent.Invoke(dto);
        }

        private void onRewardedVideoAdOpenedEvent()
        {
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdOpenedEvent.Invoke();
        }

        private void onRewardedVideoAdStartedEvent()
        {
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdStartedEvent.Invoke();
        }


        private void onRewardedVideoAdRewardedEvent(IronSourcePlacement ironSourcePlacement)
        {
            isCallRewarded = true;
            // AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdRewardedEvent.Invoke();
        }

        private void onRewardedVideoAdClosedEvent()
        {
            isCallClose = true;
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdClosedEvent.Invoke();
        }

        private void onRewardedVideoAdEndedEvent()
        {
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdEndedEvent.Invoke();
        }

        private void onRewardedVideoAdClickedEvent(IronSourcePlacement placement)
        {
            // isCallRewarded = true;
            AdNotificationCenter.Instance.RewardedNotification.onRewardedVideoAdClickedEvent.Invoke();
        }

        #endregion
    }
}