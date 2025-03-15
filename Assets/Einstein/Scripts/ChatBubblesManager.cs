using System.Collections.Generic;
using UnityEngine;

public class ChatBubblesManager : MonoBehaviour
{
    private List<GameObject> chatBubbles = new List<GameObject>();
    private int currentIndex = 0;

    void Start()
    {
        // Get all child objects and store them in the list
        foreach (Transform child in transform)
        {
            chatBubbles.Add(child.gameObject);
            child.gameObject.SetActive(false); // Start with all disabled
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleChatBubbles();
        }
    }

    private void ToggleChatBubbles()
    {
        // Disable the previous chat bubble
        if (currentIndex > 0)
        {
            chatBubbles[currentIndex - 1].SetActive(false);
        }

        // Enable the current chat bubble
        if (currentIndex < chatBubbles.Count)
        {
            chatBubbles[currentIndex].SetActive(true);
            currentIndex++;
        }
        else
        {
            // Reset to start over
            currentIndex = 0;
        }
    }
}
