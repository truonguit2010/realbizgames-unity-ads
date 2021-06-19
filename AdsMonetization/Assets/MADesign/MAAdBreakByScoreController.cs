using UnityEngine;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace MADesign
{
    [System.Serializable]
    public class ShowAdBreakByScoreEvent : UnityEvent { }

    public class MAAdBreakByScoreController : MonoBehaviour
    {

        public const string TAG = "AdBreakByScoreController";

        /// <summary>
        /// Một mảng intervals
        /// Example: [240, 240, 420, 600]
        /// </summary>
        private const string KEY_FIREBASE_ADBREAK_SCORES = "adbreaks_scores";
        /// <summary>
        /// Value: ENABLE or something else.
        /// </summary>
        private const string KEY_FIREBASE_ADBREAK_ENABLE = "adbreak_enable";

        private const string VALUE_ADBREAK_ENABLED = "SCORE";
        //--------------------------------------------------------------------------
        // Configurations
        //--------------------------------------------------------------------------
        [SerializeField]
        private int[] local_adbreak_scores = { 10, 20, 30, 40 };
        [SerializeField]
        private int[] configuration_adbreak_scores = new int[0];
        [SerializeField]
        private int[] editor_adbreak_scores = { 10, 20, 30, 40 };

        //--------------------------------------------------------------------------
        // Logic
        //--------------------------------------------------------------------------
        [SerializeField]
        private int currentScoreIndex = -1;
        [SerializeField]
        private int currentScoreTarget = 0;
        [SerializeField]
        private int currentScore = 0;

        // -------------------------------------------------------------------------
        // Callback events.
        // -------------------------------------------------------------------------
        [SerializeField]
        private ShowAdBreakByScoreEvent _showAdBreakByScoreEvent = new ShowAdBreakByScoreEvent();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //--------------------------------------------------------------------------
        // Configurations
        //--------------------------------------------------------------------------
        public string getFirebaseRemoteConfigString(string key)
        {
            return FirebaseRemoteConfig.GetValue(key).StringValue;
        }

        public void initAdBreakConfiguration()
        {
            try
            {
                var json = getFirebaseRemoteConfigString(KEY_FIREBASE_ADBREAK_SCORES);
                if (!string.IsNullOrEmpty(json))
                {
                    configuration_adbreak_scores = JsonConvert.DeserializeObject<int[]>(json);
                }
                else
                {
                    // Tính ra intervals
                    configuration_adbreak_scores = this.local_adbreak_scores;
                }
            }
            catch
            {
                // Tính ra intervals
                configuration_adbreak_scores = this.local_adbreak_scores;
            }
#if UNITY_EDITOR
            configuration_adbreak_scores = editor_adbreak_scores;
#endif
        }

        public bool isAdBreakEnabled
        {
            get
            {
                return VALUE_ADBREAK_ENABLED.Equals(getFirebaseRemoteConfigString(KEY_FIREBASE_ADBREAK_ENABLE));
            }
        }

        public int parseIntFromRemoteConfiguration(string key, int defaultValue)
        {
            try
            {
                string stringValue = getFirebaseRemoteConfigString(key);

                if (!string.IsNullOrEmpty(stringValue))
                {
                    stringValue = stringValue.Trim();
                    return int.Parse(stringValue);
                }
                else
                {
                    return defaultValue;
                }

            }
            catch
            {
                return defaultValue;
            }
        }

        //-----------------------------------------------------------------------------------------
        // Target Score step.
        //-----------------------------------------------------------------------------------------
        public int targetScoreStep
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("ad_break_target_score_step", 50);
                return Mathf.Max(a, 50);
            }
        }

        // -------------------------------------------------------------------------
        // Logic come from here.
        // -------------------------------------------------------------------------
        public void startPuzzle()
        {
            this.initAdBreakConfiguration();
            this.currentScoreIndex = -1;
            this.currentScore = 0;
            nextTargetScore();
        }

        private void nextTargetScore()
        {
            this.currentScoreIndex++;
            if (currentScoreIndex >= this.configuration_adbreak_scores.Length)
            {
                currentScoreTarget = currentScore + this.targetScoreStep;
            }
            else if (currentScoreIndex > -1)
            {
                currentScoreTarget = configuration_adbreak_scores[currentScoreIndex];
            }
        }

        private void showAdBreak()
        {
            if (_showAdBreakByScoreEvent != null)
            {
                _showAdBreakByScoreEvent.Invoke();
            }
        }

        // -------------------------------------------------------------------------
        // Call from game logic.
        // -------------------------------------------------------------------------
        public void onScoreUpdated(int score)
        {
            this.currentScore = score;
            if (this.currentScore >= this.currentScoreTarget)
            {
                nextTargetScore();
                showAdBreak();
            }
        }
    }
}
