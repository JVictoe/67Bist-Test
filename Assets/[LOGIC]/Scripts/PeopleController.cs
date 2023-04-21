using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleController : PeopleAbastract
{
    [SerializeField] private Animator anim = default;

    [SerializeField] private Ragdoll ragdoll = default;

    [SerializeField] private Transform[] points = default;
    [SerializeField] private float speed = default;

    private int currentPoint;

    public bool Captured { get { return captured; } set { captured = value; } }
    public Ragdoll Ragdoll => ragdoll;

    private void Start()
    {
        base.Start();

        speed = Random.Range(1.5f, 2f);

        currentPoint = Random.Range(0, points.Length);
    }

    private void Update()
    {
        if (captured)
        {
            anim.SetInteger(GetAnimatorHashCode("transition"), 0);
            return;
        }

        if (transform.position == points[currentPoint].position)
        {
            int oldPoint = currentPoint;

            currentPoint = Random.Range(0, points.Length);

            if(currentPoint == oldPoint)
            {
                currentPoint++;
            }

            if (currentPoint == points.Length)
            {
                currentPoint = 0;
            }
        }
        else
        {
            anim.SetInteger(GetAnimatorHashCode("transition"), 1);
            transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);
            transform.LookAt(points[currentPoint].position);
        }
    }

    public void AddForce(PeopleController people, Vector3 dir, float force, ForceMode forceMode)
    {
        if(people == this)
        {
            rb.AddForce(dir * force, forceMode);
        }
    }

    private int GetAnimatorHashCode(string hashName) => Animator.StringToHash(hashName);
}
