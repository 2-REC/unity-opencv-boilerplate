using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class KinectOpenCVTest : MonoBehaviour {

//TODO: handle diferently (not hardcoded, and depends on kinect...)
    const int COLOR_WIDTH = 1920;
    const int COLOR_HEIGHT = 1080;


    public KinectInputManager kinectInputManager;
    public Image image;

    Texture2D texture;


    void Start() {
        texture = new Texture2D(COLOR_WIDTH, COLOR_HEIGHT, TextureFormat.BGRA32, false);
        image.material.mainTexture = texture;

    }

    void Update() {
        var rawImage = kinectInputManager.GetColorBuffer();
        ProcessImage(ref rawImage, COLOR_WIDTH, COLOR_HEIGHT);
        texture.LoadRawTextureData(rawImage);
        texture.Apply();
    }


    [DllImport("UnityOpenCV")]
    static extern void ProcessImage(ref byte[] raw, int width, int height);

}
