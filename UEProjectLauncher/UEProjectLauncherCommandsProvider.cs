// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

public partial class UEProjectLauncherCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public UEProjectLauncherCommandsProvider()
    {
        DisplayName = "UE-Projects";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");

        // SettingsPage implements ICommand so it can be used directly as a CommandItem
        var settingsCommand = new CommandItem(new SettingsContentPage(new UEProjectLauncherSettingsPage()))
        {
            Title = "UE-Projects Settings"
        };

        _commands = [
            new CommandItem(new UEProjectLauncherPage()) { Title = DisplayName },
            settingsCommand
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
