/******************************************************************************
*                                                                             *
*   PROJECT : Eos Digital camera Software Development Kit EDSDK               *
*                                                                             *
*   Description: This is the Sample code to show the usage of EDSDK.          *
*                                                                             *
*                                                                             *
*******************************************************************************
*                                                                             *
*   Written and developed by Canon Inc.                                       *
*   Copyright Canon Inc. 2018 All Rights Reserved                             *
*                                                                             *
*******************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace CameraControl
{
    public struct EVFDataSet
    {
        public IntPtr stream;
        public uint zoom;
        public EDSDKLib.EDSDK.EdsRect zoomRect;
        public EDSDKLib.EDSDK.EdsPoint imagePosition;
        public EDSDKLib.EDSDK.EdsSize sizeJpegLarge;
    }

    class DownloadEvfCommand : Command
    {
        public DownloadEvfCommand(ref CameraModel model) : base(ref model) { }

        // Execute command	
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            IntPtr evfImage = IntPtr.Zero;
            IntPtr stream = IntPtr.Zero;
            UInt64 bufferSize = 2 * 1024 * 1024;
            IntPtr evfDataSetPtr = IntPtr.Zero;


            // Exit unless during live view.
            if ((_model.EvfOutputDevice & EDSDKLib.EDSDK.EvfOutputDevice_PC) == 0)
            {
                return true;
            }

            // Create memory stream.
            err = EDSDKLib.EDSDK.EdsCreateMemoryStream(bufferSize, out stream);

            // Create EvfImageRef.
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                err = EDSDKLib.EDSDK.EdsCreateEvfImageRef(stream, out evfImage);
            }

            // Download live view image data.
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                err = EDSDKLib.EDSDK.EdsDownloadEvfImage(_model.Camera, evfImage);
            }

            // Get meta data for live view image data.
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                EVFDataSet dataset = new EVFDataSet();

                dataset.stream = stream;

                // Get magnification ratio (x1, x5, or x10).
                EDSDKLib.EDSDK.EdsGetPropertyData(evfImage, EDSDKLib.EDSDK.PropID_Evf_Zoom, 0, out dataset.zoom);

                // Get position of image data. (when enlarging)
                // Upper left coordinate using JPEG Large size as a reference.
                EDSDKLib.EDSDK.EdsGetPropertyData(evfImage, EDSDKLib.EDSDK.PropID_Evf_ImagePosition, 0, out dataset.imagePosition);

                // Get rectangle of the focus border.
                EDSDKLib.EDSDK.EdsGetPropertyData(evfImage, EDSDKLib.EDSDK.PropID_Evf_ZoomRect, 0, out dataset.zoomRect);

                // Get the size as a reference of the coordinates of rectangle of the focus border.
                EDSDKLib.EDSDK.EdsGetPropertyData(evfImage, EDSDKLib.EDSDK.PropID_Evf_CoordinateSystem, 0, out dataset.sizeJpegLarge);

                _model.SetPropertyRect(EDSDKLib.EDSDK.PropID_Evf_ZoomRect, dataset.zoomRect);

                int size = Marshal.SizeOf(dataset);

                evfDataSetPtr = Marshal.AllocHGlobal(size);

                Marshal.StructureToPtr(dataset, evfDataSetPtr, false);

            }
            // Live view image transfer complete notification.
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e = new CameraEvent(CameraEvent.Type.EVFDATA_CHANGED, evfDataSetPtr);
                _model.NotifyObservers(e);
            }

            if (stream != IntPtr.Zero)
            {
                EDSDKLib.EDSDK.EdsRelease(stream);
                stream = IntPtr.Zero;
            }

            if (evfImage != IntPtr.Zero)
            {
                EDSDKLib.EDSDK.EdsRelease(evfImage);
                evfImage = IntPtr.Zero;
            }

            if (evfDataSetPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(evfDataSetPtr);
            }

            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {

                // Retry getting image data if EDS_ERR_OBJECT_NOTREADY is returned
                // when the image data is not ready yet.
                if (err == EDSDKLib.EDSDK.EDS_ERR_OBJECT_NOTREADY)
                {
                    return false;
                }

                // It retries it at device busy
                if (err == EDSDKLib.EDSDK.EDS_ERR_DEVICE_BUSY)
                {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
                    _model.NotifyObservers(e);
                    return false;
                }

                {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                    _model.NotifyObservers(e);
                }
            }
            return true;
        }
    }
}
