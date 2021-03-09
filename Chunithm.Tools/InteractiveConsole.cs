using Nim.Console;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chunithm.Tools
{
    public static class InteractiveConsole
    {
        public static ConsoleMenu Menu { get; set; }
        public static PreProcessing PreProcessing { get; set; }

        public static void GenerateMenu(PreProcessing preProcessing)
        {
            PreProcessing = preProcessing;

            Menu = new ConsoleMenu
            (
                Program.Header,
                new[]
                {
                    new ConsoleMenu.Item("Patch ChuniApp.exe", DisplayPatchesMenu, 1),
                    new ConsoleMenu.Item("XML Unlocker", UnlockerMenu.DisplayUnlockerMenu, 2),
                    new ConsoleMenu.Item("Option Folders Details", DisplayOptionFolderDetailsMenu, 2),
                    new ConsoleMenu.Item("Run Validators", DisplayValidators, 1),
                    new ConsoleMenu.Item("Export FileList", ExportFileList) { IsToggle = true },
                    new ConsoleMenu.Item("Compare FileList", CompareFileList) { IsToggle = true },
                    new ConsoleMenu.Item("Local IP", GetLocalIPAddress) { IsToggle = true }, 
                    new ConsoleMenu.Item("Exit", MenuExit),
                }
            );

            Menu.Main.MaxColumns = 1;
            Menu.WriteLine("Use ←↑↓→ for navigation.");
            Menu.WriteLine("Press Esc for main menu.");
            Menu.WriteLine("Press Backspace for parent menu.");
            Menu.WriteLine("Press Del for clear log.");

            Menu.Begin();
        }

        private static void GetLocalIPAddress()
        {
            List<string> ips = new List<string>();

            System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            foreach (System.Net.IPAddress ip in entry.AddressList)
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ips.Add(ip.ToString());

            Menu.Log.AddRange(ips);
        }

        private static void CompareFileList()
        {
            if (!File.Exists("masterlist.json"))
            {
                Menu.WriteLine("masterlist.json seems to be missing?");
                return;
            }

            Menu.Log.Clear();

            var json = File.ReadAllText("masterlist.json");
            var compareFile = JsonSerializer.Deserialize<List<string>>(json);
            var files = PreProcessing.Options.SelectMany(x => x.Value.OptionSubFolders.SelectMany(y => y.Value).Select(z => z.DirectoryInfo.Name)).ToList();

            var missingFiles = compareFile.Except(files);
            var extraFiles = files.Except(compareFile);
            var total = compareFile.Intersect(files);

            foreach (var missingFile in missingFiles)
            {
                Menu.WriteLine($"Missing {missingFile}");
            }

            Menu.WriteLine($"Total Missing: {missingFiles.Count()}");
            Menu.WriteLine($"Total Extra: {extraFiles.Count()}");
            Menu.WriteLine($"Total: {total.Count()}");
            Menu.WriteLine("Compare Completed!");
        }

        private static void ExportFileList()
        {
            Menu.WriteLine("Exporting list of Files...");
            var files = PreProcessing.Options.SelectMany(x => x.Value.OptionSubFolders.SelectMany(y => y.Value).Select(z => z.DirectoryInfo.Name)).ToList();
            var json = JsonSerializer.Serialize(files, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
            File.WriteAllText("export.json", json);
            Menu.WriteLine("Exporting to export.json Completed.");
        }

        private static void DisplayValidators()
        {
            if (Menu.Selected.Items.Count > 0)
            {
                return;
            }

            foreach (var validator in PreProcessing.Settings.DefaultSubFolderValdiator)
            {
                Menu.Selected.Add(new ConsoleMenu.Item(validator.Key.ToString(), () => ValidatorMenu.DisplayValidatorMenu(validator.Value), 1));
            }
        }

        private static void MenuExit()
        {
            Menu.Close();
        }

        private static void DisplayPatchesMenu()
        {
            if (Menu.Selected.Items.Count > 0)
            {
                return;
            }

            foreach (var patch in PreProcessing.CurrentChuniAppPatches)
            {
                var sub = new ConsoleMenu.Item($"{patch.Key,-30}\t{patch.Value}")
                {
                    IsToggle = true
                };
                sub.Action = () =>
                {
                    PreProcessing.CurrentChuniAppPatches[patch.Key] = PreProcessing.FlipChuniAppPatch(patch.Key);
                    sub.Name = $"{patch.Key,-30}\t{PreProcessing.CurrentChuniAppPatches[patch.Key]}";
                    DisplayPatchesMenu();
                };
                Menu.Selected.Add(sub);
            }
        }

        private static void DisplayOptionFolderDetailsMenu()
        {
            if (Menu.Selected.Items.Count > 0)
            {
                return;
            }

            foreach (var option in PreProcessing.OptionsSummary)
            {
                var sub = new ConsoleMenu.Item(option.Key, () =>
                {
                    Menu.Log.Clear();
                    Menu.WriteLine(option.Value);
                });
                Menu.Selected.Add(sub);
            }
        }
    }
}
