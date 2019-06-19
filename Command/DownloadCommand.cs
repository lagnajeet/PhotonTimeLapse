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
    class DownloadCommand : Command
    {
        private IntPtr _directoryItem = IntPtr.Zero;

        public DownloadCommand(ref CameraModel model,ref IntPtr inRef) : base(ref model)
        {
            _directoryItem = inRef;
        }

        ~DownloadCommand() 
        {
            if (_directoryItem != IntPtr.Zero)
            {
                EDSDKLib.EDSDK.EdsRelease(_directoryItem);
                _directoryItem = IntPtr.Zero;
            }
        }

        private CameraEvent _event;
        public override bool Execute()
	    {
                if(_model.canDownloadImage == false)
                {
                    return true;
                }

		        uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
		        IntPtr stream = IntPtr.Zero;


		        //Acquisition of the downloaded image information
		        EDSDKLib.EDSDK.EdsDirectoryItemInfo	dirItemInfo;
		        err = EDSDKLib.EDSDK.EdsGetDirectoryItemInfo( _directoryItem, out dirItemInfo);
	
		        // Forwarding beginning notification	
		        if(err == EDSDKLib.EDSDK.EDS_ERR_OK)
		        {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.DOWNLOAD_START, stream);
                    _model.NotifyObservers(e);
		        }

		        //Make the file stream at the forwarding destination
		        if(err == EDSDKLib.EDSDK.EDS_ERR_OK)
		        {	
			        err = EDSDKLib.EDSDK.EdsCreateFileStream(dirItemInfo.szFileName, EDSDKLib.EDSDK.EdsFileCreateDisposition.CreateAlways, EDSDKLib.EDSDK.EdsAccess.ReadWrite, out stream);
		        }

                //Set Progress
                if (err == EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    err = EDSDKLib.EDSDK.EdsSetProgressCallback(stream, ProgressFunc, EDSDKLib.EDSDK.EdsProgressOption.Periodically, stream);
                }

                //Down load image
		        if(err == EDSDKLib.EDSDK.EDS_ERR_OK)
		        {
			        err = EDSDKLib.EDSDK.EdsDownload( _directoryItem, dirItemInfo.Size, stream);
		        }

		        //Forwarding completion
		        if(err == EDSDKLib.EDSDK.EDS_ERR_OK)
		        {
			        err = EDSDKLib.EDSDK.EdsDownloadComplete( _directoryItem);
		        }

		        //Release Item
		        if(_directoryItem != IntPtr.Zero)
		        {
			        err = EDSDKLib.EDSDK.EdsRelease( _directoryItem);
			        _directoryItem = IntPtr.Zero;
		        }

		        //Release stream
		        if(stream != IntPtr.Zero)
		        {
			        err =  EDSDKLib.EDSDK.EdsRelease(stream);
			        stream = IntPtr.Zero;
		        }		
		
		        // Forwarding completion notification
		        if( err == EDSDKLib.EDSDK.EDS_ERR_OK)
		        {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, stream);
                    _model.NotifyObservers(e);
		        }

		        //Notification of error
		        if( err != EDSDKLib.EDSDK.EDS_ERR_OK)
		        {
                    CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
                    _model.NotifyObservers(e);
		        }
		        return true;
	    }

        private uint ProgressFunc(uint inPercent, IntPtr inContext, ref bool outCancel)
        {
            _event = new CameraEvent(CameraEvent.Type.PROGRESS, (IntPtr)inPercent);
            _model.NotifyObservers(_event);
            return 0;
        }
    }
}
