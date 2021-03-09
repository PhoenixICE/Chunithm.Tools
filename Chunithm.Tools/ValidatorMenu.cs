using Chunithm.Tools.Interface;
using Nim.Console;
using System.Collections.Generic;
using System.Linq;

namespace Chunithm.Tools
{
    public static class ValidatorMenu
    {
        public static void DisplayValidatorMenu(Dictionary<string, IFolderValidator> validators)
        {
            if (InteractiveConsole.Menu.Selected.Items.Count > 0)
            {
                return;
            }

            foreach (var option in validators)
            {
                var sub = new ConsoleMenu.Item(option.Key, () => RunValidator(option.Value), 2) { IsToggle = true };
                InteractiveConsole.Menu.Selected.Add(sub);
            }
        }

        private static void RunValidator(IFolderValidator validator)
        {
            InteractiveConsole.Menu.Log.Clear();
            InteractiveConsole.Menu.Log.AddRange(validator.ValidateFolder(InteractiveConsole.PreProcessing.Options.Values.SelectMany(x => x.OptionSubFolders.SelectMany(y => y.Value))));
        }
    }
}
