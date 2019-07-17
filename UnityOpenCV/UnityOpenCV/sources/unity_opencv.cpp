#include "unity_opencv.h"

#include "opencv2/opencv.hpp"

using namespace cv;
using namespace std;


#ifdef __cplusplus
extern "C" {
#endif


void ProcessImage(byte **ppRaw, int width, int height) {

	Mat image(height, width, CV_8UC4, *ppRaw);

	// Process frame here
	//...
	Mat edges;
	Canny(image, edges, 50, 200);
	cvtColor(edges, edges, COLOR_GRAY2RGBA);
	multiply(image, edges, image);
}


void ProcessImageRegion(byte **ppRaw, int width, int height, cv::Rect region) {

	Mat image(height, width, CV_8UC4, *ppRaw);

	//cout << "ROI: " << region.x << ", " << region.y << ", " << region.width << ", " << region.height << endl;
//TODO: Should check that ROI is in image!
	Mat imageROI = image(region);

	Mat imageHSV;
	Mat threshold;

	// hardcoded "pink" colour (?)
	Scalar col1(161, 155, 84);
	Scalar col2(179, 255, 255);

	// detect a colour in ROI
	cvtColor(imageROI, imageHSV, COLOR_BGR2HSV);
	inRange(imageHSV, col1, col2, threshold);
	cvtColor(threshold, imageROI, COLOR_GRAY2RGBA);
	imageROI.copyTo(image(region));
}


void ApplyMask(byte **ppRaw, int width, int height, cv::Rect region, byte* pMask, int maskWidth, int maskHeight) {

	Mat image(height, width, CV_8UC4, *ppRaw);

//TODO: Should check that ROI is in image!
	Mat imageROI = image(region);

	Mat imageMask(maskHeight, maskWidth, CV_8U, pMask);
	cvtColor(imageMask, imageMask, COLOR_GRAY2BGRA);

	Size size(width, height);

	Mat masked;
	resize(imageMask, masked, size);

	Mat maskROI = masked(region);
	bitwise_and(imageROI, maskROI, imageROI);
}


#ifdef __cplusplus
}
#endif
