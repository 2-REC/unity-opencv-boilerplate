# UNITY OPENCV BOILERPLATE

## INTRODUCTION

Boilerplate project for using OpenCV in Unity.<br>
It allows to create a library written in C++ using OpenCV, that can be imported in Unity.<br>


## PREREQUISITES

This project uses the precompiled libraries of OpenCV.<br>
Libraries can also be compiled from the sources, but this is not covered here.<br>

- Download the latest OpenCV libraries from the [official website](https://opencv.org/)<br>
    At the time of writing, the latest version was "4.1.0":<br>
    https://sourceforge.net/projects/opencvlibrary/files/4.1.0/opencv-4.1.0-vc14_vc15.exe/download
- Extract the downloaded archive

For easier setup and use, an environment variable pointing to the extracted directory can de defined:<br>
E.g.:
<b><OPENCV_INSTALL_DIR></b>


## CREATE THE LIBRARY

The Visual Studio solution in the "UnityOpenCV" directory can be used to generate the library.<br>

The library can also be generated from scratch, as described in the following paragraphs.<br>

REMARK: The following process is using Visual Studio as the IDE. This is thus only valid for Windows.<br>
The steps should however be similar in any IDE or OS.<br>
The version of VS that was used is 2017, though there shouldn't be big differences with other versions of the IDE.<br>


- Create a new empty project<br>
    - "File" => "New" => "Project..."<br>
    - "Visual C++" => "Empty Project"<br>
    - Specify the name of the project (and the solution), as well as its location<br>
        => Any name will do, e.g.: "UnityOpenCV".<br>


- Add the boilerplate source files<br>
    - Navigate to the directory of the newly created project, and create a subdirectory "source"<br>
        => A different directory structure can be used if desired.<br>
    - Copy the 2 source files from the "UnityOpenCV/UnityOpenCV/sources" directory of this repository to the new directory<br>
        - <b>"unity_opencv.h"</b>
        - <b>"unity_opencv.cpp"</b>
        ! - TODO: In order to be used on a different OS, changes in the code are required ("decl")!<br>
    - In Visual Studio, add the 2 files to the solution<br>
        - "Project" => "Add Existing Item..."<br>
        - Navigate to the directory where the 2 files have been copied, and select them.<br>


- Set OpenCV include path<br>
    - "Project" => "Properties" => "C/C++" => "Additional Include Directories"<br>
    ! - Select "All Configurations" to have settings made for both debug and release configurations<br>
    - Add the path to the include directory of the OpenCV installation<br>
        <b><OPENCV_INSTALL_DIR>\build\include</b>


- Set OpenCV library path<br>
    - "Project" => "Properties" => "Linker" => "Additional Library Directories"<br>
    ! - Select "All Configurations" to have settings made for both debug and release configurations<br>
    - Add the path to the lib directory of the OpenCV installation<br>
        <b><OPENCV_INSTALL_DIR>\build\x64\vc15\lib</b>
        <br>(The directory might be different depending on the desired version and build)<br>


- Add OpenCV library dependency<br>
    - "Project" => "Properties" => "Linker" => "Input" => "Additional Dependencies"<br>
    ! - Do that for each configuration separately (debug & release), adding the specific version of the library:<br>
        - Debug: <b>"opencv_world410d.lib"</b>
        - Release: <b>"opencv_world410.lib"</b>


- Set project as a dynamic library<br>
    - "Project" => "Properties" => "General" => "Configuration Type"<br>
    ! - Select "All Configurations" to have settings made for both debug and release configurations<br>
    - Select "Dynamic Library (.dll)"<br>


- Set the desired platform (in my case, 64 bits)<br>
    - "Build" => "Configuration Manager" => "Active Solution Platform"<br>
    - Select"x64"<br>


- Build the (empty) library<br>
    - Select the desired configuration (32/64 bits, debug/release)<br>
    - "Build" => "Build Solution"<br>
    - If everything went OK, a DLL file should have been generated<br>


The built library contains an empty method "ProcessImage", and doesn't do anything.<br>


## WRITE IMAGE ANALYSIS PROCESS

Some process needs to be added to make the library useful.<br>

=> Implement the "ProcessImage" method.<br>
A lot of documentation and tutorials can be found explaining how to use the OpenCV library.<br>


## USE IN UNTIY

- Create a new project or open an existing one<br>


- Import the libraries<br>
    - Create a "Plugins" directory in "Assets" (if doesn't already exist)
    - Import the OpenCV library:<br>
        - <b>"opencv_world410d.dll"</b> for debug use<br>
        - <b>"opencv_world410.dll"</b> for release use<br>
        => Copy the file(s) in the "Plugins" directory.<br>
    - OpenCV Unity library:<br>
        => Copy the generated library in the "Plugins" directory.<br>


- Add the following to a new script or to an existing one:<br>
    - Import the "ProcessImage" method from the library:<br>
        ```cs
        [DllImport("UnityOpenCV")]
        static extern void ProcessImage(ref byte[] raw, int width, int height);
        ```
        Where "UnityOpenCV" is the name of the library.<br>
        It requires to import the Unity Interop Services:<br>
        ```cs
        using System.Runtime.InteropServices;
        ```
    - Call the method:<br>
        ```cs
        ProcessImage(ref rawImage, width, height);
        ```
        Where "rawImg" is a byte array:<br>
        ```cs
        byte[] rawImage;
        ```

### Sample Unity Projects

The provided Unity sample projects can be used as example or starting base.<br>

- "unity-opencv": Very basic project acquiring a video feed from the device's webcam and sending the image to OpenCV.<br>
    It is a slightly modified version of Amin Ahmadi's project (see DISCLAIMER below).<br>
- "unity-kinect-opencv": Shows how to use OpenCV with the Kinect RGB video feed.<br>
    It is based on the "Green Screen" sample from the [MS Kinect SDK](https://developer.microsoft.com/en-us/windows/kinect).<br>


The projects require the OpenCV library as well as the generated C++ library to be added to the Assets in a "plugins" directory.<br>


## DISCLAIMER

The project is based on the following posts from [Amin Ahmadi](https://amin-ahmadi.com/):<br>
- ["How to Use OpenCV in Unity"](http://amin-ahmadi.com/2017/05/24/how-to-use-opencv-in-unity-example-project/)
- ["How to Pass Images Between OpenCV and Unity"](https://amin-ahmadi.com/2019/06/01/how-to-pass-images-between-opencv-and-unity/)
