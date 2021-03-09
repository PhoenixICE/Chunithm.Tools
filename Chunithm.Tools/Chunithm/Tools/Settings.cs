using Chunithm.Tools.Interface;
using Chunithm.Tools.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chunithm.Tools
{
    public class Settings
    {
        public Dictionary<FileType, ChunithmFile> DefaultFiles { get; set; }

        public Dictionary<FolderType, ChunithmFolder> DefaultFolders { get; set; }

        public Dictionary<string, OptionSubFolderType> DefaultOptionFolders { get; set; }

        public Dictionary<ChuniAppPatchType, List<ChuniAppPatch>> ChuniAppPatches { get; set; }

        public Dictionary<string, bool> RegexMatch { get; set; }

        [JsonIgnore]
        public Dictionary<OptionSubFolderType, Dictionary<string, IFolderValidator>> DefaultSubFolderValdiator = new Dictionary<OptionSubFolderType, Dictionary<string, IFolderValidator>>()
        {
            [OptionSubFolderType.music] = new Dictionary<string, IFolderValidator>()
            {
                ["Music&Cue Validator"] = new CueMusicFolderValidator(),
                ["MusicSort Validator"] = new MusicSortValidator(),
                ["MusicDuplicator Validator"] = new MusicDuplicatorValidator(),
            }
        };

        [JsonIgnore]
        private const string SettingFileName = "chunithm.tools.settings.json";

        public void SaveSettings()
        {
            RegexMatch = new Dictionary<string, bool>()
            {
                [@"(<alwaysOpen>)(false|true)(<\/alwaysOpen>)"] = true,
                [@"(<defaultHave>)(false|true)(<\/defaultHave>)"] = true,
                [@"(<firstLock>)(false|true)(<\/firstLock>)"] = false
            };

            DefaultFiles = new Dictionary<FileType, ChunithmFile>
            {
                [FileType.ChuniApp] = new ChunithmFile("chuniApp.exe", FileType.ChuniApp, FolderType.Bin, Array.Empty<string>()),
                [FileType.SegaToolsIni] = new ChunithmFile("segatools.ini", FileType.SegaToolsIni, FolderType.Bin, Array.Empty<string>())
            };

            DefaultFolders = new Dictionary<FolderType, ChunithmFolder>
            {
                [FolderType.App] = new ChunithmFolder("app", null, Array.Empty<string>(), false),
                [FolderType.Options] = new ChunithmFolder("option", null, new[] { @"bin\option", @"app\bin\option", @"app\option" }),
                [FolderType.Bin] = new ChunithmFolder("bin", FolderType.App, Array.Empty<string>()),
                [FolderType.Data] = new ChunithmFolder("data", FolderType.App, new[] { @"app\data" }),
                [FolderType.A000] = new ChunithmFolder("A000", FolderType.Data, Array.Empty<string>())
            };

            DefaultOptionFolders = new Dictionary<string, OptionSubFolderType>
            {
                ["chara"] = OptionSubFolderType.chara,
                ["event"] = OptionSubFolderType.@event,
                ["music"] = OptionSubFolderType.music,
                ["skill"] = OptionSubFolderType.skill,
                ["cueFile"] = OptionSubFolderType.cueFile,
                ["trophy"] = OptionSubFolderType.trophy,
                ["namePlate"] = OptionSubFolderType.namePlate,
                ["mapIcon"] = OptionSubFolderType.mapIcon,
                ["systemVoice"] = OptionSubFolderType.systemVoice
            };

            ChuniAppPatches = new Dictionary<ChuniAppPatchType, List<ChuniAppPatch>>
            {
                [ChuniAppPatchType.AllowLocalHost] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.AllowLocalHost, 0x9B5BF0, new byte[] { 0x55, 0x8B, 0xEC }, new byte[] { 0x31, 0xC0, 0xC3 }),
                    new ChuniAppPatch(ChuniAppPatchType.AllowLocalHost, 0x1743510, new byte[] { 0x31, 0x32, 0x37, 0x2F }, new byte[] { 0x30, 0x2F, 0x38, 0x00 }),
                },
                [ChuniAppPatchType.DisableShopCloseLockout] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.DisableShopCloseLockout, 0x9DD843, new byte[] { 0x74 }, new byte[] { 0xEB }),
                },
                [ChuniAppPatchType.ForceSharedAudioMode] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.ForceSharedAudioMode, 0xD1134A, new byte[] { 0x01 }, new byte[] { 0x00 }),
                },
                [ChuniAppPatchType.Force2ChannelAudioOutput] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.Force2ChannelAudioOutput, 0xD11421, new byte[] { 0x75, 0x3F }, new byte[] { 0x90, 0x90 }),
                },
                [ChuniAppPatchType.DisableSongSelectTimer] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.DisableSongSelectTimer, 0x784A42, new byte[] { 0x74 }, new byte[] { 0xEB }),
                },
                [ChuniAppPatchType.SetAllTimersTo999] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.SetAllTimersTo999, 0x62D790, new byte[] { 0x8B, 0x44, 0x24, 0x04, 0x69, 0xC0, 0xE8, 0x03, 0x00, 0x00 }, new byte[] { 0xB8, 0x58, 0x3E, 0x0F, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90 }),
                },
                [ChuniAppPatchType.PatchForHeadToHeadPlayer] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.PatchForHeadToHeadPlayer, 0x48C9B3, new byte[] { 0x01 }, new byte[] { 0x00 }),
                },
                [ChuniAppPatchType.AutoPlay] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.AutoPlay, 0x747369, new byte[] { 0x00 }, new byte[] { 0x01 }),
                },
                [ChuniAppPatchType.PatchForWindows7] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.PatchForWindows7, 0x1bdd428, new byte[] { 0x45, 0x78 }, new byte[] { 0x5F, 0x41 }),
                    new ChuniAppPatch(ChuniAppPatchType.PatchForWindows7, 0x1bdd430, new byte[] { 0x43, 0x61, 0x6E, 0x63, 0x65, 0x6C, 0x51, 0x75, 0x65, 0x72, 0x79 }, new byte[] { 0x51, 0x75, 0x65, 0x72, 0x79, 0x5F, 0x41, 0x00, 0x00, 0x00, 0x00 }),
                },
                [ChuniAppPatchType.IncreaseMaxCreditsTo254] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.IncreaseMaxCreditsTo254, 0xCB28D7, new byte[] { 0x8A, 0x5D, 0x14 }, new byte[] { 0xB3, 0xFE, 0x90 }),
                },
                [ChuniAppPatchType.FreePlay] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.FreePlay, 0xCB2CF5, new byte[] { 0x28 }, new byte[] { 0x08 }),
                },
                [ChuniAppPatchType.DummyLED] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.DummyLED, 0x24A5E7, new byte[] { 0x00 }, new byte[] { 0x01 }),
                },
                [ChuniAppPatchType.NoEncryption] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.NoEncryption, 0x3E2832, new byte[] { 0x39 }, new byte[] { 0xC3 }),
                },
                [ChuniAppPatchType.NOTLS] = new List<ChuniAppPatch>()
                {
                    new ChuniAppPatch(ChuniAppPatchType.NOTLS, 0xD04D09, new byte[] { 0x81, 0xE3, 0x00, 0x00, 0x80, 0x00 }, new byte[] { 0x31, 0xDB, 0x90, 0x90, 0x90, 0x90 }),
                },
            };

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
            File.WriteAllText("chunithm.tools.settings.json", json);
        }

        public static Settings LoadOrSaveSettings()
        {
            if (File.Exists(SettingFileName))
            {
                var json = File.ReadAllText(SettingFileName);
                return JsonSerializer.Deserialize<Settings>(json, new JsonSerializerOptions()
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                });
            }
            else
            {
                var settings = new Settings();
                settings.SaveSettings();
                return settings;
            }
        }
    }
}
