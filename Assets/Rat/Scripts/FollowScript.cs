using UnityEngine;
using UnityEngine.AI;

public class FollowScript : MonoBehaviour
{
    public NavMeshAgent pet;
    public Transform Player;

    void Start()
    {

    }

    void Update()
    {
        pet.SetDestination(Player.position);
    }
}
