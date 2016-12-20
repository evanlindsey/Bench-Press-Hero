using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private Music music;

    private string repText, timeText;
    private float gameTimer, keepTimer;

    private int repCount;
    private float timeRep;
    private string boxText, centerText;
    private bool stop, move, lockout, leftAlign;
    public int RepCount { get { return repCount; } set { repCount = value; } }
    public float TimeRep { get { return timeRep; } set { timeRep = value; } }
    public string BoxText { get { return boxText; } set { boxText = value; } }
    public string CenterText { get { return centerText; } set { centerText = value; } }
    public bool Stop { get { return stop; } set { stop = value; } }
    public bool Move { get { return move; } set { move = value; } }
    public bool Lockout { get { return lockout; } set { lockout = value; } }
    public bool LeftAlign { get { return leftAlign; } set { leftAlign = value; } }

    void Awake()
    {
        stop = true;
        move = false;
        lockout = false;
        leftAlign = false;

        music = GameObject.Find("_MUSIC").GetComponent<Music>();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Active") == 11)
        {
            if (stop)
                centerText = "HIT AS MANY\nREPS AS POSSIBLE";
            else
                centerText = "REPS: " + repCount.ToString();

            if (repCount > PlayerPrefs.GetInt("CombineScore"))
                PlayerPrefs.SetInt("CombineScore", repCount);

            boxText = "COMBINE";
        }
        else if (PlayerPrefs.GetInt("Active") == 12)
        {
            if (stop)
            {
                centerText = "HIT 10 REPS\nAS FAST AS POSSIBLE";
                repText = timeRep + " of 10";
                timeText = "00.00";
            }
            else
            {
                if (timeRep < 10)
                {
                    leftAlign = true;
                    gameTimer += Time.deltaTime;
                    centerText = timeText;
                }
                else
                {
                    leftAlign = false;
                    keepTimer = gameTimer;
                    centerText = "GOAL!\n" + timeText;

                    if (PlayerPrefs.GetFloat("TrialScore") == 0)
                        PlayerPrefs.SetFloat("TrialScore", keepTimer);
                    else if (gameTimer < PlayerPrefs.GetFloat("TrialScore"))
                        PlayerPrefs.SetFloat("TrialScore", keepTimer);
                }

                if (gameTimer < 10)
                    timeText = "0" + gameTimer.ToString("F3");
                else
                    timeText = gameTimer.ToString("F3");

                repText = timeRep + " of 10";
            }

            boxText = repText;
        }
        else
        {
            if (stop)
                centerText = "TAP POWER\nWHEN READY";
            else if (lockout)
            {
                if (PlayerPrefs.GetInt("Active") == 10)
                    centerText = "WINNER!";
                else
                    centerText = "LOCKOUT!";
            }
            else
                centerText = "";

            boxText = Scale.lbs[PlayerPrefs.GetInt("Active") - 1] + " / " + Scale.kgs[PlayerPrefs.GetInt("Active") - 1];
        }
    }

    public void MenuReturn()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
            music.Pause = false;

        if (PlayerPrefs.GetInt("Active") == 11 || PlayerPrefs.GetInt("Active") == 12)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("WeightSelect");
    }

    public void Goal(int level)
    {
        if (PlayerPrefs.GetInt("Level") < level && level != 12)
            PlayerPrefs.SetInt("Level", level);

        music.Pause = true;
        GetComponent<AudioSource>().Play();

        Invoke("MenuReturn", 3f);
    }
}
