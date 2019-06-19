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
    public class ActionEvent
    {
        public enum Command
        {
            NONE,
            DOWNLOAD,
            TAKE_PICTURE,
            SET_CAMERASETTING,
            PRESS_COMPLETELY,
            PRESS_HALFWAY,
            PRESS_OFF,
            START_EVF,
            END_EVF,
            GET_PROPERTY,
            GET_PROPERTYDESC,
            DOWNLOAD_EVF,
            SET_AE_MODE,
            SET_DRIVE_MODE,
            SET_WHITE_BALANCE,
            SET_METERING_MODE,
            SET_EXPOSURE_COMPENSATION,
            SET_IMAGEQUALITY,
            SET_AV,
            SET_TV,
            SET_ISO_SPEED,
            SET_EVF_AFMODE,
            SET_ZOOM,
            SET_AF_MODE,
            SET_FLASH_MODE,
            EVF_AF_ON,
            EVF_AF_OFF,
            FOCUS_NEAR1,
            FOCUS_NEAR2,
            FOCUS_NEAR3,
            FOCUS_FAR1,
            FOCUS_FAR2,
            FOCUS_FAR3,
            ZOOM_FIT,
            ZOOM_ZOOM,
            POSITION_UP,
            POSITION_LEFT,
            POSITION_RIGHT,
            POSITION_DOWN,
            REMOTESHOOTING_START,
            REMOTESHOOTING_STOP,
            SHUT_DOWN,
            CLOSING
        }

        private Command _command = Command.NONE;
        private IntPtr _arg;

        public ActionEvent(Command command, IntPtr arg)
        {
            _command = command;
            _arg = arg;
        }

        public Command GetActionCommand() { return _command; }
        public IntPtr GetArg() { return _arg; }

    }
}
