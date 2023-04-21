using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Rigidbody mainRigdbody = default;
    [SerializeField] private Collider mainCollider = default;
    [SerializeField] private Animator anim = default;

    private List<Rigidbody> m_Rigidbodies;
    private List<Collider> m_Colliders;

    void Start()
    {
        m_Rigidbodies = new List<Rigidbody>();
        m_Colliders = new List<Collider>();

        StartRagdoll();
    }

    private void StartRagdoll()
    {
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < rigs.Length; i++)
        {
            if (rigs[i] == mainRigdbody) continue;

            m_Rigidbodies.Add(rigs[i]);
            rigs[i].isKinematic = true;

            Collider col = rigs[i].GetComponent<Collider>();
            col.isTrigger = true;
            m_Colliders.Add(col);
        }
    }

    public void EnableRagdoll(bool enable)
    {
        for (int i = 0; i < m_Rigidbodies.Count; i++)
        {
            m_Rigidbodies[i].isKinematic = !enable;
            m_Colliders[i].isTrigger = !enable;
        }
    }

    public void SetMainComponents(bool enable)
    {
        mainRigdbody.isKinematic = enable;
        mainCollider.isTrigger = enable;
    }

    public void EnableAnimator(bool enable)
    {
        anim.enabled = enable;
    }
}
