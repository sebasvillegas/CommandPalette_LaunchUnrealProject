using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace UEProjectLauncher;

public static class UEProjectLauncherSettings
{
    private const string CustomPathsKey = "CustomProjectPaths";

    public static string RawCustomPaths
    {
        get
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values.TryGetValue(CustomPathsKey, out object value) && value is string pathsStr)
                {
                    return pathsStr;
                }
            }
            catch
            {
            }
            return string.Empty;
        }
        set
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values[CustomPathsKey] = value;
            }
            catch
            {
            }
        }
    }

    public static List<string> GetCustomPaths()
    {
        var pathsStr = RawCustomPaths;
        if (string.IsNullOrWhiteSpace(pathsStr))
        {
            return new List<string>();
        }
        return pathsStr.Split(';', StringSplitOptions.RemoveEmptyEntries)
                       .Select(p => p.Trim())
                       .ToList();
    }
}
