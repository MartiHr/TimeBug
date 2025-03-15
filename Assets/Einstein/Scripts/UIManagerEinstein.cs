using UnityEngine;
using System.Collections;
using System;

public class UIManagerEinstein : MonoBehaviour
{
    public GameObject uiElement; // Assign this in the Inspector
    public GameObject sittingPlayer;
    public GameObject player;
    public GameObject thirdPersonCamera;
    public GameObject secondPersonCamera;
    public GameObject portal;
    public GameObject chat;
    private bool isPlayerInZone = false; // Track if player is inside the trigger zone

    void Start()
    {
        if (uiElement != null)
            uiElement.SetActive(false); // Hide UI at the start
        sittingPlayer.SetActive(false);
        secondPersonCamera.SetActive(false);
        portal.SetActive(false);
        chat.SetActive(false);
    }

    void Update()
    {
        // Check if "E" is pressed inside the zone
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false); // Hide UI element
                sittingPlayer.SetActive(true);
                player.SetActive(false);
                thirdPersonCamera.SetActive(false);
                secondPersonCamera.SetActive(true);
                chat.SetActive(true);// Call Coroutine to stop animation after 3 seconds
            }
        }
    }

    public void StopAnimationOnClick()
    {
        sittingPlayer.SetActive(false);
        player.SetActive(true);
        thirdPersonCamera.SetActive(true);
        secondPersonCamera.SetActive(false);
        portal.SetActive(true);
        chat.SetActive(false);
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
