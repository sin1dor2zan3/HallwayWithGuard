using UnityEngine;

public class HatBehavior : MonoBehaviour
{
    public float bobHeight = 0.2f;
    public float bobSpeed = 2f;
    public float rotateSpeed = 40f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate slowly
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }
}