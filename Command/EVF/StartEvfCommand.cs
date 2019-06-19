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
    class StartEvfCommand : Command
    {
        public StartEvfCommand(ref CameraModel model) : base(ref model) { }


        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            /// Change settings because live view cannot be started
            /// when camera settings are set to "do not perform live view."
            uint evfMode = _model.EvfMode;

            if (evfMode == 0)
            {
                evfMode = 1;

                // Set to the camera.
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_Mode, 0, sizeof(uint), evfMode);
            }

            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                // Get the current output device.
                uint device = _model.EvfOutputDevice;

                // Set the PC as the current output device.
                device |= EDSDKLib.EDSDK.EvfOutputDevice_PC;

                // Set to the camera.
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_Evf_OutputDevice, 0, sizeof(uint), device);
            }

            //Notification of error
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
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
