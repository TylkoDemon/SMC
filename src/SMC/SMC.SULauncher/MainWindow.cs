//
// Super Minecraft Launcher(SU) Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core;
using JEM.Downloader;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SMC.SULauncher
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            RestartComponents();

            SMCDownloader.OnStatusChanged += SMCDownloader_StatusChanged;
            SMCDownloader.OnProgressChange += SMCDownloader_ProgressChanged;
            SMCDownloader.OnShutdownRequest += SMCDownloader_ShutdownRequest;
        }

        private void RestartComponents()
        {
            labelCurrent.Text = string.Empty;
            labelGlobal.Text = string.Empty;

            progressBarCurrent.Value = 0;
            progressBarGlobal.Value = 0;
        }

        private void SMCDownloader_StatusChanged(JEMDownloaderStatus JEMDownloaderStatus)
        {
            JEMLogger.Log($"SMCDownloader status changed: {JEMDownloaderStatus}");
            Invoke((MethodInvoker)delegate
            {
                switch (JEMDownloaderStatus)
                {
                    case JEMDownloaderStatus.STARTING:
                        labelCurrent.Text = @"Starting SMC";
                        progressBarCurrent.Value = 00;
                        labelGlobal.Text = @"Starting SMC";
                        progressBarGlobal.Value = 00;
                        break;
                    case JEMDownloaderStatus.PREPARING_SPACE:
                        labelCurrent.Text = @"Starting SMC";
                        progressBarCurrent.Value = 10;
                        labelGlobal.Text = @"Starting SMC";
                        progressBarGlobal.Value = 20;
                        break;
                    case JEMDownloaderStatus.INFO_FAILED:
                        labelCurrent.Text = @"SMC Info Failed";
                        MessageBox.Show(
                            @"The system was unable to download the necessary information needed for the update. Please try again later.",
                            @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Environment.Exit(9);
                        break;
                    case JEMDownloaderStatus.INFO_LOADED:
                        labelCurrent.Text = @"SMC Info Loaded";
                        progressBarCurrent.Value = 30;
                        labelGlobal.Text = @"Starting SMC";
                        progressBarGlobal.Value = 30;
                        break;
                    case JEMDownloaderStatus.DISABLED_BY_INFO:
                        labelCurrent.Text = @"SMC Disabled by server.";
                        MessageBox.Show(
                            @"Sorry but our servers are currently disabled. Most likely, this is due to maintenance work or an update process. Please try again later.",
                            @"Oops");
                        Environment.Exit(9);
                        break;
                    case JEMDownloaderStatus.INITIALIZED:
                        labelCurrent.Text = @"SMC Initialized";
                        progressBarCurrent.Value = 40;
                        labelGlobal.Text = @"Starting SMC";
                        progressBarGlobal.Value = 40;
                        if (!SMCDownloader.MakeUpdate(true))
                        {
                            MessageBox.Show(
                                @"System was unable to run update process. Internal downloader error.",
                                @"Oops");
                            Environment.Exit(9);
                        }

                        break;
                    case JEMDownloaderStatus.INITIALIZING_PACKAGES:
                        labelCurrent.Text = @"Getting server packages";
                        progressBarCurrent.Value = 55;
                        labelGlobal.Text = @"Checking Files";
                        progressBarGlobal.Value = 55;
                        break;
                    case JEMDownloaderStatus.INITIALIZING_SERVERCHECKSUM:
                        labelCurrent.Text = @"Getting server files";
                        progressBarCurrent.Value = 50;
                        labelGlobal.Text = @"Checking Files";
                        progressBarGlobal.Value = 50;
                        break;
                    case JEMDownloaderStatus.INITIALIZING_CLIENTCHECKSUM:
                        labelCurrent.Text = @"Getting local files";
                        progressBarCurrent.Value = 60;
                        labelGlobal.Text = @"Checking Files";
                        progressBarGlobal.Value = 50;
                        break;
                    case JEMDownloaderStatus.INITIALIZING_LIST:
                        labelCurrent.Text = @"Getting target files";
                        progressBarCurrent.Value = 70;
                        labelGlobal.Text = @"Checking Files";
                        progressBarGlobal.Value = 50;
                        break;
                    case JEMDownloaderStatus.DOWNLOADING:
                        labelCurrent.Text = @"Downloading target files";
                        progressBarCurrent.Value = 80;
                        labelGlobal.Text = @"Updating";
                        progressBarGlobal.Value = 60;
                        break;
                    case JEMDownloaderStatus.INSTALLING:
                        labelCurrent.Text = @"Installing target files";
                        progressBarCurrent.Value = 90;
                        labelGlobal.Text = @"Updating";
                        progressBarGlobal.Value = 70;
                        break;
                    case JEMDownloaderStatus.INSTALLED:
                        labelCurrent.Text = @"New files successfully installed";
                        progressBarCurrent.Value = 95;
                        labelGlobal.Text = @"Updating";
                        progressBarGlobal.Value = 80;
                        break;
                    case JEMDownloaderStatus.READY:
                        RestartComponents();

                        labelGlobal.Text = @"SMC Ready";
                        progressBarGlobal.Value = 100;

                        // start self updater
                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = Environment.GetCommandLineArgs()[1],
                                WorkingDirectory = Environment.CurrentDirectory,
                                WindowStyle = ProcessWindowStyle.Normal
                            }
                        };

                        JEMLogger.Log(
                            $"Starting updater ({process.StartInfo.FileName} at {process.StartInfo.WorkingDirectory}).");
                        process.Start();

                        Process.GetCurrentProcess().Kill();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(JEMDownloaderStatus), JEMDownloaderStatus, null);
                }
            });
        }

        private void SMCDownloader_ShutdownRequest(bool isError, string reason)
        {
            Invoke((MethodInvoker)delegate
            {
                JEMLogger.LogWarning(
                    $"Shutting down by downloader. Error: {(isError ? "Yes" : "no")} Reason: {reason}");
                if (isError)
                {
                    MessageBox.Show(reason, @"Oops (Downloader)", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                Environment.Exit(9);
            });
        }

        private void SMCDownloader_ProgressChanged(JEMDownloaderProgressType type, int value, string text)
        {
            Invoke((MethodInvoker)delegate
            {
                switch (type)
                {
                    case JEMDownloaderProgressType.ACTION:
                        labelCurrent.Text = text;
                        progressBarGlobal.Value = value;
                        break;
                    case JEMDownloaderProgressType.GLOBAL:
                        labelGlobal.Text = text;
                        progressBarGlobal.Value = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            });
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            SMCDownloader.Initialize();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SMCDownloader.ShutDownDownloader();
        }

        /// <summary>
        /// SMC Downloader instance.
        /// </summary>
        public JEMDownloader SMCDownloader { get; } = new JEMDownloader(Environment.CurrentDirectory,
            "http://nggamesrg.nazwa.pl/smcup_self/", "info.json", new string[0]);
    }
}
