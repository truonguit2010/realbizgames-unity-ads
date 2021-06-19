using System.Collections.Generic;

namespace MADesign
{
    public interface IMATrackingImplement
    {
        void doTracking(string eventName, Dictionary<string, object> parameters);

        void setUserProperty(string key, string value);
    }

    public class MATrackingFunctions
    {
        private const string TAG = "MATrackingFunctions";

        public static IMATrackingImplement trackingImplement;

        public static void doTrackingImplement(string eventName, Dictionary<string, object> parameters)
        {
            if (trackingImplement != null)
            {
                trackingImplement.doTracking(eventName, parameters);
            }
        }

        public static void doSetUserProperty(string key, string v)
        {
            if (trackingImplement != null)
            {
                trackingImplement.setUserProperty(key, v);
            }
        }

        #region Banner Ads
        public static void trackingBannerAd_CallRequest()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("banner_ad_request", dataDic);
        }

        public static void trackingBannerAd_ShowBanner()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("banner_ad_show", dataDic);
        }

        public static void trackingBannerAd_ShowBannerERROR(string errorType = null)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["type"] = string.IsNullOrEmpty(errorType) ? "UNKNOW" : errorType;
            doTrackingImplement("show_banner_ads_error", dataDic);
        }

        public static void trackingBannerAd_HideBanner()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("banner_ad_hide", dataDic);
        }

        public static void trackingBannerAd_OnAdFailedToLoad_1(string provider, string msg, int loadNewAdInterval)
        {
            string message = string.IsNullOrEmpty(msg) ? "IsNullOrEmpty" : msg;
            string shortMessage = message.Length > 40 ? message.Substring(0, 40) : message;

            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["type"] = "banner";
            dataDic["msg"] = message;
            dataDic["short_msg"] = shortMessage;
            dataDic["delay_to_reload"] = loadNewAdInterval.ToString();

            doTrackingImplement("banner_ad_failed_to_load", dataDic);
        }

        public static void trackingBannerAd_OnAdLoaded_2(string provider)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;

            doTrackingImplement("banner_ad_loaded", dataDic);
        }

        public static void trackingBannerAd_OnAdOpening_3(string provider)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;

            doTrackingImplement("banner_ad_opening", dataDic);
        }

        public static void trackingBannerAd_OnPaidEvent_4(string provider, string CurrencyCode, long Value)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["currency"] = CurrencyCode;
            dataDic["value"] = Value;

            doTrackingImplement("banner_ad_paid", dataDic);
        }

        public static void trackingBannerAd_OnAdLeavingApplication_5(string provider)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;

            doTrackingImplement("banner_ad_leaving_application", dataDic);
        }
        #endregion

        #region Interstitial Ads

        public static void trackingInterstitialAd_ShowInterstitial_Kick(string source)
        {
            Dictionary<string, object> dataDicTotal = new Dictionary<string, object>();
            dataDicTotal["source"] = source;

            doTrackingImplement("interstitial_ad_kick", dataDicTotal);
        }

        public static void trackingInterstitialAd_ShowInterstitial_Show(string source, int interstitialAdRequiredIntervalInSeconds = 0)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["interstitialAdRequiredIntervalInSeconds"] = interstitialAdRequiredIntervalInSeconds.ToString();
            doTrackingImplement("interstitial_ad_show", dataDic);
        }

        public static void trackingInterstitialAd_ShowInterstitial_Show_Error(string source, string errorType = "")
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["type"] = string.IsNullOrEmpty(errorType) ? "Unknow" : errorType;

            doTrackingImplement("interstitial_ad_show_error", dataDic);
        }

        public static void trackingInterstitialAd_RequestInterstitial(bool _allowShowInterstitialWhenLoaded)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["type"] = "interstitial";
            dataDic["show_when_loaded"] = _allowShowInterstitialWhenLoaded.ToString();

            doTrackingImplement("interstitial_ad_request", dataDic);
        }

        public static void trackingInterstitialAd_OnAdLoaded_1(string provider, bool showInterstitialAdOnLoaded)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["show_interstitial_ad_on_loaded"] = showInterstitialAdOnLoaded.ToString();

            doTrackingImplement("interstitial_ad_loaded", dataDic);
        }

        public static void trackingInterstitialAd_OnAdFailedToLoad_2(string provider, string msg, int _timeDelayRequestAdWhenFailed)
        {
            string message = string.IsNullOrEmpty(msg) ? "IsNullOrEmpty" : msg;
            string shortMessage = message.Length > 40 ? message.Substring(0, 40) : message;

            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["type"] = "interstitial";
            dataDic["msg"] = message;
            dataDic["short_msg"] = shortMessage;
            dataDic["delay_to_reload"] = _timeDelayRequestAdWhenFailed.ToString();

            doTrackingImplement("interstitial_ad_failed_to_load", dataDic);
        }

        public static void trackingInterstitialAd_OnAdClosed_3(string provider, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["source"] = source;

            doTrackingImplement("interstitial_ad_closed", dataDic);
        }

        public static void trackingInterstitialAd_OnAdOpening_4(string provider, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["source"] = source;

            doTrackingImplement("interstitial_ad_opening", dataDic);
        }

        public static void trackingInterstitialAd_OnAdPaidEvent_5(string provider, string CurrencyCode, long Value, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["currency"] = CurrencyCode;
            dataDic["value"] = Value.ToString();
            dataDic["source"] = source;

            doTrackingImplement("interstitial_ad_paid", dataDic);
        }

        public static void trackingInterstitialAd_OnAdLeavingApplication_6(string provider, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["source"] = source;

            doTrackingImplement("interstitial_ad_leaving_application", dataDic);
        }
        #endregion

        #region Reward Ads
        public static void trackingRewardedAd_RequestRewardVideo(bool force = false)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("rewarded_ad_request", dataDic);
        }

        public static void trackingRewardedAd_ShowRewardVideo_Kick(string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;

            doTrackingImplement("rewarded_ad_kick", dataDic);
        }

        public static void trackingRewardedAd_ShowRewardVideo_Show(string source, bool forceResetShowRewardType = false)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["forceResetShowRewardType"] = forceResetShowRewardType.ToString();

            doTrackingImplement("rewarded_ad_show", dataDic);
        }

        public static void trackingRewardedAd_ShowRewardVideo_Show_ERROR(string source, string errorType = "Empty")
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["errorType"] = errorType;

            doTrackingImplement("rewarded_ad_show_error", dataDic);
        }

        public static void trackingRewardedAd_OnAdLoaded_1(string provider)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;

            doTrackingImplement("rewarded_ad_loaded", dataDic);
        }

        public static void trackingRewardedAd_OnAdFailedToLoad_2(string provider, string msg, int _timeDelayRequestAdWhenFailed)
        {
            string message = string.IsNullOrEmpty(msg) ? "IsNullOrEmpty" : msg;
            string shortMessage = message.Length > 40 ? message.Substring(0, 40) : message;

            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["type"] = "rewarded";
            dataDic["msg"] = message;
            dataDic["short_msg"] = shortMessage;
            dataDic["delay_to_reload"] = _timeDelayRequestAdWhenFailed.ToString();

            doTrackingImplement("rewarded_ad_failed_to_load", dataDic);
        }

        public static void trackingRewardedAd_OnAdFailedToShow_3(string provider, string msg, int _timeDelayRequestAdWhenFailed, string errorCode)
        {
            string message = string.IsNullOrEmpty(msg) ? "IsNullOrEmpty" : msg;
            string shortMessage = message.Length > 40 ? message.Substring(0, 40) : message;

            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["msg"] = message;
            dataDic["short_msg"] = shortMessage;
            dataDic["delay_to_reload"] = _timeDelayRequestAdWhenFailed.ToString();
            dataDic["type"] = "rewarded";
            dataDic["error_code"] = string.IsNullOrEmpty(errorCode) ? "null_or_empty" : errorCode;

            doTrackingImplement("rewarded_ad_failed_to_show", dataDic);
        }

        public static void trackingRewardedAd_OnAdOpening_4(string provider, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["source"] = source;

            doTrackingImplement("rewarded_ad_opening", dataDic);
        }

        public static void trackingRewardedAd_OnUserEarnedReward_5(string provider, string source, bool _isRewardClosed)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["provider"] = provider;
            param["source"] = source;
            param["is_ad_closed"] = _isRewardClosed.ToString();

            doTrackingImplement("rewarded_ad_earned_reward", param);
        }

        public static void trackingRewardedAd_OnAdClosed_6(string provider, string source)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["provider"] = provider;
            param["source"] = source;

            doTrackingImplement("rewarded_ad_closed", param);
        }

        public static void trackingRewardedAd_OnPaidEvent_7(string provider, string CurrencyCode, long Value, string source)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["provider"] = provider;
            dataDic["currency"] = CurrencyCode;
            dataDic["value"] = Value.ToString();
            dataDic["source"] = source;

            doTrackingImplement("rewarded_ad_paid", dataDic);
        }

        public static void trackingRewardedAd_HandleRewardCode(string source)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["source"] = source;
            doTrackingImplement("rewarded_ad_reward_user", param);
        }

        public static void trackingRewardedAd_HandleRewardCodeCoroutine(string source)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["source"] = source;
            doTrackingImplement("rewarded_ad_reward_user_in_coroutine", param);
        }
        #endregion

        #region Common functions.
        public static void tracking_DeepLink(string deeplink) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["deep_link"] = deeplink;
            doTrackingImplement("deep_link_receive", param);
        }
        #endregion

        public static void trackingBack2GameAd_Active(bool adClosedAllowShowResumeAd, bool appOpenCountAllowShowAd, bool bannerAdAllowShowResumeAd, bool configIntervalAllowShowResumeAd, bool allowShowResumeAd)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["adClosedAllowShowResumeAd"] = adClosedAllowShowResumeAd;
            param["appOpenCountAllowShowAd"] = appOpenCountAllowShowAd;
            param["bannerAdAllowShowResumeAd"] = bannerAdAllowShowResumeAd;
            param["configIntervalAllowShowResumeAd"] = configIntervalAllowShowResumeAd;
            param["allowShowResumeAd"] = allowShowResumeAd;
            doTrackingImplement("back_to_game_ad", param);
        }

        // ---------------------------------------------------------------------
        // ---------------------------------------------------------------------
        // User property
        // ---------------------------------------------------------------------
        public static void setUserLevel(int level)
        {
            doSetUserProperty("user_reach_level", level.ToString());
        }

        public static void setUserSoundSetting(bool isOn)
        {
            doSetUserProperty("user_sound_setting", isOn ? "on" : "off");
        }

        public static void setUserMusicSetting(bool isOn)
        {
            doSetUserProperty("user_music_setting", isOn ? "on" : "off");
        }

        public static void setUserBuyRemoveAd()
        {
            doSetUserProperty("user_buy_remove_ads", "1");
        }

        public static void setUserBuyIAP()
        {
            doSetUserProperty("user_buy_iap", "1");
        }
    }
}
