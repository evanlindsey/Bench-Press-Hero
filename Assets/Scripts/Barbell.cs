using UnityEngine;

public class Barbell : MonoBehaviour
{
    public float power = 100;
    public float[] gravity = new float[12];
    public Sprite[] barbells = new Sprite[12];

    private Level level;
    private Rigidbody2D rigidBody;


    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<Level>();

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity[PlayerPrefs.GetInt("Active") - 1];

        GetComponent<SpriteRenderer>().sprite = barbells[PlayerPrefs.GetInt("Active") - 1];
    }

    void Update()
    {
        if (level.Stop)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Bench()
    {
        if (level.Move)
            rigidBody.AddForce(new Vector2(0, power));
        else if (level.Lockout)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
