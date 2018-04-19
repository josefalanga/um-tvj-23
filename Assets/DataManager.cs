using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    [System.Serializable]
    public class DataModel
    {
        [System.Serializable]
        public class Hardness
        {
            public string name;
            public int enemies;
            public int bosses;
            public bool chaosMode;
        }
        [System.Serializable]
        public class Player
        {
            public string name;
            public int health;
            public int damage;
            public int speed;
        }
        [System.Serializable]
        public class Minion
        {
            public string name;
            public int health;
            public int damage;
            public double speed;
        }
        [System.Serializable]
        public class Boss
        {
            public string name;
            public int health;
            public int damage;
            public double speed;
        }
        [System.Serializable]
        public class Icon
        {
            public string name;
            public string url;
        }
        [System.Serializable]
        public class Block
        {
            public string name;
            public string url;
        }

        public string gameName;
        public string gameDescription;
        public List<Hardness> hardness;
        public Player player;
        public Minion minion;
        public Boss boss;
        public List<Icon> icons;
        public List<Block> blocks;
    }

    public DataModel dataModel;

    private void Start()
    {
        StartCoroutine(Download("http://um-tecvg-23.herokuapp.com/datos.json"));
    }

    IEnumerator Download(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JsonUtility.FromJsonOverwrite(www.downloadHandler.text, dataModel);
        }
    }
}
