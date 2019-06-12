using UnityEngine;
using Windows.Kinect;
using System.Runtime.InteropServices;

public class KinectInputManager : MonoBehaviour {

    const int DEPTH_WIDTH = 512;
    const int DEPTH_HEIGHT = 424;
    const int COLOR_WIDTH = 1920;
    const int COLOR_HEIGHT = 1080;


    private KinectSensor kinectSensor;
    private CoordinateMapper coordinateMapper;
    private MultiSourceFrameReader multiSourceFrameReader;
    private DepthSpacePoint[] pDepthCoordinates;

    private byte[] pColorBuffer;
    private byte[] pBodyIndexBuffer;
    private ushort[] pDepthBuffer;

//////// FPS - BEGIN
//TODO: remove or make optional
    long frameCount = 0;
    double elapsedCounter = 0.0;
    double fps = 0.0;
//////// FPS - END

//////// NULL_FRAME - BEGIN
//TODO: remove or make optional
//    bool nullFrame;
//////// NULL_FRAME - END

    Texture2D colorTexture;

    bool init;


    void Awake() {
        init = false;

        pColorBuffer = new byte[COLOR_WIDTH * COLOR_HEIGHT * 4];
        pBodyIndexBuffer = new byte[DEPTH_WIDTH * DEPTH_HEIGHT];
        pDepthBuffer = new ushort[DEPTH_WIDTH * DEPTH_HEIGHT];

        colorTexture = new Texture2D(COLOR_WIDTH, COLOR_HEIGHT, TextureFormat.BGRA32, false);

        pDepthCoordinates = new DepthSpacePoint[COLOR_WIDTH * COLOR_HEIGHT];

//////// NULL_FRAME - BEGIN
//        nullFrame = false;
//////// NULL_FRAME - END

        InitializeDefaultSensor();
    }

    void Update() {
//////// FPS - BEGIN
        elapsedCounter +=Time.deltaTime;
        if (elapsedCounter > 1.0) {
            fps = frameCount / elapsedCounter;
            frameCount = 0;
            elapsedCounter = 0.0;
        }
//////// FPS - END

        if (!init) {
            return;
        }

        var pMultiSourceFrame = multiSourceFrameReader.AcquireLatestFrame();
        if (pMultiSourceFrame != null) {
//////// FPS - BEGIN
            frameCount++;
//////// FPS - END
//////// NULL_FRAME - BEGIN
//            nullFrame = false;
//////// NULL_FRAME - END

            using (var pDepthFrame = pMultiSourceFrame.DepthFrameReference.AcquireFrame()) {
                if (pDepthFrame != null) {
                    var pDepthData = GCHandle.Alloc(pDepthBuffer, GCHandleType.Pinned);
                    pDepthFrame.CopyFrameDataToIntPtr(pDepthData.AddrOfPinnedObject(), (uint)pDepthBuffer.Length * sizeof(ushort));
                    pDepthData.Free();
                }
            }

            using (var pColorFrame = pMultiSourceFrame.ColorFrameReference.AcquireFrame()) {
                if (pColorFrame != null) {
                    var pColorData = GCHandle.Alloc(pColorBuffer, GCHandleType.Pinned);
                    pColorFrame.CopyConvertedFrameDataToIntPtr(pColorData.AddrOfPinnedObject(), (uint)pColorBuffer.Length, ColorImageFormat.Bgra);
                    pColorData.Free();
                }
            }

            using (var pBodyIndexFrame = pMultiSourceFrame.BodyIndexFrameReference.AcquireFrame()) {
                if (pBodyIndexFrame != null) {
                    var pBodyIndexData = GCHandle.Alloc(pBodyIndexBuffer, GCHandleType.Pinned);
                    pBodyIndexFrame.CopyFrameDataToIntPtr(pBodyIndexData.AddrOfPinnedObject(), (uint)pBodyIndexBuffer.Length);
                    pBodyIndexData.Free();
                }
            }

            ProcessFrame();
        }
//////// NULL_FRAME - BEGIN
/*
        else {
            nullFrame = true;
        }
*/
//////// NULL_FRAME - END
    }

    void OnApplicationQuit() {
        pDepthBuffer = null;
        pColorBuffer = null;
        pBodyIndexBuffer = null;

        if (pDepthCoordinates != null) {
            pDepthCoordinates = null;
        }

        if (multiSourceFrameReader != null) {
            multiSourceFrameReader.Dispose();
            multiSourceFrameReader = null;
        }

        if (kinectSensor != null) {
            kinectSensor.Close();
            kinectSensor = null;
        }
    }


//////// GUI - BEGIN
//////// FPS - BEGIN
    Rect fpsRect = new Rect(10, 10, 200, 30);
//////// FPS - END
    Rect nullFrameRect = new Rect(10, 50, 200, 30);

    void OnGUI () {
//////// FPS - BEGIN
        GUI.Box (fpsRect, "FPS: " + fps.ToString("0.00"));
//////// FPS - END

//////// NULL_FRAME - BEGIN
/*
        if (nullFrame) {
            GUI.Box (nullFrameRect, "NULL MSFR Frame");
        }
*/
//////// NULL_FRAME - END
    }
//////// GUI - END


    void InitializeDefaultSensor() {
        kinectSensor = KinectSensor.GetDefault();
        if (kinectSensor == null) {
            Debug.LogError("ERROR: No Kinect found!");
            return;
        }

        coordinateMapper = kinectSensor.CoordinateMapper;

        kinectSensor.Open();
        if (kinectSensor.IsOpen) {
            multiSourceFrameReader = kinectSensor.OpenMultiSourceFrameReader(
                    FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.BodyIndex);
            init = true;
        }
    }

    void ProcessFrame() {
        var pDepthData = GCHandle.Alloc(pDepthBuffer, GCHandleType.Pinned);
        var pDepthCoordinatesData = GCHandle.Alloc(pDepthCoordinates, GCHandleType.Pinned);

        coordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(pDepthData.AddrOfPinnedObject(), (uint)pDepthBuffer.Length * sizeof(ushort),
                pDepthCoordinatesData.AddrOfPinnedObject(), (uint)pDepthCoordinates.Length);

        pDepthCoordinatesData.Free();
        pDepthData.Free();

/*
        colorTexture.LoadRawTextureData(pColorBuffer);
        colorTexture.Apply();
*/
    }


    public Texture2D GetColorTexture() {
        return colorTexture;
    }

    public byte[] GetBodyIndexBuffer() {
        return pBodyIndexBuffer;
    }

    public DepthSpacePoint[] GetDepthCoordinates() {
        return pDepthCoordinates;
    }

    public byte[] GetColorBuffer() {
        return pColorBuffer;
    }

}

