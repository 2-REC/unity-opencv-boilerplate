#ifndef UNITY_OPENCV_H
#define UNITY_OPENCV_H

typedef unsigned char byte;

#ifdef __cplusplus
extern "C" {
#endif

    __declspec(dllexport) void ProcessImage(byte **raw, int width, int height);

#ifdef __cplusplus
}
#endif

#endif // UNITY_OPENCV_H
