//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;

namespace SMC.Launcher.Scripts
{
    /// <summary>
    /// Javascript execution helper class.
    /// </summary>
    public class Javascript
    {
        /// <summary>
        /// Runs javascript in the main frame.
        /// </summary>
        /// <param name="js">The javascript code.</param>
        public static void Run(string js)
        {
            MainWindow.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
        }

        /// <summary>
        /// Runs javascript in the main frame with C# callback.
        /// </summary>
        /// <param name="js">The javascript code.</param>
        /// <param name="callback">The c# callback when execution ends and it's successful.</param>
        public static void Run(string js, Action<object> callback)
        {
            MainWindow.Browser.GetBrowser().MainFrame.EvaluateScriptAsync(js, null).ContinueWith(task =>
            {
                var result = task.Result;
                if (result.Success)
                    callback(result.Result);
            });
        }
    }
}
