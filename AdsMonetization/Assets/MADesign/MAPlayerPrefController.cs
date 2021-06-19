using UnityEngine;

public class MAPlayerPrefController
{
    public const string KeyRemoveAds = "KeyRemoveAds";
    private const string KEY_OPEN_APPLICATION_COUNT = "KEY_OPEN_APPLICATION_COUNT";
    private const string FINISH_TUTORIAL = "FINISH_TUTORIAL";
    private const string KEY_LANGUAGE_ID = "KEY_LANGUAGE_ID";

    //--------------------------------------------------------------------------
    // Cache remove ads variable
    //--------------------------------------------------------------------------
    private static bool _isRemoveAds = false;
    public static bool IsRemoveAds
    {
        get
        {
            //return PlayerPrefs.GetInt(KeyRemoveAds, 0) != 0;
            return _isRemoveAds;
        }
    }

    public static bool IsNotRemoveAds
    {
        get
        {
            return !IsRemoveAds;
        }
    }

    public static void SetRemoveAds()
    {
        //PlayerPrefs.SetInt(KeyRemoveAds, 1);
        //PlayerPrefs.Save();
        _isRemoveAds = true;
    }

    //--------------------------------------------------------------------------
    // Count open application
    //--------------------------------------------------------------------------
    public static int increaseOpenApplicationCount()
    {
        int v = PlayerPrefs.GetInt(KEY_OPEN_APPLICATION_COUNT, 0);
        v++;
        PlayerPrefs.SetInt(KEY_OPEN_APPLICATION_COUNT, v);
        PlayerPrefs.Save();
        return v;
    }

    //--------------------------------------------------------------------------
    //  Flag check show highscore popup
    //--------------------------------------------------------------------------
    public static bool IsFinishTutorial
    {
        get
        {
            return PlayerPrefs.GetInt(FINISH_TUTORIAL, 0) != 0;
        }
    }

    public static void SetFinishTutorial()
    {
        PlayerPrefs.SetInt(FINISH_TUTORIAL, 1);
        PlayerPrefs.Save();
    }

    //--------------------------------------------------------------------------
    //  Set language
    //--------------------------------------------------------------------------
    public static int languageId {
        get {
            return PlayerPrefs.GetInt(KEY_LANGUAGE_ID, 0);
        }
        set {
            PlayerPrefs.SetInt(KEY_LANGUAGE_ID, value);
            PlayerPrefs.Save();
        }
    }


}
