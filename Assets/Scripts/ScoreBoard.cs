using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private GUISkin skin;
    [SerializeField] private GUIStyle style;

    private Vector2 scrollPosition1, scrollPosition2, scrollPosition3;
    private float trialScore;
    private int maxScore, combineScore;
    private bool hasNetworkConnection, showNameWindow, postingMax, postingCombine, postingTrial;
    private string maxTitle, combineTitle, trialTitle, postTitle, maxList, combineList, trialList, nameInput;

    private const int NAME_LENGTH = 10;
    private const string MAX_SCOREBOARD_ID = "max";
    private const string COMBINE_SCOREBOARD_ID = "combine";
    private const string TRIAL_SCOREBOARD_ID = "trial";
    private const string NO_CNXN_TXT = "NO NETWORK CONNECTION";
    private const string FETCHING_SCORES_TXT = "FETCHING SCORES";
    private const string POSTING_SCORE_TXT = "POSTING SCORE";
    private const string MAX_TITLE = "MAX OUT - TOP 100";
    private const string COMBINE_TITLE = "COMBINE - TOP 100";
    private const string TRIAL_TITLE = "TIME TRIAL - TOP 100";

    private enum GameMode
    {
        Max,
        Combine,
        Trial
    }

    private string GetScoreBoardId(GameMode mode)
    {
        return mode switch
        {
            GameMode.Max => MAX_SCOREBOARD_ID,
            GameMode.Combine => COMBINE_SCOREBOARD_ID,
            GameMode.Trial => TRIAL_SCOREBOARD_ID,
            _ => "",
        };
    }

    async void Awake()
    {
        hasNetworkConnection = Auth.CheckNetworkStatus();

        if (hasNetworkConnection)
        {
            await Auth.SignInAnonymously();

            // If Name Has Been Used, Fill Name Input
            if (Storage.Username != "")
            {
                nameInput = Storage.Username;
            }
            else
            {
                nameInput = "";
            }

            // Get Scores
            await LoadBoards();

            // Scroll View Start Positions
            scrollPosition1 = Vector2.zero;
            scrollPosition2 = Vector2.zero;
            scrollPosition3 = Vector2.zero;

            // Set Current High Scores
            trialScore = Storage.TrialScore;
            combineScore = Storage.CombineScore;
            if (Storage.LastLevel > 0)
            {
                var levelIndex = Storage.LastLevel - 1;
                maxScore = Convert.ToInt32(Scale.lbs[levelIndex]);
            }
            else
            {
                maxScore = 0;
            }

            // If No Previous Trial Score, Set Score at Limit
            if (Storage.SavedTrial == 0)
            {
                Storage.SavedTrial = 100000;
            }
        }
        else
        {
            maxTitle = NO_CNXN_TXT;
            combineTitle = NO_CNXN_TXT;
            trialTitle = NO_CNXN_TXT;
        }
    }

    async Task LoadBoards()
    {
        await Task.WhenAll(GetScore(GameMode.Max, false), GetScore(GameMode.Combine, false), GetScore(GameMode.Trial, false));
    }

    private void DisplayScoreMax(List<LeaderboardEntry> results)
    {
        // Clear String
        maxList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in results)
        {
            // Translate weight
            int index = Array.IndexOf(Scale.lbs, scoreObj.Score.ToString());
            string weight = $"{Scale.lbs[index]}/{Scale.kgs[index]}";

            // Append Name + Score
            maxList += $"{i}. {scoreObj.PlayerName.ToUpper()}: {weight}\n";

            i++;
        }

        // Show Score Title
        maxTitle = MAX_TITLE;
    }

    private void DisplayScoreCombine(List<LeaderboardEntry> results)
    {
        // Clear String
        combineList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in results)
        {
            // Get Rep Text
            string repTxt = "REP";
            if (scoreObj.Score > 1)
            {
                repTxt += "S";
            }

            // Append Name + Score
            combineList += $"{i}. {scoreObj.PlayerName.ToUpper()}: {scoreObj.Score} {repTxt}\n";

            i++;
        }

        // Show Score Title
        combineTitle = COMBINE_TITLE;
    }

    private void DisplayScoreTrial(List<LeaderboardEntry> results)
    {
        // Clear String
        trialList = "";

        // Loop Through Return
        int i = 1;
        foreach (var scoreObj in results)
        {
            // Append Name + Score
            trialList += $"{i}. {scoreObj.PlayerName.ToUpper()}: {scoreObj.Score}\n";

            i++;
        }

        // Show Score Title
        trialTitle = TRIAL_TITLE;
    }

    private async Task GetScore(GameMode mode, bool posting)
    {
        // If Not Posting, Set Fetching Title
        if (!posting)
        {
            switch (mode)
            {
                case GameMode.Max:
                    maxTitle = FETCHING_SCORES_TXT;
                    break;
                case GameMode.Combine:
                    combineTitle = FETCHING_SCORES_TXT;
                    break;
                case GameMode.Trial:
                    trialTitle = FETCHING_SCORES_TXT;
                    break;
            }
        }

        // Get Scores
        var scoreBoardId = GetScoreBoardId(mode);
        var returnObj = await LeaderboardsService.Instance.GetScoresAsync(scoreBoardId);

        // Parse Output
        switch (mode)
        {
            case GameMode.Max:
                DisplayScoreMax(returnObj.Results);
                break;
            case GameMode.Combine:
                DisplayScoreCombine(returnObj.Results);
                break;
            case GameMode.Trial:
                DisplayScoreTrial(returnObj.Results);
                break;
        }
    }

    private async Task PostScore(GameMode mode)
    {
        // Save Name Input
        Storage.Username = nameInput;
        await AuthenticationService.Instance.UpdatePlayerNameAsync(nameInput);

        double score = 0;
        if (mode == GameMode.Max)
        {
            maxTitle = POSTING_SCORE_TXT;

            // Save as Latest High Score
            score = maxScore;
            Storage.SavedMax = (int)score;
        }
        else if (mode == GameMode.Combine)
        {
            combineTitle = POSTING_SCORE_TXT;

            // Save as Latest High Score
            score = combineScore;
            Storage.SavedCombine = (int)score;
        }
        else if (mode == GameMode.Trial)
        {
            trialTitle = POSTING_SCORE_TXT;

            // Save as Latest High Score
            score = trialScore;
            Storage.SavedTrial = (float)score;
        }

        // Post Score
        var scoreBoardId = GetScoreBoardId(mode);
        await LeaderboardsService.Instance.AddPlayerScoreAsync(scoreBoardId, score);

        // Reload Boards
        await LoadBoards();
    }

    private void IsPosting(GameMode mode)
    {
        if (mode == GameMode.Max)
        {
            if (!postingMax)
            {
                postTitle = MAX_SCOREBOARD_ID.ToUpper();
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
        else if (mode == GameMode.Combine)
        {
            if (!postingCombine)
            {
                postTitle = COMBINE_SCOREBOARD_ID.ToUpper();
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
        else if (mode == GameMode.Trial)
        {
            if (!postingTrial)
            {
                postTitle = TRIAL_SCOREBOARD_ID.ToUpper();
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

        // Max Score Display
        GUI.Box(new Rect(0, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), maxScore.ToString());
        // Combine Score Display
        GUI.Box(new Rect((Screen.width / 2) - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), combineScore.ToString());
        // Trial Score Display
        GUI.Box(new Rect(Screen.width / 1.5f, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), trialScore.ToString());

        // Max Submit Button
        if (maxScore > Storage.SavedMax && maxScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
            {
                IsPosting(GameMode.Max);
            }
        }
        else if (maxScore == 0)
        {
            GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        }
        else
        {
            GUI.Button(new Rect(Screen.width / 6, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");
        }

        // Combine Submit Button
        if (combineScore > Storage.SavedCombine && combineScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
            {
                IsPosting(GameMode.Combine);
            }
        }
        else if (combineScore == 0)
        {
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        }
        else
        {
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");
        }

        // Trial Submit Button
        if (trialScore < Storage.SavedTrial && trialScore > 0)
        {
            if (GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POST"))
            {
                IsPosting(GameMode.Trial);
            }
        }
        else if (trialScore == 0)
        {
            GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "NO SCORE");
        }
        else
        {
            GUI.Button(new Rect(Screen.width - (Screen.width / 6), Screen.height / 1.76f, Screen.width / 6, Screen.height / 10), "POSTED!");
        }
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
    private async void NameWindow(int windowID)
    {
        // Winow Skin / Sizing
        GUI.skin = skin;
        skin.window.fontSize = Screen.width / 60;
        skin.button.fontSize = Screen.width / 65;
        skin.textField.fontSize = Screen.width / 65;

        // Name Input Text Box
        GUI.SetNextControlName("Name_Input");
        nameInput = GUI.TextField(new Rect(12, 25, Screen.width / 3.15f, Screen.height / 10), nameInput, NAME_LENGTH);

        // Submit Button
        if (GUI.Button(new Rect(Screen.width / 3.08f, 25, Screen.width / 6, Screen.height / 10), "SUBMIT"))
        {
            if (nameInput != "")
            {
                showNameWindow = false;

                if (postingMax)
                {
                    await PostScore(GameMode.Max);
                    postingMax = false;
                }
                else if (postingCombine)
                {
                    await PostScore(GameMode.Combine);
                    postingCombine = false;
                }
                else if (postingTrial)
                {
                    await PostScore(GameMode.Trial);
                    postingTrial = false;
                }
            }
        }
    }
}
