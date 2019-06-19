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
    class DriveLensCommand : Command
    {

        private uint _parameter;

        public DriveLensCommand(ref CameraModel model, uint parameter): base(ref model)
        {
            _parameter = parameter;
        }

        // Execute command	
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EdsSendCommand(_model.Camera, EDSDKLib.EDSDK.CameraCommand_DriveLensEvf, (int)_parameter);

            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e;
                // It retries it at device busy
                if (err == EDSDKLib.EDSDK.EDS_ERR_DEVICE_BUSY)
                {
                    e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
                    _model.NotifyObservers(e);
                    return true;
                }

                    e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                    _model.NotifyObservers(e);
            }
            return true;
        }
    }
}
