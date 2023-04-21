using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.MobileInputControls
{
    public class DemoOfRotation : MonoBehaviour
    {
        private Rigidbody rb;

        public JoystickAxis joystick;
        public bool usePhysX = false;

        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            //If physics is not desired. (Uses -1 to invert rotation)
            if (usePhysX == false)
            {
                this.transform.Rotate(0, 0, 100 * joystick.inputVector.x * Time.deltaTime * -1);
            }
            //If physics is desired
            if (usePhysX == true)
            {
                rb.AddTorque(0, 0, 0.5f * joystick.inputVector.x * -1, ForceMode.Acceleration);
            }
        }
    }
}
