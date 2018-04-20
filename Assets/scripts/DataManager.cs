using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//delegado para establecer la firma de los metodos que usaremos para saber si las descargas han terminado
//este delegado devolverá true solo si no hay descargas pendientes
public delegate void DownloadChanged(bool finished);

public class DataManager : MonoBehaviour
{
    //evento publico al que los metodos de otras clases se pueden suscribir, si tiene la misma firma que el delegado
    public event DownloadChanged OnDownloadChanged;

    //contador interno de descargar pendientes
    int pendingDownloads = 0;

    //este es nuestro modelo de datos vacío (rellenado luego con el json)
    public DataModel dataModel;

    //esta es nuestra lista de texturas descargadas
    Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    //establecemos la url base aca, por si el server cambia de lugar, no tenemos que andar buscando todos los string hardcodeados por todo el proyecto
    const string baseURL = "http://um-tecvg-23.herokuapp.com/";

    //cada url que necesitemos va a estar compuesta de la base mas el path
    string dataModelPath { get { return Path.Combine(baseURL, "datos.json"); } }

    //usamos Path.Combine, para asegurarndos de que siempre sea un path válido
    string leaderboardPath { get { return Path.Combine(baseURL, "leaderboard.php"); } }

    void Start()
    {
        //lo primero que hacemos es poner a descargar el data model
        DownloadDataModel();
    }

    void DownloadDataModel()
    {
        //avisamos con el evento que estamos comenzando una descarga
        OnDownloadChanged(false);
        //aparte de la url de descarga, le pasamos las acciones que queremos que se ejecuten al terminar, bien o con error
        DownloadText(dataModelPath, LoadDataModel, ShowError);
    }

    //metodo especial que descarga solo texto
    void DownloadText(string url, Action<string> OnSuccess, Action<string> OnError)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        StartCoroutine(Download(uwr, (DownloadHandler result) =>
        {
            OnSuccess(result.text);
        }, OnError));
    }

    //metodo especial que descarga solo una textura
    void DownloadTexture2D(string url, Action<Texture2D> OnSuccess, Action<string> OnError)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        StartCoroutine(Download(uwr, (DownloadHandler result) =>
        {
            OnSuccess(DownloadHandlerTexture.GetContent(uwr));
        }, OnError));
    }

    //este metodo pondrá a descargar el webrequest que le pasemos, y siempre reportará las acciones de ok o de error
    IEnumerator Download(UnityWebRequest webRequest, Action<DownloadHandler> OnSuccess, Action<string> OnError)
    {
        if (string.IsNullOrEmpty(webRequest.url))
        {
            //si la url que queremos descargar está vacía, mostramos error
            ShowError("La url está vacía.");
        }
        else
        {
            //una descarga esta por comenzar, la contamos
            pendingDownloads++;

            //esperamos la descarga mientras sigue la ejecución del juego
            yield return webRequest.SendWebRequest();

            //al terminar, nos fijamos que trato merece, si es un error o un resultado ok
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                OnError(webRequest.error);
            }
            else
            {
                OnSuccess(webRequest.downloadHandler);
            }

            //la descarga y sus acciones han terminado, las descontamos
            pendingDownloads--;

            //avisamos por el evento, que una descarga ha terminado, pasandole el booleano de si quedan mas por terminar
            OnDownloadChanged(pendingDownloads < 1);
        }
    }

    //este metodo sirve para mostrar errores de alguna manera que crean conveniente. 
    void ShowError(string errorMessage)
    {
        //si tuvieran una ventana para mostrar mensajes de error, la llamarían acá en vez del Debug.Log.
        Debug.Log(errorMessage);
    }

    //metodo para cargar todo nuestro DataModel en unity, asi lo podemos usar
    void LoadDataModel(string json)
    {
        //cargamos nuestro objeto previamente existente con los datos que nos pasan
        JsonUtility.FromJsonOverwrite(json, dataModel);

        //agregamos los bloques a la lista de imagenes
        foreach (DataModel.Block block in dataModel.blocks)
        {
            textures.Add(block.url, new Texture2D(0, 0));
        }

        //agregamos los iconos a la lista de imagenes
        foreach (DataModel.Icon icon in dataModel.icons)
        {
            textures.Add(icon.url, new Texture2D(0, 0));
        }

        //nos ponemos a descargar toda la lista de imagenes
        foreach (KeyValuePair<string, Texture2D> entry in textures)
        {
            DownloadTexture2D(entry.Key, (Texture2D result) => { textures[entry.Key] = result; }, ShowError);
        }
    }

    public Texture GetTexture(string url)
    {
        //si la textura existe en la lista, la devolvemos
        if (textures.ContainsKey(url))
        {
            return textures[url];
        }
        else
        {
            //sino, la agregamos a la lista, y la ponemos a descargar inmediatamente
            textures.Add(url, new Texture2D(0, 0));
            DownloadTexture2D(url, (Texture2D result) => { textures[url] = result; }, ShowError);
            return textures[url];
        }
    }

    public Sprite GetSprite(Texture textureToConvert)
    {
        //es mas simple usar esta llamada directamente cuando les haga falta, se las dejo como ejemplo
        return textureToConvert.GetSprite();
    }
}
