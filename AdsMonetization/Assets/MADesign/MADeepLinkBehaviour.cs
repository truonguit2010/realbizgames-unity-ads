using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
#if UNITY_ANDROID
using Google.Play.Review;
#endif

namespace MADesign
{
    [System.Serializable]
    public class CallRatingAndReviewEvent : UnityEvent { }

    [System.Serializable]
    public class CallOpenPopupEvent : UnityEvent <string> { }

    // https://docs.unity3d.com/Manual/enabling-deep-linking.html
    [DisallowMultipleComponent]
    public class MADeepLinkBehaviour : MonoBehaviour
    {
        public const string TAG = "MADeepLinkBehaviour";

        public const string DEEP_LINK_RATING_CONTAINS = "rating_the_game";

        public const string PARAM_ENABLE_BANNER_AD = "bannerad";
        public const string PARAM_ON_SCREEN_MESSAGE = "osm";

        public const string PARAM_ON_TEST_INTERSTITIAL_AD = "interstitial";
        public const string PARAM_ON_TEST_REWARDED_AD = "rewarded";

        public const string PARAM_ON_THROWS_EXCEPTION = "exception";
        public const string PARAM_CRASH = "crash";

        public const int SHOW_HAND_MALE = 1;
        public const int SHOW_HAND_FEMALE = 2;
        public const int SHOW_HAND_CARTOON = 3;

        public static MADeepLinkBehaviour Instance { get; private set; }

        [SerializeField]
        private string editorTestDeepLink = "tetridoku://play_script_1?showHand=1&b=asdghj";
        public string deeplinkURL;
        public Dictionary<string, string> deeplinkParams = new Dictionary<string, string>();

        public string linkAction;
        public int showHand;

        // Some interfaces options
        public CallRatingAndReviewEvent callRatingAndReviewEvent = new CallRatingAndReviewEvent();

        [SerializeField]
        private bool catchRating = true;

#if UNITY_ANDROID
        private ReviewManager _reviewManager;
#endif

        public bool isPlayScriptDeepLink
        {
            get
            {
                return !string.IsNullOrEmpty(linkAction) && linkAction.StartsWith("play_script", StringComparison.Ordinal);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Debug.LogFormat("{0} - deeplink Awake int {1}", TAG, Application.absoluteURL);

                Instance = this;
                Application.deepLinkActivated += onDeepLinkActivated;
                if (!string.IsNullOrEmpty(Application.absoluteURL))
                {
                    // Cold start and Application.absoluteURL not null so process Deep Link.
                    onDeepLinkActivated(Application.absoluteURL);
                }
                // Initialize DeepLink Manager global variable.
                else deeplinkURL = "[none]";
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(this.editorTestDeepLink))
            {
                onDeepLinkActivated(this.editorTestDeepLink);
            }
#endif
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
            }
            else {
                StartCoroutine(checkDeepLinkByMe());
            }
        }

        private IEnumerator checkDeepLinkByMe() {
            Debug.LogFormat("{0} - deeplink checkDeepLinkByMe Start {1}", TAG, Application.absoluteURL);
            yield return new WaitForSeconds(3);
            Debug.LogFormat("{0} - deeplink checkDeepLinkByMe After Wait {1}", TAG, Application.absoluteURL);
        }

        public void printDebug()
        {
            Debug.LogFormat("{0} - deeplink: {1}", TAG, this.deeplinkURL);
            Debug.LogFormat("{0} - string_query params: {1}", TAG, parseDictionaryToString(deeplinkParams));
        }

        public void cleanDeepLinkInformation()
        {
            deeplinkURL = string.Empty;
            linkAction = string.Empty;
            deeplinkParams = new Dictionary<string, string>();
        }

        public int getIntValueFromParams(string key, int defaultValue)
        {
            if (deeplinkParams != null && deeplinkParams.ContainsKey(key))
            {
                return int.Parse(deeplinkParams[key]);
            }
            return defaultValue;
        }

        public string getStringValueFromParams(string key, string defaultValue)
        {
            if (deeplinkParams != null && deeplinkParams.ContainsKey(key))
            {
                return deeplinkParams[key];
            }
            return defaultValue;
        }

        // ---------------------------------------------------------------------
        // ---------------------------------------------------------------------
        // Delay time
        // ---------------------------------------------------------------------
        public float getDelayTime {
            get {
                string va = getStringValueFromParams("delay", "0");
                return float.Parse(va);
            }
        }

        public bool isShowPopupDeeplink {
            get {
                return linkAction == "show_popup";
            }
        }

        public string popupId {
            get {
                return getStringValueFromParams("popup", string.Empty);
            }
        }

        public bool enableBannerAd {
            get {
                return getStringValueFromParams(PARAM_ENABLE_BANNER_AD, string.Empty) == "yes";
            }
        }

        public bool notEnableBannerAd
        {
            get
            {
                return getStringValueFromParams(PARAM_ENABLE_BANNER_AD, string.Empty) == "no";
            }
        }

        public string onScreenMessage {
            get {
                return getStringValueFromParams(PARAM_ON_SCREEN_MESSAGE, string.Empty);
            }
        }

        public bool enableTestInterstitial {
            get
            {
                return getStringValueFromParams(PARAM_ON_TEST_INTERSTITIAL_AD, string.Empty) == "yes";
            }
        }

        public bool enableTestRewarded
        {
            get
            {
                return getStringValueFromParams(PARAM_ON_TEST_REWARDED_AD, string.Empty) == "yes";
            }
        }


        public bool enableException
        {
            get
            {
                return getStringValueFromParams(PARAM_ON_THROWS_EXCEPTION, string.Empty) == "yes";
            }
        }

        public bool enableCrash
        {
            get
            {
                return getStringValueFromParams(PARAM_CRASH, string.Empty) == "yes";
            }
        }


        //throw new System.Exception("test exception please ignore");
        public void onDeepLinkActivated(string url)
        {
            Debug.LogFormat("{0} - deeplink onDeepLinkActivated {1}", TAG, url);

            MATrackingFunctions.tracking_DeepLink(url);
            // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
            deeplinkURL = url;
            deeplinkParams = GetParams(deeplinkURL);

            showHand = getIntValueFromParams("showHand", 0);

            if (deeplinkURL.Contains("?"))
            {
                int indexStart = deeplinkURL.IndexOf("://", 0, StringComparison.Ordinal) + 3;
                int indexEnd = deeplinkURL.IndexOf("?", 0, StringComparison.Ordinal);
                int length = indexEnd - indexStart;
                linkAction = deeplinkURL.Substring(indexStart, length);
            }
            else
            {
                int indexStart = deeplinkURL.IndexOf("://", 0, StringComparison.Ordinal) + 3;
                int indexEnd = deeplinkURL.Length;
                int length = indexEnd - indexStart;
                linkAction = deeplinkURL.Substring(indexStart, length);
            }

//#if UNITY_EDITOR
            printDebug();
//#endif

            if (deeplinkURL.Contains(DEEP_LINK_RATING_CONTAINS))
            {
                if (catchRating)
                {
#if UNITY_IOS
                    UnityEngine.iOS.Device.RequestStoreReview();
#elif UNITY_ANDROID
                    // Android review code come from here.
                    // https://developer.android.com/guide/playcore/in-app-review/unity
                    StartCoroutine(StartAndroidReviewFlow());
#endif
                }
                callRatingAndReviewEvent.Invoke();
            }

            if (enableException) {
                StartCoroutine(throwsException());
            }

            if (enableCrash) {
                StartCoroutine(crashGame());
            }
        }

        private IEnumerator throwsException() {
            yield return new WaitForSeconds(2);
            throw new System.Exception("test exception please ignore");
        }

        private IEnumerator crashGame()
        {
            yield return new WaitForSeconds(2);
            string a = null;
            a.ToString();
        }

#if UNITY_ANDROID
        IEnumerator StartAndroidReviewFlow()
        {
            _reviewManager = new ReviewManager();
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            PlayReviewInfo playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;

            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }

        }
#endif

        static public Dictionary<string, string> GetParams(string uri)
        {
            var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            var keyValues = new Dictionary<string, string>(matches.Count);
            foreach (Match m in matches)
                keyValues.Add(Uri.UnescapeDataString(m.Groups[2].Value), Uri.UnescapeDataString(m.Groups[3].Value));

            return keyValues;
        }

        public static string parseDictionaryToString(Dictionary<string, string> keyValuePairs)
        {
            string returnedString = "{";
            foreach (var keyValuePair in keyValuePairs)
            {
                returnedString += keyValuePair.Key + ":" + keyValuePair.Value + ",";
            }
            returnedString += "}";
            return returnedString;
        }
    }
}
