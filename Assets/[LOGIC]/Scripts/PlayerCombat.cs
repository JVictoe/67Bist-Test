using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator anim = default;
    [SerializeField] private bool inCombat = default;
    [SerializeField] private float force = default;
    [SerializeField] private Collider collider = default;
    [SerializeField] private ForceMode forceMode = default;
    [SerializeField] private Transform parent = default;
    [SerializeField] private List<Transform> childs = default;
    [SerializeField] private Camera mainCamera = default;

    private WaitForSeconds time1 = new WaitForSeconds(0.2f);
    private WaitForSeconds time2 = new WaitForSeconds(1f);
    public List<Transform> Childs => childs;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("People") && !inCombat) 
        {
            if (childs.Count >= PlayerStatusManager.Instance.MaxPeopleToLoad) return;

            inCombat = true;
            Physics.IgnoreCollision(collision.collider, collider, true);
            StartCoroutine(StartCombat(collision));
            Physics.IgnoreCollision(collision.collider, collider, false);
        }
    }

    private IEnumerator StartCombat(Collision collision)
    {
        anim.SetLayerWeight(1, 1);
        anim.SetInteger(GetAnimatorHashCode("transitionCombat"), 1);
        PeopleController peopleController = collision.gameObject.GetComponent<PeopleController>();
        Ragdoll ragdoll = peopleController.Ragdoll;

        peopleController.Captured = true;

        yield return time1;

        mainCamera.transform.DOShakePosition(0.5f, 1f, 5); 

        ragdoll.EnableAnimator(false);
        ragdoll.EnableRagdoll(true);

        peopleController.AddForce(peopleController, -collision.contacts[0].normal, force, forceMode);

        yield return time1;

        ragdoll.SetMainComponents(true);

        yield return time1;

        anim.SetInteger(GetAnimatorHashCode("transitionCombat"), 0);
        anim.SetLayerWeight(1, 0);

        yield return time2;

        childs.Add(peopleController.transform);

        if (childs.Count == 1) peopleController.transform.SetParent(parent);
        else peopleController.transform.SetParent(childs[childs.Count - 2].transform);

        peopleController.transform.SetLocalPositionAndRotation(new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 0));

        ragdoll.EnableRagdoll(false);
        ragdoll.EnableAnimator(true);

        PlayerStatusManager.Instance.SetMaxPlayer(false);

        inCombat = false;
    }

    private int GetAnimatorHashCode(string hashName) => Animator.StringToHash(hashName);
}
