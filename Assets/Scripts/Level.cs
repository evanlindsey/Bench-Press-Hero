using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    private Music music;
    private int repCount;
    private float gameTimer, keepTimer, timeRep;
    private string repText, timeText, boxText, centerText;
    private bool stop, move, lockout, leftAlign;

    public int RepCount { get { return repCount; } set { repCount = value; } }
    public float TimeRep { get { return timeRep; } set { timeRep = value; } }
    public string BoxText { get { return boxText; } set { boxText = value; } }
    public string CenterText { get { return centerText; } set { centerText = value; } }
    public bool Stop { get { return stop; } set { stop = value; } }
    public bool Move { get { return move; } set { move = value; } }
    public bool Lockout { get { return lockout; } set { lockout = value; } }
    public bool LeftAlign { get { return leftAlign; } set { leftAlign = value; } }

    void Start()
    {
        stop = true;
        move = false;
        lockout = false;
        leftAlign = false;

        var musicObj = GameObject.Find("_MUSIC");
        if (musicObj)
        {
            music = musicObj.GetComponent<Music>();
        }
        else
        {
            Debug.LogError("Music object not found!");
        }
    }

    void Update()
    {
        var activeLevel = Storage.ActiveLevel;
        if (activeLevel < 11)
        {
            {
                if (stop)
                {
                    centerText = "TAP POWER\nWHEN READY";
                }
                else if (lockout)
                {
                    if (activeLevel == 10)
                    {
                        centerText = "WINNER!";
                    }
                    else
                    {
                        centerText = "LOCKOUT!";
                    }
                }
                else
                {
                    centerText = "";
                }

                var scaleIndex = activeLevel - 1;
                boxText = $"{Scale.lbs[scaleIndex]} / {Scale.kgs[scaleIndex]}";
            }
        }
        else if (activeLevel == 11)
        {
            if (stop)
            {
                centerText = "HIT AS MANY\nREPS AS POSSIBLE";
            }
            else
            {
                centerText = $"REPS: {repCount}";
            }

            if (repCount > Storage.CombineScore)
            {
                Storage.CombineScore = repCount;
            }

            boxText = "COMBINE";
        }
        else if (activeLevel == 12)
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

                    if (Storage.TrialScore == 0 || gameTimer < Storage.TrialScore)
                    {
                        Storage.TrialScore = (float)Math.Round(keepTimer, 4);
                    }
                }

                if (gameTimer < 10)
                {
                    timeText = "0" + gameTimer.ToString("F4");
                }
                else
                {
                    timeText = gameTimer.ToString("F4");
                }

                repText = timeRep + " of 10";
            }

            boxText = repText;
        }

    }

    public void MenuReturn()
    {
        if (!Storage.SoundOff)
        {
            music.Pause = false;
        }

        var activeLevel = Storage.ActiveLevel;
        if (activeLevel == 11 || activeLevel == 12)
        {
            Scene.LoadMenu();
        }
        else
        {
            Scene.LoadWeightSelect();
        }
    }

    public void Goal(int level)
    {
        if (Storage.LastLevel < level && level != 12)
        {
            Storage.LastLevel = level;
        }

        music.Pause = true;
        GetComponent<AudioSource>().Play();

        Invoke(nameof(MenuReturn), 3f);
    }
}
