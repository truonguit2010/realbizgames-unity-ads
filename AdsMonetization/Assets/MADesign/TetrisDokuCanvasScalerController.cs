using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TetrisDokuCanvasScalerController : MonoBehaviour
{
    const string TAG = "TetrisDokuCanvasScalerController";

    [SerializeField]
    private CanvasScaler _canvasScaler;

    [SerializeField]
    private ScreenOrientation _screenOrientation;

    [SerializeField]
    private bool _doLayout = false;

    public int screenWidth {
        get {
#if UNITY_EDITOR
            return (int) GetMainGameViewSize().x;
#else
            return Screen.width;
#endif
        }
    }

    public int screenHeight {
        get {
#if UNITY_EDITOR
            return (int)GetMainGameViewSize().y;
#else
            return Screen.height;
#endif
        }
    }

    public float widthOnHeightRatio() {
        return screenWidth / ((float)screenHeight);
    }

    private float _designOnRatio = 16 / 9;

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }

    public CanvasScaler canvasScaler {
        get {
            if (_canvasScaler == null) {
                _canvasScaler = GetComponent<CanvasScaler>();
            }
            return _canvasScaler;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        setUpCanvasScalerWithOrientation();
    }

    private void Update()
    {
        if (_screenOrientation != Screen.orientation) {
            setUpCanvasScalerWithOrientation();
        }
    }

    private void setUpCanvasScalerWithOrientation() {
        //if (canvasScaler != null)
        //{
        //if (Screen.orientation == ScreenOrientation.Landscape)
        //{
        //    _canvasScaler.matchWidthOrHeight = 1f;
        //}
        //else {
        //    _canvasScaler.matchWidthOrHeight = 0.5f;
        //}
        //_screenOrientation = Screen.orientation;

        //}
        updateMatchWidthOrHeight();
    }

    // ---------------- matchWidthOrHeight = 1;
    // ipad     : 3:4 = 0.75
    // iphone 4s: 640/960 = 0.6666666667

    // ---------------- matchWidthOrHeight = 0;
    // iphone 6s: 750/1334 = 0.5622188906  [0-1: Always OK]
    // Samsung Galaxy S5 Mini: 720/1280 = 0.5625 [0-1: Always OK]

    // ---------------- matchWidthOrHeight = 0.5f;
    // Samsung note 10lite capture: 576/1280 = 0.45, 1080/2400 = 0.45
    // iPhone XR: 828/1792 = 0.4620535714
    // iPhone X/XS: 1125/2436 = 0.4618226601
    // iPhone XS Max: 1242/2688 = 0.4620535714
    // Samsung Galaxy S10+: 1440/3040 = 0.4736842105
    // Samsung Galaxu Note 9: 1440/2960 = 0.4864864865
    private float _oneRatio = 0.6f;
    private void updateMatchWidthOrHeight() {

        if (_canvasScaler != null)
        {

            float _ratio = this.widthOnHeightRatio();
            //if (_ratio <= 0.49f)
            //{
            //    _canvasScaler.matchWidthOrHeight = 0.25f;
            //}
            //else {
                _canvasScaler.matchWidthOrHeight = (_ratio >= _oneRatio) ? 1f : 0f;
            //}
            

#if UNITY_EDITOR
            Debug.LogFormat("{0} - updateMatchWidthOrHeight _ratio: {1} _oneRatio: {2}", TAG, _ratio, _oneRatio);
#endif
        }
        else {
            Debug.LogErrorFormat("{0} - Cannot found Canvas Scaler.", TAG);
        }
    }

    private void OnGUI()
    {
        
    }

    private void OnValidate()
    {
        if (_doLayout) {
            _doLayout = false;
            logScreenResolution();
            updateMatchWidthOrHeight();
        }
    }

    private void logScreenResolution() {
        Debug.LogFormat("{0} - {1}", TAG, screenResolution);
    }

    public string screenResolution {
        get {
            return string.Format("Resolution width: {0} height: {1}", screenWidth, screenHeight);
        }
    }


}
