#include "unity_opencv.h"

#include "opencv2/opencv.hpp"

#ifdef __cplusplus
extern "C" {
#endif

void ProcessImage(byte **raw, int width, int height) {
    using namespace cv;
    using namespace std;

	Mat image(height, width, CV_8UC4, *raw);

    // Process frame here
    //...
	Mat edges;
	Canny(image, edges, 50, 200);
	cvtColor(edges, edges, COLOR_GRAY2RGBA);
	multiply(image, edges, image);
}

#ifdef __cplusplus
}
#endif
