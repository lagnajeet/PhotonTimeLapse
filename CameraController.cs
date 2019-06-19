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
    public class CameraController : ActionListener
    {
        private CameraModel _model;
        CommandProcessor _processor = new CommandProcessor();

        public CameraController(ref CameraModel model)
        {
            _model = model;
        }

        public void Run()
        {
            _processor.Start();

            //The communication with the camera begins

            _processor.PostCommand(new OpenSessionCommand(ref _model));

            _processor.PostCommand(new GetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_ProductName));

            //It is necessary to acquire the property information that cannot acquire in sending OpenSessionCommand automatically by manual operation.
        }

        public CameraModel GetModel()
        {
            return _model;
        }

        public override void ActionPerformed(ActionEvent e)
        {
            ActionEvent.Command command = e.GetActionCommand();

            uint inPropertyID;
            uint data;
            EDSDKLib.EDSDK.EdsPoint point;
            switch (command)
            {

                case ActionEvent.Command.GET_PROPERTY:
                    inPropertyID = (uint)e.GetArg();
                    _processor.PostCommand(new GetPropertyCommand(ref _model, inPropertyID));
                    break;

                case ActionEvent.Command.GET_PROPERTYDESC:
                    inPropertyID = (uint)e.GetArg();
                    _processor.PostCommand(new GetPropertyDescCommand(ref _model, inPropertyID));
                    break;

                case ActionEvent.Command.TAKE_PICTURE:
                    _processor.PostCommand(new TakePictureCommand(ref _model));
                    break;

                case ActionEvent.Command.PRESS_COMPLETELY:
                    _processor.PostCommand(new PressShutterCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_Completely));
                    break;

                case ActionEvent.Command.PRESS_HALFWAY:
                    _processor.PostCommand(new PressShutterCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_Halfway));
                    break;

                case ActionEvent.Command.PRESS_OFF:
                    _processor.PostCommand(new PressShutterCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsShutterButton.CameraCommand_ShutterButton_OFF));
                    break;

                case ActionEvent.Command.START_EVF:
                    _model.isEvfEnable = true;
                    _processor.PostCommand(new StartEvfCommand(ref _model));
                    break;

                case ActionEvent.Command.END_EVF:
                     _model.isEvfEnable = false;
                    // When exit LiveView, cancel EVF AF ON
                    _processor.PostCommand(new DoEvfAFCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsEvfAf.CameraCommand_EvfAf_OFF));
                    _processor.PostCommand(new EndEvfCommand(ref _model));
                    break;

                case ActionEvent.Command.DOWNLOAD_EVF:
                    _processor.PostCommand(new DownloadEvfCommand(ref _model));
                    break;

                case ActionEvent.Command.SET_AF_MODE:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_AFMode, data));
                    break;

                case ActionEvent.Command.SET_AV:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Av, data));
                    break;

                case ActionEvent.Command.SET_TV:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Tv, data));
                    break;

                case ActionEvent.Command.SET_ISO_SPEED:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_ISOSpeed, data));
                    break;

                case ActionEvent.Command.SET_METERING_MODE:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_MeteringMode, data));
                    break;

                case ActionEvent.Command.SET_DRIVE_MODE:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_DriveMode, data));
                    break;

                case ActionEvent.Command.SET_WHITE_BALANCE:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_WhiteBalance, data));
                    break;

                case ActionEvent.Command.SET_EXPOSURE_COMPENSATION:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_ExposureCompensation, data));
                    break;

                case ActionEvent.Command.SET_IMAGEQUALITY:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_ImageQuality, data));
                    break;

                case ActionEvent.Command.SET_EVF_AFMODE:
                    // When switching a EVF AF MODE, cancel EVF AF ON
                    _processor.PostCommand(new DoEvfAFCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsEvfAf.CameraCommand_EvfAf_OFF));
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Evf_AFMode, data));
                    break;

                case ActionEvent.Command.SET_ZOOM:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_DC_Zoom, data));
                    break;

                case ActionEvent.Command.SET_FLASH_MODE:
                    data = (uint)e.GetArg();
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_DC_Strobe, data));
                    break;

                case ActionEvent.Command.FOCUS_NEAR1:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Near1));
                    break;

                case ActionEvent.Command.FOCUS_NEAR2:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Near2));
                    break;

                case ActionEvent.Command.FOCUS_NEAR3:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Near3));
                    break;

                case ActionEvent.Command.FOCUS_FAR1:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Far1));
                    break;

                case ActionEvent.Command.FOCUS_FAR2:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Far2));
                    break;

                case ActionEvent.Command.FOCUS_FAR3:
                    _processor.PostCommand(new DriveLensCommand(ref _model, EDSDKLib.EDSDK.EvfDriveLens_Far3));
                    break;

                case ActionEvent.Command.DOWNLOAD:
                    IntPtr inRef = (IntPtr)e.GetArg();
                    _processor.PostCommand(new DownloadCommand(ref _model, ref inRef));
                    break;

                case ActionEvent.Command.CLOSING:
                    _processor.PostCommand(new CloseSessionCommand(ref _model));
                    _processor.Stop();
                    break;

                case ActionEvent.Command.SHUT_DOWN:
                    _processor.Stop();

                    CameraEvent shotDownEvent = new CameraEvent(CameraEvent.Type.SHUT_DOWN, IntPtr.Zero);
                    _model.NotifyObservers(shotDownEvent);
                    break;
                
                case ActionEvent.Command.EVF_AF_ON:
                    _processor.PostCommand(new DoEvfAFCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsEvfAf.CameraCommand_EvfAf_ON));
                    break;

                case ActionEvent.Command.EVF_AF_OFF:
                     _processor.PostCommand(new DoEvfAFCommand(ref _model, (uint)EDSDKLib.EDSDK.EdsEvfAf.CameraCommand_EvfAf_OFF));
                    break;

                case ActionEvent.Command.ZOOM_FIT:
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Evf_Zoom, EDSDKLib.EDSDK.EvfZoom_Fit));
                    break;

                case ActionEvent.Command.ZOOM_ZOOM:
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Evf_Zoom, EDSDKLib.EDSDK.EvfZoom_x5));
                    break;

                case ActionEvent.Command.POSITION_UP:
                case ActionEvent.Command.POSITION_DOWN:
                    const int stepY = 128;
                    point = _model.GetZoomPosition();
                    if (command == ActionEvent.Command.POSITION_UP)
                    {
                        point.y -= stepY;
                        if (point.y < 0) point.y = 0;
                    }
                    if (command == ActionEvent.Command.POSITION_DOWN) point.y += stepY;
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Evf_ZoomPosition, point));
                    break;

                case ActionEvent.Command.POSITION_LEFT:
                case ActionEvent.Command.POSITION_RIGHT:
                    const int stepX = 128;
                    point = _model.GetZoomPosition();
                    if (command == ActionEvent.Command.POSITION_LEFT)
                    {
                        point.x -= stepX;
                        if (point.x < 0) point.x = 0;
                    }
                    if (command == ActionEvent.Command.POSITION_RIGHT) point.x += stepX;
                    _processor.PostCommand(new SetPropertyCommand(ref _model, EDSDKLib.EDSDK.PropID_Evf_ZoomPosition, point));
                    break;

                case ActionEvent.Command.REMOTESHOOTING_START:
                    _processor.PostCommand(new SetRemoteShootingCommand(ref _model, (uint)EDSDKLib.EDSDK.DcRemoteShootingMode.DcRemoteShootingModeStart));
                    break;

                case ActionEvent.Command.REMOTESHOOTING_STOP:
                    _processor.PostCommand(new SetRemoteShootingCommand(ref _model, (uint)EDSDKLib.EDSDK.DcRemoteShootingMode.DcRemoteShootingModeStop));
                    break;
            }
        }

        public void DownloadFile()
        {
            _processor.PostCommand(new DownloadAllFilesCommand(ref _model));
        }

        public void DeleteFile()
        {
            _processor.PostCommand(new DeleteAllFilesCommand(ref _model));
        }

        public void FormatVolumeCommand()
        {
            _processor.PostCommand(new FormatVolumeCommand(ref _model));
        }
    }
}
