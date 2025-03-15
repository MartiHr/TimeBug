using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class PortalScript : MonoBehaviour
{
    [SerializeField] private AudioSource soundAffect;
    [SerializeField] private string sceneToLoad; // Name of the scene to load
    [SerializeField] private float scaleSpeed = 2f; // Speed of shrinking animation

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers the portal
        {
            soundAffect.Play();
            StartCoroutine(ShrinkAndLoadScene());
        }
    }

    private System.Collections.IEnumerator ShrinkAndLoadScene()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration of the shrink animation

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime * scaleSpeed;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        SceneManager.LoadScene(sceneToLoad);
    }
}
