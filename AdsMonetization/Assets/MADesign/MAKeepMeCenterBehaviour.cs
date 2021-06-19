using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MADesign
{
    [ExecuteInEditMode]
    public class MAKeepMeCenterBehaviour : MonoBehaviour
    {
        const string TAG = "MAKeepMeCenterBehaviour";

        [SerializeField]
        private Transform _topTransform;
        [SerializeField]
        private Transform _bottomTransform;
        [SerializeField]
        private Transform _leftTransform;
        [SerializeField]
        private Transform _rightTransform;

        [SerializeField]
        private Transform _centerTransform;

        [SerializeField]
        private bool _keepCenterX = true;
        [SerializeField]
        private bool _keepCenterY = true;


        float _xx = 0;
        float _yy = 0;

        // Update is called once per frame
        void Update()
        {
            if (_centerTransform != null) {
                _xx = _centerX();
                _yy = _centerY();
                _centerTransform.position = new Vector3(_xx, _yy, _centerTransform.position.z);
            }
        }

        private float _centerX() {
            if (_keepCenterX && _leftTransform != null && _rightTransform != null)
            {
                return _leftTransform.position.x + ((_rightTransform.position.x - _leftTransform.position.x) / 2);
            }
            else {
                return _centerTransform.position.x;
            }
        }

        private float _centerY()
        {
            if (_keepCenterY && _topTransform != null && _bottomTransform != null)
            {
                return _topTransform.position.y + ((_bottomTransform.position.y - _topTransform.position.y) / 2);
            }
            else
            {
                return _centerTransform.position.y;
            }
        }
    }
}

