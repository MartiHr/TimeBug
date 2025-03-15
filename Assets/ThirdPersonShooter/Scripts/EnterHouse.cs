using UnityEngine;
using UnityEngine.UI; // Needed for UI elements

public class EnterHouse : MonoBehaviour
{
    public GameObject houseImage; // Assign this in the Inspector

    void Start()
    {
        if (houseImage != null)
        {
            houseImage.SetActive(false); // Ensure it's hidden at start
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters
        {
            if (houseImage != null)
            {
                houseImage.SetActive(true); // Show image
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player exits
        {
            if (houseImage != null)
            {
                houseImage.SetActive(false); // Hide image
            }
        }
    }
}
