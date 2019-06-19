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
    class GetPropertyDescCommand : Command
    {

        private uint _propertyID;

        public GetPropertyDescCommand(ref CameraModel model, uint propertyID)
            : base(ref model)
        {
            _propertyID = propertyID;

        }

        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            //Get property
            err = this.GetPropertyDesc(_propertyID);


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



        private uint GetPropertyDesc(uint propertyID)
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            EDSDKLib.EDSDK.EdsPropertyDesc propertyDesc = new EDSDKLib.EDSDK.EdsPropertyDesc();

            if (propertyID == EDSDKLib.EDSDK.PropID_Unknown)
            {
                //If unknown is returned for the property ID , the required property must be retrieved again
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_DriveMode);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_WhiteBalance);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_Tv);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_Av);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_ISOSpeed);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_MeteringMode);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_ExposureCompensation);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_ImageQuality);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = this.GetPropertyDesc(EDSDKLib.EDSDK.PropID_Evf_AFMode);

                return err;
            }

            //Acquisition of value list that can be set
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                err = EDSDKLib.EDSDK.EdsGetPropertyDesc(_model.Camera,
                                        propertyID,
                                        out propertyDesc);
            }

            //The value list that can be the acquired setting it is set		
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                _model.SetPropertyDesc(propertyID, ref propertyDesc);
            }

            //Update notification
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)propertyID);
                _model.NotifyObservers(e);

            }
            return err;
        }
    }
}

