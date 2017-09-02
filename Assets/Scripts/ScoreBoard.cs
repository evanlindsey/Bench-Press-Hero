using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class ScoreBoard : MonoBehaviour
{
    public GUISkin skin;
    public GUIStyle style;

    private ReturnObject returnObj;
    private Vector2 scrollPosition1, scrollPosition2, scrollPosition3;

    private float trialScore;
    private int maxScore, combineScore;
    private bool showNameWindow, postingMax, postingCombine, postingTrial;
    private string maxTitle, combineTitle, trialTitle, postTitle, maxList, combineList, trialList, nameInput;

    private const int nameLength = 15;

    // Hosting Environment (0 = Dev, 1 = Prod)
    public int environment = 1;
    // Score Service Endpoints
    private const string maxScores = "max";
    private const string trialScores = "trial";
    private const string combineScores = "combine";
    private const string queryString = @"?{""$sort"":{""score"":-1},""$limit"":100}";
    // Score Service URLs
    private const string development = "http://localhost:1337/";
    private const string production = "https://bench-press-hero.herokuapp.com/";
    private string[] services = new string[] { development, production };
    private string service;

    [Serializable]
    private struct ScoreObject
    {
        public string name;
        public string score;
        public string date;
    }
    [Serializable]
    private struct ReturnObject
    {
        public ScoreObject[] scores;
    }


    void Start()
    {
        service = services[environment];

        nameInput = "";

        // Get Scores
        StartCoroutine(GetScoreMax(false));
        StartCoroutine(GetScoreCombine(false));
        StartCoroutine(GetScoreTrial(false));

        // Scroll View Start Positions
        scrollPosition1 = Vector2.zero;
        scrollPosition2 = Vector2.zero;
        scrollPosition3 = Vector2.zero;

        // If Name Has Been Used, Fill Name Input
        if (PlayerPrefs.GetString("Name") != "")
            nameInput = PlayerPrefs.GetString("Name");

        // Set Current High Score
        trialScore = PlayerPrefs.GetFloat("TrialScore");
        combineScore = PlayerPrefs.GetInt("CombineScore");
        if (PlayerPrefs.GetInt("Level") > 0)
            maxScore = Convert.ToInt32(Scale.lbs[PlayerPrefs.GetInt("Level") - 1]);
        else
            maxScore = 0;

        // If No Previous Trial Score, Set Score at Limit
        if (PlayerPrefs.GetFloat("LastTrial") == 0)
            PlayerPrefs.SetFloat("LastTrial", 100000);
    }

    private IEnumerator GetScore(string board)
    {
        // Call Endpoint
        var www = new WWW(service + board + queryString);
        yield return www;

        // JSON Return
        string json = @"{ ""scores"": " + www.text + " }";
        returnObj = JsonUtility.FromJson<ReturnObject>(json);
    }

    // Get Score Coroutine
    private IEnumerator GetScoreMax(bool posting)
    {
        // If not Called from Post Score
        if (!posting)
            maxTitle = "FETCHING SCORES";

        // Fetch Scores
        yield return StartCoroutine(GetScore(maxScores));

        // Clear String
        maxList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in returnObj.scores)
        {
            // Append Name
            maxList += i + ". ";
            maxList += scoreObj.name.ToUpper() + ": ";

            // Translate weight
            int index = Array.IndexOf(Scale.lbs, scoreObj.score);
            string weight = Scale.lbs[index] + "/" + Scale.kgs[index];

            // Append Score
            maxList += weight + "\n";

            i++;
        }

        // Show Score Title
        maxTitle = "MAX OUT - TOP 100";
    }

    // Get Score Coroutine
    private IEnumerator GetScoreCombine(bool posting)
    {
        // If not Called from Post Score
        if (!posting)
            combineTitle = "FETCHING SCORES";

        // Fetch Scores
        yield return StartCoroutine(GetScore(combineScores));

        // Clear String
        combineList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in returnObj.scores)
        {
            // Append Name
            combineList += i + ". ";
            combineList += scoreObj.name.ToUpper() + ": ";

            // Append Score
            string rep = "REPS";
            if (scoreObj.score == "1")
                rep = "REP";
            combineList += scoreObj.score + " " + rep + "\n";

            i++;
        }

        // Show Score Title
        combineTitle = "COMBINE - TOP 100";
    }

    // Get Score Coroutine
    private IEnumerator GetScoreTrial(bool posting)
    {
        // If not Called from Post Score
        if (!posting)
            trialTitle = "FETCHING SCORES";

        // Fetch Scores
        yield return StartCoroutine(GetScore(trialScores));

        // Clear String
        trialList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in returnObj.scores)
        {
            // Append Name + Score
            trialList += i + ". ";
            trialList += scoreObj.name.ToUpper() + ": ";
            trialList += scoreObj.score + "\n";

            i++;
        }

        // Show Score Title
        trialTitle = "TIME TRIAL - TOP 100";
    }

    // Post Score Coroutine
    private IEnumerator PostScore(string mode, float score)
    {
        string setFloat = "";
        float setScore = 0;
        string board = "";
        if (mode == "Max")
        {
            maxTitle = "POSTING SCORE";
            setFloat = "LastMax";
            setScore = maxScore;
            board = maxScores;
        }
        else if (mode == "Combine")
        {
            combineTitle = "POSTING SCORE";
            setFloat = "LastCombine";
            setScore = combineScore;
            board = combineScores;
        }
        else if (mode == "Trial")
        {
            trialTitle = "POSTING SCORE";
            setFloat = "LastTrial";
            setScore = trialScore;
            board = trialScores;
        }

        // JSON to Post
        var scoreObj = new ScoreObject();
        scoreObj.name = nameInput;
        scoreObj.score = score.ToString();
        scoreObj.date = DateTime.Now.ToString();
        string json = JsonUtility.ToJson(scoreObj);
        var formData = Encoding.UTF8.GetBytes(json);

        // Post Score
        var postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-Type", "application/json");
        var www = new WWW(service + board, formData, postHeader);
        yield return www;

        // Save Name Input
        PlayerPrefs.SetString("Name", nameInput);

        // Save as Latest High Score
        PlayerPrefs.SetFloat(setFloat, setScore);

        // Reload Board
        if (mode == "Max")
            StartCoroutine(GetScoreMax(true));
        else if (mode == "Combine")
            StartCoroutine(GetScoreCombine(true));
        else if (mode == "Trial")
            StartCoroutine(GetScoreTrial(true));
    }

    private void Post(string mode)
    {
        if (mode == "Max")
        {
            if (!postingMax)
            {
                postTitle = "MAX";
                postingMax = true;
                postingCombine = false;
                postingTrial = false;
                showNameWindow = true;
                GUI.BringWindowToFront(4);
            }
            else
            {
                postingMax = false;
                showNameWindow = false;
            }
        }
        else if (mode == "Combine")
        {
            if (!postingCombine)
            {
                postTitle = "COMBINE";
                postingMax = false;
                postingCombine = true;
                postingTrial = false;
                showNameWindow = true;
                GUI.BringWindowToFront(4);
            }
            else
            {
                postingCombine = false;
                showNameWindow = false;
            }
        }
        else if (mode == "Trial")
        {
            if (!postingTrial)
            {
                postTitle = "TRIAL";
                postingMax = false;
                postingCombine = false;
                postingTrial = true;
                showNameWindow = true;
                GUI.BringWindowToFront(4);
            }
            else
            {
                postingTrial = false;
                showNameWindow = false;
            }
        }
    }

    // High Scores GUI
    void OnGUI()
    {
        // GUI Skin and Sizing
        GUI.skin = skin;
        style.alignment = TextAnchor.UpperLeft;
        skin.box.fontSize = Screen.width / 60;
        skin.button.fontSize = Screen.width / 65;

        // Max Score Window
        GUI.Window(1, new Rect(0, 0, Screen.width / 3, Screen.height / 1.74f), MaxWindow, maxTitle);
        // Combine Score Window
        GUI.Window(2, new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height / 1.74f), CombineWindow, combineTitle);
        // Trial Score Window
        GUI.Window(3, new Rect(Screen.width / 1.5f, 0, Screen.width / 3, Screen.height / 1.74f), TrialWindow, trialTitle);

        // Name Input Window
        if (showNameWindow)
        {
            GUI.Window(4, new Rect(Screen.width / 4, (Screen.height / 1.5f) - (Screen.height / 10) - ((Screen.height / 10) + 35), Screen.width / 2, (Screen.height / 10) + 38), NameWindow, "ENTER YOUR NAME - " + postTitle);
            GUI.BringWindowToFront(4);
        }

        // Combine Score Display
        GUI.Box(new Rect(0, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), maxScore.ToString());
        // Combine Score Display
        GUI.Box(new Rect((Screen.width / 2) - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), combineScore.ToString());
        // Trial Score Display
        GUI.Box(new Rect(Screen.width / 1.5f, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), trialScore.ToString());

        // Max Submit Button
        if (maxScore > PlayerPrefs.GetFloat("LastMax") && maxScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
                Post("Max");
        }
        else if (maxScore == 0)
            GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        else
            GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");

        // Combine Submit Button
        if (combineScore > PlayerPrefs.GetFloat("LastCombine") && combineScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
                Post("Combine");
        }
        else if (combineScore == 0)
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        else
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");

        // Trial Submit Button
        if (trialScore < PlayerPrefs.GetFloat("LastTrial") && trialScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
                Post("Trial");
        }
        else if (trialScore == 0)
            GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        else
            GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");
    }

    // Max Scores Window
    private void MaxWindow(int windowID)
    {
        // Window Skin / Sizing
        GUI.skin = skin;
        skin.label.fontSize = Screen.width / 100;
        skin.window.fontSize = Screen.width / 60;

        // Scroll View w/ Label
        scrollPosition1 = GUI.BeginScrollView(new Rect(0, Screen.height / 22, Screen.width / 3, Screen.height / 2), scrollPosition1, new Rect(0, 0, Screen.width / 3.2f, Screen.width * 1.35f));
        GUI.Label(new Rect(0, 0, Screen.width / 3, Screen.width * 1.35f), maxList);
        GUI.EndScrollView();
    }

    // Combine Scores Window
    private void CombineWindow(int windowID)
    {
        // Window Skin / Sizing
        GUI.skin = skin;
        skin.label.fontSize = Screen.width / 100;
        skin.window.fontSize = Screen.width / 60;

        // Scroll View w/ Label
        scrollPosition2 = GUI.BeginScrollView(new Rect(0, Screen.height / 22, Screen.width / 3, Screen.height / 2), scrollPosition2, new Rect(0, 0, Screen.width / 3.2f, Screen.width * 1.35f));
        GUI.Label(new Rect(0, 0, Screen.width / 3, Screen.width * 1.35f), combineList);
        GUI.EndScrollView();
    }

    // Trial Scores Window
    private void TrialWindow(int windowID)
    {
        // Winow Skin / Sizing
        GUI.skin = skin;
        skin.label.fontSize = Screen.width / 100;
        skin.window.fontSize = Screen.width / 60;

        // Scroll View w/ Label
        scrollPosition3 = GUI.BeginScrollView(new Rect(0, Screen.height / 22, Screen.width / 3, Screen.height / 2), scrollPosition3, new Rect(0, 0, Screen.width / 3.2f, Screen.width * 1.35f));
        GUI.Label(new Rect(0, 0, Screen.width / 3, Screen.width * 1.35f), trialList);
        GUI.EndScrollView();
    }

    // Name Input Window
    private void NameWindow(int windowID)
    {
        // Winow Skin / Sizing
        GUI.skin = skin;
        skin.window.fontSize = Screen.width / 60;
        skin.button.fontSize = Screen.width / 65;
        skin.textField.fontSize = Screen.width / 65;

        // Name Input Text Box
        GUI.SetNextControlName("Name_Input");
        nameInput = GUI.TextField(new Rect(12, 25, Screen.width / 3.15f, Screen.height / 10), nameInput, nameLength);

        // Submit Button
        if (GUI.Button(new Rect(Screen.width / 3.08f, 25, Screen.width / 6, Screen.height / 10), "SUBMIT"))
        {
            if (nameInput != "")
            {
                showNameWindow = false;

                if (postingMax)
                {
                    StartCoroutine(PostScore("Max", maxScore));
                    postingMax = false;
                }
                else if (postingCombine)
                {
                    StartCoroutine(PostScore("Combine", combineScore));
                    postingCombine = false;
                }
                else if (postingTrial)
                {
                    StartCoroutine(PostScore("Trial", trialScore));
                    postingTrial = false;
                }
            }
        }
    }
}
