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
    abstract class Command
    {
        protected CameraModel _model;

        public Command(ref CameraModel model)
        {
            _model = model;
        }

        public CameraModel GetCameraModel()
        {
            return _model;
        }

        public virtual bool Execute()
        {
            return true;
        }
    }
}
