using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseExample : MonoBehaviour
{
    public MeshRenderer cubeRenderer, planeRenderer;
    public Image iconImage;
    public Text titleText;

    DataManager dataManager { get { return GetComponent<DataManager>(); } }

    void Start()
    {
        dataManager.OnDownloadChanged += CheckDownload;
    }

    void CheckDownload(bool finished)
    {
        if (finished)
        {
            AssingData();
        }
    }

    void AssingData()
    {
        planeRenderer.material.mainTexture = dataManager.GetTexture(dataManager.dataModel.blocks[1].url);

        planeRenderer.material.mainTexture.filterMode = FilterMode.Point;

        planeRenderer.material.SetTextureScale("_MainTex", new Vector2(10, 10));

        cubeRenderer.material.mainTexture = dataManager.GetTexture(dataManager.dataModel.blocks[0].url);

        cubeRenderer.material.mainTexture.filterMode = FilterMode.Point; //para que quede mas cool hipster pixel artsy

        iconImage.sprite = dataManager.GetTexture(dataManager.dataModel.icons[0].url).GetSprite();

        titleText.text = dataManager.dataModel.gameName;
    }
}
