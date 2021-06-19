using UnityEngine;
using System.Collections.Generic;
//using Facebook.Unity;

namespace MADesign
{

    //FirebaseAnalytics.LogEvent(nameEvent, parameters.ToArray());
    //AppsFlyer.trackRichEvent(eventName, events);
    //FB.LogAppEvent(appEventName, valueToSum, parameters);//using Facebook.Unity;
    //AnalyticManager.Instance.CustomeEvent(nameEvent, eventData: (System.Collections.Generic.IDictionary<string, object>) events);// unity
    public class MATrackingImplement : IMATrackingImplement
    {
        const string TAG = "MATrackingImplement";

        public void doTracking(string eventName, Dictionary<string, object> parameters) {

#if UNITY_EDITOR
            string logParam = "";
            foreach (KeyValuePair<string, object> kvp in parameters)
            {
                logParam += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value) + "\n";
            }

            if (string.IsNullOrEmpty(logParam)) {
                logParam = "empty params";
            }

            Debug.LogFormat("{0} - doTrackingImplement {1} - {2}", TAG, eventName, logParam);
#endif

            List<Firebase.Analytics.Parameter> firebaseParameters = new List<Firebase.Analytics.Parameter>();
            Dictionary<string, string> stringDict = new Dictionary<string, string>();
            foreach (var pair in parameters)
            {
                firebaseParameters.Add(new Firebase.Analytics.Parameter(pair.Key, pair.Value.ToString()));
                stringDict[pair.Key] = pair.Value.ToString();
            }

            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, firebaseParameters.ToArray());

            // ------------------------------------------------------------------
            // For Facebook
            // ------------------------------------------------------------------
   //         if (FB.IsInitialized) { 
			//	FB.LogAppEvent(eventName, 1, parameters);
			//}

            // ------------------------------------------------------------------
            // For Unity
            // ------------------------------------------------------------------
            UnityEngine.Analytics.Analytics.CustomEvent(eventName, eventData: parameters);// unity

            // ------------------------------------------------------------------
            // For Appsflyer
            // ------------------------------------------------------------------
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, stringDict);
        }

        //Firebase.Analytics.FirebaseAnalytics.SetUserProperty("favorite_food", "ice cream");
        public void setUserProperty(string key, string value)
        {
#if UNITY_EDITOR
            Debug.LogFormat("{0} - setUserProperty {1} - {2}", TAG, key, value);
#endif
            Firebase.Analytics.FirebaseAnalytics.SetUserProperty(key, value);
        }
    }

    public class YourGameTrackingImplement : MonoBehaviour
    {
        const string TAG = "TetrisDokuTrackingImplement";

        [SerializeField]
        private bool initAsAwake = true;

        private void Awake()
        {
            if (initAsAwake) {
                this.implement();
            }
        }

        public void implement()
        {
            Debug.LogFormat("{0} - implement", TAG);

            MATrackingImplement mATrackingImplement = new MATrackingImplement();
            YourGameTrackingFunctions.trackingImplement = mATrackingImplement;
            MATrackingFunctions.trackingImplement = mATrackingImplement;
        }
    }
}