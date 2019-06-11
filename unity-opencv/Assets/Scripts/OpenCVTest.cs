using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class OpenCVTest : MonoBehaviour {

    public Image cameraImage;

    WebCamTexture webcam;
    Texture2D cameraTexture;


    void Start() {
        webcam = new WebCamTexture();
        webcam.Play(); //! MUST BE BEFORE SETTING THE TEXTURE!

        cameraTexture = new Texture2D(webcam.width, webcam.height);
        cameraImage.material.mainTexture = cameraTexture;

    }

    void Update() {
        if (webcam.isPlaying) {
            var rawImage = webcam.GetPixels32();
            ProcessImage(ref rawImage, webcam.width, webcam.height);
            cameraTexture.SetPixels32(rawImage);
            cameraTexture.Apply();
        }
    }


    [DllImport("UnityOpenCV")]
    static extern void ProcessImage(ref Color32[] raw, int width, int height);

}
