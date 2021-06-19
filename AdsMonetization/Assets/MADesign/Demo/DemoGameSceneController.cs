using System.Collections;
using UnityEngine;

namespace MADesign
{

    public class DemoGameSceneController : MonoBehaviour
    {
        private const string TAG = "DemoGameSceneController";
        private int _currentLevel = 0;

        void Start()
        {
            addIAPHandlers();
            startHandleLateIAPEnumerator();
        }

        private void OnDestroy()
        {
            removeIAPHandlers();
        }

        public void buyRemoveAd()
        {
            IAPManager.Instance.BuyProduct(IAP_REMOVE_ADS);
        }

        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // IAP handlers.
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        public const string IAP_REMOVE_ADS = "com.remove.ads";

        private void addIAPHandlers()
        {
            if (IAPManager.Instance != null)
            {
                IAPManager.Instance.purchaseSucceedEvent.AddListener(iap_PurchaseSucceedEvent);
                IAPManager.Instance.purchaseFailedEvent.AddListener(iap_PurchaseFailedEvent);
            }
        }

        private void removeIAPHandlers()
        {
            if (IAPManager.Instance != null)
            {
                IAPManager.Instance.purchaseSucceedEvent.RemoveListener(iap_PurchaseSucceedEvent);
                IAPManager.Instance.purchaseFailedEvent.RemoveListener(iap_PurchaseFailedEvent);
            }
        }

        // Handle case by case.
        private void iap_PurchaseSucceedEvent(string productId, string purchaseToke, string uk)
        {
            if (IAP_REMOVE_ADS == productId)
            {
                MATrackingFunctions.setUserBuyRemoveAd();
                MAAdController.userBuyVIPRemoveAdPackage();
                // Do some your nessesary logics HERE.
                // ......
            }
            else
            {

            }
        }

        private void iap_PurchaseFailedEvent(string productId, string reason)
        {
            Debug.LogErrorFormat("{0} - iap_PurchaseFailedEvent {1}", TAG, reason);
            YourGameTrackingFunctions.PlayScene_IAP_Purchase_Helper_Failed_Event(_currentLevel, reason: reason);
            // Implement more your logics HERE
        }


        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // Late IAP Handlers
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------

        // Call this function in Start()
        private void startHandleLateIAPEnumerator()
        {
            StartCoroutine(handleLateIAP());
        }

        private IEnumerator handleLateIAP()
        {
            yield return new WaitUntil(() => IAPManager.Instance != null && IAPManager.Instance.isSetupDone);
            checkAndHandleLateRemoveAds();
        }

        private void checkAndHandleLateRemoveAds()
        {
            if (IAPManager.Instance != null)
            {
                if (IAPManager.Instance.isGotProduct(IAP_REMOVE_ADS))
                {
                    // Already buy remove Ads, set to the core to remove Ads.
                    MAAdController.userBuyVIPRemoveAdPackage();
                }
                else
                {
                    // Not using remove ads package, We need to enable banner ads.
                    MAAdController.Instance.enableBannerInGlobal = true;
                }
            }
        }


        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // Show Interstitial Ad
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        private void showEndGameInterstitialAd()
        {
            MAAdController.Instance.ShowInterstitial("end_game_interstitial_ad");
        }

        private void showOpenSettingDialogInterstitialAd()
        {
            MAAdController.Instance.ShowInterstitial("open_setting_dialog_interstitial_ad");
        }

        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        // Rewarded Ads
        // ---------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------
        private const string REWARDED_AD_PLACEMENT_ONE_MORE_LIVE = "more_live";
        private const string REWARDED_AD_PLACEMENT_MORE_GOLD = "more_gold";

        // Call in Start()
        private void addAdListeners()
        {
            if (MAAdController.Instance != null)
            {
                MAAdController.Instance.onUserEarnedRewardEvent.AddListener(onUserEarnedRewardEvent);
                MAAdController.Instance.onCallShowRewardedAdFailedEvent.AddListener(onCallShowRewardedAdFailedEvent);
                //MAAdController.Instance.onInterstitialClosedEvent.AddListener(onInterstitialClosedEvent);
                //MAAdController.Instance.onInterstitialShowErrorEvent.AddListener(onInterstitialShowErrorEvent);
            }
        }
        // Call in OnDestroy()
        private void removeAdListeners()
        {
            if (MAAdController.Instance != null)
            {
                MAAdController.Instance.onUserEarnedRewardEvent.RemoveListener(onUserEarnedRewardEvent);
                MAAdController.Instance.onCallShowRewardedAdFailedEvent.RemoveListener(onCallShowRewardedAdFailedEvent);
            }
        }

        private void onUserEarnedRewardEvent(string placement)
        {
            if (placement == REWARDED_AD_PLACEMENT_ONE_MORE_LIVE)
            {
                // Add more live for the user
            }
            else if (placement == REWARDED_AD_PLACEMENT_MORE_GOLD)
            {
                // Add more gold for the user
            }
        }

        private void onCallShowRewardedAdFailedEvent(string placement, string reason)
        {
            // Fail to show rewarded ad for placement with reason.
            // Just log or custom tracking to know the issue.
        }

        public void showRewardedAdForOneMoreLive()
        {
            MAAdController.Instance.ShowRewardVideo(REWARDED_AD_PLACEMENT_ONE_MORE_LIVE);
        }

        public void showRewardedAdForMoreGold()
        {
            MAAdController.Instance.ShowRewardVideo(REWARDED_AD_PLACEMENT_MORE_GOLD);
        }

    }
}