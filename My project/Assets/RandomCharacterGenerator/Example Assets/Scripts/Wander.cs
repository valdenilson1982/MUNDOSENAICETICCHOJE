using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Wander : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator anim;

    public float searchRadius, waitTime;
    public float stoppingDst = 1, updateTime = 1;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(Wandering());
    }

    Vector3 randomPoint
    {
        get
        {
            Vector2 point = Random.insideUnitCircle * searchRadius;
            return new Vector3(point.x, 0, point.y);
        }
    }

    protected virtual IEnumerator Wandering ()
    {
        while (true)
        {
            if (agent.remainingDistance <= stoppingDst)
            {
                agent.SetDestination(transform.position + randomPoint);
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitForSeconds(updateTime);
        }
    }

    private void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}
