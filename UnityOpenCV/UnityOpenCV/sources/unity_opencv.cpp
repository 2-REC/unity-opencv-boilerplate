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

void ProcessImageRegion(byte **raw, int width, int height, cv::Rect region) {
    using namespace cv;
    using namespace std;

	Mat image(height, width, CV_8UC4, *raw);

//cout << "ROI: " << region.x << ", " << region.y << ", " << region.width << ", " << region.height << endl;

	Mat roi = image(region);

    // Process frame here
    //...
	Mat edges;
    Canny(roi, edges, 5, 250);
	cvtColor(edges, edges, COLOR_GRAY2RGBA);
    multiply(roi, edges, roi);

	roi.copyTo(image(region));

}

#ifdef __cplusplus
}
#endif
