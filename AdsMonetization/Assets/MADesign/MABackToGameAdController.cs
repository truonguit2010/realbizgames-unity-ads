

using UnityEngine;
using System.Collections;
using System;
using Firebase.RemoteConfig;
using UnityEngine.Events;

namespace MADesign
{
    // -----------------------------------------------------------------------------
    [System.Serializable]
    public class ShowBackToGameAdEvent : UnityEvent { }

    public class MABackToGameAdController : MonoBehaviour
    {

        // -------------------------------------------------------------------------
        // PARAMS ĐỂ XỬ LÝ CHO RESUME ADS.
        // -------------------------------------------------------------------------
        private DateTime interstitialOrRewardedClosedTime = DateTime.Now;
        private DateTime leaveGameDateTime = DateTime.Now;

        private bool leaveGameByBannerAd = false;
        private DateTime leaveGameByBannerTime = DateTime.Now;

        // -------------------------------------------------------------------------
        // Callback events.
        // -------------------------------------------------------------------------
        [SerializeField]
        private ShowBackToGameAdEvent _showBackToGameAdEvent = new ShowBackToGameAdEvent();

        // Use this for initialization
        void Start()
        {
            DateTime epochAgo = DateTime.Now.AddMinutes(-20);
            interstitialOrRewardedClosedTime = epochAgo;
            leaveGameDateTime = epochAgo;
            leaveGameByBannerTime = epochAgo;
        }

        // Update is called once per frame
        // void Update()
        // {
        // 
        // }

        // -------------------------------------------------------------------------
        // Call function này khi user click vào banner.
        // -------------------------------------------------------------------------
        public void onLeaveGameByBannerAd()
        {
            leaveGameByBannerAd = true;
            leaveGameByBannerTime = DateTime.Now;
        }

        // -------------------------------------------------------------------------
        // Call function này khi interstitial or rewarded as is closed.
        // -------------------------------------------------------------------------
        public void onInterstitialOrRewardedAdClosed()
        {
            //
            interstitialOrRewardedClosedTime = DateTime.Now;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                leaveGameDateTime = System.DateTime.Now;
            }
            else
            {
                StartCoroutine(HandleOnApplicationResumeCoroutine());
            }
        }

        // Hiện tại chỉ dùng coroutine để fix trường hợp event rewarded|interstitial ad close sau khi hàm OnApplicationPause được call.
        // Mình làm điều này để support cho cả Admob và Ironsource.
        // Flow Admob: Ad sẽ call close trước khi application call OnApplicationPause
        // Flow IronSource: Application sẽ call OnApplicationPause trước khi ad call close
        private IEnumerator HandleOnApplicationResumeCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            yield return new WaitForEndOfFrame();

            //double leaveAppIntervalInMinutes = System.DateTime.Now.Subtract(leaveGameDateTime).TotalMinutes;
            double leaveAppIntervalInSeconds = System.DateTime.Now.Subtract(leaveGameDateTime).TotalSeconds;
            double leaveGameByBannerAdInMinutes = DateTime.Now.Subtract(leaveGameByBannerTime).TotalMinutes;
            double adClosedToNowInSeconds = DateTime.Now.Subtract(interstitialOrRewardedClosedTime).TotalSeconds;
            int openApplicationCount = increaseOpenApplicationCount();

            bool adClosedAllowShowResumeAd = adClosedToNowInSeconds < -5 || adClosedToNowInSeconds > 5;
            bool appOpenCountAllowShowAd = openApplicationCount > MAFirebaseRemoteConfig.gameOpenCountAllowShowResumeAd;
            bool bannerAdAllowShowResumeAd = !leaveGameByBannerAd || (leaveGameByBannerAd && leaveGameByBannerAdInMinutes > 3);
            bool configIntervalAllowShowResumeAd = leaveAppIntervalInSeconds >= MAFirebaseRemoteConfig.resumeAdRequiredIntervalInSeconds;
            bool allowShowResumeAd = MAFirebaseRemoteConfig.allowShowResumeAd;

            leaveGameByBannerAd = false;

            bool aa = adClosedAllowShowResumeAd && appOpenCountAllowShowAd && bannerAdAllowShowResumeAd && configIntervalAllowShowResumeAd && allowShowResumeAd;

            MATrackingFunctions.trackingBack2GameAd_Active(adClosedAllowShowResumeAd, appOpenCountAllowShowAd, bannerAdAllowShowResumeAd, configIntervalAllowShowResumeAd, allowShowResumeAd);

            if (aa)
            {
                callShowInterstitialAd();
            }

        }

        private void callShowInterstitialAd()
        {
            if (this._showBackToGameAdEvent != null)
            {
                _showBackToGameAdEvent.Invoke();
            }
        }

        // -------------------------------------------------------------------------
        // Utilility functions.
        // -------------------------------------------------------------------------
        private const string KEY_OPEN_APPLICATION_COUNT = "KEY_OPEN_APPLICATION_COUNT_BY_BACK_TO_GAME_AD";
        public static int increaseOpenApplicationCount()
        {
            int v = PlayerPrefs.GetInt(KEY_OPEN_APPLICATION_COUNT, 0);
            v++;
            PlayerPrefs.SetInt(KEY_OPEN_APPLICATION_COUNT, v);
            PlayerPrefs.Save();
            return v;
        }
    }
}

