using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class AITarget : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    Vector3 lastPlayerPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastPlayerPos = target.position;
    }

    void Update()
    {
        if (Vector3.Distance(lastPlayerPos, target.position) > 0.5f)
        {
            agent.SetDestination(target.position);
            lastPlayerPos = target.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }
}