using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource audioSource;
    private static Music instance;
    private bool pause;

    public static Music Instance { get { return instance; } }
    public bool Pause { get { return pause; } set { pause = value; } }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!pause)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
}
