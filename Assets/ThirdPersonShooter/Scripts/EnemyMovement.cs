using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;  // The player's transform
    public float moveSpeed = 3f;  // Movement speed of the enemy
    public float rotationSpeed = 5f;  // Speed at which the enemy turns towards the player
    public float stoppingDistance = 2f;  // Distance at which the enemy will stop moving

    private float initialYPosition;  // To store the initial y-position of the enemy

    void Start()
    {
        initialYPosition = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stoppingDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;

                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
