using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private static MainCamera instance = null;
    public static MainCamera Instance { get { return instance; } }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
