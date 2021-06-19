using UnityEngine;
using UnityEngine.Events;

namespace MADesign
{
    public enum MAGameObjectControl
    {
        Disable,
        Destroy,
        Active,
        CallAction
    }

    [System.Serializable]
    public class CallActionEvent : UnityEvent<string> { }

    public class MAShowInterstitialAdBehaviour : MonoBehaviour
    {
        public const string TAG = "ShowInterstitialAdBehaviour";

        [SerializeField]
        private InterstitialAdShowType interstitialAdShowType = InterstitialAdShowType.None;

        [SerializeField]
        private MAGameObjectControl gameObjectControl;

        [SerializeField]
        private GameObject controledGameObject;

        [SerializeField]
        public CallActionEvent callActionEvent = new CallActionEvent();

        public void showInterstitialAd()
        {
            this.addListeners();
            MAAdController.Instance.ShowInterstitial(interstitialAdShowType.ToString());
        }

        private void addListeners()
        {
            MAAdController.Instance.onInterstitialShowErrorEvent.AddListener(onInterstitialShowErrorEvent);
            MAAdController.Instance.onInterstitialClosedEvent.AddListener(onInterstitialClosedEvent);
        }

        private void removeListeners()
        {
            MAAdController.Instance.onInterstitialShowErrorEvent.RemoveListener(onInterstitialShowErrorEvent);
            MAAdController.Instance.onInterstitialClosedEvent.RemoveListener(onInterstitialClosedEvent);
        }

        private void onInterstitialShowErrorEvent(string source, string errorType)
        {
            Debug.LogFormat("{0} - onInterstitialShowErrorEvent {1} - {2}", TAG, source, errorType);
            this.exeAfterInterstitialAdClosed();
        }

        private void onInterstitialClosedEvent(string source)
        {
            Debug.LogFormat("{0} - onInterstitialClosedEvent {1}", TAG, source);
            this.exeAfterInterstitialAdClosed();
        }

        private void exeAfterInterstitialAdClosed()
        {
            this.removeListeners();
            if (callActionEvent != null) {
                callActionEvent.Invoke(interstitialAdShowType.ToString());
            }
            if (this.controledGameObject != null)
            {
                if (this.gameObjectControl == MAGameObjectControl.Destroy)
                {
                    Destroy(controledGameObject);
                } else if (this.gameObjectControl == MAGameObjectControl.Active)
                {
                    controledGameObject.SetActive(true);
                }
                else
                {
                    controledGameObject.SetActive(false);
                }
            }
        }
    }
}
