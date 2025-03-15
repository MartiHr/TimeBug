using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPortalManager : MonoBehaviour
{
    public GameObject interactableObj; // Assign this in the inspector
    public string sceneToLoad; // Set the scene name in the Inspector
    private bool isPlayerInTrigger = false;

    private void Start()
    {
        if (interactableObj != null)
        {
            interactableObj.SetActive(false); // Ensure it's hidden at start
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            isPlayerInTrigger = true;
            if (interactableObj != null)
            {
                interactableObj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (interactableObj != null)
            {
                interactableObj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
