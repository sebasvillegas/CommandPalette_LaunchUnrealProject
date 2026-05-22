// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

[Guid("3c65ee5b-abba-43e2-a03e-24570d282c2b")]
public sealed partial class UEProjectLauncher : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly UEProjectLauncherCommandsProvider _provider = new();
    private readonly ICommandSettings _settingsProvider;

    public UEProjectLauncher(ManualResetEvent extensionDisposedEvent)
    {
        this._extensionDisposedEvent = extensionDisposedEvent;

        var settings = new Settings();
        settings.Add(new TextSetting("CustomPaths", string.Join(";", UEProjectLauncherSettings.GetCustomPaths()))
        {
            Name = "Custom Project Search Paths",
            Description = "Semicolon-separated list of directories to scan for .uproject files (e.g. D:\\MyGames;C:\\Work)"
        });

        settings.SettingsChanged += (s, e) =>
        {
            if (e.TryGetSetting("CustomPaths", out TextSetting pathSetting))
            {
                var newPaths = pathSetting.Value.Split(';', StringSplitOptions.RemoveEmptyEntries);
                UEProjectLauncherSettings.SaveCustomPaths(newPaths);
            }
        };

        _settingsProvider = settings;
    }

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => _provider,
            ProviderType.Settings => _settingsProvider,
            _ => null,
        };
    }

    public void Dispose() => this._extensionDisposedEvent.Set();
}
