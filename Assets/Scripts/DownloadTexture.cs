/*Loading texture from the http by using UnityEngine Networking system. 
The Texture is used as a Image screen for the Menu Scene*/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadTexture : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    //Create the Sprite using a URL from web
    void Start()
    {   
        string url = "https://yppedia.puzzlepirates.com/images/1/17/Monthly_cattrin_field_of_clovers.png";
        GetTexture(url, (string error) => { }, (Texture2D texture2D) =>
        {
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, Screen.width, Screen.height), new Vector2(.5f, .5f), 10f);
            spriteRenderer.sprite = sprite;
        });
    }

    private void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    private IEnumerator GetCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();

            //check for error else download Image to use as a Texture
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}
