using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLabScript : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Name of the scene to load
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
