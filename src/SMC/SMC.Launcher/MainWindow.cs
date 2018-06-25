//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using CefSharp;
using CefSharp.WinForms;
using JEM.Core;
using JEM.Downloader;
using SMC.Launcher.Scripts;
using SMC.Launcher.Scripts.Handlers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SMC.Launcher
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            Instance = this;

            InitializeComponent();
            InitializeChromium();

            SMCDownloader.OnStatusChanged += SMCDownloader_StatusChanged;
            SMCDownloader.OnProgressChange += SMCDownloader_ProgressChanged;
            SMCDownloader.OnShutdownRequest += SMCDownloader_ShutdownRequest;

            JEMSelfUpdater.OnStatusChanged += SMCSelfUpdater_StatusChanged;
            JEMSelfUpdater.OnShutdownRequest += SMCSelfUpdater_ShutdownRequest;
        }

        /// <summary>
        /// Initializes chromium.
        /// </summary>
        private void InitializeChromium()
        {
            var settings = new CefSettings
            {
                RemoteDebuggingPort = 8088,
                CachePath = "cache",
                MultiThreadedMessageLoop = true,
                ExternalMessagePump = false,
                FocusedNodeChangedEnabled = true
            };

            settings.CefCommandLineArgs.Add("no-proxy-server", "1");

            if (!Cef.Initialize(settings))
            {
                throw new Exception("Unable to Initialize Cef");
            }

            var page = $@"{Application.StartupPath}\Resources\html\index.html";
            if (!File.Exists(page))
            {
                MessageBox.Show(@"Unable to load chromium. Error The html file doesn't exists: " + page);
                Application.Exit();
                return;
            }

            Browser = new ChromiumWebBrowser(page)
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#212121")
            };

            // register js handlers
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            Browser.RegisterJsObject("csWin", new JavascriptHandlerWindows(), BindingOptions.DefaultBinder);
            Browser.RegisterJsObject("csMain", new JavascriptHandlerMain(), BindingOptions.DefaultBinder);

            // register browser handlers
            // Browser.JsDialogHandler = new CefWindowJsDialogHandler();
            Browser.IsBrowserInitializedChanged += (sender, args) =>
            {
                Instance.Invoke((MethodInvoker) delegate
                {
                    JEMLogger.Log($"Browser initialized {args.IsBrowserInitialized}");
                    if (!args.IsBrowserInitialized)
                        return;
                    //Browser.ShowDevTools();
                });
            };
            var browserSettings = new BrowserSettings
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            Browser.BrowserSettings = browserSettings;

            Controls.Add(Browser);
        }

        private void RestartComponents()
        {
            UpdateButtons(false);

            InternalCurrentText = string.Empty;
            InternalGlobalText = string.Empty;

            InternalCurrentValue = 0;
            InternalGlobalValue = 0;

            InternalUpdateProgress();
        }

        private void SMCDownloader_StatusChanged(JEMDownloaderStatus JEMDownloaderStatus)
        {
            JEMLogger.Log($"SMCDownloader status changed: {JEMDownloaderStatus}");
            Invoke((MethodInvoker) delegate
            {
                switch (JEMDownloaderStatus)
                {
                    case JEMDownloaderStatus.STARTING:
                        InternalCurrentText = @"Starting SMC";
                        InternalCurrentValue = 00;
                        InternalGlobalText= @"Starting SMC";
                        InternalGlobalValue = 00;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.PREPARING_SPACE:
                        InternalCurrentText = @"Starting SMC";
                        InternalCurrentValue = 10;
                        InternalGlobalText= @"Starting SMC";
                        InternalGlobalValue = 20;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INFO_FAILED:
                        InternalCurrentText = @"SMC Info Failed";
                        InternalUpdateProgress();
                        MessageBox.Show(
                            @"The system was unable to download the necessary information needed for the update. Please try again later.",
                            @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Environment.Exit(9);
                        break;
                    case JEMDownloaderStatus.INFO_LOADED:
                        InternalCurrentText = @"SMC Info Loaded";
                        InternalCurrentValue = 30;
                        InternalGlobalText= @"Starting SMC";
                        InternalGlobalValue = 30;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.DISABLED_BY_INFO:
                        InternalCurrentText = @"SMC Disabled by server.";
                        InternalUpdateProgress();
                        MessageBox.Show(
                            @"Sorry but our servers are currently disabled. Most likely, this is due to maintenance work or an update process. Please try again later.",
                            @"Oops");
                        Environment.Exit(9);
                        break;
                    case JEMDownloaderStatus.INITIALIZED:
                        InternalCurrentText = @"SMC Initialized";
                        InternalCurrentValue = 40;
                        InternalGlobalText= @"Starting SMC";
                        InternalGlobalValue = 40;
                        InternalUpdateProgress();
                        if (!SMCDownloader.MakeUpdate(false))
                        {
                            MessageBox.Show(
                                @"System was unable to run update process. Internal downloader error.",
                                @"Oops");
                            Environment.Exit(9);
                        }

                        break;
                    case JEMDownloaderStatus.INITIALIZING_PACKAGES:
                        InternalCurrentText = @"Getting server packages";
                        InternalCurrentValue = 55;
                        InternalGlobalText= @"Checking Files";
                        InternalGlobalValue = 55;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INITIALIZING_SERVERCHECKSUM:
                        InternalCurrentText = @"Getting server files";
                        InternalCurrentValue = 50;
                        InternalGlobalText= @"Checking Files";
                        InternalGlobalValue = 50;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INITIALIZING_CLIENTCHECKSUM:
                        InternalCurrentText = @"Getting local files";
                        InternalCurrentValue = 60;
                        InternalGlobalText= @"Checking Files";
                        InternalGlobalValue = 50;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INITIALIZING_LIST:
                        InternalCurrentText = @"Getting target files";
                        InternalCurrentValue = 70;
                        InternalGlobalText= @"Checking Files";
                        InternalGlobalValue = 50;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.DOWNLOADING:
                        InternalCurrentText = @"Downloading target files";
                        InternalCurrentValue = 80;
                        InternalGlobalText= @"Updating";
                        InternalGlobalValue = 60;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INSTALLING:
                        InternalCurrentText = @"Installing target files";
                        InternalCurrentValue = 90;
                        InternalGlobalText= @"Updating";
                        InternalGlobalValue = 70;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.INSTALLED:
                        InternalCurrentText = @"New files successfully installed";
                        InternalCurrentValue = 95;
                        InternalGlobalText= @"Updating";
                        InternalGlobalValue = 80;
                        InternalUpdateProgress();
                        break;
                    case JEMDownloaderStatus.READY:
                        RestartComponents();

                        InternalCurrentText = @"SMC Ready";
                        InternalCurrentValue = 100;
                        InternalGlobalValue = 100;
                        InternalUpdateProgress();

                        UpdateButtons(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(JEMDownloaderStatus), JEMDownloaderStatus, null);
                }
            });
        }

        private void SMCDownloader_ShutdownRequest(bool isError, string reason)
        {
            Invoke((MethodInvoker) delegate
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
            Invoke((MethodInvoker) delegate
            {
                switch (type)
                {
                    case JEMDownloaderProgressType.ACTION:
                        InternalCurrentText = text;
                        InternalCurrentValue = value;
                        break;
                    case JEMDownloaderProgressType.GLOBAL:
                        InternalGlobalText= text;
                        InternalGlobalValue = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                InternalUpdateProgress();
            });
        }

        private void SMCSelfUpdater_StatusChanged(JEMSelfUpdaterStatus JEMSelfUpdaterStatus)
        {
            JEMLogger.Log($"SMCSelfUpdater status changed: {JEMSelfUpdaterStatus}");
            Invoke((MethodInvoker) delegate
            {
                switch (JEMSelfUpdaterStatus)
                {
                    case JEMSelfUpdaterStatus.STARTING:
                        InternalCurrentText = @"Starting SMC (SU)";
                        InternalCurrentValue = 00;
                        InternalGlobalText= @"Starting SMC (SU)";
                        InternalGlobalValue = 00;
                        InternalUpdateProgress();
                        break;
                    case JEMSelfUpdaterStatus.PREPARING_SPACE:
                        InternalCurrentText = @"Starting SMC (SU)";
                        InternalCurrentValue = 05;
                        InternalGlobalText= @"Starting SMC (SU)";
                        InternalGlobalValue = 05;
                        InternalUpdateProgress();
                        break;
                    case JEMSelfUpdaterStatus.INFO_FAILED:
                        InternalCurrentText = @"SMC (SU) Info Failed";
                        InternalUpdateProgress();
                        MessageBox.Show(
                            @"The system was unable to download the necessary information needed for the update. Please try again later.",
                            @"Error (SU)", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Environment.Exit(9);
                        break;
                    case JEMSelfUpdaterStatus.INFO_LOADED:
                        InternalCurrentText = @"SMC (SU) Info Loaded";
                        InternalUpdateProgress();
                        break;
                    case JEMSelfUpdaterStatus.DISABLED_BY_INFO:
                        InternalCurrentText = @"SMC (SU) Disabled by server.";
                        InternalUpdateProgress();
                        MessageBox.Show(
                            @"Sorry but our servers are currently disabled. Most likely, this is due to maintenance work or an update process. Please try again later.",
                            @"Oops (SU)");
                        Environment.Exit(9);
                        break;
                    case JEMSelfUpdaterStatus.INITIALIZED:
                        InternalCurrentText = @"SMC (SU) Initialized";
                        InternalCurrentValue = 30;
                        InternalGlobalText= @"Starting SMC";
                        InternalGlobalValue = 10;
                        InternalUpdateProgress();
                        break;
                    case JEMSelfUpdaterStatus.DOWNLOADING:
                        break;
                    case JEMSelfUpdaterStatus.INSTALLING:
                        break;
                    case JEMSelfUpdaterStatus.INSTALLED:
                        break;
                    case JEMSelfUpdaterStatus.READY:
                        RestartComponents();

                        InternalGlobalText= @"SMC (SU) Ready";
                        InternalGlobalValue = 100;
                        InternalUpdateProgress();

                        // when selfupdater is done, initialize game downloader
                        SMCDownloader.Initialize();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(JEMSelfUpdaterStatus), JEMSelfUpdaterStatus, null);
                }
            });
        }

        private void SMCSelfUpdater_ShutdownRequest(bool isError, string reason)
        {
            Invoke((MethodInvoker) delegate
            {
                if (isError)
                    JEMLogger.LogError(
                        $"Shutting down by selfupdater. Reason: {reason}");
                else
                    JEMLogger.LogWarning(
                        $"Shutting down by selfupdater. Reason: {reason}");

                if (isError)
                {
                    MessageBox.Show(reason, @"Oops (SU)", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                Environment.Exit(9);
            });
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_continueQuitting)
            {
                e.Cancel = true;

                // show shutdown question
                Javascript.Run("showQuitQuestionMessage();");
            }
        }

        /// <summary>
        /// Initialize JEM Updater.
        /// </summary>
        public static void InitializeJEMUpdater()
        {
            Instance.RestartComponents();
            JEMSelfUpdater.Initialize(Application.ProductVersion, "http://nggamesrg.nazwa.pl/smcup_self/", "self.json");
        }

        private static bool _continueQuitting;

        /// <summary>
        /// Quit application.
        /// </summary>
        public static void Quit()
        {
            // don't show shutdown question message
            _continueQuitting = true;

            // show shutdown notify
            Javascript.Run("showShutdownMessage();");

            // save configuration
            SMCConfiguration.Save();

            // shutdown application from thread
            var wait = new Thread(() =>
            {
                // shutdown working downloader
                Instance.SMCDownloader.ShutDownDownloader();

                // wait
                Thread.Sleep(800);

                // shutdown application
                Instance.Invoke((MethodInvoker) delegate
                {
                    Instance.Hide();
                    Browser.Dispose();
                    Cef.Shutdown();
                    Process.GetCurrentProcess().Kill();
                });
            });
            wait.Start();
        }

        private static string InternalCurrentText { get; set; }
        private static int InternalCurrentValue { get; set; }
        private static string InternalGlobalText { get; set; }
        private static int InternalGlobalValue { get; set; }

        /// <summary>
        /// Updates current progress.
        /// </summary>
        public static void UpdateCurrentProgress(string text, int value)
        {
            InternalCurrentText = text;
            InternalCurrentValue = value;
            InternalUpdateProgress();
        }

        /// <summary>
        /// Updates global progress.
        /// </summary>
        public static void UpdateGlobalProgress(string text, int value)
        {
            InternalGlobalText = text;
            InternalGlobalValue = value;
            InternalUpdateProgress();
        }

        private static string _lastInternalCurrentText { get; set; }
        private static int _lastInternalCurrentValue { get; set; }
        private static string _lastInternalGlobalText { get; set; }
        private static int _lastInternalGlobalValue { get; set; }

        private static void InternalUpdateProgress()
        {
            if (_lastInternalCurrentText == InternalCurrentText && _lastInternalCurrentValue == InternalCurrentValue &&
                _lastInternalGlobalText == InternalGlobalText && _lastInternalGlobalValue == InternalGlobalValue)
            {
                return;
            }

            _lastInternalCurrentText = InternalCurrentText;
            _lastInternalCurrentValue = InternalCurrentValue;
            _lastInternalGlobalText = InternalGlobalText;
            _lastInternalGlobalValue = InternalGlobalValue;

            Javascript.Run(
                $"updateProgress('{InternalCurrentText}', {InternalCurrentValue}, '{InternalGlobalText}', {InternalGlobalValue});");
        }

        public static void UpdateButtons(bool activeState)
        {
            Javascript.Run($"setActiveButtons('{activeState}', '{activeState}');");
        }

        /// <summary>
        /// Dispose all displayed modals.
        /// </summary>
        public static void DisposeAllModals()
        {
            Javascript.Run("disposeAllModals();");
        }

        /// <summary>
        /// Draws processing message on window.
        /// </summary>
        /// <param name="title">Title of window.</param>
        /// <param name="message">Message of window.</param>
        public static void DrawProcessingMessage(string title, string message)
        {
            ResetProcessingMessage();
            Javascript.Run(
                $"setActiveProcessingWindow({(string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message) ? "false" : "true")}," +
                $" '{title}', '{message}');");
        }

        /// <summary>
        /// Restarts processing message. (hide)
        /// </summary>
        public static void ResetProcessingMessage()
        {
            Javascript.Run("setActiveProcessingWindow(false, \'\', \'\');");
        }

        /// <summary>
        /// Draws fatal message with quit button.
        /// </summary>
        /// <param name="title">Title of window.</param>
        /// <param name="message">Message of window.</param>
        public static void DrawFatalMessage(string title, string message)
        {
            Javascript.Run($"setFatalWindow('{title}', '{message}');");
        }

        /// <summary>
        /// SMC Downloader instance.
        /// </summary>
        public JEMDownloader SMCDownloader { get; } = new JEMDownloader(Environment.CurrentDirectory,
            "http://nggamesrg.nazwa.pl/smcup/", "info.json", new string [0]);

        /// <summary>
        /// MainWindow instance.
        /// </summary>
        public static MainWindow Instance { get; private set; }

        /// <summary>
        /// Instance of chromium web browser.
        /// </summary>
        public static ChromiumWebBrowser Browser { get; private set; }

    }
}
