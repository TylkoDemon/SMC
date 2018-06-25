//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core;
using System;
using System.Windows.Forms;

namespace SMC.Launcher
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

            if (Environment.CurrentDirectory.Contains(" "))
            {
                JEMLogger.LogError("SMC.Launcher can't work with directory that contains spaces. Please move SMC.Launcher to directory like D:/Games/SMC.");
                MessageBox.Show(
                    @"SMC.Launcher can't work with directory that contains spaces. Please move SMC.Launcher to directory like D:/Games/SMC.",
                    @"Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            // run window
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartupWindow());
        }
    }
}
