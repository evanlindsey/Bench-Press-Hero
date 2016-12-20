using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Music music;
    private ScoreBoard scoreBoard;
    private GUITexture logo, trophy;

    private bool extra, scores;
    private string audioText, trophyText;
    public bool Extra { get { return extra; } set { extra = value; } }
    public bool Scores { get { return scores; } set { scores = value; } }
    public string AudioText { get { return audioText; } set { audioText = value; } }
    public string TrophyText { get { return trophyText; } set { trophyText = value; } }


    void Awake()
    {
        if (GameObject.Find("Logo"))
            logo = GameObject.Find("Logo").GetComponent<GUITexture>();

        if (GameObject.Find("Trophy"))
        {
            trophy = GameObject.Find("Trophy").GetComponent<GUITexture>();
            trophy.enabled = false;
        }

        if (GameObject.Find("ScoreBoard"))
        {
            scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoard>();
            scoreBoard.enabled = false;
        }

        music = GameObject.Find("_MUSIC").GetComponent<Music>();
        SetMusic();
    }

    private void SetTrophyText()
    {
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            trophy.enabled = false;
            trophyText = "NO\nWEIGHT\nMOVED!";
        }
        else
            trophyText = Scale.lbs[PlayerPrefs.GetInt("Level") - 1] + " LB\n" + Scale.kgs[PlayerPrefs.GetInt("Level") - 1] + " KG";
    }

    private void SetMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            audioText = "SOUND:\nOFF";
            music.Pause = true;
            AudioListener.volume = 0;
        }
        else if (PlayerPrefs.GetInt("Music") == 1)
        {
            audioText = "SOUND:\nON";
            music.Pause = false;
            AudioListener.volume = 1f;
        }
    }

    public void NewLevel(int number)
    {
        PlayerPrefs.SetInt("Active", number);
        SceneManager.LoadScene("Level");
    }

    public void Trophy()
    {
        if (extra)
        {
            trophy.enabled = false;
            logo.enabled = true;
            extra = false;
        }
        else
        {
            trophy.enabled = true;
            logo.enabled = false;
            scores = false;
            extra = true;

            SetTrophyText();
        }
    }

    public void Music()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
            PlayerPrefs.SetInt("Music", 1);
        else if (PlayerPrefs.GetInt("Music") == 1)
            PlayerPrefs.SetInt("Music", 0);

        SetMusic();
    }

    public void HighScores()
    {
        if (!scores)
        {
            scoreBoard.enabled = true;
            trophy.enabled = false;
            logo.enabled = false;
            scores = true;
            extra = false;
        }
        else
        {
            scoreBoard.enabled = false;
            logo.enabled = true;
            scores = false;
        }
    }
}
