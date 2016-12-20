using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance = null;
    public static Music Instance { get { return instance; } }

    private AudioSource audioSource;

    private bool pause;
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
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!pause)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Pause();
    }
}
