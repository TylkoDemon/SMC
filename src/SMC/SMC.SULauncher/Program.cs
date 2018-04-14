//
// Super Minecraft Launcher(SU) Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.IO;
using System.Windows.Forms;
using JEM.Core;

namespace SMC.SULauncher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // clear logger
            JEMLogger.ClearLoggerDirectory();
            JEMLogger.Log("Hello, SMC!");

            // check arguments
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
            {
                JEMLogger.LogError("Unable to run application. Invalid arguments (0).");
                Environment.Exit(0);
            }

            if (!File.Exists(args[1]))
            {
                JEMLogger.LogError("Unable to run application. Invalid arguments (1).");
                Environment.Exit(0);
            }

            // run window
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
