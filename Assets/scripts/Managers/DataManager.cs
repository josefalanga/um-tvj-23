using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public DataModel dataModel;

    Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    string dataModelPath { get { return Path.Combine(RequestManager.baseURL, "datos.json"); } }

    public void DownloadDataModel(Action<string> OnError)
    {
        RequestManager.instance.DownloadText(dataModelPath, LoadDataModel, OnError);
    }

    void LoadDataModel(string json)
    {
        JsonUtility.FromJsonOverwrite(json, dataModel);

        foreach (DataModel.Block block in dataModel.blocks)
        {
            textures.Add(block.url, new Texture2D(0, 0));
        }

        foreach (DataModel.Icon icon in dataModel.icons)
        {
            textures.Add(icon.url, new Texture2D(0, 0));
        }

        foreach (KeyValuePair<string, Texture2D> entry in textures)
        {
            RequestManager.instance.DownloadTexture2D(entry.Key, (Texture2D result) => { textures[entry.Key] = result; }, (string errorMessage) => { Debug.Log(errorMessage); });
        }
    }

    public Texture GetTexture(string url, Action<string> OnError)
    {
        if (textures.ContainsKey(url))
        {
            return textures[url];
        }
        else
        {
            textures.Add(url, new Texture2D(0, 0));
            RequestManager.instance.DownloadTexture2D(url, (Texture2D result) => { textures[url] = result; }, OnError);
            return textures[url];
        }
    }

    public Sprite GetSprite(Texture textureToConvert)
    {
        return textureToConvert.GetSprite();
    }
}
