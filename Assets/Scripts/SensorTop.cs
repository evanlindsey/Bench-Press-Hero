using UnityEngine;

public class SensorTop : MonoBehaviour
{
    public float increaseRate = 0.025f;

    private Level level;
    private Rigidbody2D barbellRigidBody;


    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<Level>();

        barbellRigidBody = GameObject.Find("Barbell").GetComponent<Barbell>().GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Barbell")
        {
            if (PlayerPrefs.GetInt("Active") == 11)
            {
                level.Move = false;
                barbellRigidBody.gravityScale = barbellRigidBody.gravityScale + increaseRate;
                level.RepCount++;
            }
            else if (PlayerPrefs.GetInt("Active") == 12)
            {
                level.Move = false;
                barbellRigidBody.gravityScale = barbellRigidBody.gravityScale + increaseRate;
                level.TimeRep++;

                if (level.TimeRep == 10)
                {
                    level.Lockout = true;
                    level.Goal(PlayerPrefs.GetInt("Active"));
                }
            }
            else
            {
                level.Move = false;
                level.Lockout = true;
                level.Goal(PlayerPrefs.GetInt("Active"));
            }
        }
    }
}
