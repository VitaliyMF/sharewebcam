ShareWebCam is an utility written in C# that grabs screen shots from webcam (or another video input) and pushes them to the web server by HTTP.

## Project Status ##
This utility was written in 2007 for personal usage. Utility was written in hurry ("just get it works") and code style is not very pretty, but in overall it seems to be stable and can be used by anyone. Any contribution is welcome.

## Main features ##
  * DirectShow is used for accessing any web-cam registered as DD video input device
  * various pushing options (image size, upload speed etc)
  * can push images to any web-site (simple HTTP POST is used)
  * unfinished feature (grabbing/transcoding code present): push video chuncks as FLV (ffmpeg used for transcoding)
![https://sites.google.com/site/fedorchenko/projects/sharewebcam_screen.gif](https://sites.google.com/site/fedorchenko/projects/sharewebcam_screen.gif)


