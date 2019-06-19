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
    class OpenSessionCommand : Command
    {
        public OpenSessionCommand(ref CameraModel model) : base(ref model) { }

        public override bool Execute()
        {
            //The communication with the camera begins
            uint err = EDSDKLib.EDSDK.EdsOpenSession(_model.Camera);

            bool locked = false;
            err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_SaveTo, 0, sizeof(uint), (uint)EDSDKLib.EDSDK.EdsSaveTo.Host);

            //UI lock
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                err = EDSDKLib.EDSDK.EdsSendStatusCommand(_model.Camera, EDSDKLib.EDSDK.CameraState_UILock, 0);
            }

            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                locked = true;
            }

            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                EDSDKLib.EDSDK.EdsCapacity Capacity;
                Capacity.NumberOfFreeClusters = 0x7FFFFFFF;
                Capacity.BytesPerSector = 0x1000;
                Capacity.Reset = 1;
                err = EDSDKLib.EDSDK.EdsSetCapacity(_model.Camera, Capacity);
            }

            //It releases it when locked
            if (locked)
            {
                err = EDSDKLib.EDSDK.EdsSendStatusCommand(_model.Camera, EDSDKLib.EDSDK.CameraState_UIUnLock, 0);
            }	

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
