//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using JEM.Core;

namespace SMC.Launcher
{
    public partial class LaunchWindow : Form
    {
        public LaunchWindow()
        {
            InitializeComponent();
        }

        private void SaveLaunchOptions()
        {
            SMCConfiguration.Loaded.UserName = textBoxUsername.Text;
            if (int.TryParse(textBoxRam.Text, out var ram))
            {
                SMCConfiguration.Loaded.UserRAM = ram;
            }
            SMCConfiguration.Save();
        }

        private void LaunchWindow_Load(object sender, EventArgs e)
        {
            textBoxUsername.Text = SMCConfiguration.Loaded.UserName;
            textBoxRam.Text = SMCConfiguration.Loaded.UserRAM.ToString();
        }

        private void LaunchWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLaunchOptions();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxRam.Text, out var ram))
            {
                MessageBox.Show(@"Given RAM is invalid.", @"Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBoxUsername.Text.Length < 3)
                {
                    MessageBox.Show(@"Given Username is too short.", @"Oops", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    var executable = $@"{MainWindow.Instance.SMCDownloader.WorkDir}\SMC.MC.exe";
                    if (!File.Exists(executable))
                    {
                        var result =
                            MessageBox.Show($@"Target file {executable} not exist. Do you want to run repair process?",
                                @"Oops.", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (result == DialogResult.Yes)
                        {
                            MainWindow.Instance.SMCDownloader.MakeUpdate(true);
                        }

                        return;
                    }

                    // start game
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = executable,
                            WorkingDirectory = MainWindow.Instance.SMCDownloader.WorkDir,
                            WindowStyle = ProcessWindowStyle.Normal,
                            Arguments = string.Join(" ", textBoxUsername.Text, ram.ToString())
                        }
                    };

                    JEMLogger.Log(
                        $"Starting game ({process.StartInfo.FileName} at {process.StartInfo.WorkingDirectory}).");
                    process.Start();

                    SaveLaunchOptions();
                    Process.GetCurrentProcess().Kill();
                }
            }
        }
    }
}
