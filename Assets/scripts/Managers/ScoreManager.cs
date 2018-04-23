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
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        formData.Add(new MultipartFormDataSection(string.Format("deviceId={0}", SystemInfo.deviceUniqueIdentifier)));
        formData.Add(new MultipartFormDataSection(string.Format("playerName={0}", name)));
        formData.Add(new MultipartFormDataSection(string.Format("score={0}", score.ToString())));

        UnityWebRequest uwr = UnityWebRequest.Post(leaderboardPath, formData);

        RequestManager.instance.SendRequest(uwr, () =>
        {
            Debug.Log("Score sent successfully!");
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
