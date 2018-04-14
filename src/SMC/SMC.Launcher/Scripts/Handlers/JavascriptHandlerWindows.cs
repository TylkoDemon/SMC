//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Drawing;
using System.Windows.Forms;

namespace SMC.Launcher.Scripts.Handlers
{
    internal class JavascriptHandlerWindows
    {
        private bool _drag;
        private bool _dragrun;
        private bool _dragisresize;
        private Point _dragStart;
        private Point _dragStartLocation;
        private Size _dragStartSize;

        /// <summary>
        /// JS Callback
        /// </summary>
        public void mouseMove()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                if (!_drag)
                {
                    var point = new Point(Cursor.Position.X - MainWindow.Instance.Location.X,
                        Cursor.Position.Y - MainWindow.Instance.Location.Y);
                    var resizePoint = new Point(point.X - MainWindow.Instance.Size.Width,
                        point.Y - MainWindow.Instance.Size.Height);
                    if (resizePoint.X > -20 && resizePoint.Y > -20)
                    {
                        //_dragisresize = true;
                    }
                    else if (point.Y > 55)
                        return;
                    else
                        _dragisresize = false;
                }

                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (MainWindow.Instance.WindowState == FormWindowState.Maximized)
                    {
                        MainWindow.Instance.WindowState = FormWindowState.Normal;
                        MainWindow.Instance.Location = new Point(Cursor.Position.X - MainWindow.Instance.Size.Width / 2,
                            Cursor.Position.Y - 15);
                    }

                    if (_drag)
                    {
                        if (!_dragrun)
                        {

                        }

                        _dragrun = true;

                        var cursorPosition = Cursor.Position;
                        var delta = new Point(cursorPosition.X - _dragStart.X, cursorPosition.Y - _dragStart.Y);

                        if (_dragisresize)
                        {
                            if (_dragStartSize.Width + delta.X < MainWindow.Instance.MinimumSize.Width ||
                                _dragStartSize.Height + delta.Y < MainWindow.Instance.MinimumSize.Height)
                            {
                                MainWindow.Instance.Size = new Size(MainWindow.Instance.MinimumSize.Width,
                                    MainWindow.Instance.MinimumSize.Height);
                            }
                            else
                            {
                                MainWindow.Instance.Size = new Size(_dragStartSize.Width + delta.X,
                                    _dragStartSize.Height + delta.Y);
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.SizeAll;

                            MainWindow.Instance.Location = new Point(_dragStartLocation.X + delta.X,
                                _dragStartLocation.Y + delta.Y);
                        }
                    }
                    else
                    {
                        _dragStartLocation = MainWindow.Instance.Location;
                        _dragStartSize = MainWindow.Instance.Size;
                        _dragStart = Cursor.Position;
                        _drag = true;
                    }
                }
                else
                {
                    _drag = false;
                    _dragrun = false;

                    if (_dragisresize)
                        Cursor.Current = Cursors.SizeNWSE;
                    else
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            });
        }

        public void onQuitRequest()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                // show shutdown question
                Javascript.Run("showQuitQuestionMessage();");
            });
        }

        public void onMinimaize()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                MainWindow.Instance.WindowState = FormWindowState.Minimized;
            });
        }

        public void onMaximize()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                MainWindow.Instance.WindowState = MainWindow.Instance.WindowState != FormWindowState.Maximized
                    ? FormWindowState.Maximized
                    : FormWindowState.Normal;
            });
        }

        public void onOptions()
        {
            MainWindow.Instance.Invoke((MethodInvoker) delegate
            {
                // show options window
                Javascript.Run("showOptionsWindow();");
            });
        }

        public void onQuit()
        {
            MainWindow.Instance.Invoke((MethodInvoker) MainWindow.Quit);
        }
    }
}
