using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject uiElement; // Assign this in the Inspector
    public GameObject notesPlayer;
    public GameObject playerModel;
    public GameObject thirdPersonCamera;
    public GameObject secondPersonCamera;
    private bool isPlayerInZone = false; // Track if player is inside the trigger zone

    void Start()
    {
        if (uiElement != null)
            uiElement.SetActive(false); // Hide UI at the start
        notesPlayer.SetActive(false);
        secondPersonCamera.SetActive(false);
    }

    void Update()
    {
        // Check if "E" is pressed inside the zone
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false); // Hide UI element
                notesPlayer.SetActive(true);
                playerModel.SetActive(false);
                thirdPersonCamera.SetActive(false);
                secondPersonCamera.SetActive(true);    
                StartCoroutine(StopAnimationAfterDelay(3f)); // Call Coroutine to stop animation after 3 seconds
            }
        }
    }

    private IEnumerator StopAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time (3 seconds)
        notesPlayer.SetActive(false);
        playerModel.SetActive(true);
        thirdPersonCamera.SetActive(true);
        secondPersonCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // Mark player as inside
            if (uiElement != null)
                uiElement.SetActive(true); // Show UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false; // Mark player as outside
            if (uiElement != null)
                uiElement.SetActive(false); // Hide UI when leaving zone
        }
    }
}
