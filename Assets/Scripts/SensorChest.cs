using UnityEngine;

public class SensorChest : MonoBehaviour
{
    private Level level;


    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<Level>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Barbell")
        {
            level.Lockout = false;
            level.Move = true;
        }
    }
}
