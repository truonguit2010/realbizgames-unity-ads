using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MADesign
{
    [System.Serializable]
    public class OnUserEarnedRewardEvent : UnityEvent<string> { }
    [System.Serializable]
    public class OnCallShowRewardedAdFailedEvent : UnityEvent<string, string> { }

    [System.Serializable]
    public class OnInterstitialShowErrorEvent : UnityEvent<string, string> { }
    [System.Serializable]
    public class OnInterstitialClosedEvent : UnityEvent<string> { }

    [System.Serializable]
    public class OnBannerAdForceClosedEvent : UnityEvent { }

    [System.Serializable]
    public class OnBannerAdForceShowedEvent : UnityEvent { }

    [System.Serializable]
    public class OnUserUseRemoveAdsEvent : UnityEvent { }

    [DisallowMultipleComponent]
    public class MAAdController : MonoBehaviour, IGlobalAdCallbackInterface
    {
        private const string TAG = "MAAdController";

        public const string PROVIDER_ADMOB = "admob";
        public const string PROVIDER_IRONSOURCE = "ironsource";

        // -------------------------------------------------------------------------
        // DEFINE Error code come from here.
        // -------------------------------------------------------------------------
        public const string ERROR_REWARDED_AD_IS_SHOWED = "REWARDED_AD_IS_SHOWED";
        public const string ERROR_REWARDED_AD_IS_NOT_READY_TO_SHOW = "REWARDED_AD_NOT_READY_TO_SHOW";
        public const string ERROR_REWARDED_AD_FAILED_TO_SHOW = "REWARDED_AD_FAILED_TO_SHOW";
        public const string ERROR_REWARDED_AD_PURPOSE_IS_NULL = "ERROR_REWARDED_AD_PURPOSE_IS_NULL";

        public const string ERROR_INTERSTITIAL_NOT_MATCH_INTERSTITIAL_INTERVAL_REQUIRED = "NOT_MATCH_INTERSTITIAL_INTERVAL_REQUIRED";
        public const string ERROR_INTERSTITIAL_BUY_REMOVE_AD = "BUY_REMOVE_AD";
        public const string ERROR_INTERSTITIAL_AD_NOT_LOADED = "AD_NOT_LOADED";
        public const string ERROR_INTERSTITIAL_UNKNOW = "UNKNOW";

        // -------------------------------------------------------------------------
        // COMMON
        // -------------------------------------------------------------------------
        [SerializeField]
        private MAIronSourceController ironSourceController;

        public IGlobalAdAPIInterface globalAdAPIInterface;


        // -------------------------------------------------------------------------
        // CONFIG ON/OFF Ad Format.
        // -------------------------------------------------------------------------
        [SerializeField]
        private bool _enableBanner = true;

        [SerializeField]
        private bool _enableInterstitial = true;

        [SerializeField]
        private bool _enableRewarded = true;

        public bool enableBannerInGlobal
        {
            get
            {
                return _enableBanner && MAPlayerPrefController.IsNotRemoveAds;
            }
            set {
                if (!_enableBanner && value)
                {
                    RequestBanner();
                    if (this.onBannerAdForceShowedEvent != null) {
                        this.onBannerAdForceShowedEvent.Invoke();
                    }
                }
                else if (!value) {
                    HideBanner();
                }
                _enableBanner = value;
            }
        }

        public bool enableInterstitialInGlobal
        {
            get
            {
                return _enableInterstitial && MAPlayerPrefController.IsNotRemoveAds;
            }
        }

        // -------------------------------------------------------------------------
        // CHECK REWARD CHO USERS.
        // -------------------------------------------------------------------------
        private bool _isRewardedAd_RewardUserEventTrigger = false;
        private bool _isRewardedAd_ClosedEventtrigger = false;

        // -------------------------------------------------------------------------
        // FIX BUG REWARDED ADS.
        // -------------------------------------------------------------------------
        private DateTime callShowRewardedTime = DateTime.Now;
        private int rewardedAdResetPurposeIntervalInSeconds = 20;

        // -------------------------------------------------------------------------
        // PARAMS CONTROL INTERSTITIAL ADS.
        // -------------------------------------------------------------------------
        private bool _allowShowInterstitialWhenLoaded = false;

        // -------------------------------------------------------------------------
        // CHECK XEM USER ĐANG XEM INTERSTITIAL OR REWARDED ĐỂ LÀM GÌ?
        // -------------------------------------------------------------------------
        private string showRewardedAdPurpose = string.Empty;
        private string showInterstitialAdPurpose = string.Empty;

        // -------------------------------------------------------------------------
        // Check time giữa 2 lần show interstitial ad hay giữa lên show rewarded ad với interstitial ad.
        // -------------------------------------------------------------------------
        private DateTime interstitialOrRewardedClosedTime = DateTime.Now;
        public int interstitialAdRequiredIntervalInSeconds = 0;
        // -------------------------------------------------------------------------
        // PARAMS ĐỂ XỬ LÝ REQUEST ADS.
        // -------------------------------------------------------------------------
        public int requestBannerAdIntervalInSeconds = 40;
        public int requestInterstitialAdIntervalInSeconds = 30;
        public int requestRewardedAdIntervalInSeconds = 30;

        private DateTime lastRequestInterstitialAdTime = DateTime.Now;
        private DateTime lastRequestRewardedAdTime = DateTime.Now;
        private DateTime lastRequestBannerAdTime = DateTime.Now;

        private double bannerUpdateIntervalCounter;
        private double interstitialUpdateIntervalCounter;
        private double rewardedUpdateIntervalCounter;

        private float updateRemoteConfigurationTimeCounter = 0;

        // -------------------------------------------------------------------------
        // EDITOR
        // -------------------------------------------------------------------------
        [Header("Test rewarded ad flow.")]
        [SerializeField]
        private bool _editorAdSuccess = true;
        private bool isEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        [SerializeField]
        private bool _editorRewardedAdAvailable = true;

        // -------------------------------------------------------------------------
        // Attached Components
        // -------------------------------------------------------------------------
        [SerializeField]
        private MABackToGameAdController backToGameAdController;

        // -------------------------------------------------------------------------
        // Callback events.
        // -------------------------------------------------------------------------
        public OnUserEarnedRewardEvent onUserEarnedRewardEvent = new OnUserEarnedRewardEvent();
        public OnCallShowRewardedAdFailedEvent onCallShowRewardedAdFailedEvent = new OnCallShowRewardedAdFailedEvent();
        public OnInterstitialShowErrorEvent onInterstitialShowErrorEvent = new OnInterstitialShowErrorEvent();
        public OnInterstitialClosedEvent onInterstitialClosedEvent = new OnInterstitialClosedEvent();
        public OnBannerAdForceClosedEvent onBannerAdForceClosedEvent = new OnBannerAdForceClosedEvent();
        public OnBannerAdForceShowedEvent onBannerAdForceShowedEvent = new OnBannerAdForceShowedEvent();
        public OnUserUseRemoveAdsEvent onUserUseRemoveAdsEvent = new OnUserUseRemoveAdsEvent();
        // ------------------------------------------------------------------------- 
        // Singleton params.
        // -------------------------------------------------------------------------
        private static MAAdController _instance;
        public static MAAdController Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);

                lastRequestBannerAdTime = DateTime.Now;
                lastRequestInterstitialAdTime = DateTime.Now;
                lastRequestRewardedAdTime = DateTime.Now;

                this.InitAdNetwork();
            }
            else
            {
                Destroy(this.gameObject);
            }
        } 

        public static void userBuyVIPRemoveAdPackage() {
            MAPlayerPrefController.SetRemoveAds();
            if (Instance != null)
            {
                Instance.HideBanner();
                Instance.onBannerAdForceClosedEvent.Invoke();
            }
        }

        public static bool isUserBuyRemoveAdPackage() {
            return MAPlayerPrefController.IsRemoveAds;
        }

        // Dùng hàm update loop call request ad để trong bất cứ tình huống nào mình cũng sẽ không bị die ad.
        private void Update()
        {
            updateRemoteConfigurationTimeCounter += Time.deltaTime;
            if (updateRemoteConfigurationTimeCounter > 20)
            {
                updateRemoteConfigurationTimeCounter = 0;
                UpdateRemoteConfiguration();
            }

            bannerUpdateIntervalCounter = DateTime.Now.Subtract(lastRequestBannerAdTime).TotalSeconds;
            if (bannerUpdateIntervalCounter >= requestBannerAdIntervalInSeconds || bannerUpdateIntervalCounter < 0)
            {
                // Có trường hợp nhỏ hơn 0 khi user cheat time.

                // Update remote config data theo banner interval.
                lastRequestBannerAdTime = DateTime.Now;
                RequestBanner(true);
            }

            interstitialUpdateIntervalCounter = DateTime.Now.Subtract(lastRequestInterstitialAdTime).TotalSeconds;
            if (interstitialUpdateIntervalCounter >= requestInterstitialAdIntervalInSeconds || interstitialUpdateIntervalCounter < 0)
            {
                lastRequestInterstitialAdTime = DateTime.Now;
                RequestInterstitial();
            }

            rewardedUpdateIntervalCounter = DateTime.Now.Subtract(lastRequestRewardedAdTime).TotalSeconds;
            if (rewardedUpdateIntervalCounter >= requestRewardedAdIntervalInSeconds || rewardedUpdateIntervalCounter < 0)
            {
                lastRequestRewardedAdTime = DateTime.Now;
                RequestRewardVideo();
            }
        }

        private void UpdateRemoteConfiguration()
        {
            requestBannerAdIntervalInSeconds = MAFirebaseRemoteConfig.requestBannerAdIntervalInSeconds;
            requestInterstitialAdIntervalInSeconds = MAFirebaseRemoteConfig.requestInterstitialAdIntervalInSeconds;
            requestRewardedAdIntervalInSeconds = MAFirebaseRemoteConfig.requestRewardedAdIntervalInSeconds;

            interstitialAdRequiredIntervalInSeconds = MAFirebaseRemoteConfig.interstitialAdRequiredIntervalInSeconds;
            rewardedAdResetPurposeIntervalInSeconds = MAFirebaseRemoteConfig.rewardedAdResetPurposeIntervalInSeconds;
        }

        private void InitAdNetwork()
        {
            this.globalAdAPIInterface = (IGlobalAdAPIInterface)ironSourceController;
            this.globalAdAPIInterface.InitAdNetwork(this);

            if (this.enableBannerInGlobal)
            {
                RequestBanner();
            }
            if (this.enableInterstitialInGlobal)
            {
                RequestInterstitial();
            }
            if (this._enableRewarded)
            {
                RequestRewardVideo();
            }
        }

        #region Banner
        private void RequestBanner(bool forceHideLastBannerAd = false)
        {
            if (forceHideLastBannerAd)
            {
                HideBanner();
            }
            if (enableBannerInGlobal)
            {
                if (this.globalAdAPIInterface != null)
                {
                    this.globalAdAPIInterface.RequestBanner_1();

                    MATrackingFunctions.trackingBannerAd_CallRequest();
                }
            }
        }

        public void ShowBanner()
        {
            if (this.enableBannerInGlobal)
            {
                if (globalAdAPIInterface != null)
                {
                    globalAdAPIInterface.ShowBanner_2();
                }
                MATrackingFunctions.trackingBannerAd_ShowBanner();
            }
            else
            {
                if (MAPlayerPrefController.IsRemoveAds)
                {
                    MATrackingFunctions.trackingBannerAd_ShowBannerERROR("BUY_REMOVE_AD");
                }
                else
                {
                    MATrackingFunctions.trackingBannerAd_ShowBannerERROR("UNKNOW");
                }
            }
        }

        public void HideBanner()
        {
            MATrackingFunctions.trackingBannerAd_HideBanner();
            if (globalAdAPIInterface != null)
            {
                globalAdAPIInterface.HideBanner_3();
            }
        }

        public void Banner_OnAdFailedToLoad_1(string provider, string msg)
        {
            MATrackingFunctions.trackingBannerAd_OnAdFailedToLoad_1(provider, msg, requestBannerAdIntervalInSeconds);

            this.requestBannerAdIntervalInSeconds = MAFirebaseRemoteConfig.requestBannerAdIntervalWhenFailedInSeconds;
        }

        public void Banner_OnAdLoaded_2(string provider)
        {
            MATrackingFunctions.trackingBannerAd_OnAdLoaded_2(provider);

            if (this.enableBannerInGlobal)
            {
                ShowBanner();
            }
        }

        public void Banner_OnAdOpening_3(string provider)
        {
            MATrackingFunctions.trackingBannerAd_OnAdOpening_3(provider);
        }

        public void Banner_OnPaidEvent_4(string provider, string CurrencyCode, long Value)
        {
            MATrackingFunctions.trackingBannerAd_OnPaidEvent_4(provider, CurrencyCode, Value);
        }

        public void Banner_OnAdLeavingApplication_5(string provider)
        {
            MATrackingFunctions.trackingBannerAd_OnAdLeavingApplication_5(provider);

            if (backToGameAdController != null)
            {
                backToGameAdController.onLeaveGameByBannerAd();
            }
        }
        #endregion

        #region Interstitial Ads

        public bool IsInterstitialAdsReady()
        {
            if (globalAdAPIInterface != null)
            {
                return globalAdAPIInterface.IsInterstitialAdsReady_1();
            }
            else
            {
                return false;
            }
        }

        

        public void ShowInterstitial(string _showInterstitialAdPurpose)
        {
            this.showInterstitialAdPurpose = _showInterstitialAdPurpose;

            MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Kick(_showInterstitialAdPurpose);

            if (IsInterstitialAdsReady() && enableInterstitialInGlobal)
            {
                double closedInterstitialOrRewardedToNowInSeconds = DateTime.Now.Subtract(interstitialOrRewardedClosedTime).TotalSeconds;
                if (closedInterstitialOrRewardedToNowInSeconds < interstitialAdRequiredIntervalInSeconds)
                {
                    MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Show_Error(showInterstitialAdPurpose, ERROR_INTERSTITIAL_NOT_MATCH_INTERSTITIAL_INTERVAL_REQUIRED);
                    this.onInterstitialShowErrorEvent.Invoke(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_NOT_MATCH_INTERSTITIAL_INTERVAL_REQUIRED);
                }
                else
                {
                    globalAdAPIInterface.ShowInterstitial_2(this.showInterstitialAdPurpose);

                    MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Show(showInterstitialAdPurpose, interstitialAdRequiredIntervalInSeconds);
                }
            }
            else
            {
                if (MAPlayerPrefController.IsRemoveAds)
                {
                    MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Show_Error(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_BUY_REMOVE_AD);
                    this.onInterstitialShowErrorEvent.Invoke(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_BUY_REMOVE_AD);
                }
                else if (IsInterstitialAdsReady() == false)
                {
                    MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Show_Error(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_AD_NOT_LOADED);
                    this.onInterstitialShowErrorEvent.Invoke(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_AD_NOT_LOADED);
                }
                else
                {
                    MATrackingFunctions.trackingInterstitialAd_ShowInterstitial_Show_Error(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_UNKNOW);
                    this.onInterstitialShowErrorEvent.Invoke(_showInterstitialAdPurpose, ERROR_INTERSTITIAL_UNKNOW);
                }
            }
        }

        private void DestroyInterstitialAdmob()
        {
            if (globalAdAPIInterface != null)
            {
                globalAdAPIInterface.DestroyInterstitialAd_3();
            }
        }

        private void RequestInterstitial(bool force = false, bool allowShowInterstitialWhenLoaded = false)
        {
            if (MAPlayerPrefController.IsRemoveAds) return;

            if (force)
            {
                DestroyInterstitialAdmob();
                lastRequestInterstitialAdTime = DateTime.Now;
            }

            if (IsInterstitialAdsReady())
            {
            }
            else
            {
                // Ad chưa ready thì request lại.
                if (globalAdAPIInterface != null)
                {
                    if (globalAdAPIInterface.IsRequestingInterstitial_5())
                    {

                    }
                    else
                    {
                        this._allowShowInterstitialWhenLoaded = allowShowInterstitialWhenLoaded;
                        globalAdAPIInterface.RequestInterstitial_4();
                    }
                    MATrackingFunctions.trackingInterstitialAd_RequestInterstitial(allowShowInterstitialWhenLoaded);
                }
            }
        }

        public void Interstitial_OnAdLoaded_1(string provider)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdLoaded_1(provider, _allowShowInterstitialWhenLoaded);

            if (_allowShowInterstitialWhenLoaded)
            {
                _allowShowInterstitialWhenLoaded = false;
                ShowInterstitial(this.showInterstitialAdPurpose);
            }
        }

        public void Interstitial_OnAdFailedToLoad_2(string provider, string msg, string adid)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdFailedToLoad_2(provider, msg, requestInterstitialAdIntervalInSeconds);

            // Delay to recall request interstitial ad.
            DestroyInterstitialAdmob();
        }

        public void Interstitial_OnAdClosed_3(string provider)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdClosed_3(provider, showInterstitialAdPurpose);

            // Request next ad
            RequestInterstitial(true);

            interstitialOrRewardedClosedTime = DateTime.Now;

            this.onInterstitialClosedEvent.Invoke(this.showInterstitialAdPurpose);

            if (backToGameAdController != null) {
                backToGameAdController.onInterstitialOrRewardedAdClosed();
            }
        }

        public void Interstitial_OnAdOpening_4(string provider)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdOpening_4(provider, showInterstitialAdPurpose);
        }

        public void Interstitial_OnAdPaidEvent_5(string provider, string CurrencyCode, long Value)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdPaidEvent_5(provider, CurrencyCode, Value, showInterstitialAdPurpose);
        }

        public void Interstitial_OnAdLeavingApplication_6(string provider)
        {
            MATrackingFunctions.trackingInterstitialAd_OnAdLeavingApplication_6(provider, showInterstitialAdPurpose);
        }
        #endregion

        #region Reward Ads
        private void DestroyRewardedAd()
        {
            if (globalAdAPIInterface != null)
            {
                globalAdAPIInterface.DestroyRewardedAd_4();
            }
        }

        private void RequestRewardVideo(bool force = false)
        {
            if (force)
            {
                DestroyRewardedAd();
                lastRequestRewardedAdTime = DateTime.Now;
            }

            if (IsRewardedAdReadyToShow())
            {
            }
            else
            {
                if (globalAdAPIInterface != null)
                {
                    if (globalAdAPIInterface.IsRequestingRewarded_5())
                    {
                        // None
                    }
                    else
                    {
                        globalAdAPIInterface.RequestRewardVideo_2();
                        MATrackingFunctions.trackingRewardedAd_RequestRewardVideo();
                    }
                }
                else {
                    Debug.LogErrorFormat("{0} - RequestRewardVideo globalAdAPIInterface == null", TAG);
                }
            }
        }

        public bool IsRewardedAdReadyToShow()
        {
            if (isEditor)
            {
                return _editorRewardedAdAvailable;
            }
            else if (globalAdAPIInterface != null)
            {
                return globalAdAPIInterface.IsRewardedAdsReady_1();
            }
            else
            {
                return false;
            }
        }



        public void ShowRewardVideo(string functionShowRewardedAdPurpose)
        {
//#if UNITY_EDITOR
            Debug.LogFormat("{0} - ShowRewardVideo {1} with current purpose {2}", TAG, functionShowRewardedAdPurpose, showRewardedAdPurpose);
//#endif
            MATrackingFunctions.trackingRewardedAd_ShowRewardVideo_Kick(functionShowRewardedAdPurpose);
            if (isEditor) {
                if (_editorAdSuccess)
                {
                    this.onUserEarnedRewardEvent.Invoke(functionShowRewardedAdPurpose);
                }
                else {
                    onCallShowRewardedAdFailedEvent.Invoke(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_IS_NOT_READY_TO_SHOW);
                }
            }
            else if (IsRewardedAdReadyToShow())
            {
                bool forceResetShowRewardType = false;
                double timeOutResetShowReward = System.DateTime.Now.Subtract(callShowRewardedTime).TotalSeconds;
                if (string.IsNullOrEmpty(functionShowRewardedAdPurpose))
                {
                    onCallShowRewardedAdFailedEvent.Invoke(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_PURPOSE_IS_NULL);
                    MATrackingFunctions.trackingRewardedAd_ShowRewardVideo_Show_ERROR(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_PURPOSE_IS_NULL);
                }
                else
                {
                    if (!string.IsNullOrEmpty(showRewardedAdPurpose) && timeOutResetShowReward >= this.rewardedAdResetPurposeIntervalInSeconds)
                    {
                        this.showRewardedAdPurpose = null;
                        forceResetShowRewardType = true;
                    }

                    if (string.IsNullOrEmpty(showRewardedAdPurpose))
                    {
                        showRewardedAdPurpose = functionShowRewardedAdPurpose;

                        _isRewardedAd_RewardUserEventTrigger = false;
                        _isRewardedAd_ClosedEventtrigger = false;

                        callShowRewardedTime = DateTime.Now;

                        globalAdAPIInterface.ShowRewardVideo_3(functionShowRewardedAdPurpose);

                        MATrackingFunctions.trackingRewardedAd_ShowRewardVideo_Show(functionShowRewardedAdPurpose, forceResetShowRewardType);
                    }
                    else
                    {
//#if UNITY_EDITOR
                        Debug.LogFormat("{0} - Cannot show rewarded Ad for {1} with error {2}", TAG, showRewardedAdPurpose, ERROR_REWARDED_AD_IS_SHOWED);
//#endif
                        onCallShowRewardedAdFailedEvent.Invoke(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_IS_SHOWED + "" + showRewardedAdPurpose);
                        //onCallShowRewardedAdFailedEvent.Invoke(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_IS_SHOWED);
                        MATrackingFunctions.trackingRewardedAd_ShowRewardVideo_Show_ERROR(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_IS_SHOWED);
                    }
                }
            }
            else
            {
                onCallShowRewardedAdFailedEvent.Invoke(functionShowRewardedAdPurpose, ERROR_REWARDED_AD_IS_NOT_READY_TO_SHOW);
                MATrackingFunctions.trackingRewardedAd_ShowRewardVideo_Show_ERROR(showRewardedAdPurpose, ERROR_REWARDED_AD_IS_NOT_READY_TO_SHOW);
            }
        }

        public void RewardedAd_OnAdLoaded_1(string provider)
        {
            MATrackingFunctions.trackingRewardedAd_OnAdLoaded_1(provider);
        }

        public void RewardedAd_OnAdFailedToLoad_2(string provider, string msg, string adId)
        {
            MATrackingFunctions.trackingRewardedAd_OnAdFailedToLoad_2(provider, msg, requestRewardedAdIntervalInSeconds);

            // Delay to recall request ads.
            DestroyRewardedAd();
        }

        public void RewardedAd_OnAdFailedToShow_3(string provider, string msg, string adId, string errorCode)
        {
            Time.timeScale = 1;

            onCallShowRewardedAdFailedEvent.Invoke(showRewardedAdPurpose, ERROR_REWARDED_AD_FAILED_TO_SHOW);

            showRewardedAdPurpose = null;

            MATrackingFunctions.trackingRewardedAd_OnAdFailedToShow_3(provider, msg, requestRewardedAdIntervalInSeconds, errorCode);

            // Delay and call request ads.
            DestroyRewardedAd();
        }

        public void RewardedAd_OnAdOpening_4(string provider)
        {
            MATrackingFunctions.trackingRewardedAd_OnAdOpening_4(provider, showRewardedAdPurpose);

            Time.timeScale = 0;
        }

        public void RewardedAd_OnUserEarnedReward_5(string provider)
        {
            _isRewardedAd_RewardUserEventTrigger = true;

            MATrackingFunctions.trackingRewardedAd_OnUserEarnedReward_5(provider, showRewardedAdPurpose, _isRewardedAd_ClosedEventtrigger);
        }

        public void RewardedAd_OnAdClosed_6(string provider)
        {

            _isRewardedAd_ClosedEventtrigger = true;
            Time.timeScale = 1;

            MATrackingFunctions.trackingRewardedAd_OnAdClosed_6(provider, showRewardedAdPurpose);

            // Handle reward or not/
            if (_isRewardedAd_RewardUserEventTrigger)
            {
                HandleRewardCode();
            }
            else
            {
                StartCoroutine(HandleRewardCodeCoroutine());
            }

            // Request new ad video.
            RequestRewardVideo(true);

            interstitialOrRewardedClosedTime = DateTime.Now;

            if (backToGameAdController != null)
            {
                backToGameAdController.onInterstitialOrRewardedAdClosed();
            }
        }

        public void RewardedAd_OnPaidEvent_7(string provider, string CurrencyCode, long Value)
        {
            MATrackingFunctions.trackingRewardedAd_OnPaidEvent_7(provider, CurrencyCode, Value, showRewardedAdPurpose);
        }

        private IEnumerator HandleRewardCodeCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            yield return new WaitForEndOfFrame();

            MATrackingFunctions.trackingRewardedAd_HandleRewardCodeCoroutine(showRewardedAdPurpose);

            HandleRewardCode();
        }

        private void HandleRewardCode()
        {
            if (_isRewardedAd_ClosedEventtrigger && _isRewardedAd_RewardUserEventTrigger)
            {
                _isRewardedAd_ClosedEventtrigger = false;
                _isRewardedAd_RewardUserEventTrigger = false;
                this.onUserEarnedRewardEvent.Invoke(this.showRewardedAdPurpose);
                this.showRewardedAdPurpose = null;
            }
        }
        #endregion
    }
}

