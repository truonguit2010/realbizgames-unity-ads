using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase;

[DisallowMultipleComponent]
public class GameInit : MonoBehaviour
{
    const string TAG = "GameInit";

    private bool firebaseInitialized;

    FirebaseApp app;
    private bool _isFirebaseReady = false;
    private bool _isFacebookReady = false;


    [SerializeField]
    private bool _firstTimeInit = true;


    [SerializeField]
    private int targetFrameRate = 60;

    [SerializeField]
    private List<string> consumableItemIds;

    [SerializeField]
    private List<string> nonconsumableItemIds;

    [SerializeField]
    private string nextScene = "GameScene";

    [SerializeField]
    private float minStayTimeInSeconds = 1.0f;

    public static GameInit Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            //FPSDisplay.Instance.run();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _firstTimeInit = true;

        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

        Social_addListeners();

        initGooglePlayGame();
        initIAP();
        initFacebook();
        initFirebase();

        setupFrameRate();

        StartCoroutine(moveToNextScene());
    }

    private IEnumerator moveToNextScene() {
        yield return new WaitForSeconds(minStayTimeInSeconds);
        if (!string.IsNullOrEmpty(this.nextScene)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.nextScene);
        }
        
    }



    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            checkAndLoginSocial();

            if (_isFirebaseReady) {
                startFetchDataFromRemoteConfigAsyncTask();
            }
        }
    }

    // ------------------------------------------------------------------------------------------
    // Render Settings
    // ------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------

    public void setupFrameRate() {
        // https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
        if (targetFrameRate < 30) {
            targetFrameRate = 60;
        }
        Application.targetFrameRate = targetFrameRate;
    }

    // ------------------------------------------------------------------------------------------
    // Firebase cloud message callbacks
    // ------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

    // ------------------------------------------------------------------------------------------
    // Init come from here.
    // ------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------
    private void Social_addListeners()
    {
        //GooglePlayServicesSignIn.Instance.onLoginSocialResultEvent.AddListener(onSocialLoginResult);
    }

    private void initGooglePlayGame()
    {
        // Bò qua không init ngay khi bắt đầu mở game.
        //GooglePlayServicesSignIn.Instance.InitializeGooglePlayGames();
    }

    private void checkAndLoginSocial()
    {
        //int finishGameCount = LocalStorageManager.Instance.finishGameCount;
        //if (LocalStorageManager.Instance.socialLoginResult)
        //{
        //    if (GooglePlayServicesSignIn.Instance.isNotLoginSocial)
        //    {
        //        GooglePlayServicesSignIn.Instance.SignIn(SignInFor.Nothing);
        //    }
        //}
        //if (finishGameCount > 2)
        //{
        //    int tryCount = LocalStorageManager.Instance.socialLoginTryCount;
        //    if (tryCount == 0 || (tryCount % 5 == 0 && tryCount < 21))
        //    {
        //        if (GooglePlayServicesSignIn.Instance.isNotLoginSocial)
        //        {
        //            GooglePlayServicesSignIn.Instance.SignIn(SignInFor.Nothing);
        //        }
        //    }
        //}
    }

    private void onSocialLoginResult(bool success)
    {
        //if (success)
        //{
        //    int highestScore = LocalStorageManager.Instance.highestScore;
        //    if (highestScore > 0)
        //    {
        //        GooglePlayServicesSignIn.Instance.AddScoreToLeaderboard(Constants.LEADERBOARD_ID, score: highestScore);
        //    }
        //}
    }

    private void initIAP()
    {
        List<IAPManager.IAPItem> listInIap = new List<IAPManager.IAPItem>();
        foreach (string id in consumableItemIds)
        {
            listInIap.Add(new IAPManager.IAPItem()
            {
                _itemId = id,
                _type = IAPManager.IAPItemType.Consumable,
            });
        }

        foreach (string id in nonconsumableItemIds)
        {
            listInIap.Add(new IAPManager.IAPItem()
            {
                _itemId = id,
                _type = IAPManager.IAPItemType.NonConsumable,
            });
        }

        IAPManager.Instance.SetProducts(listInIap);
    }

    private void initFacebook()
    {
        //Facebook.Unity.FB.Init(() =>
        //{
        //    _isFacebookReady = true;
        //});
    }

    private void initFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _isFirebaseReady = true;

                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                startFetchDataFromRemoteConfigAsyncTask();

                //MADesign.TetrisDokuTrackingFunctions.loading_InitFirebase_Tracking_Done();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    /// <summary>
    /// show dialog đăng ký push notification ở iOS.
    /// </summary>
    public void registerPushNotificationService()
    {
        //int finishGameCount = LocalStorageManager.Instance.finishGameCount;
        //if (finishGameCount > 2)
        //{
        //    // show dialog đăng ký push notification ở iOS.
        //    Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
        //}
    }

    void startFetchDataFromRemoteConfigAsyncTask()
    {
        System.Threading.Tasks.Task.WhenAll(
            fetchRemoteConfigDataTask()
          ).ContinueWith(task => {
              //firebaseInitialized = true;

              //// Remote Config data has been fetched, so this applies it for this play session:
              Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();

              Debug.LogFormat("{0} - {1} Fetch data from remote config done!", TAG, name);
          });
    }

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    System.Threading.Tasks.Task fetchRemoteConfigDataTask()
    {
        return Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
    }

}