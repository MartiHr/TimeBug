using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public CutsceneManager cutsceneManager;

    void Start()
    {
        // Automatically start the cutscene when the scene loads
        cutsceneManager.StartCutscene();
    }
}