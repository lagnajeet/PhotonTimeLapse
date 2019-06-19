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

namespace CameraControl
{
    class EndEvfCommand : Command
    {
        public EndEvfCommand(ref CameraModel model) : base(ref model) { }
        
        // Execute command	
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            // Get the current output device.
            uint device = _model.EvfOutputDevice;

            // Do nothing if the remote live view has already ended.
            if ((device & EDSDKLib.EDSDK.EvfOutputDevice_PC) == 0)
            {
                return true;
            }

            // Get depth of field status.
            uint depthOfFieldPreview = _model.EvfDepthOfFieldPreview;

            // Release depth of field in case of depth of field status.
            if (depthOfFieldPreview != EDSDKLib.EDSDK.EvfDepthOfFieldPreview_OFF)
            {
                int size;
                EDSDKLib.EDSDK.EdsDataType datatype;

                err = EDSDKLib.EDSDK.EdsGetPropertySize(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_DepthOfFieldPreview, 0, out datatype, out size);
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_DepthOfFieldPreview, 0, size, EDSDKLib.EDSDK.EvfDepthOfFieldPreview_OFF);

                // Standby because commands are not accepted for awhile when the depth of field has been released.
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            // Change the output device.
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                device = EDSDKLib.EDSDK.EvfOutputDevice_TFT;

                int size;
                EDSDKLib.EDSDK.EdsDataType datatype;
                err = EDSDKLib.EDSDK.EdsGetPropertySize(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_OutputDevice, 0, out datatype, out size);
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_OutputDevice, 0, size, device);
            }
            
            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e;
                if (err == EDSDKLib.EDSDK.EDS_ERR_OBJECT_NOTREADY)
                {
                    return false;
                }

                // It retries it at device busy
                if (err == EDSDKLib.EDSDK.EDS_ERR_DEVICE_BUSY)
                {
                     e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
                    _model.NotifyObservers(e);
                    return false;
                }

                    e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                    _model.NotifyObservers(e);
                    return false;    
            }
            return true;
        }
    }
}
