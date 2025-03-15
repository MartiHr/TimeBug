using UnityEngine;

public class SciFiCharge : MonoBehaviour
{
    public float speed = 30f;
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after a set time
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; // Move forward in a straight line
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy charge on impact
        }
    }
}
