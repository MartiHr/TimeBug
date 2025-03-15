using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    //public AudioSource SFXSource;

    public AudioClip background;
    public AudioClip portal;

    public static GameAudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
