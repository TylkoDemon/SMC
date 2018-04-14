//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Threading;
using System.Windows.Forms;

namespace SMC.Launcher
{
    public partial class StartupWindow : Form
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void StartupWindow_Load(object sender, EventArgs e)
        {
            var random = new Random().Next(0, 4);
            if (random == 1)
            {
                BackgroundImage = Properties.Resources.BANER_2;
            }
            else if (random == 2)
            {
                BackgroundImage = Properties.Resources.BANER_3;
            }
            else if (random == 3)
            {
                BackgroundImage = Properties.Resources.BANER_4;
            }
            else if (random == 4)
            {
                BackgroundImage = Properties.Resources.BANER_5;
            }
            else
            {
                BackgroundImage = Properties.Resources.BANER_1;
            }
        }

        private void StartupWindow_Shown(object sender, EventArgs e)
        {
            var waitForMainWindow = new Thread(() => { Invoke((MethodInvoker) delegate
            {
                SMCConfiguration.Load();
                Thread.Sleep(1000);
                Hide();
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }); });
            waitForMainWindow.Start();
        }
    }
}
