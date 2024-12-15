using UnityEngine;

public class SensorTop : MonoBehaviour
{
    [SerializeField] private float increaseRate = 0.025f;

    private Level level;
    private Rigidbody2D barbellRigidBody;

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

        var barbellObj = GameObject.Find("Barbell");
        if (barbellObj)
        {
            barbellRigidBody = barbellObj.GetComponent<Barbell>().GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Barbell object not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        var activeLevel = Storage.ActiveLevel;
        if (c.gameObject.CompareTag("Barbell"))
        {
            if (activeLevel < 11)
            {
                level.Move = false;
                level.Lockout = true;
                level.Goal(activeLevel);
            }
            else if (activeLevel == 11)
            {
                level.Move = false;
                barbellRigidBody.gravityScale += increaseRate;
                level.RepCount++;
            }
            else if (activeLevel == 12)
            {
                level.Move = false;
                barbellRigidBody.gravityScale += increaseRate;
                level.TimeRep++;

                if (level.TimeRep == 10)
                {
                    level.Lockout = true;
                    level.Goal(activeLevel);
                }
            }
        }
    }
}
