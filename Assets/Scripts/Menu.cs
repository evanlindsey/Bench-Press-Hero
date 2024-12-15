using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Music music;
    private ScoreBoard scoreBoard;
    private Image logo, trophy;
    private bool extra, scores;
    private string audioText, trophyText;

    public bool Extra { get { return extra; } set { extra = value; } }
    public bool Scores { get { return scores; } set { scores = value; } }
    public string AudioText { get { return audioText; } set { audioText = value; } }
    public string TrophyText { get { return trophyText; } set { trophyText = value; } }

    private const string NO_TROPHY_TXT = "NO\nWEIGHT\nMOVED!";
    private const string SOUND_ON_TXT = "SOUND:\nON";
    private const string SOUND_OFF_TXT = "SOUND:\nOFF";

    void Start()
    {
        var logoObj = GameObject.Find("Logo");
        if (logoObj)
        {
            logo = logoObj.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Logo object not found!");
        }

        var trophyObj = GameObject.Find("Trophy");
        if (trophyObj)
        {
            trophy = trophyObj.GetComponent<Image>();
            trophy.enabled = false;
        }
        else
        {
            Debug.LogError("Trophy object not found!");
        }

        var scoreBoardObj = GameObject.Find("ScoreBoard");
        if (scoreBoardObj)
        {
            scoreBoard = scoreBoardObj.GetComponent<ScoreBoard>();
            scoreBoard.enabled = false;
        }
        else
        {
            Debug.LogError("ScoreBoard object not found!");
        }

        var musicObj = GameObject.Find("_MUSIC");
        if (musicObj)
        {
            music = musicObj.GetComponent<Music>();
            SetMusic();
        }
        else
        {
            Debug.LogError("Music object not found!");
        }
    }

    private void SetTrophyText()
    {
        if (Storage.LastLevel == 0)
        {
            trophy.enabled = false;
            trophyText = NO_TROPHY_TXT;
        }
        else
        {
            var scaleIndex = Storage.LastLevel - 1;
            trophyText = $"{Scale.lbs[scaleIndex]} LB\n {Scale.kgs[scaleIndex]} KG";
        }
    }

    public void TrophyBtnPress()
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

    private void SetMusic()
    {
        if (Storage.SoundOff)
        {
            audioText = SOUND_OFF_TXT;
            music.Pause = true;
            AudioListener.volume = 0;
        }
        else
        {
            audioText = SOUND_ON_TXT;
            music.Pause = false;
            AudioListener.volume = 1f;
        }
    }

    public void MusicBtnPress()
    {
        if (Storage.SoundOff)
        {
            Storage.SoundOff = false;
        }
        else
        {
            Storage.SoundOff = true;
        }

        SetMusic();
    }

    public void ScoresBtnPress()
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
