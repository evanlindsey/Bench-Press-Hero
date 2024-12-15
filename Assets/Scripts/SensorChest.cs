using UnityEngine;

public class SensorChest : MonoBehaviour
{
    private Level level;

    void Start()
    {
        var levelObj = GameObject.Find("Level");
        if (levelObj)
        {
            level = levelObj.GetComponent<Level>();
        }
        else
        {
            Debug.LogError("Level object not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Barbell"))
        {
            level.Lockout = false;
            level.Move = true;
        }
    }
}
