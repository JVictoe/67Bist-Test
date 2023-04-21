using UnityEngine;

namespace MTAssets.MobileInputControls
{
    public class PlayerDemoMIC : MonoBehaviour
    {

        public Animator playerAnimator;
        public Rigidbody playerRigidbody;
        public Transform playerTransform;
        public JoystickAxis movementJoystick;
        public DragArea rotationTouchArea;

        void Update()
        {
            //If is pressed, change the animation
            playerAnimator.SetBool("walk", movementJoystick.pressed);

            //Rotate, and move the character with the joystick
            if (movementJoystick.pressed == true)
            {
                playerTransform.localRotation = Quaternion.Lerp(playerTransform.localRotation, Quaternion.LookRotation(
                    new Vector3(movementJoystick.inputVector.x, 0, movementJoystick.inputVector.y), Vector3.up), 20 * Time.deltaTime);

                playerRigidbody.velocity = transform.TransformVector(
                    new Vector3(movementJoystick.inputVector.x * 3, playerRigidbody.velocity.y, movementJoystick.inputVector.y * 3));
            }
            //Rotate the character to forward when stop moving
            if (movementJoystick.pressed == false)
            {
                playerTransform.localRotation = Quaternion.Lerp(playerTransform.localRotation, Quaternion.Euler(0, 0, 0), 20 * Time.deltaTime);
                playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
            }
            //Rotate the player with Horizontal axis of Drag Area
            transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y + (rotationTouchArea.positionDeltaVector.x * Time.deltaTime * 8f), 0);
        }
    }
}