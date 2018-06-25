//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SMC.Launcher.Scripts.Handlers
{
    internal class JavascriptHandlerMain
    {
        /// <summary>
        /// HTML initialized and ready to work.
        /// </summary>
        public void onHTMLReady()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                JEMLogger.Log("Www initialized and ready.");
                MainWindow.InitializeJEMUpdater();
            });
        }

        public void onPlay()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                MainWindow.Instance.Hide();
                var launchWindow = new LaunchWindow();
                launchWindow.ShowDialog();
                MainWindow.Instance.Show();
                //Javascript.Run($"showPlayWindow();");
            });
        }

        public void onRepair()
        {
            MainWindow.Instance.SMCDownloader.MakeUpdate(true);
        }

        public void onPlayRequest(string userName, string ram)
        {
            SMCConfiguration.Loaded.UserName = userName;
            if (int.TryParse(ram, out var RAM))
            {
                SMCConfiguration.Loaded.UserRAM = RAM;
            }
            else
            {
                MessageBox.Show(@"Given RAM is invalid.", @"Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SMCConfiguration.Save();

            if (userName.Length < 3)
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
                        Arguments = string.Join(" ", userName, RAM.ToString())
                    }
                };

                JEMLogger.Log(
                    $"Starting game ({process.StartInfo.FileName} at {process.StartInfo.WorkingDirectory}).");
                process.Start();

                Process.GetCurrentProcess().Kill();
            }
        }
    }
}