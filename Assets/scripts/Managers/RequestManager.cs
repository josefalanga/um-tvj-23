using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void RequestFinished();

public class RequestManager : MonoBehaviour
{
    static RequestManager _instance;
    public static RequestManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RequestManager>();
            }

            if (_instance == null)
            {
                _instance = new GameObject("RequestManager").AddComponent<RequestManager>();
            }

            return _instance;
        }
    }

    public const string baseURL = "http://um-tecvg-23.herokuapp.com/";

    public event RequestFinished OnRequestFinished;

    int pendingRequests = 0;

    void Awake()
    {
        _instance = this;

        OnRequestFinished += () =>
        {
            Debug.Log("All Requests Finished!");
        };
    }

    public void DownloadText(string url, Action<string> OnSuccess, Action<string> OnError)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        StartCoroutine(SendRequest(uwr, (DownloadHandler result) =>
        {
            OnSuccess(result.text);
        }, OnError));
    }

    public void DownloadTexture2D(string url, Action<Texture2D> OnSuccess, Action<string> OnError)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        StartCoroutine(SendRequest(uwr, (DownloadHandler result) =>
        {
            OnSuccess(DownloadHandlerTexture.GetContent(uwr));
        }, OnError));
    }

    public void SendRequest(UnityWebRequest webRequest, Action OnSuccess, Action<string> OnError)
    {
        StartCoroutine(SendRequest(webRequest, (DownloadHandler result) =>
        {
            OnSuccess();
        }, OnError));
    }

    IEnumerator SendRequest(UnityWebRequest webRequest, Action<DownloadHandler> OnSuccess, Action<string> OnError)
    {
        if (string.IsNullOrEmpty(webRequest.url))
        {
            OnError("La url está vacía.");
        }
        else
        {
            pendingRequests++;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                OnError(webRequest.error);
            }
            else
            {
                OnSuccess(webRequest.downloadHandler);
            }

            pendingRequests--;

            if (pendingRequests < 1)
            {
                OnRequestFinished();
            }
        }
    }
}
