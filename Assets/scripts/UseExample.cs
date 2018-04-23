using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseExample : MonoBehaviour
{
    public MeshRenderer cubeRenderer, planeRenderer;
    public Image iconImage;
    public Text titleText;

    [Space(10)]
    public string playerName;
    public int score;

    DataManager dataManager { get { return GetComponent<DataManager>(); } }
    ScoreManager scoreManager { get { return GetComponent<ScoreManager>(); } }

    void Start()
    {
        //le pedimos al data manager que cargue las cosas
        dataManager.DownloadDataModel(ShowError);
        //nos suscribimos para saber cuando termina la cosa
        RequestManager.instance.OnRequestFinished += AssingData;
    }

    void ShowError(string errorMessage)
    {
        Debug.Log(errorMessage);
    }

    void AssingData()
    {
        //una vez que termina, no nos interesa mas la cosa. nos desuscribimos
        RequestManager.instance.OnRequestFinished -= AssingData;

        //ponemo' una de las texturas en el plano
        planeRenderer.material.mainTexture = dataManager.GetTexture(dataManager.dataModel.blocks[1].url, ShowError);
        planeRenderer.material.mainTexture.filterMode = FilterMode.Point;
        planeRenderer.material.SetTextureScale("_MainTex", new Vector2(10, 10));

        //ponemo otra en el cubo
        cubeRenderer.material.mainTexture = dataManager.GetTexture(dataManager.dataModel.blocks[0].url, ShowError);
        cubeRenderer.material.mainTexture.filterMode = FilterMode.Point; //para que quede mas cool hipster pixel artsy

        //ponemo el sprite del icono que ma' nos guste
        iconImage.sprite = dataManager.GetTexture(dataManager.dataModel.icons[0].url, ShowError).GetSprite();

        //ponemos el texto donde va
        titleText.text = dataManager.dataModel.gameName;
    }

    public void SendMyScore()
    {
        //enviamos el score
        scoreManager.SendScore(playerName, score, ShowError);
    }

    public void GetScores()
    {
        //pedimos los scores, a la vuelta la respuesta la maneja ShowScores
        scoreManager.GetLeaderboard(ShowScores, ShowError);
    }

    void ShowScores(LeaderboardModel model)
    {
        //lo mostramos en la UI
        //implementacion libre :D
    }
}
