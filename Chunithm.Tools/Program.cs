using System;
using System.Threading.Tasks;

namespace Chunithm.Tools
{
    class Program
    {
        public static Version Version = new Version(0, 1, 7, 0);
        public static string Header = $@"██╗ ██████╗██╗   ██╗██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
██║██╔════╝╚██╗ ██╔╝██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
██║██║      ╚████╔╝ ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
██║██║       ╚██╔╝  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
██║╚██████╗   ██║   ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
╚═╝ ╚═════╝   ╚═╝   ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
Chunithm.Tools v{Version} (Paradise Only)";

        static async Task Main(string[] args)
        {
            Console.WriteLine(Header);
            try
            {
                var settings = Settings.LoadOrSaveSettings();
                var preProcessing = new PreProcessing(settings);
                await preProcessing.Run();
                InteractiveConsole.GenerateMenu(preProcessing);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                LogInfo("Press Any Key to Quit..");
                Console.ReadKey();
            }
        }

        public static void LogWarning(string str, bool newLine = true, params object[] args)
            => WriteToLog(str, ConsoleColor.Black, ConsoleColor.Yellow, newLine, args);

        public static void LogError(string str, bool newLine = true, params object[] args)
            => WriteToLog(str, ConsoleColor.Red, ConsoleColor.Black, newLine, args);

        public static void LogInfo(string str, bool newLine = true, params object[] args)
            => WriteToLog(str, ConsoleColor.Blue, ConsoleColor.Black, newLine, args);

        public static void Log(string str, bool newLine = true, params object[] args)
            => WriteToLog(str, null, null, newLine, args);

        private static void WriteToLog(string str, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor, bool newLine, params object[] args)
        {
            if (foregroundColor != null)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }

            if (backgroundColor != null)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }
            if (newLine)
            {
                Console.WriteLine(str, args);
            }
            else
            {
                Console.Write(str, args);
            }
            Console.ResetColor();
        }

        public static void LogBreak()
            => WriteToLog("======================================", null, null, true);
    }
}
