using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.programmersought.com/article/12983962524/
public class MAAndroidScreenManager : MonoBehaviour
{
    [SerializeField]
    private bool _allowFullScreen = true;

    [SerializeField]
    private bool _allowSettingStatusBar = true;

    [SerializeField]
    private ApplicationChrome.States _statusBarState = ApplicationChrome.States.Visible;

    private void Awake()
    {
#if UNITY_ANDROID
        // Showing the virtual navigation bar at the bottom is simple (although I found Baidu one day o(╥﹏╥)o)
        Screen.fullScreen = _allowFullScreen;
        //if (_allowSettingStatusBar) {
        //    ApplicationChrome.statusBarState = _statusBarState;
        //    ApplicationChrome.statusBarColor = 0x00000000;
        //    //ApplicationChrome.dimmed = false;
        //}
#endif

        // Makes the status bar and navigation bar visible over the content (different content resize method) 
        //ApplicationChrome.statusBarState = ApplicationChrome.navigationBarState = ApplicationChrome.States.VisibleOverContent;
    }
}
