using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GUISkin skin;
    [SerializeField] private GUIStyle largeStyle, largeLeftStyle, menuStyle, levelStyle, barbellStyle;

    private Menu menu;
    private Level level;
    private Barbell barbell;
    private WeightSelect weightSelect;
    private static UI instance;

    private const string TROPHY_BTN_TXT = "TROPHY";
    private const string COPYRIGHT_TXT = "evan\nlindsey\n.net\n© 2024";
    private const string MAX_BTN_TXT = "MAX\nOUT";
    private const string COMBINE_BTN_TXT = "COMBINE\nMODE";
    private const string TRIAL_BTN_TXT = "TIME\nTRIAL";
    private const string SCORE_BTN_TXT = "HIGH\nSCORES";
    private const string CHOOSE_WEIGHT_TXT = "CHOOSE\nWEIGHT:";
    private const string MENU_BTN_TXT = "MAIN\nMENU";
    private const string QUIT_BTN_TXT = "QUIT\nATTEMPT";
    private const string POWER_BTN_TXT = "POWER";
    private const string LB_LABEL = "LB";
    private const string KG_LABEL = "KG";

    public static UI Instance { get { return instance; } }

    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void UpdateStyles()
    {
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
    }

    void CreateButton(float x, float y, float width, float height, string buttonTxt, Action buttonAction)
    {
        if (GUI.Button(new Rect(x, y, width, height), buttonTxt))
        {
            buttonAction();
        }
    }

    void CreateBox(float x, float y, float width, float height)
    {
        GUI.Box(new Rect(x, y, width, height), "");
    }

    void CreateLabel(float x, float y, float width, float height, string labelText, GUIStyle labelStyle)
    {
        GUI.Label(new Rect(x, y, width, height), labelText, labelStyle);
    }

    void CreateBoxWithLabel(float x, float y, float width, float height, string labelText, GUIStyle labelStyle)
    {
        CreateBox(x, y, width, height);
        CreateLabel(x, y, width, height, labelText, labelStyle);
    }

    void CreateButtonWithLabel(float x, float y, float width, float height, string labelText, GUIStyle labelStyle, Action buttonAction)
    {
        CreateButton(x, y, width, height, "", buttonAction);
        CreateLabel(x, y, width, height, labelText, labelStyle);
    }

    void RenderMenu()
    {
        var menuObj = GameObject.Find("Menu");
        if (menuObj)
        {
            menu = menuObj.GetComponent<Menu>();

            skin.button.fontSize = Screen.width / 80;

            if (!menu.Scores)
            {
                CreateButton(0, Screen.height / 1.76f, Screen.width / 9.95f, Screen.height / 10, TROPHY_BTN_TXT, () =>
                {
                    menu.TrophyBtnPress();
                });

                CreateButton(Screen.width - (Screen.width / 9.95f), Screen.height / 1.76f, Screen.width / 9.95f, Screen.height / 10, menu.AudioText, () =>
                {
                    menu.MusicBtnPress();
                });
            }

            CreateButtonWithLabel(0, Screen.height / 1.5f, Screen.width / 4.015f, Screen.height / 3, MAX_BTN_TXT, menuStyle, () =>
            {
                Scene.LoadWeightSelect();
            });

            CreateButtonWithLabel(Screen.width / 3.999f, Screen.height / 1.5f, Screen.width / 4.01f, Screen.height / 3, COMBINE_BTN_TXT, menuStyle, () =>
            {
                Scene.LoadLevel(11);
            });

            CreateButtonWithLabel(Screen.width / 1.999f, Screen.height / 1.5f, Screen.width / 4.01f, Screen.height / 3, TRIAL_BTN_TXT, menuStyle, () =>
            {
                Scene.LoadLevel(12);
            });

            CreateButtonWithLabel(Screen.width - (Screen.width / 4.01f), Screen.height / 1.5f, Screen.width / 4.01f, Screen.height / 3, SCORE_BTN_TXT, menuStyle, () =>
            {
                menu.ScoresBtnPress();
            });

            if (menu.Extra)
            {
                CreateLabel(Screen.width / 5.8f, Screen.height / 6, Screen.width / 4, Screen.height / 3, menu.TrophyText, largeStyle);

                CreateLabel(Screen.width / 1.74f, Screen.height / 6, Screen.width / 4, Screen.height / 3, COPYRIGHT_TXT, largeStyle);
            }
        }
    }

    void RenderLevel()
    {
        var levelObj = GameObject.Find("Level");
        if (levelObj)
        {
            level = levelObj.GetComponent<Level>();

            if (level.LeftAlign)
            {
                CreateLabel(Screen.width / 2.9f, 0, Screen.width / 2, Screen.height / 3, level.CenterText, largeLeftStyle);
            }
            else
            {
                CreateLabel(Screen.width / 4, 0, Screen.width / 2, Screen.height / 3, level.CenterText, largeStyle);

            }

            CreateButtonWithLabel(0, 0, Screen.width / 5, Screen.height / 6, QUIT_BTN_TXT, levelStyle, () =>
            {
                level.MenuReturn();
            });

            CreateBoxWithLabel(Screen.width - (Screen.width / 5), 0, Screen.width / 5, Screen.height / 6, level.BoxText, levelStyle);
        }
    }

    void RenderBarbell()
    {
        var barbellObj = GameObject.Find("Barbell");
        if (barbellObj)
        {
            barbell = barbellObj.GetComponent<Barbell>();

            CreateButtonWithLabel(Screen.width / 2.25f, Screen.height / 2.5f, Screen.width / 9, Screen.height / 6, POWER_BTN_TXT, barbellStyle, () =>
            {
                if (level.Stop)
                {
                    level.Stop = false;
                }
                else
                {
                    barbell.Bench();
                }
            });
        }
    }

    void RenderWeightSelect()
    {
        var weightSelectObj = GameObject.Find("WeightSelect");
        if (weightSelectObj)
        {
            weightSelect = weightSelectObj.GetComponent<WeightSelect>();

            CreateBoxWithLabel(0, Screen.height - (Screen.height / 4 * 4), Screen.width / 3, Screen.height / 4, CHOOSE_WEIGHT_TXT, menuStyle);

            var lastLevel = Storage.LastLevel;
            if (lastLevel >= 0)
            {
                CreateButtonWithLabel(Screen.width / 2.999f, Screen.height - (Screen.height / 4 * 4), Screen.width / 3, Screen.height / 4, $"{Scale.lb1} {LB_LABEL}\n{Scale.kg1} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(1);
                });
            }

            if (lastLevel >= 1)
            {
                CreateButtonWithLabel(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4 * 4), Screen.width / 3, Screen.height / 4, $"{Scale.lb2} {LB_LABEL}\n{Scale.kg2} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(2);
                });
            }

            if (lastLevel >= 2)
            {
                CreateButtonWithLabel(0, Screen.height - (Screen.height / 4 * 3), Screen.width / 3, Screen.height / 4, $"{Scale.lb3} {LB_LABEL}\n{Scale.kg3} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(3);
                });
            }

            if (lastLevel >= 3)
            {
                CreateButtonWithLabel(Screen.width / 2.999f, Screen.height - (Screen.height / 4 * 3), Screen.width / 3, Screen.height / 4, $"{Scale.lb4} {LB_LABEL}\n{Scale.kg4} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(4);
                });
            }

            if (lastLevel >= 4)
            {
                CreateButtonWithLabel(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4 * 3), Screen.width / 3, Screen.height / 4, $"{Scale.lb5} {LB_LABEL}\n{Scale.kg5} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(5);
                });
            }

            if (lastLevel >= 5)
            {
                CreateButtonWithLabel(0, Screen.height - (Screen.height / 4 * 2), Screen.width / 3, Screen.height / 4, $"{Scale.lb6} {LB_LABEL}\n{Scale.kg6} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(6);
                });
            }

            if (lastLevel >= 6)
            {
                CreateButtonWithLabel(Screen.width / 2.999f, Screen.height - (Screen.height / 4 * 2), Screen.width / 3, Screen.height / 4, $"{Scale.lb7} {LB_LABEL}\n{Scale.kg7} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(7);
                });
            }

            if (lastLevel >= 7)
            {
                CreateButtonWithLabel(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4 * 2), Screen.width / 3, Screen.height / 4, $"{Scale.lb8} {LB_LABEL}\n{Scale.kg8} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(8);
                });
            }

            if (lastLevel >= 8)
            {
                CreateButtonWithLabel(0, Screen.height - (Screen.height / 4), Screen.width / 3, Screen.height / 4, $"{Scale.lb9} {LB_LABEL}\n{Scale.kg9} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(9);
                });
            }

            if (lastLevel >= 9)
            {
                CreateButtonWithLabel(Screen.width / 2.999f, Screen.height - (Screen.height / 4), Screen.width / 3, Screen.height / 4, $"{Scale.lb10} {LB_LABEL}\n{Scale.kg10} {KG_LABEL}", menuStyle, () =>
                {
                    weightSelect.NewLevel(10);
                });
            }

            CreateButtonWithLabel(Screen.width - (Screen.width / 3), Screen.height - (Screen.height / 4), Screen.width / 3, Screen.height / 4, MENU_BTN_TXT, menuStyle, () =>
            {
                Scene.LoadMenu();
            });
        }
    }

    void OnGUI()
    {
        UpdateStyles();

        RenderMenu();

        RenderLevel();

        RenderBarbell();

        RenderWeightSelect();
    }
}
