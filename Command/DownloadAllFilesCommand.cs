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
using System.Collections.Generic;

namespace CameraControl
{
    class DownloadAllFilesCommand : Command
    {
        public DownloadAllFilesCommand(ref CameraModel model) : base(ref model) { }

        private static FileCounterCommand.FileNumber _fileNumber;
        private static int _currentFileNum = 0;
        private static int _fileCount = 0;
        private static List<IntPtr> _imageItems = new List<IntPtr>();

        private CameraEvent _event;
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            IntPtr camera = _model.Camera;
            IntPtr directoryItem = IntPtr.Zero;
            FileCounterCommand fileCounter = new FileCounterCommand(ref _model);

            // Prepare Download
            if (_currentFileNum == 0)
            {
                int directoryCount;
                err = fileCounter.CountDirectory(camera, ref directoryItem, out directoryCount);

                _fileNumber = new FileCounterCommand.FileNumber(directoryCount);
                _fileNumber.DcimItem = directoryItem;
                err = fileCounter.CountImages(camera, ref directoryItem, ref directoryCount, ref _fileCount, ref _fileNumber, ref _imageItems);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return false;
                }

                // Show progress.
                _event = new CameraEvent(CameraEvent.Type.DOWNLOAD_START,(IntPtr) _fileCount);
                _model.NotifyObservers(_event);
            }

            // Download file.
            err = DownloadImageByDirectory(camera);
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                return false;
            }
            return true;
        }

        private uint DownloadImageByDirectory( IntPtr camera)
        {
            IntPtr fileitem;
            EDSDKLib.EDSDK.EdsDirectoryItemInfo dirItemInfo;
            dirItemInfo.szFileName = "";
            dirItemInfo.Size = 0;

            int index = 0;

            for (index = 0; index < _imageItems.Count; ++index)
            {

                fileitem = _imageItems[_currentFileNum];
                uint err = EDSDKLib.EDSDK.EdsGetDirectoryItemInfo(fileitem, out dirItemInfo);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return err;
                }

                // Create file stream for transfer destination
                IntPtr stream;
                var szDstFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                szDstFileName += "\\" + dirItemInfo.szFileName;

                err = EDSDKLib.EDSDK.EdsCreateFileStream(szDstFileName,
                    EDSDKLib.EDSDK.EdsFileCreateDisposition.CreateAlways,
                    EDSDKLib.EDSDK.EdsAccess.ReadWrite, out stream);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return err;
                }

                //Set Progress
                //err = EdsSetProgressCallback(stream, ProgressFunc, EDSDKLib.EDSDK.kEdsProgressOption_Periodically, this);
                // Download image
                err = EDSDKLib.EDSDK.EdsDownload(fileitem, dirItemInfo.Size, stream);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return err;
                }

                // Issue notification that download is complete
                err = EDSDKLib.EDSDK.EdsDownloadComplete(fileitem);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return err;
                }

                //Release Item
                if (fileitem != null)
                {
                    err = EDSDKLib.EDSDK.EdsRelease(fileitem);
                    fileitem = IntPtr.Zero;
                }

                //Release stream
                if (stream != null)
                {
                    err = EDSDKLib.EDSDK.EdsRelease(stream);
                    stream = IntPtr.Zero;
                }

                _currentFileNum += 1;
                // Continue next 1 file download. 
                if (_model._ExecuteStatus == CameraModel.Status.CANCELING)
                {
                    _event = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, (IntPtr)index);
                    _model.NotifyObservers(_event);
                    _currentFileNum = 0;
                    _imageItems.Clear();
                }
                else
                {
                    _event = new CameraEvent(CameraEvent.Type.PROGRESS_REPORT, (IntPtr)_currentFileNum);
                    _model.NotifyObservers(_event);
                }
            }
            // Finish download files.
            _event = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, camera);
            _model.NotifyObservers(_event);
            _currentFileNum = 0;
            _imageItems.Clear();
            return EDSDKLib.EDSDK.EDS_ERR_OK;

        }

    }

}
