using Nim.Console;
using System.Collections.Generic;
using System.Linq;

namespace Chunithm.Tools
{
    public static class UnlockerMenu
    {
        public static void DisplayUnlockerMenu()
        {
            if (InteractiveConsole.Menu.Selected.Items.Count > 0)
            {
                return;
            }

            InteractiveConsole.Menu.Selected.Add(new ConsoleMenu.Item("Unlock Everything", UnlockEverything) { ActionIfConfirmed = true });
            InteractiveConsole.Menu.Selected.Add(new ConsoleMenu.Item("Restore Everything", RestoreEverything) { ActionIfConfirmed = true });

            foreach (var option in InteractiveConsole.PreProcessing.Options.Where(x => x.Value.OptionSubFolders.Any(y => y.Value.Any(z => z.XMLTags.Any()))))
            {
                var sub = new ConsoleMenu.Item(option.Key, () => DisplayUnlockerSubMenu(option.Value), 2);
                InteractiveConsole.Menu.Selected.Add(sub);
            }

            InteractiveConsole.Menu.WriteLine("Press Delete Scroll Backup.");
        }

        private static void DisplayUnlockerSubMenu(Option option)
        {
            InteractiveConsole.Menu.Selected.Clear();

            var foldersWithUnlocks = option.OptionSubFolders.Where(x => x.Value.Any(y => y.XMLTags.Any())).ToList();
            var flattenedSubFolders = foldersWithUnlocks.SelectMany(x => x.Value).ToList();
            InteractiveConsole.Menu.Selected.Add(new ConsoleMenu.Item("Unlock All", () => Unlock(option.DirectoryInfo.FullName, flattenedSubFolders)) { ActionIfConfirmed = true });
            InteractiveConsole.Menu.Selected.Add(new ConsoleMenu.Item("Restore All", () => Restore(option.DirectoryInfo.FullName, flattenedSubFolders)) { ActionIfConfirmed = true });

            foreach (var optionSubFolder in foldersWithUnlocks)
            {
                var sub = new ConsoleMenu.Item(optionSubFolder.Key.ToString(), () => DisplayUnlockerRestoreOrUnlock(option.DirectoryInfo.FullName, optionSubFolder.Value));
                InteractiveConsole.Menu.Selected.Add(sub);
            }
        }

        private static void DisplayUnlockerRestoreOrUnlock(string optionFolder, List<OptionSubFolder> optionSubFolder)
        {
            if (InteractiveConsole.Menu.Selected.Items.Count > 0)
            {
                return;
            }

            InteractiveConsole.Menu.Selected.Add("Unlock", () => Unlock(optionFolder, optionSubFolder));
            InteractiveConsole.Menu.Selected.Add("Restore", () => Restore(optionFolder, optionSubFolder));
        }

        private static void RestoreEverything()
        {
            foreach (var option in InteractiveConsole.PreProcessing.Options)
            {
                foreach (var optionSubFolders in option.Value.OptionSubFolders)
                {
                    Restore(option.Value.DirectoryInfo.FullName, optionSubFolders.Value);
                }
            }

        }

        private static void Restore(string optionFolder, List<OptionSubFolder> optionSubFolders)
        {
            foreach (var optionSubFolder in optionSubFolders)
            {
                Restore(optionSubFolder);
            }
            InteractiveConsole.Menu.WriteLine($"{optionFolder} Restore Completed.");
        }

        private static void Restore(OptionSubFolder optionSubFolder)
        {
            if (InteractiveConsole.PreProcessing.Restore(optionSubFolder, out var log))
            {
                Program.Log(log);
            }
        }

        private static void UnlockEverything()
        {
            foreach (var option in InteractiveConsole.PreProcessing.Options)
            {
                foreach (var optionSubFolders in option.Value.OptionSubFolders)
                {
                    Unlock(option.Value.DirectoryInfo.FullName, optionSubFolders.Value);
                }
            }
        }

        private static void Unlock(string optionFolder, List<OptionSubFolder> optionSubFolders)
        {
            foreach (var optionSubFolder in optionSubFolders)
            {
                Unlock(optionSubFolder);
            }

            InteractiveConsole.Menu.WriteLine($"{optionFolder} Unlock Completed.");
        }

        private static void Unlock(OptionSubFolder optionSubFolder)
        {
            if (InteractiveConsole.PreProcessing.Unlock(optionSubFolder, out var log))
            {
                Program.Log(log);
            }
        }
    }
}
