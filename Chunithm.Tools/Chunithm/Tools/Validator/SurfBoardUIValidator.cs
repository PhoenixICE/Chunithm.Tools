using Chunithm.Tools.Interface;
using Chunithm.Tools.Validator.XMLClasses;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Chunithm.Tools.Chunithm.Tools.Validator
{
    public class SurfBoardUIValidator : IFolderValidator
    {
        public IEnumerable<string> ValidateFolder(IEnumerable<OptionSubFolder> optionSubFolders)
        {
            var log = new List<string>();
            Program.LogInfo("Checking for Surf Board UI...");

            var serializer = new XmlSerializer(typeof(EventData));
            foreach (var eventData in optionSubFolders.Where(x => x.OptionType == OptionSubFolderType.@event))
            {
                var eventFile = System.IO.File.ReadAllText(eventData.XMLFile.FullName);
                using (var reader = new StringReader(eventFile))
                {
                    try
                    {
                        var @event = (EventData)serializer.Deserialize(reader);
                        var surfBoardUI = @event.Substances?.ChangeSurfBoardUI?.ResourceVersion?.Str;
                        if (!string.IsNullOrWhiteSpace(surfBoardUI))
                        {
                            log.Add($"Found: {surfBoardUI} File: {eventData.XMLFile.FullName}");
                        }
                    }
                    catch
                    {
                        log.Add($"Cannot phrase XML, {eventData.XMLFile.FullName} is potentially corrupted?");
                        continue;
                    }
                }
            }
            log.Add("Completed checking for Surf Board UI.");
            return log;
        }
    }
}
