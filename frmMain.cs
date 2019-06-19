using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using CameraControl;
using System.Runtime.InteropServices;

namespace PhotonController
{
    public partial class frmMain : Form, IObserver
    {
        private CameraController _controller = null;
        private ActionSource _actionSource = new ActionSource();
        private List<ActionListener> _actionListenerList = new List<ActionListener>();
        private IObserver formAs = null;

        int receiverPort = 4023;
        int takeEvery = 3; //th picture
        int skipFirst = 6; //frames
        double MaxLiftHeight = 45;
        IPEndPoint ipEndPoint;
        UdpClient receiver;
        string gcodeCmd = "";
        int framenumber = 0;
        double progressBefore = -9999;
        double zposbefore = -9999;
        double prevzoffset = 0;
        bool timelapse = false;
        frmLog logForm = new frmLog();
        int progressCount = 0;
        bool isPrinting = false;
        bool FrameSnapped = false;
        bool FirstSnap = false;
        bool snapComplete = false;
        bool snapped = false;
        public frmMain(ref CameraController controller)
        {
            InitializeComponent();
            _controller = controller;
            _actionListenerList.Add((ActionListener)_controller);
            _actionListenerList.ForEach(actionListener => _actionSource.AddActionListener(ref actionListener));
        }

        public void Update(Observable from, CameraEvent e)
        {
            CameraEvent.Type eventType = e.GetEventType();

            switch (eventType)
            {
                case CameraEvent.Type.DOWNLOAD_COMPLETE:
                    SetText(logForm.txtResponse.Text+"Image Saved to Disk\r\n");
                    snapComplete = true;
                    break;
                case CameraEvent.Type.SHUT_DOWN:
                    SetText(logForm.txtResponse.Text+"Camera is disconnected\r\n");
                    break;
                default:
                    break;
            }
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (logForm.txtResponse.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                logForm.txtResponse.Text = text;
                logForm.txtResponse.SelectionStart = logForm.txtResponse.Text.Length;
                logForm.txtResponse.ScrollToCaret();
                logForm.txtResponse.Refresh();
            }
        }

        delegate void SetZheightCallback(string text);

        private void SetZheight(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lblZheight.InvokeRequired)
            {
                SetZheightCallback d = new SetZheightCallback(SetZheight);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblZheight.Text = text;
                lblZheight.Refresh();
            }
        }


        delegate void SetProgressCallback(int value);

        private void SetProgress(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.PrintProgress.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetProgress);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.PrintProgress.Value = value;
            }
        }
        delegate void SetLblCallback(string text);

        private void SetLbl(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.PrintProgress.InvokeRequired)
            {
                SetLblCallback d = new SetLblCallback(SetLbl);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblPercentDone.Text = text;
            }
        }
        private void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

            //SetText(logForm.txtResponse.Text + receivedText);
            //logForm.txtResponse.SelectionStart = logForm.txtResponse.Text.Length;
            if (gcodeCmd == "M27")     //progress report
            {
                string[] lines = receivedText.Split('\n');
                lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                double[] progress = new double[2];
                if ((lines[0].Substring(0, 5).ToUpper() != "ERROR") && (lines[0].Substring(0, 2).ToUpper() != "OK"))
                {
                    isPrinting = true;
                    lines = lines[0].Split(new[] { "byte" }, StringSplitOptions.None);
                    lines = lines[1].Split('/');
                    progress[0] = double.Parse(lines[0].Trim());
                    progress[1] = double.Parse(lines[1].Trim());
                    //SetText(logForm.txtResponse.Text + progressCount.ToString()+"\r\n");
                    if (progress[0] > progressBefore && progressCount>=1)
                    {
                        progressCount = 0;
                        int PrintProgress = (int)Math.Round((progress[0] * 100) / progress[1]);
                        SetProgress(PrintProgress);
                        SetLbl("Percent done : " + PrintProgress.ToString());
                        progressBefore = progress[0];
                        SetText(logForm.txtResponse.Text + "Frame = "+framenumber.ToString() + ", ");

                        if (timelapse)
                        {
                            FrameSnapped = false;
                            if (framenumber >= skipFirst)
                            {
                                if ((framenumber- skipFirst) % takeEvery == 0)
                                {
                                    gcodeCmd = "M25";
                                    receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                    receivedBytes = receiver.Receive(ref ipEndPoint);
                                    receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

                                    gcodeCmd = "M114";
                                    //receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                    //receivedBytes = receiver.Receive(ref ipEndPoint);
                                    //receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                                    FrameSnapped = true;
                                }
                                //else
                                //{
                                //    if(PrintProgress>=100)          //It's not the nth frame but the print finished. take the last frame
                                //    {
                                //        gcodeCmd = "M25";
                                //        receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                //        receivedBytes = receiver.Receive(ref ipEndPoint);
                                //        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

                                //        gcodeCmd = "M114";
                                //    }
                                //}
                            }
                        }
                        framenumber++;
                    }
                    else if (progress[0] > progressBefore && progressCount <1)
                    {
                        progressCount++;
                    }

                    //txtResponse.SelectionStart = txtResponse.Text.Length;
                    //txtResponse.ScrollToCaret();
                    //txtResponse.Refresh();
                }
                else if(lines[0].Substring(0, 5).ToUpper() == "ERROR")
                {
                    if(isPrinting)      //means it was printing and now it finsihed
                    {
                        SetProgress(100);
                        SetLbl("Percent done : 100" );
                        if(timelapse)
                        {
                            if(!FrameSnapped)   //The lasr frame was not snapped coz it was not the nth frame
                            {
                                gcodeCmd = "M114";
                                //receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                //receivedBytes = receiver.Receive(ref ipEndPoint);
                                //receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                                FrameSnapped = true;
                            }
                            timelapse = false;                  //stop time lapse
                        }
                    }
                    isPrinting = false;
                }

            }
            else if (gcodeCmd == "M114")        //Get current Z value untill it settles
            {
                if (receivedText.IndexOf("X:0.000000") != -1)
                {
                    string[] lines = receivedText.Split(new[] { "Z:" }, StringSplitOptions.None);
                    lines = lines[1].Split(new[] { "E:" }, StringSplitOptions.None);
                    double zpos = double.Parse(lines[0]);
                    SetZheight("Z Height = " + Math.Round(zpos, 3).ToString());
                    if (zposbefore != zpos)
                        zposbefore = zpos;
                    else
                    {
                        double Zoffset = MaxLiftHeight - zpos;
                        gcodeCmd = "G91, G0 Z" + Zoffset.ToString() + " F300";
                        receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                        prevzoffset = Zoffset;
                        zposbefore = 0;
                        snapped = false;
                        snapComplete = false;
                        gcodeCmd = "M4000";
                    }

                }
            }
            else if (gcodeCmd == "M4000")        //Get current Z value untill it settles
            {
                if (receivedText.IndexOf("X:0.000") != -1)
                {
                    string[] lines = receivedText.Split(new[] { "Z:" }, StringSplitOptions.None);
                    lines = lines[1].Split(new[] { "F:" }, StringSplitOptions.None);
                    double zpos = double.Parse(lines[0]);
                    SetZheight("Z Height = "+Math.Round(zpos, 3).ToString());
                    if (zposbefore != zpos)
                        zposbefore = zpos;
                    else
                    {
                        if (!snapped)
                        {
                            if (!FirstSnap)
                            {
                                SetText(logForm.txtResponse.Text + "Time Lapse Starts\r\n");
                                MessageBox.Show("Setup the Camera for time Lapse and click OK.");
                                FirstSnap = true;
                            }
                            snapComplete = false;
                            _actionSource.FireEvent(ActionEvent.Command.TAKE_PICTURE, IntPtr.Zero);
                            SetText(logForm.txtResponse.Text + "Take Picture\r\n");
                            snapped = true;
                        }
                        if (snapComplete)
                        {
                            gcodeCmd = "G91, G0 Z" + (-1 * prevzoffset).ToString() + " F300"; ;
                            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                            receivedBytes = receiver.Receive(ref ipEndPoint);
                            receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

                            if (isPrinting)      //if the printing it going on then resume print
                            {
                                gcodeCmd = "M24";
                                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                receivedBytes = receiver.Receive(ref ipEndPoint);
                                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                            }
                            zposbefore = 0;
                            snapComplete = false;
                            snapped = false;
                            gcodeCmd = "M27";
                        }
                    }

                }
            }
            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            receiver = new UdpClient(receiverPort);
            //receiver.BeginReceive(DataReceived, receiver);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));
            receiver.Connect(ipEndPoint);

            gcodeCmd = "M114";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            if (receivedText.IndexOf("X:0.000000") != -1)
                lblStatus.Text = "Connected";
            else
                lblStatus.Text = "Not Connected";
            gcodeCmd = "M27";
            receiver.BeginReceive(DataReceived, receiver);
            pollTimer.Enabled = true;
        }

        public void SendGcode()
        {

            //using (UdpClient sender1 = new UdpClient(ipEndPoint))
            gcodeCmd = logForm.txtCmd.Text;
            receiver.Send(Encoding.ASCII.GetBytes(logForm.txtCmd.Text), logForm.txtCmd.Text.Length);
        }

        private void pollTimer_Tick(object sender, EventArgs e)
        {
            if(gcodeCmd=="M27" || gcodeCmd=="M114" || gcodeCmd=="M4000")
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
        }

        private void getPrintPercent()
        {
            gcodeCmd = "M27";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M25";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = false;
            //btnStart.Enabled = true;
            //btnStop.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M24";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = true;
            //btnStart.Enabled = false;
            //btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M29";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = false;
            //btnStart.Enabled = true;
            //btnStop.Enabled = false;
        }

        private void btnStartTimeLapse_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M27";
            takeEvery = (int)numTakeEvery.Value; //th picture
            skipFirst = (int)numSkipFirst.Value; //frames
            MaxLiftHeight = (double)numMaxLiftHeight.Value;
            PrintProgress.Value = 0;
            timelapse = true;
            FirstSnap = false;
        }

        private void btnStopTimeLapse_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M27";
            timelapse = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _actionSource.FireEvent(ActionEvent.Command.TAKE_PICTURE, IntPtr.Zero);
        }
    }
}
