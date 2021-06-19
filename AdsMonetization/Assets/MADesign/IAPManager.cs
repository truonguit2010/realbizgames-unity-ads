using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

// https://learn.unity.com/tutorial/unity-iap#5c7f8528edbc2a002053b46e
[DisallowMultipleComponent]
public class IAPManager : MonoBehaviour, IStoreListener
{
    const string TAG = "IAPManager";
    public enum IAPItemType
    {
        Consumable, NonConsumable, Subscription
    }

    [System.Serializable]
    public class IAPItem
    {
        public string _key;
        public string _name;
        public IAPItemType _type;
        public string _itemId;
        public float _price;
        public int _point;
        public float _bonusPointPercent;
        public bool _enable_iOS;
        public bool _enable_Android;
        public string _img_url;
        public string _img_url_en;
    }

    public class PurchaseSucceedEvent : UnityEngine.Events.UnityEvent<string, string, string> { };
    public class PurchaseFailedEvent : UnityEngine.Events.UnityEvent<string, string> { };

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    PurchaseSucceedEvent _purchaseSucceedEvent = new PurchaseSucceedEvent();
    PurchaseFailedEvent _purchaseFailedEvent = new PurchaseFailedEvent();

    public PurchaseSucceedEvent purchaseSucceedEvent {
        get {
            return _purchaseSucceedEvent;
        }
    }

    public PurchaseFailedEvent purchaseFailedEvent {
        get {
            return _purchaseFailedEvent;
        }
    }

    [HideInInspector]
    public List<IAPItem> _products = new List<IAPItem>();

    private Dictionary<string, Product> _initialAlreadyBoughtNonComsummableProducts;

    public static IAPManager Instance { get; private set;}

    [SerializeField]
    private bool _editorTestSuccess = true;

    private bool _isSetupDone = false;
    public bool isSetupDone {
        get {
            return _isSetupDone;
        }
    }

    /// <summary>
    /// Được xử dụng để test trường hợp lỗi IAP do chập chờn hay 1 lý do bất kì nào đó.
    /// </summary>
    private bool _pendingPurchaseSuccess = false;

    /// <summary>
    /// Được xử dụng để test trường hợp lỗi IAP do chập chờn hay 1 lý do bất kì nào đó.
    /// </summary>
    public bool pendingPurchaseSuccess {
        set {
            _pendingPurchaseSuccess = value;
        }
        get {
            return _pendingPurchaseSuccess;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }


    public void SetProducts(List<IAPItem> products)
    {
        _products.Clear();
        _products.AddRange(products);

#if UNITY_ANDROID || UNITY_IOS
        InitializePurchasing();
#endif
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            Debug.Log("InitializePurchasing has already!");
            return;
        }
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        /*builder.AddProduct(kProductIDConsumable, ProductType.Consumable);*/

        // Continue adding the non-consumable product.
        /*builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);*/

        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 

        foreach (IAPItem item in _products)
        {
            string itemId = item._itemId;

            if (!string.IsNullOrEmpty(itemId))
            {
                if (item._type == IAPItemType.Consumable)
                {
                    builder.AddProduct(itemId, ProductType.Consumable);
                }
                else if (item._type == IAPItemType.NonConsumable)
                {
                    builder.AddProduct(itemId, ProductType.NonConsumable);
                }
                else if(item._type == IAPItemType.Subscription)
                {
                    builder.AddProduct(itemId, ProductType.Subscription);
                }
            }

        }

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void SetPurchaseSucceedListener(UnityEngine.Events.UnityAction<string, string, string> listener)
    {
        _purchaseSucceedEvent.RemoveAllListeners();
        _purchaseSucceedEvent.AddListener(listener);
    }

    public void SetPurchaseFailedListener(UnityEngine.Events.UnityAction<string, string> listener)
    {
        _purchaseFailedEvent.RemoveAllListeners();
        _purchaseFailedEvent.AddListener(listener);
    }

    public List<IAPManager.IAPItem> GetProducts()
    {
        List<IAPItem> items = new List<IAPItem>();

        foreach (IAPItem item in _products)
        {
            bool enable = IsItemEnable(item);

            if (enable)
            {
                items.Add(item);
            }
        }

        return items;
    }

    public IAPItem GetItem(string productId)
    {
        foreach (IAPItem item in _products)
        {
            bool enable = IsItemEnable(item);
            string itemId = item._itemId;

            if (itemId == productId)
                return item;
        }

        return null;
    }

    public IAPItem GetItemByKey(string key)
    {
        foreach (IAPItem item in _products)
        {
            bool enable = IsItemEnable(item);
            string itemKey = item._key;

            if (itemKey == key)
                return item;
        }

        return null;
    }

    public bool isUnityEditor {
        get {
#if UNITY_EDITOR

            return true;
#else
            return false;
#endif
        }
    }

    public void BuyProduct(string productId)
    {
        if (isUnityEditor)
        {
            if (_editorTestSuccess)
            {
                _purchaseSucceedEvent.Invoke(productId, string.Empty, "0");
            }
            else {
                string reason = "Editor test IAP Fail";
                _purchaseFailedEvent.Invoke(productId, reason);
            }
        } else {
            foreach (IAPItem item in _products)
            {
                string itemId = item._itemId;

                if (!string.IsNullOrEmpty(itemId) && itemId == productId)
                {
                    //Debug.Log("Buy product name = " + item._name + ", type = " + item._type + ", id = " + itemId);

                    BuyProductID(itemId);

                    break;
                }
            }
        }
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public static bool IsItemEnable(IAPItem item)
    {
        bool enable = false;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            enable = item._enable_iOS;
        else if (Application.platform == RuntimePlatform.Android)
            enable = item._enable_Android;
#if UNITY_EDITOR
        else if (Application.platform == RuntimePlatform.OSXEditor)
            enable = item._enable_iOS;
        else if (Application.platform == RuntimePlatform.WindowsEditor)
            enable = item._enable_Android;
#endif

        return enable;
    }

    public bool isGotProduct(string productId) {
        if (_initialAlreadyBoughtNonComsummableProducts.ContainsKey(productId))
        {
            return true;
        }
        else {
            return false;
        }
    }

    //  
    // --- IStoreListener
    //
    IAppleExtensions m_AppleExtensions;
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

        _initialAlreadyBoughtNonComsummableProducts = new Dictionary<string, Product>();

        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

        foreach (var item in controller.products.all)
        {
            Debug.LogFormat("{0} - OnInitialized - product: {1} has receipt: {2}", TAG, item.definition.id, item.hasReceipt);
            if (item.availableToPurchase)
            {
                //Debug.Log(string.Join(" - ",
                //    new[]
                //    {
                //    item.metadata.localizedTitle,
                //    item.metadata.localizedDescription,
                //    item.metadata.isoCurrencyCode,
                //    item.metadata.localizedPrice.ToString(),
                //    item.metadata.localizedPriceString,
                //    item.transactionID,
                //    item.receipt
                //    }));


                // this is the usage of SubscriptionManager class
                if (item.receipt != null)
                {
                    if (item.definition.type == ProductType.Subscription)
                    {
                        if (checkIfProductIsAvailableForSubscriptionManager(item.receipt))
                        {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();
                            //Debug.Log("product id is: " + info.getProductId());
                            //Debug.Log("purchase date is: " + info.getPurchaseDate());
                            //Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            //Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            //Debug.Log("is expired? " + info.isExpired().ToString());
                            //Debug.Log("is cancelled? " + info.isCancelled());
                            //Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            //Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            //Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            //Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            //Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            //Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            //Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
                        }
                        else
                        {
                            //Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }

                        //MADesign.MAAdController.userBuyVIPRemoveAdPackage();
                    } else if (item.definition.type == ProductType.NonConsumable)
                    {
                        _initialAlreadyBoughtNonComsummableProducts[item.definition.id] = item;
                    }
                    else
                    {
                        Debug.Log("the product is not a subscription product");
                    }
                }
                else
                {
                    Debug.Log("the product should have a valid receipt");
                }
            }
        }

        _isSetupDone = true;
    }

    private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
    {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
        {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper["Store"];
        var payload = (string)receipt_wrapper["Payload"];

        if (payload != null)
        {
            switch (store)
            {
                case GooglePlay.Name:
                    {
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                            return false;
                        }
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                            return false;
                        }
                        var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                        var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                        if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                            return false;
                        }
                        return true;
                    }
                case AppleAppStore.Name:
                case AmazonApps.Name:
                case MacAppStore.Name:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        return false;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: Begin. Product: '{0}'", args.purchasedProduct.definition.id));
        IAPItem product = null;
        foreach (IAPItem item in _products)
        {
            string itemId = item._itemId;

            if (!string.IsNullOrEmpty(itemId) && itemId == args.purchasedProduct.definition.id)
            {
                product = item;
                break;
            }
        }

//#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR

        var validPurchase = true;

        if (product != null)
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                //Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                    if (null != google)
                    {
                        // This is Google's Order ID.
                        // Note that it is null when testing in the sandbox
                        // because Google's sandbox does not provide Order IDs.
                        //Debug.Log(google.transactionID);
                        //Debug.Log(google.purchaseState);
                        //Debug.Log(google.purchaseToken);
                        MADesign.MATrackingFunctions.setUserBuyIAP();
                        _purchaseSucceedEvent.Invoke(google.productID, google.purchaseToken, "0");
                    }
                    else
                    {
                        AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                        if (null != apple)
                        {
                            //Debug.Log(apple.originalTransactionIdentifier);
                            //Debug.Log(apple.subscriptionExpirationDate);
                            //Debug.Log(apple.cancellationDate);
                            //Debug.Log(apple.quantity);
                            MADesign.MATrackingFunctions.setUserBuyIAP();
                            _purchaseSucceedEvent.Invoke(args.purchasedProduct.definition.id, string.Empty, "0");
                        }
                    }
                }
            }
            catch (IAPSecurityException)
            {
                Debug.Log("Invalid receipt, not unlocking content");
                validPurchase = false;
            }
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

            _purchaseFailedEvent.Invoke(args.purchasedProduct.definition.id, "ProcessPurchase: FAIL. Unrecognized product.");
        }
//#endif

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed.
        return _pendingPurchaseSuccess ? PurchaseProcessingResult.Pending : PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        
        _purchaseFailedEvent.Invoke(product.definition.storeSpecificId, failureReason.ToString());
    }

    public void Fulfill(Product product)
    {
        if (product != null)
        {
            Debug.Log("Buy succeed: " + product.definition.id);
        }
    }

    public void FulfillFailed(Product product, PurchaseFailureReason reason)
    {
        if (product != null)
        {
            Debug.Log("Buy false: " + product.definition.id + ", reason: " + reason.ToString());
        }
    }
}