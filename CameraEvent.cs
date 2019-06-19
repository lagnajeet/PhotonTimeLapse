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
    public class CameraEvent
    {
        public enum Type
        {
            NONE,
            ERROR,
            DEVICE_BUSY,
            DOWNLOAD_START,
            DOWNLOAD_COMPLETE,
            EVFDATA_CHANGED,
            PROGRESS_REPORT,
            PROPERTY_CHANGED,
            PROPERTY_DESC_CHANGED,
            DELETE_START,
            DELETE_COMPLETE,
            PROGRESS,
            SHUT_DOWN
        }

        private Type _type = Type.NONE;
        private IntPtr _arg;

        public CameraEvent(Type type, IntPtr arg)
        {
            _type = type;
            _arg = arg;
        }

        public Type GetEventType() { return _type; }
        public IntPtr GetArg() { return _arg; }
    }
}
