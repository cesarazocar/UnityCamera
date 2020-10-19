using System;
using UnityEngine;
using UnityEngine.UI;

public class SampleCamera : MonoBehaviour
{
    public Button openCamera, openPhoto, openPhotos;
    public Button savePhoto;
    public RawImage rawImage;
    void Start()
    {
        openCamera.onClick.AddListener(OpenCamera);
        openPhoto.onClick.AddListener(OpenPhoto);
        openPhotos.onClick.AddListener(OpenPhotos);
        savePhoto.onClick.AddListener(SavePhoto);
    }
    private void OpenCamera()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        });
    }
    private void OpenPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        });
    }

    private void OpenPhotos()
    {
        /*
                NativeCall.OpenPhotos((Texture2D[] tex) =>
                {

                    rawImage.texture = tex;
                    rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
                });


            */

        //this.OpenImages();


    }
    private void SavePhoto()
    {
        NativeCall.SavePhoto(rawImage.texture as Texture2D);
    }


    public void OpenImages(Action<Texture2D[]> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((string[] path) =>
        {
            if (path == null || path.Length == 0) return;
            Texture2D[] texs = new Texture2D[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(path[i]))
                        texs[i] = NativeGallery.LoadImageAtPath(path[i]);
                }
                catch (Exception ex) { Debug.LogWarning("Error " + i + " Falló el procesamiento de la imagen : " + ex.Message + "\n" + path[i]); }
            }
            if (callBack != null) callBack(texs);
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            //ShowToast("No hay acceso al álbum en este momento, abre en la configuración");

            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
    }

       
}
