using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MAWorldCameraAndCanvasScaler : MonoBehaviour
{
    const string TAG = "MAWorldCameraAndCanvasScaler";

    [SerializeField]
    private Transform _leftTransform;

    [SerializeField]
    private Transform _rightTransform;

    [SerializeField]
    private Transform _boardTopLeftTransform;

    [SerializeField]
    private float designWidth = 8.852774f;
    [SerializeField]
    private float finalWidth = 0;

    [SerializeField]
    private float designOrthographicSize = 8.8f;
    [SerializeField]
    private float finalOrthographicSize = 0;

    [SerializeField]
    private float scaleRate;

    [SerializeField]
    private float currentScaleFactor = 0;

    [SerializeField]
    private CanvasScaler _canvasScaler;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private bool logInformation = false;

    [SerializeField]
    private bool updateCameraSizeEditor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (logInformation) {
            logResolutionInformation();
            logWidth();
            logInformation = false;
        }

        if (updateCameraSizeEditor) {
            updateCameraSize();
            updateCameraSizeEditor = false;
        }
    }

    private void logResolutionInformation() {
        if (_canvasScaler != null) {
            Debug.LogFormat("{0} - _canvasScaler.scaleFactor: {1}", TAG, _canvasScaler.scaleFactor);
        }

        if (_camera != null) {
            if (_camera.orthographic) {
                //_camera.orthographicSize
                Debug.LogFormat("{0} - _camera.orthographicSize: {1}", TAG, _camera.orthographicSize);
            }
            if (_leftTransform != null) {
                Debug.LogFormat("{0} - left Position: {1}", TAG, _camera.WorldToScreenPoint(_leftTransform.position));
            }
            if (_boardTopLeftTransform != null) {
                Debug.LogFormat("{0} - board top left Position: {1}", TAG, _camera.WorldToScreenPoint(_boardTopLeftTransform.position));
            }
        }
    }

    private void logWidth() {
        if (_leftTransform != null && _rightTransform != null) {
            float ww = _rightTransform.position.x - _leftTransform.position.x;
            Debug.LogFormat("{0} - screen ({1}, {2}) width: {3}", TAG, Screen.width, Screen.height, ww);
        }
    }


    public void updateCameraSize() {
        
        finalWidth = _rightTransform.position.x - _leftTransform.position.x;
        scaleRate = designWidth / finalWidth;
        finalOrthographicSize = designOrthographicSize * scaleRate;
         //_canvasScaler.scaleFactor
        Debug.LogFormat("{0} update Camera size from {1} to {2}", TAG, designOrthographicSize, finalOrthographicSize);

        if (_camera != null) {
            _camera.orthographicSize = finalOrthographicSize;
        }
    }


    // 1.
    //https://answers.unity.com/questions/689696/changing-orthographic-size-according-to-resolution.html
    //Camera.main.orthographicSize = boardSize / 2 * Screen.height / Screen.width;


}
