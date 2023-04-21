using MTAssets.MobileInputControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private JoystickAxis joystickAxis = default;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private Animator anim = default;
    [SerializeField] private Transform character = default;
    [SerializeField] private PlayerCombat playerCombat = default;

    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float rotSpeed = default;

    private float x;
    private float z;

    public PlayerCombat PlayerCombat => playerCombat;

    private void Update()
    {
        x = joystickAxis.inputVector.x;
        z = joystickAxis.inputVector.y;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (x != 0 || z != 0)
        {
            rb.MovePosition(rb.position + (moveSpeed * Time.deltaTime * new Vector3(x, rb.velocity.y, z)));

            RotatePlayer(x, z);

            anim.SetInteger(GetAnimatorHashCode("transitionMovement"), 1);

            for (int i = 0; i < playerCombat.Childs.Count; i++)
            {
                if (i <= 4) playerCombat.Childs[i].transform.localRotation = Quaternion.Lerp(playerCombat.Childs[i].transform.localRotation, Quaternion.Euler(-15, 0, 0), rotSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.SetInteger(GetAnimatorHashCode("transitionMovement"), 0);

            foreach (Transform t in playerCombat.Childs)
            {
                t.transform.localRotation = Quaternion.Lerp(t.transform.localRotation, Quaternion.Euler(0, 0, 0), rotSpeed * Time.deltaTime);
            }
        }
    }
    
    private void RotatePlayer(float x, float y)
    {
        character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up), rotSpeed * Time.deltaTime);
    }

    private int GetAnimatorHashCode(string hashName) => Animator.StringToHash(hashName);
}
