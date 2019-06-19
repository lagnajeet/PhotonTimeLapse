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
    class DeleteAllFilesCommand : Command
    {
        public DeleteAllFilesCommand(ref CameraModel model) : base(ref model) { }

        private static FileCounterCommand.FileNumber _fileNumber;
        private static int _currentFileNum = 0;
        private static List<IntPtr> _imageItems;

        private CameraEvent _event;
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            IntPtr camera = _model.Camera;
            IntPtr directoryItem = IntPtr.Zero;
            FileCounterCommand fileCounter = new FileCounterCommand(ref _model);
            _imageItems = new List<IntPtr>();

            // Prepare delete
            if (_currentFileNum == 0)
            {
                int directoryCount = 0;
                int fileCount = 0;
                // Show progress. Display Message.
                _event = new CameraEvent(CameraEvent.Type.DELETE_START, (IntPtr)fileCount);
                _model.NotifyObservers(_event);

                err = fileCounter.CountDirectory(camera, ref directoryItem, out directoryCount);

                _fileNumber = new FileCounterCommand.FileNumber(directoryCount);
                _fileNumber.DcimItem = directoryItem;

                err = fileCounter.CountImages(camera, ref directoryItem, ref directoryCount, ref fileCount, ref _fileNumber, ref _imageItems);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return false;
                }
                // Show progress.
                _event = new CameraEvent(CameraEvent.Type.DELETE_START, (IntPtr)fileCount);
                _model.NotifyObservers(_event);

                if (fileCount == 0)
                {
                    _event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, (IntPtr) fileCount);
                    _model.NotifyObservers(_event);
                }

            }

            // Delete file.
            err = DeleteFileByDirectory(camera);
            if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
            {
                return false;
            }
            return true;
        }

        private uint DeleteFileByDirectory(IntPtr camera)
        {
            int index = 0;

            for (index = 0; index < _imageItems.Count ; ++index)
            {
                uint err = EDSDKLib.EDSDK.EdsDeleteDirectoryItem(_imageItems[index]);
                if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                {
                    return err;
                }
                EDSDKLib.EDSDK.EdsRelease(_imageItems[index]);
            
                if (_model._ExecuteStatus == CameraModel.Status.CANCELING)
                {
                    _event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, (IntPtr) index);
                    _model.NotifyObservers(_event);
                    _currentFileNum = 0;
                    // Stop always.
                    _imageItems.Clear();
                    return EDSDKLib.EDSDK.EDS_ERR_OK;
                }
                else
                {
                    _event = new CameraEvent(CameraEvent.Type.PROGRESS_REPORT, (IntPtr) index);
                    _model.NotifyObservers(_event);
                }

            } // End for item_count.
            // Finish delete files.
            _event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, camera);
            _model.NotifyObservers(_event);
            _currentFileNum = 0;
            return EDSDKLib.EDSDK.EDS_ERR_OK;
        }
    }
}
