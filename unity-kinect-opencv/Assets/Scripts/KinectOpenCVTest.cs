using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class KinectOpenCVTest : MonoBehaviour {

//TODO: handle diferently (not hardcoded, and depends on kinect...)
    const int COLOR_WIDTH = 1920;
    const int COLOR_HEIGHT = 1080;


    public KinectInputManager kinectInputManager;
    public Image image;

    public bool processFullImage = true;

    Texture2D texture;


    void Start() {
        texture = new Texture2D(COLOR_WIDTH, COLOR_HEIGHT, TextureFormat.BGRA32, false);
        image.material.mainTexture = texture;

    }

    void Update() {

        var rawImage = kinectInputManager.GetColorBuffer();

        if (processFullImage) {
            // Process full image
            ProcessImage(ref rawImage, COLOR_WIDTH, COLOR_HEIGHT);
        }
        else {
            // Process regions of the image
            //TODO: set these as input parameters
            List<RectInt> rois = new List<RectInt>();
            rois.Add(new RectInt(0, 0, 100, 100));
            rois.Add(new RectInt(200, 200, 200, 100));
            rois.Add(new RectInt(500, 500, 250, 250));
            foreach (RectInt roi in rois) {
                ProcessImageRegion(ref rawImage, COLOR_WIDTH, COLOR_HEIGHT, roi);
            }
        }

        texture.LoadRawTextureData(rawImage);
        texture.Apply();
    }


    [DllImport("UnityOpenCV")]
    static extern void ProcessImage(ref byte[] raw, int width, int height);

    [DllImport("UnityOpenCV")]
    static extern void ProcessImageRegion(ref byte[] raw, int width, int height, RectInt roi);

}
