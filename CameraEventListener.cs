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
using System.Runtime.InteropServices;

namespace CameraControl
{
    class CameraEventListener
    {
        public static uint HandleObjectEvent(uint inEvent, IntPtr inRef, IntPtr inContext)
        {
            GCHandle handle = GCHandle.FromIntPtr(inContext);
            CameraController controller = (CameraController)handle.Target;

            switch (inEvent)
            {
                //case EDSDKLib.EDSDK.ObjectEvent_DirItemCreated :
                case EDSDKLib.EDSDK.ObjectEvent_DirItemRequestTransfer:

                    FireEvent(ref controller, ActionEvent.Command.DOWNLOAD, inRef);
                    break;

                default:
                    //Object without the necessity is released
                    if (inRef != IntPtr.Zero)
                    {
                        EDSDKLib.EDSDK.EdsRelease(inRef);
                    }
                    break;
            }
            return EDSDKLib.EDSDK.EDS_ERR_OK;
        }


        public static uint HandlePropertyEvent(uint inEvent, uint inPropertyID, uint inParam, IntPtr inContext)
        {
            GCHandle handle = GCHandle.FromIntPtr(inContext);
            CameraController controller = (CameraController)handle.Target;

            switch (inEvent)
            {
                case EDSDKLib.EDSDK.PropertyEvent_PropertyChanged:
                    FireEvent(ref controller, ActionEvent.Command.GET_PROPERTY, (IntPtr)inPropertyID);
                    break;

                case EDSDKLib.EDSDK.PropertyEvent_PropertyDescChanged:
                    FireEvent(ref controller, ActionEvent.Command.GET_PROPERTYDESC, (IntPtr)inPropertyID);
                    break;
            }
            return EDSDKLib.EDSDK.EDS_ERR_OK;
        }


        public static uint HandleStateEvent(uint inEvent, uint inParameter, IntPtr inContext)
        {
            GCHandle handle = GCHandle.FromIntPtr(inContext);
            CameraController controller = (CameraController)handle.Target;

            switch (inEvent)
            {
                case EDSDKLib.EDSDK.StateEvent_Shutdown:
                    FireEvent(ref controller, ActionEvent.Command.SHUT_DOWN, IntPtr.Zero);
                    break;
            }
            return EDSDKLib.EDSDK.EDS_ERR_OK;
        }

        private static void FireEvent(ref CameraController controller, ActionEvent.Command command, IntPtr arg)
        {
            ActionEvent e = new ActionEvent(command, arg);
            controller.ActionPerformed(e);
        }
    }
}
