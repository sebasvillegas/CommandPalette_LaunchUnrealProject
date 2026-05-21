using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

// We use a FormPage to create a UI for settings
internal sealed partial class UEProjectLauncherSettingsPage : FormPage
{
    private TextFormItem _customPathsItem;

    public UEProjectLauncherSettingsPage()
    {
        Icon = new IconInfo("\uE713"); // Settings gear
        Title = "Unreal Launcher Settings";
        Name = "Settings";

        _customPathsItem = new TextFormItem()
        {
            Title = "Custom Project Paths",
            PlaceholderText = "E.g. D:\\MyGames;C:\\Work",
            Value = UEProjectLauncherSettings.RawCustomPaths,
            IsMultiline = true
        };

        Items = [
            _customPathsItem
        ];
    }

    public override CommandResult SubmitForm(string payload)
    {
        // When the user clicks the checkmark/save in the Command Palette form,
        // it updates the values. We just need to read it back and save it.
        var newPaths = _customPathsItem.Value;
        UEProjectLauncherSettings.RawCustomPaths = newPaths;

        return CommandResult.KeepOpen();
    }
}
