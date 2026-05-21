using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace UEProjectLauncher;

public static class UEProjectLauncherSettings
{
    private const string CustomPathsKey = "CustomProjectPaths";

    public static List<string> GetCustomPaths()
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.TryGetValue(CustomPathsKey, out object value) && value is string pathsStr)
            {
                if (string.IsNullOrWhiteSpace(pathsStr))
                {
                    return new List<string>();
                }
                // Paths are stored as a semicolon-separated string
                return pathsStr.Split(';', StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim())
                               .ToList();
            }
        }
        catch
        {
            // Fallback if ApplicationData fails
        }

        return new List<string>();
    }

    public static void SaveCustomPaths(IEnumerable<string> paths)
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var cleanPaths = paths.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim());
            localSettings.Values[CustomPathsKey] = string.Join(";", cleanPaths);
        }
        catch
        {
            // Ignore storage errors
        }
    }

    public static void AddCustomPath(string path)
    {
        var paths = GetCustomPaths();
        if (!string.IsNullOrWhiteSpace(path) && !paths.Contains(path, StringComparer.OrdinalIgnoreCase))
        {
            paths.Add(path);
            SaveCustomPaths(paths);
        }
    }

    public static void RemoveCustomPath(string path)
    {
        var paths = GetCustomPaths();
        if (paths.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase)) > 0)
        {
            SaveCustomPaths(paths);
        }
    }
}
