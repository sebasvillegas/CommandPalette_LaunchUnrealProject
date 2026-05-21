using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

internal sealed partial class UEProjectLauncherPage : ListPage
{
    public UEProjectLauncherPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Unreal Engine Projects";
        Name = "UE Projects";
    }

    public override IListItem[] GetItems()
    {
        var projects = GetUnrealProjects();

        if (projects.Count == 0)
        {
            return [
                new ListItem(new NoOpCommand())
                {
                    Title = "No Unreal Engine projects found",
                    Subtitle = "Make sure you have projects created or registered in the Epic Games Launcher."
                }
            ];
        }

        return projects.Select(p => new ListItem(new LaunchProjectCommand(p))
        {
            Title = Path.GetFileNameWithoutExtension(p),
            Subtitle = p
        }).ToArray();
    }

    private List<string> GetUnrealProjects()
    {
        var projects = new List<string>();
        var scannedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var settingsPath = Path.Combine(appData, "EpicGamesLauncher", "Saved", "Config", "Windows", "GameUserSettings.ini");

            if (File.Exists(settingsPath))
            {
                var lines = File.ReadAllLines(settingsPath);
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("CreatedProjectPaths="))
                    {
                        var path = line.Substring(line.IndexOf('=') + 1).Trim();
                        if (Directory.Exists(path) && scannedDirectories.Add(path))
                        {
                            ScanDirectoryForProjects(path, projects);
                        }
                    }
                }
            }

            // Fallback to default documents path if we didn't find any in the ini, or just to be thorough
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var defaultUePath = Path.Combine(docsPath, "Unreal Projects");
            if (Directory.Exists(defaultUePath) && scannedDirectories.Add(defaultUePath))
            {
                ScanDirectoryForProjects(defaultUePath, projects);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error finding Unreal Engine projects: {ex.Message}");
        }

        return projects.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private void ScanDirectoryForProjects(string path, List<string> projects)
    {
        try
        {
            // Unreal Projects are usually folders containing a .uproject file.
            // We search top directory only to avoid deep recursive scans of large project folders.
            // GameUserSettings.ini stores the *parent* directory of the projects.
            var subDirectories = Directory.GetDirectories(path);
            foreach (var dir in subDirectories)
            {
                var uprojectFiles = Directory.GetFiles(dir, "*.uproject", SearchOption.TopDirectoryOnly);
                projects.AddRange(uprojectFiles);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to scan directory {path}: {ex.Message}");
        }
    }
}
