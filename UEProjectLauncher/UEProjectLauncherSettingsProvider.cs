using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CommandPalette.Extensions;

namespace UEProjectLauncher;

public partial class UEProjectLauncherSettingsProvider : ISettingsProvider
{
    public ISetting[] GetSettings()
    {
        return [
            new Setting
            {
                Id = "CustomProjectPaths",
                SettingData = new StringSettingData
                {
                    Name = "Custom Project Search Paths",
                    Description = "Semicolon-separated list of directories to scan for .uproject files (e.g. D:\\MyGames;C:\\Work)",
                    Value = string.Join(";", UEProjectLauncherSettings.GetCustomPaths())
                }
            }
        ];
    }

    public void UpdateSetting(string settingId, ISettingData updatedData)
    {
        if (settingId == "CustomProjectPaths" && updatedData is StringSettingData stringData)
        {
            var paths = stringData.Value.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());
            UEProjectLauncherSettings.SaveCustomPaths(paths);
        }
    }
}
