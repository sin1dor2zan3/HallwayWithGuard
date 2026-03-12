using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class AITarget : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    [Header("Chasing")]
    public float chaseDistance;
    public bool isChasing;

    [Header("Roaming")]
    public float roamRadius;
    public float roamDelay;

    float roamTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickRoamPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= chaseDistance)
        {
            isChasing = true;
            agent.SetDestination(target.position);
        }
        else
        {
            isChasing = false;

            roamTimer += Time.deltaTime;

            if (roamTimer >= roamDelay && agent.remainingDistance < 1f)
            {
                PickRoamPoint();
                roamTimer = 0f;
            }
        }
    }

    void PickRoamPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}