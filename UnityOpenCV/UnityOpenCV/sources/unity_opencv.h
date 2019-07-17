#ifndef UNITY_OPENCV_H
#define UNITY_OPENCV_H

typedef unsigned char byte;
#include "opencv2/opencv.hpp"

#ifdef __cplusplus
extern "C" {
#endif

	/**
	 * Process on full image
	 * Basic example: Edge detection
	 * => Modifies the original image
	 */
	__declspec(dllexport) void ProcessImage(byte **ppRaw, int width, int height);

	/**
	 * Process on ROI in image
	 * Basic example: Colour detection (hardcoded colour)
	 * => Modifies the original image
	 */
	__declspec(dllexport) void ProcessImageRegion(byte **ppRaw, int width, int height, cv::Rect roi);

	/**
	 * Apply a mask to ROI
	 * => Mask image can be of different resolution
	 * => Modifies the original image
	 */
	__declspec(dllexport) void ApplyMask(byte **ppRaw, int width, int height, cv::Rect region, byte* pMask, int maskWidth, int maskHeight);


#ifdef __cplusplus
}
#endif

#endif // UNITY_OPENCV_H
