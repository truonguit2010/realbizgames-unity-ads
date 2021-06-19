using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleCondition {
    EXE_Smaller,
    EXE_Bigger,
    Always
}

[ExecuteInEditMode]
public class MultiResolutionScaler : MonoBehaviour
{

    const string TAG = "MultiResolutionScaler";

    [Header("UI objects")]
    [SerializeField]
    private Transform _uiATransform;
    [SerializeField]
    private Transform _uiBTransform;
    [SerializeField]
    private Transform _uiTransform;

    [Header("World objects.")]
    [SerializeField]
    private Transform _exeATransform;
    [SerializeField]
    private Transform _exeBTransform;

    [SerializeField]
    private Transform _exeTransform;

    [Header("Some layout options")]
    [SerializeField]
    private float _expectedScaleFactor = 1.0f;

    [SerializeField]
    private ScaleCondition _scaleCondition = ScaleCondition.EXE_Bigger;

    [SerializeField]
    private bool _allowFollowPosition = true;

    [SerializeField]
    private bool _alwaysUpdateIfNeed = true;

    [SerializeField]
    private bool updateLayout = false;

    public bool validateParams {
        get {
            return _uiATransform != null && _uiBTransform != null && _exeATransform != null && _exeBTransform != null && _exeTransform != null;
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (updateLayout)
        {
            updateLayoutFunc();
            updateLayout = false;
        }
#endif
    }

    float a = 0f;
    float b = 0f;

    float c = 0f;

    bool allowUpdate = false;

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (_alwaysUpdateIfNeed) {
            updateLayoutFunc();
        }
#endif
    }

    private void updateLayoutFunc() {
        if (validateParams) {
            a = getUIWidthFrom2Points();
            b = getWorldWidgetFrom2Points();

            allowUpdate = false;
            if (_scaleCondition == ScaleCondition.EXE_Bigger)
            {
                allowUpdate = b > a;
            }
            else if (_scaleCondition == ScaleCondition.EXE_Smaller)
            {
                allowUpdate = b < a;
            }
            else
            {
                c = b / a;
                allowUpdate = !Mathf.Approximately(c, _expectedScaleFactor);
            }

            Debug.LogFormat("{0} - updateLayoutFunc a={1} b={2} update={3}", TAG, a, b, allowUpdate);

            if (allowUpdate)
            {
                // Update
                Vector3 sc = _uiTransform.localScale;
                _exeTransform.localScale = new Vector3(sc.x * _expectedScaleFactor, sc.y * _expectedScaleFactor, _exeTransform.localScale.z);
            }
        }
    }

    public bool areTheyTheSameWidth(float _uiWidth, float _worldWidth)
    {
        return Mathf.Approximately(_uiWidth, _worldWidth);
    }

    float getUIWidthFrom2Points()
    {
        return Vector3.Distance(_uiATransform.position, _uiBTransform.position);
    }

    float getWorldWidgetFrom2Points()
    {
        return Vector3.Distance(_exeATransform.position, _exeBTransform.position);
    }
}
