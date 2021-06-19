
using UnityEngine;
using UnityEngine.Events;

namespace MADesign
{
    public enum LoadingShowInterstitialState
    {
        None,
        Runing,
        Finished
    }

    [System.Serializable]
    public class DoShowInterstitialAdEvent : UnityEvent<GameObject> { }

    // -----------------------------------------------------------------------------
    // Support show interstital
    // -----------------------------------------------------------------------------
    public class MALoadingShowInterstitialController : MonoBehaviour
    {
        // -------------------------------------------------------------------------
        // Logic variables
        // -------------------------------------------------------------------------
        [SerializeField]
        private float timeToDelayInSeconds = 5;

        [SerializeField]
        private float timeCounter = 0;

        [SerializeField]
        private LoadingShowInterstitialState loadingShowInterstitialState = LoadingShowInterstitialState.None;

        // -------------------------------------------------------------------------
        // Callback events.
        // -------------------------------------------------------------------------
        [SerializeField]
        private DoShowInterstitialAdEvent _doShowInterstitialAdEvent = new DoShowInterstitialAdEvent();

        // -------------------------------------------------------------------------
        // Gameobject will disable or destroy after the ad closed.
        // -------------------------------------------------------------------------
        [SerializeField]
        private GameObject _adControledGameObject;

        public GameObject adControledGameObject
        {
            get
            {
                return this._adControledGameObject == null ? this.gameObject : this._adControledGameObject;
            }
        }

        // Use this for initialization
        //void Start()
        //{
        //
        //}

        private void OnEnable()
        {
            this.startScheduled();
        }

        // Update is called once per frame
        void Update()
        {
            if (loadingShowInterstitialState == LoadingShowInterstitialState.Runing)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= timeToDelayInSeconds)
                {
                    loadingShowInterstitialState = LoadingShowInterstitialState.Finished;
                    if (_doShowInterstitialAdEvent != null)
                    {
                        _doShowInterstitialAdEvent.Invoke(this.adControledGameObject);
                    }
                }
            }
        }


        // -------------------------------------------------------------------------
        // Logic code come from here.
        // -------------------------------------------------------------------------
        public void startScheduled()
        {
            this.timeCounter = 0;
            this.loadingShowInterstitialState = LoadingShowInterstitialState.Runing;
        }
    }

}
