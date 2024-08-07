using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Survivor : Wander
{
    public LayerMask threatMask;

    // Start is called before the first frame update
    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine(Wandering());
    }

    Vector3 runawayPoint (Collider[] threats)
    {
        Vector3 threatsSumatory = Vector3.zero;
        foreach (Collider threat in threats)
        {
            threatsSumatory += threat.transform.position;
        }

        Vector3 point = transform.position - threatsSumatory;
        point.Normalize();
        return point * searchRadius;
    }

    protected override IEnumerator Wandering()
    {
        while (true)
        {

            Collider[] threats = Physics.OverlapSphere(transform.position, searchRadius, threatMask);

            if (threats.Length == 0)
            {
                yield return base.Wandering();
            }
            else
            {
                agent.SetDestination(runawayPoint(threats));
            }
            yield return new WaitForSeconds(updateTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}
