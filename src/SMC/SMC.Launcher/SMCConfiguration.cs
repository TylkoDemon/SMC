//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.IO;
using JEM.Core;
using Newtonsoft.Json;

namespace SMC.Launcher
{
    [Serializable]
    public class SMCConfiguration
    {
        public string UserName = "Ramaja";
        public int UserRAM = 3584;

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public static void Load()
        {
            if (File.Exists(ConfigurationPath))
            {
                try
                {
                    Loaded = JsonConvert.DeserializeObject<SMCConfiguration>(
                        JEMFile.LoadTextFromFile(ConfigurationPath));
                    return;
                }
                catch (Exception e)
                {
                    JEMLogger.LogException(e, "Failed to load SMCConfiguration file.");
                }
            }

            Loaded = new SMCConfiguration();
            Save();
        }

        /// <summary>
        /// Saves loaded configuration.
        /// </summary>
        public static void Save()
        {
            if (Loaded == null)
                throw new InvalidOperationException($"Loaded configuration is null.");

            var directory = Path.GetDirectoryName(ConfigurationPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory ?? throw new InvalidOperationException());

            JEMFile.SaveTextToFile(ConfigurationPath, JsonConvert.SerializeObject(Loaded));
        }

        /// <summary>
        /// Path to configuration file.
        /// </summary>
        public static string ConfigurationPath => Environment.CurrentDirectory + @"\config.json";

        /// <summary>
        /// Loaded instance of configuration.
        /// </summary>
        public static SMCConfiguration Loaded { get; private set; }
    }
}
