using UnityEngine;
using Firebase.RemoteConfig;

namespace MADesign
{
    public class MAFirebaseRemoteConfig
    {

        public static string getFirebaseRemoteConfigString(string key, string defaultValue = "")
        {
            string v = FirebaseRemoteConfig.GetValue(key).StringValue;
            return string.IsNullOrEmpty(v) ? defaultValue : v;
        }

        public static int parseIntFromRemoteConfiguration(string key, int defaultValue)
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

        public static bool parseBoolFromRemoteConfiguration(string key, string expectedValue, bool defaultValue)
        {
            string stringValue = getFirebaseRemoteConfigString(key);
            if (!string.IsNullOrEmpty(stringValue))
            {
                stringValue = stringValue.Trim();
                return stringValue.Equals(expectedValue);
            }
            else
            {
                return defaultValue;
            }
        }

        #region Back to Game Ad
        //-----------------------------------------------------------------------------------------
        // Thời gian resume ads bắt buộc phải có giữa 2 lần show ads. disable=-1
        //-----------------------------------------------------------------------------------------
        public static int resumeAdRequiredIntervalInSeconds
        {
            get
            {
                return parseIntFromRemoteConfiguration("resume_ad_required_interval_in_seconds", -1);
            }
        }

        public static bool allowShowResumeAd
        {
            get
            {
                return resumeAdRequiredIntervalInSeconds >= 0;
            }
        }

        public static bool notAllowShowResumeAd
        {
            get
            {
                return !allowShowResumeAd;
            }
        }

        //-----------------------------------------------------------------------------------------
        // App open count control show resume ads.
        //-----------------------------------------------------------------------------------------

        public static int gameOpenCountAllowShowResumeAd
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("game_open_count_allow_show_resume_ad", 1);
                return Mathf.Max(a, 1);
            }
        }
        #endregion

        //-----------------------------------------------------------------------------------------
        // Thời gian giữa 2 lần request ads, cái này sử dụng cho hệ thống cũ, version 1.0.6 trở về trước.
        //-----------------------------------------------------------------------------------------
        public static int GetTimeDelayRequestAd()
        {
            int a = parseIntFromRemoteConfiguration("time_delay_request_ads", 30);
            return Mathf.Max(a, 10);
        }

        //-----------------------------------------------------------------------------------------
        // Thời gian giữa 2 lần request banner ads, interstitial ads, and rewarded ads.
        //-----------------------------------------------------------------------------------------
        public static int requestBannerAdIntervalInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("request_banner_ad_interval_in_seconds", 40);
                return Mathf.Max(a, 10);
            }
        }

        public static int requestBannerAdIntervalWhenFailedInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("request_banner_ad_interval_when_failed_in_seconds", 20);
                return Mathf.Max(a, 10);
            }
        }

        public static int requestInterstitialAdIntervalInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("request_interstitial_ad_interval_in_seconds", 30);
                return Mathf.Max(a, 10);
            }
        }

        public static int requestRewardedAdIntervalInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("request_rewarded_ad_interval_in_seconds", 30);
                return Mathf.Max(a, 10);
            }
        }

        //-----------------------------------------------------------------------------------------
        // Check xem có đang cho phép mở adbreak hay không?
        //-----------------------------------------------------------------------------------------
        public static bool isEnableAdBreak
        {
            get
            {
                return parseBoolFromRemoteConfiguration("is_enable_adbreak", "ENABLE", true);
            }
        }

        public static bool isNOTEnableAdBreak
        {
            get
            {
                return !isEnableAdBreak;
            }
        }

        //-----------------------------------------------------------------------------------------
        // Adbreak effect time
        //-----------------------------------------------------------------------------------------
        public static float adbreakEffectTimeInSeconds
        {
            get
            {
                float a = parseIntFromRemoteConfiguration("adbreak_effect_time_in_milliseconds", 2000);
                return a / 1000.0f;
            }
        }

        //-----------------------------------------------------------------------------------------
        // Config interstitial ad required interval
        //-----------------------------------------------------------------------------------------
        public static int interstitialAdRequiredIntervalInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("interstitial_ad_required_interval_in_seconds", 180);
                return Mathf.Max(a, 5);
            }
        }

        //-----------------------------------------------------------------------------------------
        // Config rewarded ad reset purpose interval
        //-----------------------------------------------------------------------------------------
        public static int rewardedAdResetPurposeIntervalInSeconds
        {
            get
            {
                int a = parseIntFromRemoteConfiguration("rewarded_ad_reset_purpose_interval_in_seconds", 20);
                return Mathf.Max(a, 1);
            }
        }

        //-----------------------------------------------------------------------------------------
        // Get privacy policy url.
        //-----------------------------------------------------------------------------------------
        public const string KEY_PRIVACY_POLICY_URL = "privacy_policy_url";

        public static string getPrivacyPolicyUrl(string defaultValue)
        {
            var aa = getFirebaseRemoteConfigString(KEY_PRIVACY_POLICY_URL);
            return string.IsNullOrEmpty(aa) ? defaultValue : aa;
        }
        //-----------------------------------------------------------------------------------------
        // Get native share url.
        //-----------------------------------------------------------------------------------------
        public const string KEY_NATIVE_SHARE_URL = "native_share_url";

        public static string getNativeShareUrl(string defaultValue)
        {
            var aa = getFirebaseRemoteConfigString(KEY_NATIVE_SHARE_URL);
            return string.IsNullOrEmpty(aa) ? defaultValue : aa;
        }
    }
}