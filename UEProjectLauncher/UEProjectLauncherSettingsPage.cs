using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace UEProjectLauncher;

// Change from FormPage to FormContent
internal sealed partial class UEProjectLauncherSettingsPage : FormContent
{
    // Need to supply correct TemplateJson and StateJson
    public UEProjectLauncherSettingsPage()
    {
        // Simple adaptive card for paths
        TemplateJson = @"
        {
            ""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""body"": [
                {
                    ""type"": ""TextBlock"",
                    ""text"": ""Custom Project Search Paths"",
                    ""weight"": ""Bolder"",
                    ""size"": ""Medium""
                },
                {
                    ""type"": ""TextBlock"",
                    ""text"": ""Semicolon-separated list of directories to scan for .uproject files (e.g. D:\\MyGames;C:\\Work)"",
                    ""wrap"": true
                },
                {
                    ""type"": ""Input.Text"",
                    ""id"": ""CustomPaths"",
                    ""placeholder"": ""Enter paths..."",
                    ""isMultiline"": true,
                    ""value"": ""${customPaths}""
                }
            ],
            ""actions"": [
                {
                    ""type"": ""Action.Submit"",
                    ""title"": ""Save""
                }
            ]
        }";

        StateJson = $$"""
        {
            "customPaths": "{{UEProjectLauncherSettings.RawCustomPaths.Replace("\\", "\\\\")}}"
        }
        """;
    }

    public override CommandResult SubmitForm(string payload)
    {
        try
        {
            // payload is JSON like {"CustomPaths":"path1;path2"}
            // Quick and dirty parse to avoid bringing in full System.Text.Json dependencies if we don't have to
            var searchStr = "\"CustomPaths\":\"";
            var idx = payload.IndexOf(searchStr);
            if (idx >= 0)
            {
                var startIdx = idx + searchStr.Length;
                var endIdx = payload.IndexOf("\"", startIdx);
                if (endIdx > startIdx)
                {
                    var newPaths = payload.Substring(startIdx, endIdx - startIdx);
                    // Unescape JSON slashes
                    newPaths = newPaths.Replace("\\\\", "\\");
                    UEProjectLauncherSettings.RawCustomPaths = newPaths;
                }
            }
        }
        catch
        {
        }

        return CommandResult.KeepOpen();
    }
}
