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
    class CloseSessionCommand : Command
    {
        public CloseSessionCommand(ref CameraModel model) : base(ref model) { }

        public override bool Execute()
        {
            //The communication with the camera begins
            uint err = EDSDKLib.EDSDK.EdsCloseSession(_model.Camera);

            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                _model.NotifyObservers(e);
            }
            return true;
        }
    }
}
