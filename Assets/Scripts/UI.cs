using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GUISkin skin;
    public GUIStyle largeStyle, largeLeftStyle, menuStyle, levelStyle, barbellStyle;

    private Menu menu = null;
    private Level level = null;
    private Barbell barbell = null;
    private WeightSelect weightSelect = null;
    private bool levelAlive, menuAlive, barbellAlive, weightSelectAlive;

    private static UI instance = null;
    public static UI Instance { get { return instance; } }


    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void OnGUI()
    {
        #region "Styles"
        GUI.skin = skin;
        largeStyle.fontSize = Screen.width / 20;
        largeStyle.alignment = TextAnchor.MiddleCenter;
        largeLeftStyle.fontSize = Screen.width / 20;
        largeLeftStyle.alignment = TextAnchor.MiddleLeft;
        menuStyle.fontSize = Screen.width / 30;
        menuStyle.alignment = TextAnchor.MiddleCenter;
        levelStyle.fontSize = Screen.width / 50;
        levelStyle.alignment = TextAnchor.MiddleCenter;
        barbellStyle.fontSize = Screen.width / 55;
        barbellStyle.alignment = TextAnchor.MiddleCenter;
        #endregion

        #region "Menu"
        if (GameObject.Find("Menu"))
        {
            menu = GameObject.Find("Menu").GetComponent<Menu>();
            menuAlive = true;
        }
        else
            menuAlive = false;

        if (menuAlive)
        {
            skin.button.fontSize = Screen.width / 80;

            if (!menu.Scores)
            {
                if (GUI.Button(new Rect(0, (Screen.height / 1.76f), (Screen.width / 9.95f), (Screen.height / 10)), "TROPHY"))
                    menu.Trophy();
                if (GUI.Button(new Rect(Screen.width - (Screen.width / 9.95f), (Screen.height / 1.76f), (Screen.width / 9.95f), (Screen.height / 10)), menu.AudioText))
                    menu.Music();
            }

            if (GUI.Button(new Rect(0, (Screen.height / 1.5f), (Screen.width / 4.015f), (Screen.height / 3)), ""))
                SceneManager.LoadScene("WeightSelect");
            GUI.Label(new Rect(0, (Screen.height / 1.5f), (Screen.width / 4.015f), (Screen.height / 3)), "MAX\nOUT", menuStyle);

            if (GUI.Button(new Rect((Screen.width / 3.999f), (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), ""))
                menu.NewLevel(11);
            GUI.Label(new Rect(Screen.width / 3.999f, (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), "COMBINE\nMODE", menuStyle);

            if (GUI.Button(new Rect((Screen.width / 1.999f), (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), ""))
                menu.NewLevel(12);
            GUI.Label(new Rect(Screen.width / 1.999f, (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), "TIME\nTRIAL", menuStyle);

            if (GUI.Button(new Rect(Screen.width - (Screen.width / 4.01f), (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), ""))
                menu.HighScores();
            GUI.Label(new Rect(Screen.width - (Screen.width / 4.01f), (Screen.height / 1.5f), (Screen.width / 4.01f), (Screen.height / 3)), "HIGH\nSCORES", menuStyle);

            if (menu.Extra)
            {
                GUI.Label(new Rect((Screen.width / 5.8f), (Screen.height / 20), (Screen.width / 4), (Screen.height / 3)), menu.TrophyText, largeStyle);
                GUI.Label(new Rect((Screen.width / 1.74f), (Screen.height / 20), (Screen.width / 4), (Screen.height / 3)), "evan\nlindsey\n.net\n© 2016", largeStyle);
            }
        }
        #endregion

        #region "Level"
        if (GameObject.Find("Level"))
        {
            level = GameObject.Find("Level").GetComponent<Level>();
            levelAlive = true;
        }
        else
            levelAlive = false;


        if (levelAlive)
        {
            if (level.LeftAlign)
                GUI.Label(new Rect(Screen.width / 2.9f, 0, Screen.width / 2, Screen.height / 3), level.CenterText, largeLeftStyle);
            else
                GUI.Label(new Rect(Screen.width / 4, 0, Screen.width / 2, Screen.height / 3), level.CenterText, largeStyle);

            if (GUI.Button(new Rect(0, 0, (Screen.width / 5), (Screen.height / 6)), ""))
                level.MenuReturn();
            GUI.Label(new Rect(0, 0, (Screen.width / 5), (Screen.height / 6)), "QUIT\nATTEMPT", levelStyle);

            GUI.Box(new Rect(Screen.width - (Screen.width / 5), 0, (Screen.width / 5), (Screen.height / 6)), "");
            GUI.Label(new Rect(Screen.width - (Screen.width / 5), 0, (Screen.width / 5), (Screen.height / 6)), level.BoxText, levelStyle);
        }
        #endregion

        #region "Barbell"
        if (GameObject.Find("Barbell"))
        {
            barbell = GameObject.Find("Barbell").GetComponent<Barbell>();
            barbellAlive = true;
        }
        else
            barbellAlive = false;

        if (barbellAlive)
        {
            GUI.skin = skin;

            if (GUI.Button(new Rect((Screen.width / 2.25f), (Screen.height / 2.5f), (Screen.width / 9), (Screen.height / 6)), ""))
            {
                if (level.Stop)
                    level.Stop = false;
                else
                    barbell.Bench();
            }
            GUI.Label(new Rect((Screen.width / 2.25f), (Screen.height / 2.5f), (Screen.width / 9), (Screen.height / 6)), "POWER", barbellStyle);
        }
        #endregion

        #region "WeightSelect"
        if (GameObject.Find("WeightSelect"))
        {
            weightSelect = GameObject.Find("WeightSelect").GetComponent<WeightSelect>();
            weightSelectAlive = true;
        }
        else
            weightSelectAlive = false;

        if (weightSelectAlive)
        {
            GUI.Box(new Rect(0, Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), "");
            GUI.Label(new Rect(0, Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), "CHOOSE\nWEIGHT:", menuStyle);

            if (PlayerPrefs.GetInt("Level") >= 0)
            {
                if (GUI.Button(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(1);
                GUI.Label(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), Scale.lb1 + " LB\n" + Scale.kg1 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 1)
            {
                if (GUI.Button(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(2);
                GUI.Label(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 4), (Screen.width / 3), (Screen.height / 4)), Scale.lb2 + " LB\n" + Scale.kg2 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 2)
            {
                if (GUI.Button(new Rect(0, Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(3);
                GUI.Label(new Rect(0, Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), Scale.lb3 + " LB\n" + Scale.kg3 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 3)
            {
                if (GUI.Button(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(4);
                GUI.Label(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), Scale.lb4 + " LB\n" + Scale.kg4 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 4)
            {
                if (GUI.Button(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(5);
                GUI.Label(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 3), (Screen.width / 3), (Screen.height / 4)), Scale.lb5 + " LB\n" + Scale.kg5 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 5)
            {
                if (GUI.Button(new Rect(0, Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(6);
                GUI.Label(new Rect(0, Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), Scale.lb6 + " LB\n" + Scale.kg6 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 6)
            {
                if (GUI.Button(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(7);
                GUI.Label(new Rect((Screen.width / 2.999f), Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), Scale.lb7 + " LB\n" + Scale.kg7 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 7)
            {
                if (GUI.Button(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(8);
                GUI.Label(new Rect(Screen.width - (Screen.width / 3), Screen.height - ((Screen.height / 4) * 2), (Screen.width / 3), (Screen.height / 4)), Scale.lb8 + " LB\n" + Scale.kg8 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 8)
            {
                if (GUI.Button(new Rect(0, Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(9);
                GUI.Label(new Rect(0, Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), Scale.lb9 + " LB\n" + Scale.kg9 + " KG", menuStyle);
            }

            if (PlayerPrefs.GetInt("Level") >= 9)
            {
                if (GUI.Button(new Rect((Screen.width / 2.999f), Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), ""))
                    weightSelect.NewLevel(10);
                GUI.Label(new Rect((Screen.width / 2.999f), Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), Scale.lb10 + " LB\n" + Scale.kg10 + " KG", menuStyle);
            }

            if (GUI.Button(new Rect(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), ""))
                SceneManager.LoadScene("Menu");
            GUI.Label(new Rect(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4), (Screen.width / 3), (Screen.height / 4)), "MAIN\nMENU", menuStyle);
        }
        #endregion
    }
}
