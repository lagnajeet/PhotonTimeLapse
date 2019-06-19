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
    class GetPropertyCommand : Command
    {

        private uint _propertyID;

        public GetPropertyCommand(ref CameraModel model, uint propertyID): base(ref model)
        {
            _propertyID = propertyID;
        }

        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            //Get property value
            err = this.GetProperty(_propertyID);


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

        private uint GetProperty(uint propertyID)
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            EDSDKLib.EDSDK.EdsDataType dataType = EDSDKLib.EDSDK.EdsDataType.Unknown;
            int dataSize = 0;

            if (propertyID == EDSDKLib.EDSDK.PropID_Unknown)
            {
                //If unknown is returned for the property ID , the required property must be retrieved again
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_AEMode);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_DriveMode);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_WhiteBalance);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_Tv);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_Av);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_ISOSpeed);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_MeteringMode);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_ExposureCompensation);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_ImageQuality);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_AvailableShots);
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK) err = GetProperty(EDSDKLib.EDSDK.PropID_BatteryLevel);

                return err;
            }
                
            err = EDSDKLib.EDSDK.EdsGetPropertySize(_model.Camera, propertyID, 0, out dataType, out dataSize);

            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                if (dataType == EDSDKLib.EDSDK.EdsDataType.UInt32)
                {
                    uint data;

                    //Acquisition of the property
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera,
                                            propertyID,
                                            0,
                                            out data);

                    //Acquired property value is set
                    if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        _model.SetPropertyUInt32(propertyID, data);
                    }
                }

                else if (dataType == EDSDKLib.EDSDK.EdsDataType.Int32)
                {
                    uint data;

                    //Acquisition of the property
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera,
                                            propertyID,
                                            0,
                                            out data);

                    //Acquired property value is set
                    if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        _model.SetPropertyInt32(propertyID, data);
                    }

                }

                else if (dataType == EDSDKLib.EDSDK.EdsDataType.String)
                {
                    String str;
                    //EdsChar str[EDS_MAX_NAME];

                    //Acquisition of the property
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera,
                                            propertyID,
                                            0,
                                            out str);

                    //Acquired property value is set
                    if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        _model.SetPropertyString(propertyID, str);
                    }
                }

                else if (dataType == EDSDKLib.EDSDK.EdsDataType.FocusInfo)
                {
                    EDSDKLib.EDSDK.EdsFocusInfo focusInfo;

                    //Acquisition of the property
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera,
                                            propertyID,
                                            0,
                                            out focusInfo);

                    //Acquired property value is set
                    if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        _model.SetPropertyFocusInfo(propertyID, focusInfo);
                    }
                }
            }

            else if (dataType == EDSDKLib.EDSDK.EdsDataType.FocusInfo || err == EDSDKLib.EDSDK.EDS_ERR_PROPERTIES_UNAVAILABLE)
            {
                _model.FocusInfo = default(EDSDKLib.EDSDK.EdsFocusInfo);
                err = EDSDKLib.EDSDK.EDS_ERR_OK;
            }

            //Update notification
            if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                CameraEvent e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)propertyID);
                _model.NotifyObservers(e);
            }
            return err;
        }
    }
}
