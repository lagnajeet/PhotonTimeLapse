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
using System.Windows.Forms;

namespace CameraControl
{
    class FormatVolumeCommand : Command
    {
        public FormatVolumeCommand(ref CameraModel model) : base(ref model) { }

        public override bool Execute()
        {
            IntPtr volume = IntPtr.Zero;
            IntPtr camera = _model.Camera;
            int count = 0;
            // Get the number of camera volumes
            uint err = EDSDKLib.EDSDK.EdsGetChildCount(camera, out count);
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK && count == 0)
            {
                return true;
            }
            // Get initial volume
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                if (MessageBox.Show("Format the memory card?", "CameraControl", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return true;
                }
                for (int i = 0; i < count; i++)
                {
                    // Set SD-card number.
                    err = EDSDKLib.EDSDK.EdsGetChildAtIndex(camera, i, out volume);
                    if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        err = EDSDKLib.EDSDK.EdsFormatVolume(volume);
                    }
                }
            }

            return true;
        }
    }
}
