namespace SpaceInvaderX
{
    partial class Form1
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
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.stage1 = new SpaceInvaderX.Engine.Stage();
            this.SuspendLayout();
            // 
            // _timer
            // 
            this._timer.Interval = 200;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // stage1
            // 
            this.stage1.BackColor = System.Drawing.Color.Black;
            this.stage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stage1.FrameDuration = 15;
            this.stage1.IsStarted = false;
            this.stage1.Location = new System.Drawing.Point(0, 0);
            this.stage1.Name = "stage1";
            this.stage1.Size = new System.Drawing.Size(398, 278);
            this.stage1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 278);
            this.Controls.Add(this.stage1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Engine.Stage stage1;
        private System.Windows.Forms.Timer _timer;
    }
}

