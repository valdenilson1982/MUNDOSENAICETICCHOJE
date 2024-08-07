using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowCursor : MonoBehaviour
{
    public LayerMask hitMask;
    NavMeshAgent agent;
    Animator anim;

    Vector3 oldPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, hitMask))
        {
            if ((oldPosition - hit.point).sqrMagnitude > Mathf.Pow(0.25f, 2))
            {
                agent.SetDestination(hit.point);
                Debug.Log("Going to " + hit.point);
                oldPosition = hit.point;
            }
        }

        if (anim)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}
