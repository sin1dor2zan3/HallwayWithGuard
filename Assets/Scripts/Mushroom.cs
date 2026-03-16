using UnityEngine;
using System.Collections;

public class MushroomBounce : MonoBehaviour
{
    private AudioSource audioSource;
    private Vector3 originalScale;

    public ParticleSystem puffEffect;

    public float squishY = 0.6f;
    public float squishX = 1.15f;
    public float squishZ = 1.15f;
    public float squishTime = 0.08f;

    private bool isAnimating = false;
    private bool canTrigger = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;
        if (!other.CompareTag("Player")) return;

        canTrigger = false;

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (puffEffect != null)
        {
            puffEffect.transform.position = other.ClosestPoint(transform.position) + Vector3.up * 0.1f;
            puffEffect.Play();
        }

        HandBob handBob = other.GetComponentInChildren<HandBob>();
        if (handBob != null)
        {
            handBob.TriggerMushroomBounce();
        }

        if (!isAnimating)
        {
            StartCoroutine(Squish());
        }

        StartCoroutine(ResetTrigger());
    }

    IEnumerator Squish()
    {
        isAnimating = true;

        Vector3 squishedScale = new Vector3(
            originalScale.x * squishX,
            originalScale.y * squishY,
            originalScale.z * squishZ
        );

        float timer = 0f;
        while (timer < squishTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, squishedScale, timer / squishTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = squishedScale;

        timer = 0f;
        while (timer < squishTime)
        {
            transform.localScale = Vector3.Lerp(squishedScale, originalScale, timer / squishTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isAnimating = false;
    }

    IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.15f);
        canTrigger = true;
    }
}