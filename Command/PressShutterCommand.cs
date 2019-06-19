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
    class PressShutterCommand : Command
    {
        private uint _status;

        public PressShutterCommand(ref CameraModel model, uint status) : base(ref model)
        {
            _status = status;
        }

        // Execute command	
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EdsSendCommand(_model.Camera, EDSDKLib.EDSDK.CameraCommand_PressShutterButton, (int)_status);

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

            if (_status == (uint)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_Completely)
            {
                if (_model.DriveMode == 0x01 || _model.DriveMode == 0x04)
                {
                    //_model.canDownloadImage = false;
                }
                else
                {
                    _model.canDownloadImage = true;
                }
            }

            return true;
        }
    }
}
