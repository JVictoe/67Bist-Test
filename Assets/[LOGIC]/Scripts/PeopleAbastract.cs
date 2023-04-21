using System.Text.RegularExpressions;
using UnityEngine;

public abstract class PeopleAbastract : MonoBehaviour
{
    public Transform parent;
    public bool captured;
    public CapsuleCollider capsule;
    public Rigidbody rb;

    private Vector3 startPos;

    protected void Start()
    {
        startPos = transform.position;
        parent = transform;
    }

    private void OnDisable()
    {
        transform.position = startPos;
        captured = false;
        Invoke(nameof(EnablePeople), 3f);
    }

    private void EnablePeople()
    {
        transform.parent = parent;
        gameObject.SetActive(true);
        capsule.isTrigger = false;
        rb.isKinematic = false;
    }
}
