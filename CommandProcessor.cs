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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace CameraControl
{
    class CommandProcessor
    {
       private ConcurrentQueue<Command> _commandQueue = new ConcurrentQueue<Command>();
       private bool _running = false;
       private Task _task = null;

        public void Start()
        {
            _running = true;

            _task = Task.Run(() =>
            {
                while (_running)
                {
                    Thread.Sleep(1);

                    Command command = null;
                    _commandQueue.TryDequeue(out command);
                    if (command != null)
                    {
                        if (command.Execute() == false)
                        {
                            //If commands that were issued fail ( because of DeviceBusy or other reasons )
                            // and retry is required , note that some cameras may become unstable if multiple 
                            // commands are issued in succession without an intervening interval.
                            //Thus, leave an interval of about 500 ms before commands are reissued.
                            Thread.Sleep(500);
                            _commandQueue.Enqueue(command);
                        }
                    }
                }
            });

            // Command of end
            //if (_closeCommand != NULL)
            //{
            //    _closeCommand->execute();
            //    delete _closeCommand;
            //    _closeCommand = NULL;
            //}

        }

        public void Stop()
        {
            _running = false;
            try
            {
                _task.Wait();
            }
            catch (AggregateException)
            {
                // ...
            }

            _task.Dispose();
        }

        public void PostCommand(Command command)
        {
            _commandQueue.Enqueue(command);
        }
    }
}
