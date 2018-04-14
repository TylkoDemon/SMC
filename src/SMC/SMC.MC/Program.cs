//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Diagnostics;
using JEM.Core;

namespace SMC.MC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SMC.MC has been started with invalid arguments. (need 2)");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (Environment.CurrentDirectory.Contains(" "))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SMC.MC can't work with directory that contains spaces. Please move SMC.MC to directory like D:/Games/SMC.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            const string binDir = @"bin";

            var memory_max = int.Parse(args[1]);
            var memory_min = (int)(0.7 * memory_max);

            var username = args[0];

            var process_args_base =
                "-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump" +
                " -XX:-OmitStackTraceInFastThrow" +
                " -Xms" + memory_min + "M" +
                " -Xmx" + memory_max + "M" +
                " -XX:MetaspaceSize=128M" +
                " -Duser.language=en" +
                " -Duser.country=US" +
                " -Dfml.ignorePatchDiscrepancies=true" +
                " -Dfml.ignoreInvalidMinecraftCertificates=true" +
                " -Dfml.log.level=INFO" +
                " -XX:+UseConcMarkSweepGC" +
                " -XX:+CMSIncrementalMode" +
                " -XX:-UseAdaptiveSizePolicy";

            var process_args_natives =
                @"-Djava.library.path=" + binDir + @"\natives";

            var process_args_jar =
                @"-cp " + binDir + @"\forge-1.12.2-14.23.2.2618-universal.jar;" +
                binDir + @"\launchwrapper-1.12.jar;" +
                binDir + @"\asm-all-5.2.jar;" +
                binDir + @"\jline-2.13.jar;" +
                binDir + @"\akka-actor_2.11-2.3.3.jar;" +
                binDir + @"\config-1.2.1.jar;" +
                binDir + @"\scala-actors-migration_2.11-1.1.0.jar;" +
                binDir + @"\scala-compiler-2.11.1.jar;" +
                binDir + @"\scala-continuations-library_2.11-1.0.2.jar;" +
                binDir + @"\scala-continuations-plugin_2.11.1-1.0.2.jar;" +
                binDir + @"\scala-library-2.11.1.jar;" +
                binDir + @"\scala-parser-combinators_2.11-1.0.1.jar;" +
                binDir + @"\scala-reflect-2.11.1.jar;" +
                binDir + @"\scala-swing_2.11-1.0.1.jar;" +
                binDir + @"\scala-xml_2.11-1.0.2.jar;" +
                binDir + @"\lzma-0.0.1.jar;" +
                binDir + @"\jopt-simple-5.0.3.jar;" +
                binDir + @"\vecmath-1.5.2.jar;" +
                binDir + @"\trove4j-3.0.3.jar;" +
                binDir + @"\patchy-1.1.jar;" +
                binDir + @"\oshi-core-1.1.jar;" +
                binDir + @"\jna-4.4.0.jar;" +
                binDir + @"\platform-3.4.0.jar;" +
                binDir + @"\icu4j-core-mojang-51.2.jar;" +
                binDir + @"\codecjorbis-20101023.jar;" +
                binDir + @"\codecwav-20101023.jar;" +
                binDir + @"\libraryjavasound-20101123.jar;" +
                binDir + @"\librarylwjglopenal-20100824.jar;" +
                binDir + @"\soundsystem-20120107.jar;" +
                binDir + @"\netty-all-4.1.9.Final.jar;" +
                binDir + @"\guava-21.0.jar;" +
                binDir + @"\commons-lang3-3.5.jar;" +
                binDir + @"\commons-io-2.5.jar;" +
                binDir + @"\commons-codec-1.10.jar;" +
                binDir + @"\jinput-2.0.5.jar;" +
                binDir + @"\jutils-1.0.0.jar;" +
                binDir + @"\gson-2.8.0.jar;" +
                binDir + @"\authlib-1.5.25.jar;" +
                binDir + @"\realms-1.10.19.jar;" +
                binDir + @"\commons-compress-1.8.1.jar;" +
                binDir + @"\httpclient-4.3.3.jar;" +
                binDir + @"\commons-logging-1.1.3.jar;" +
                binDir + @"\httpcore-4.3.2.jar;" +
                binDir + @"\fastutil-7.1.0.jar;" +
                binDir + @"\log4j-api-2.8.1.jar;" +
                binDir + @"\log4j-core-2.8.1.jar;" +
                binDir + @"\lwjgl-2.9.4-nightly-20150209.jar;" +
                binDir + @"\lwjgl_util-2.9.4-nightly-20150209.jar;" +
                binDir + @"\text2speech-1.10.3.jar;" +
                binDir + @"\minecraft.jar";

            var process_args_start = @"net.minecraft.launchwrapper.Launch" + //net.minecraft.launchwrapper.Launch
                                     @" --username " + username + "" +
                                     @" --version 1.12" +
                                     $@" --gameDir {Environment.CurrentDirectory}" +
                                     $@" --assetsDir {Environment.CurrentDirectory}\resources" +
                                     @" --assetIndex 1.12" +
                                     @" --uuid " + JEMMD5.Hash($"uuid_{username}") + "" +
                                     @" --accessToken " + JEMMD5.Hash($"accessToken_{username}") + "" +
                                     @" --userType mojang" +
                                     @" --versionType release" +
                                     @" --width=1280" +
                                     @" --height=720" +
                                     @" --tweakClass=net.minecraftforge.fml.common.launcher.FMLTweaker" +
                                     @" --versionType Forge";

            var process_args = string.Join(" ", process_args_base, process_args_natives, process_args_jar,
                process_args_start);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo($"{Java.JavaInstallationPath}\\bin\\javaw.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    Arguments = process_args
                }
            };

            Console.WriteLine(
                $"Starting {process.StartInfo.FileName} at {process.StartInfo.WorkingDirectory} with arguments {process.StartInfo.Arguments}");
            process.Start();
            Console.WriteLine("Minecraft is starting, please wait!");
            Console.WriteLine($"Running as {username}");
            Console.WriteLine("(You can close this console, minecraft will start anyway)");
        }
    }
}
