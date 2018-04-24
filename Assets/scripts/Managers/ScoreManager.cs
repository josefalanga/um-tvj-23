using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public LeaderboardModel leaderboardModel;

    string leaderboardPath { get { return Path.Combine(RequestManager.baseURL, "leaderboard.php"); } }

    public void SendScore(string name, int score, Action<string> OnError)
    {
        WWWForm form = new WWWForm();

        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);
        form.AddField("playerName", name);
        form.AddField("score", score);

        UnityWebRequest uwr = UnityWebRequest.Post(leaderboardPath, form);

        uwr.chunkedTransfer = false;

        RequestManager.instance.SendRequest(uwr, () =>
        {
            Debug.Log(uwr.downloadHandler.text);
        }, OnError);
    }

    public void GetLeaderboard(Action<LeaderboardModel> OnSuccess, Action<string> OnError)
    {
        RequestManager.instance.DownloadText(leaderboardPath, (string result) =>
        {
            JsonUtility.FromJsonOverwrite(result, leaderboardModel);
            OnSuccess(leaderboardModel);
        }, OnError);
    }
}
