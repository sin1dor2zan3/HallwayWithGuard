using UnityEngine;

public class GuardSpawner : MonoBehaviour
{
    public GameObject guardPrefab;

    public Transform player;

    void Start()
    {
        GameObject guard = Instantiate(guardPrefab, transform.position, Quaternion.identity);

        AITarget ai = guard.GetComponent<AITarget>();
        ai.target = player;
    }
}