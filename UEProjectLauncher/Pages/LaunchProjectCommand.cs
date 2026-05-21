using System;
using System.Diagnostics;
using System.IO;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

internal sealed partial class LaunchProjectCommand : InvokableCommand
{
    private readonly string _projectPath;
    private readonly string _projectName;

    public LaunchProjectCommand(string projectPath)
    {
        _projectPath = projectPath;
        _projectName = Path.GetFileNameWithoutExtension(projectPath);
    }

    public override string Name => $"Launch {_projectName}";

    // Using a gamepad/gaming related icon from Segoe Fluent Icons
    public override IconInfo Icon => new("\uE7FC");

    public override CommandResult Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _projectPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to launch Unreal Engine project: {ex.Message}");
        }

        return CommandResult.KeepOpen();
    }
}
