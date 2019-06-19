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
    public interface IObserver
    {
        void Update(Observable observable, CameraEvent e);
    }

    public abstract class Observable
    {
       private List<IObserver> observers = new List<IObserver>();

        public void Add(ref IObserver observer)
        {
            observers.Add(observer);
        }

        public void Remove(ref IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(CameraEvent e)
        {
            for (var i = 0; i < observers.Count; i++)
            {
                observers[i].Update(this, e);
            }
        }
    }
}
