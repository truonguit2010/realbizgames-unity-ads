using UnityEngine;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace MADesign
{
    [System.Serializable]
    public class OnShowAdbreakEvent : UnityEvent { }

    public enum AdBreakState
    {
        None,
        Runing,
        Pause,
        Stop
    }


    // https://www.newtonsoft.com/json/help/html/DeserializeObject.htm
    public class MAAdBreakByTimeController : MonoBehaviour
    {
        public const string TAG = "AdBreakByTimeController";

        /// <summary>
        /// Một mảng intervals
        /// Example: [240, 240, 420, 600]
        /// </summary>
        private const string KEY_FIREBASE_ADBREAK_INTERVALS = "adbreaks_intervals";
        /// <summary>
        /// Value: ENABLE or something else.
        /// </summary>
        private const string KEY_FIREBASE_ADBREAK_ENABLE = "adbreak_enable";

        private const string VALUE_ADBREAK_ENABLED = "TIME";

        //--------------------------------------------------------------------------
        // Configurations
        //--------------------------------------------------------------------------
        [SerializeField]
        private int[] local_adbreak_intervals = { 240, 240, 420, 600 };
        [SerializeField]
        private int[] configuration_adbreak_intervals = new int[0];
        [SerializeField]
        private int[] editor_adbreak_intervals = { 10, 20, 15};

        [SerializeField]
        private bool editorEnableAdBreak = true;

        //--------------------------------------------------------------------------
        // Thuật toán variables.
        //--------------------------------------------------------------------------
        [SerializeField]
        private int adbreakIndex = 0;
        [SerializeField]
        private float adbreakInterval = -1;

        [SerializeField]
        private float adBreakTimeCounter;

        [SerializeField]
        private bool isPlayingTutorial = false;
        // -------------------------------------------------------------------------
        // Quản lý state của Adbreak như start, pause, stop.
        // -------------------------------------------------------------------------
        [SerializeField]
        private AdBreakState adBreakState = AdBreakState.None;

        // -------------------------------------------------------------------------
        // Callback events.
        // -------------------------------------------------------------------------
        public OnShowAdbreakEvent onShowAdbreakEvent = new OnShowAdbreakEvent();

        // -------------------------------------------------------------------------
        // Unity Behaviour State Management
        // -------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckShowInterstitialAdWithAdBreaker();
        }

        // -------------------------------------------------------------------------
        // Logic Ad break start from here!!!!!
        // -------------------------------------------------------------------------

        public string getFirebaseRemoteConfigString(string key)
        {
            return FirebaseRemoteConfig.GetValue(key).StringValue;
        }

        public void initAdBreakConfiguration()
        {
            try
            {
                var json = getFirebaseRemoteConfigString(KEY_FIREBASE_ADBREAK_INTERVALS);
                if (!string.IsNullOrEmpty(json))
                {
                    configuration_adbreak_intervals = JsonConvert.DeserializeObject<int[]>(json);
                    Debug.Log("@LOG time_adbreaks:" + json + "/ Length:" + configuration_adbreak_intervals.Length);
                }
                else
                {
                    // Tính ra intervals
                    configuration_adbreak_intervals = this.local_adbreak_intervals;
                }

            }
            catch
            {
                // Tính ra intervals
                configuration_adbreak_intervals = this.local_adbreak_intervals;
            }
#if UNITY_EDITOR
            configuration_adbreak_intervals = this.editor_adbreak_intervals;
#endif
        }

        public bool isAdBreakEnabled
        {
            get
            {
#if UNITY_EDITOR
                return this.editorEnableAdBreak;
#else
                return VALUE_ADBREAK_ENABLED.Equals(getFirebaseRemoteConfigString(KEY_FIREBASE_ADBREAK_ENABLE));
#endif
            }
        }


        public void onGameStart(bool isPlayingTutorial = false)
        {
#if UNITY_EDITOR
            Debug.LogFormat("{0} - onGameStart {1}", TAG, isPlayingTutorial);
#endif
            initAdBreakConfiguration();
            this.isPlayingTutorial = isPlayingTutorial;
            adbreakIndex = -1;
            nextAdBreakTime();
            this.adBreakState = AdBreakState.Runing;
        }

        public void nextAdBreakTime(bool resetTimeCounter = true)
        {

            // Next index
            adbreakIndex++;
            if (adbreakIndex >= configuration_adbreak_intervals.Length)
            {
                adbreakIndex = configuration_adbreak_intervals.Length - 1;
            }
            else if (adbreakIndex < 0)
            {
                adbreakIndex = 0;
            }
            // Next Value.
            if (adbreakIndex < configuration_adbreak_intervals.Length)
            {
                adbreakInterval = configuration_adbreak_intervals[adbreakIndex];
            }
            else
            {
                adbreakInterval = -1;
            }
            // Reset time counter
            if (resetTimeCounter)
            {
                adBreakTimeCounter = 0;
            }
        }

        public void callShowAdbreak()
        {
            Debug.LogFormat("{0} - callShowAdbreak", TAG);
            this.onShowAdbreakEvent.Invoke();

        }

        public void CheckShowInterstitialAdWithAdBreaker()
        {
            if (this.adBreakState == AdBreakState.Runing)
            {
                if (adbreakInterval > 0 && !isPlayingTutorial)
                {
                    this.adBreakTimeCounter += Time.deltaTime;
                    if (this.adBreakTimeCounter >= adbreakInterval)
                    {
                        nextAdBreakTime(resetTimeCounter: true);
                        if (isAdBreakEnabled)
                        {
                            callShowAdbreak();
                        }
                    }
                }
            }
        }

        public void onGameEnd() {
#if UNITY_EDITOR
            Debug.LogFormat("{0} - onGameEnd", TAG);
#endif
            this.adBreakState = AdBreakState.Stop;
            adbreakIndex = -1;
            adbreakInterval = -1;
        }

        public void onGamePause() {
#if UNITY_EDITOR
            Debug.LogFormat("{0} - onGamePause", TAG);
#endif
            if (this.adBreakState == AdBreakState.Runing)
            {
                this.adBreakState = AdBreakState.Pause;
            }
        }

        public void onGameResume() {
#if UNITY_EDITOR
            Debug.LogFormat("{0} - onGameResume", TAG);
#endif
            if (this.adBreakState == AdBreakState.Pause)
            {
                this.adBreakState = AdBreakState.Runing;
            }
        }
    }
}