using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.MobileInputControls
{
    public class CameraDemoMIC : MonoBehaviour
    {

        private float yValue;

        public DragArea rotationTouchArea;

        void Start()
        {

        }

        void Update()
        {
            //Rotate the camera in vertical, with touch
            yValue += rotationTouchArea.positionDeltaVector.y * Time.deltaTime * 4f * -1;
            yValue = Mathf.Clamp(yValue, -20, 20);
            transform.localRotation = Quaternion.Euler(yValue, 0, 0);
        }
    }
}