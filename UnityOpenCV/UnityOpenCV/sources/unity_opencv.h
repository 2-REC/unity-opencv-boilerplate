#ifndef UNITY_OPENCV_H
#define UNITY_OPENCV_H

typedef unsigned char byte;
#include "opencv2/opencv.hpp"

#ifdef __cplusplus
extern "C" {
#endif

    __declspec(dllexport) void ProcessImage(byte **raw, int width, int height);
	__declspec(dllexport) void ProcessImageRegion(byte **raw, int width, int height, cv::Rect roi);

#ifdef __cplusplus
}
#endif

#endif // UNITY_OPENCV_H
