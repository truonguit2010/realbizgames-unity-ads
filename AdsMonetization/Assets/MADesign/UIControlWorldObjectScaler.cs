using UnityEngine;

[ExecuteInEditMode]
public class UIControlWorldObjectScaler : MAMonoBehaviour
{
    const string TAG = "UIControlWorldObjectScaler";

    [Header("UI objects")]
    [SerializeField]
    private Transform _uiATransform;
    [SerializeField]
    private Transform _uiBTransform;
    [SerializeField]
    private Transform _uiTransform;

    [Header("The board world width")]
    [SerializeField]
    private float _theWorldBoardWidth = 7.182F;

    [SerializeField]
    private float _theWorldBoardWidthWithScaler;

    [SerializeField]
    private float _theUIWidth;

    [SerializeField]
    private Transform worldObjectTransform;

    [Header("Some layout options")]
    [SerializeField]
    private bool _allowFollowPosition = true;

    [SerializeField]
    private bool _alwaysUpdateIfNeed = true;

    [SerializeField]
    private bool updateLayout = false;

    private bool realCheckAllObjectReady {
        get {
            return _uiATransform != null && _uiBTransform != null && worldObjectTransform != null;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_alwaysUpdateIfNeed) {
    //        uiControlWorldScaler();
    //    }
    //}

    //    private void OnValidate()
    //    {
    //#if UNITY_EDITOR
    //        if (updateLayout) {
    //            uiControlWorldScaler();
    //            updateLayout = false;
    //        }
    //#endif
    //    }

    public void updateScale() {
        uiControlWorldScaler();
    }

    public void uiControlWorldScaler() {
        if (realCheckAllObjectReady) {
            _theUIWidth = getUIWidthFrom2Points();
            if (!areTheyTheSameWidth(_theUIWidth, _theWorldBoardWidth))
            {
                float matchRate = _theUIWidth / _theWorldBoardWidth;
                Vector3 finalScale = new Vector3(matchRate, matchRate, 1);
                worldObjectTransform.localScale = finalScale;

                //Debug.LogFormat("{0} - uiControlWorldScaler _uiWidth: {1}, _worldWidth {2}, matchRate: {3}, lastScale: {4}, finalScale: {5}", TAG, _uiWidth, _worldWidth, matchRate, scale, finalScale);
            }
            if (_allowFollowPosition && _uiTransform != null)
            {
                Vector3 lastPos = worldObjectTransform.position;
                worldObjectTransform.position = new Vector3(_uiTransform.position.x, _uiTransform.position.y, lastPos.z);
            }
        }
    }

    public bool areTheyTheSameWidth(float _uiWidth, float _worldWidth) {
        return Mathf.Approximately(_uiWidth, _worldWidth);
    }

    float getUIWidthFrom2Points() {
        return Vector3.Distance(_uiATransform.position, _uiBTransform.position);
    }

    float getWorldWidgetFrom2Points() {
        return _theWorldBoardWidth;// Vector3.Distance(_worldATransform.position, _worldBTransform.position);
    }

#if UNITY_EDITOR
    public override void GUIEditor()
    {
        if (GUILayout.Button("Update Scale"))
        {
            uiControlWorldScaler();
        }
    }
#endif
}
