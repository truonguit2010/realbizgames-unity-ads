using UnityEngine.Events;

namespace RealbizGames.Ads
{
    [System.Serializable]
    public class OnBannerAdLoadFailedEvent : UnityEvent<BannerFailedToLoadDTO> {}
    
    [System.Serializable]
    public class OnBannerAdLoadedEvent : UnityEvent {}

    [System.Serializable]
    public class OnBannerAdClickedEvent : UnityEvent {}

    [System.Serializable]
    public class OnBannerAdLeftApplicationEvent : UnityEvent {}

    public class BannerNotification
    {
        public OnBannerAdLoadFailedEvent onBannerAdLoadFailedEvent = new OnBannerAdLoadFailedEvent();

        public OnBannerAdLoadedEvent onBannerAdLoadedEvent = new OnBannerAdLoadedEvent();

        public OnBannerAdClickedEvent onBannerAdClickedEvent = new OnBannerAdClickedEvent();

        public OnBannerAdLeftApplicationEvent onBannerAdLeftApplicationEvent = new OnBannerAdLeftApplicationEvent();


        public void NotifyLoadFailed(BannerFailedToLoadDTO dto) {
            onBannerAdLoadFailedEvent.Invoke(dto);
        }

        public void NotifyLoadedEvent() {
            onBannerAdLoadedEvent.Invoke();
        }

        public void NotifyAdClickEvent() {
            onBannerAdClickedEvent.Invoke();
        }

        public void NotifyLeftApplicationEvent() {
            onBannerAdLeftApplicationEvent.Invoke();
        }
    }
}