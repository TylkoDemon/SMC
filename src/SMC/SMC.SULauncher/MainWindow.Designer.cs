namespace SMC.SULauncher
{
    partial class MainWindow
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
            this.labelGlobal = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.progressBarCurrent = new System.Windows.Forms.ProgressBar();
            this.progressBarGlobal = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelGlobal
            // 
            this.labelGlobal.AutoSize = true;
            this.labelGlobal.Location = new System.Drawing.Point(12, 61);
            this.labelGlobal.Name = "labelGlobal";
            this.labelGlobal.Size = new System.Drawing.Size(35, 13);
            this.labelGlobal.TabIndex = 8;
            this.labelGlobal.Text = "label2";
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(12, 14);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(35, 13);
            this.labelCurrent.TabIndex = 7;
            this.labelCurrent.Text = "label1";
            // 
            // progressBarCurrent
            // 
            this.progressBarCurrent.Location = new System.Drawing.Point(12, 30);
            this.progressBarCurrent.Name = "progressBarCurrent";
            this.progressBarCurrent.Size = new System.Drawing.Size(609, 23);
            this.progressBarCurrent.TabIndex = 6;
            // 
            // progressBarGlobal
            // 
            this.progressBarGlobal.Location = new System.Drawing.Point(13, 77);
            this.progressBarGlobal.Name = "progressBarGlobal";
            this.progressBarGlobal.Size = new System.Drawing.Size(609, 23);
            this.progressBarGlobal.TabIndex = 5;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 119);
            this.Controls.Add(this.labelGlobal);
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.progressBarCurrent);
            this.Controls.Add(this.progressBarGlobal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMC Launcher (Self Updater)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGlobal;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.ProgressBar progressBarCurrent;
        private System.Windows.Forms.ProgressBar progressBarGlobal;
    }
}

