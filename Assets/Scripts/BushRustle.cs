using UnityEngine;
using System.Collections;

public class BushRustle : MonoBehaviour
{
    public AudioSource rustleSource;
    public float rustleInterval = 0.7f;

    private bool playerInside = false;
    private bool canPlay = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && canPlay)
        {
            if (rustleSource != null && rustleSource.clip != null)
            {
                rustleSource.pitch = Random.Range(0.9f, 1.1f);
                rustleSource.PlayOneShot(rustleSource.clip);
                StartCoroutine(RustleCooldown());
            }
        }
    }

    IEnumerator RustleCooldown()
    {
        canPlay = false;
        yield return new WaitForSeconds(rustleInterval);
        canPlay = true;
    }
}