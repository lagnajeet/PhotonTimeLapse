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
    public class ActionSource
    {
        List<ActionListener> listeners = new List<ActionListener>();

        public void AddActionListener(ref ActionListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveActionListener(ref ActionListener listener)
        {
            listeners.Remove(listener);
        }

        public void FireEvent(ActionEvent.Command command, IntPtr arg)
        {
            ActionEvent e = new ActionEvent(command, arg);

            for (var i = 0; i < listeners.Count; i++)
            {
                listeners[i].ActionPerformed(e);
            }
        }
    }
}
