using UnityEngine;

public class Barbell : MonoBehaviour
{
    [SerializeField] private float power = 100;
    [SerializeField] private float[] gravity = new float[12];
    [SerializeField] private Sprite[] barbells = new Sprite[12];

    private Level level;
    private Rigidbody2D rigidBody;

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

        var weightIndex = Storage.ActiveLevel - 1;

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity[weightIndex];

        GetComponent<SpriteRenderer>().sprite = barbells[weightIndex];
    }

    void Update()
    {
        if (level.Stop)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Bench()
    {
        if (level.Move)
        {
            rigidBody.AddForce(new Vector2(0, power));
        }
        else if (level.Lockout)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
