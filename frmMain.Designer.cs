namespace PhotonController
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblPercentDone = new System.Windows.Forms.Label();
            this.PrintProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numMaxLiftHeight = new System.Windows.Forms.NumericUpDown();
            this.numSkipFirst = new System.Windows.Forms.NumericUpDown();
            this.numTakeEvery = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStopTimeLapse = new System.Windows.Forms.Button();
            this.btnStartTimeLapse = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblZheight = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLiftHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTakeEvery)).BeginInit();
            this.SuspendLayout();
            // 
            // pollTimer
            // 
            this.pollTimer.Interval = 1000;
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblZheight);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.lblPercentDone);
            this.groupBox1.Controls.Add(this.PrintProgress);
            this.groupBox1.Location = new System.Drawing.Point(13, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 129);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Status";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(175, 22);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(94, 22);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 15;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(13, 22);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // lblPercentDone
            // 
            this.lblPercentDone.AutoSize = true;
            this.lblPercentDone.Location = new System.Drawing.Point(10, 55);
            this.lblPercentDone.Name = "lblPercentDone";
            this.lblPercentDone.Size = new System.Drawing.Size(73, 13);
            this.lblPercentDone.TabIndex = 13;
            this.lblPercentDone.Text = "Precent Done";
            // 
            // PrintProgress
            // 
            this.PrintProgress.Location = new System.Drawing.Point(10, 71);
            this.PrintProgress.Name = "PrintProgress";
            this.PrintProgress.Size = new System.Drawing.Size(240, 23);
            this.PrintProgress.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPort);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 110);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connection Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(10, 84);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 13);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Not Connected";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(175, 74);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Default Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Photon IP Address";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(137, 45);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(113, 20);
            this.txtPort.TabIndex = 15;
            this.txtPort.Text = "3000";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(137, 19);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(113, 20);
            this.txtIP.TabIndex = 14;
            this.txtIP.Text = "192.168.1.222";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numMaxLiftHeight);
            this.groupBox3.Controls.Add(this.numSkipFirst);
            this.groupBox3.Controls.Add(this.numTakeEvery);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnStopTimeLapse);
            this.groupBox3.Controls.Add(this.btnStartTimeLapse);
            this.groupBox3.Location = new System.Drawing.Point(13, 263);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(262, 136);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Lapse Settings";
            // 
            // numMaxLiftHeight
            // 
            this.numMaxLiftHeight.Location = new System.Drawing.Point(137, 71);
            this.numMaxLiftHeight.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numMaxLiftHeight.Name = "numMaxLiftHeight";
            this.numMaxLiftHeight.Size = new System.Drawing.Size(113, 20);
            this.numMaxLiftHeight.TabIndex = 26;
            this.numMaxLiftHeight.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // numSkipFirst
            // 
            this.numSkipFirst.Location = new System.Drawing.Point(137, 45);
            this.numSkipFirst.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numSkipFirst.Name = "numSkipFirst";
            this.numSkipFirst.Size = new System.Drawing.Size(113, 20);
            this.numSkipFirst.TabIndex = 25;
            this.numSkipFirst.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numTakeEvery
            // 
            this.numTakeEvery.Location = new System.Drawing.Point(137, 19);
            this.numTakeEvery.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTakeEvery.Name = "numTakeEvery";
            this.numTakeEvery.Size = new System.Drawing.Size(113, 20);
            this.numTakeEvery.TabIndex = 24;
            this.numTakeEvery.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Move to Height (mm)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Skip First n layers";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Snap every nth layer";
            // 
            // btnStopTimeLapse
            // 
            this.btnStopTimeLapse.Location = new System.Drawing.Point(142, 102);
            this.btnStopTimeLapse.Name = "btnStopTimeLapse";
            this.btnStopTimeLapse.Size = new System.Drawing.Size(108, 23);
            this.btnStopTimeLapse.TabIndex = 16;
            this.btnStopTimeLapse.Text = "Stop Time Lapse";
            this.btnStopTimeLapse.UseVisualStyleBackColor = true;
            this.btnStopTimeLapse.Click += new System.EventHandler(this.btnStopTimeLapse_Click);
            // 
            // btnStartTimeLapse
            // 
            this.btnStartTimeLapse.Location = new System.Drawing.Point(10, 102);
            this.btnStartTimeLapse.Name = "btnStartTimeLapse";
            this.btnStartTimeLapse.Size = new System.Drawing.Size(108, 23);
            this.btnStartTimeLapse.TabIndex = 15;
            this.btnStartTimeLapse.Text = "Start Time Lapse";
            this.btnStartTimeLapse.UseVisualStyleBackColor = true;
            this.btnStartTimeLapse.Click += new System.EventHandler(this.btnStartTimeLapse_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Log";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(397, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Snap";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblZheight
            // 
            this.lblZheight.AutoSize = true;
            this.lblZheight.Location = new System.Drawing.Point(10, 107);
            this.lblZheight.Name = "lblZheight";
            this.lblZheight.Size = new System.Drawing.Size(106, 13);
            this.lblZheight.TabIndex = 20;
            this.lblZheight.Text = "Z Height = Unknown";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 413);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.Text = "Photon Time Lapse Controller";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLiftHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTakeEvery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer pollTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblPercentDone;
        private System.Windows.Forms.ProgressBar PrintProgress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStopTimeLapse;
        private System.Windows.Forms.Button btnStartTimeLapse;
        private System.Windows.Forms.NumericUpDown numMaxLiftHeight;
        private System.Windows.Forms.NumericUpDown numSkipFirst;
        private System.Windows.Forms.NumericUpDown numTakeEvery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblZheight;
    }
}

