using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AITarget : MonoBehaviour
{
    public Transform target;
    public float attackRange;

    private NavMeshAgent agent;
    //private Animator;
    private float distanceToTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(agent.transform.position, target.position);

        if(distanceToTarget < attackRange)
        {
            agent.isStopped = true;
            //animator.SetBool("Attack", true);
        }
        else
        {
            agent.isStopped = false;
            //animator.SetBool("Attack", false);
            agent.SetDestination(target.position);
        }
    }
}
