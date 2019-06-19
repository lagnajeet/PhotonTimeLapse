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
    class TakePictureCommand : Command
    {
        public TakePictureCommand(ref CameraModel model) : base(ref model) { }

        public override bool Execute()
        {
            //Taking a picture
            uint err = EDSDKLib.EDSDK.EdsSendCommand(_model.Camera, EDSDKLib.EDSDK.CameraCommand_PressShutterButton, (int)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_Completely);
            err = EDSDKLib.EDSDK.EdsSendCommand(_model.Camera, EDSDKLib.EDSDK.CameraCommand_PressShutterButton, (int)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_OFF);

            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                // It retries it at device busy
                if (err == EDSDKLib.EDSDK.EDS_ERR_DEVICE_BUSY)
                {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
                    _model.NotifyObservers(e);
                    return true;
                }
                else
                {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                    _model.NotifyObservers(e);
                    return true;
                }
            }
            _model.canDownloadImage = true;

            return true;
        }
    }
}
