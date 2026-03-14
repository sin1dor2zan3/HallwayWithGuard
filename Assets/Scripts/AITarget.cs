using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class AITarget : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private AudioSource audioSource;

    [Header("Chasing")]
    public float chaseDistance;
    public bool isChasing;

    [Header("Roaming")]
    public float roamRadius;
    public float roamDelay;

    [Header("Audio")]
    public AudioClip screechClip;

    float roamTimer;
    private bool hasScreeched = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        PickRoamPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= chaseDistance)
        {
            if (!isChasing)
            {
                PlayScreech();
            }

            isChasing = true;
            agent.SetDestination(target.position);
        }
        else
        {
            isChasing = false;
            hasScreeched = false;

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

    void PlayScreech()
    {
        if (!hasScreeched && screechClip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(screechClip);
            hasScreeched = true;
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