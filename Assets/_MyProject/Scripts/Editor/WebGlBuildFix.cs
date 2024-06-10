using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class WebGlBuildFix
{
    private static string projectName = "QoomonQuest";

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string destinationPath)
    {
        if (target != BuildTarget.WebGL)
        {
            return;
        }

        string indexPath = AssetDatabase.GetAssetPath(Resources.Load<TextAsset>("WebTemplate/indexTemplate"));
        File.Copy(indexPath, Path.Combine(destinationPath, "index.html"), true);
        Debug.Log($"Overridden index.html using template from {indexPath}");

        // Copy manifest.webmanifest
        string manifestPath = Path.Combine(destinationPath, "manifest.webmanifest");
        string manifestData = @"
        {
            ""name"": ""Qoomon Quest"",
            ""short_name"": ""Qoomon Quest"",
            ""start_url"": ""index.html"",
            ""display"": ""fullscreen"",
            ""background_color"": ""#231F20"",
            ""theme_color"": ""#000"",
            ""description"": """",
            ""icons"": [
                {
                    ""src"": ""TemplateData/icons/unity-logo-dark.png"",
                    ""sizes"": ""144x144"",
                    ""type"": ""image/png"",
                    ""purpose"": ""any maskable""
                },
                {
                    ""src"": ""TemplateData/icons/unity-logo-dark.png"",
                    ""sizes"": ""512x512"",
                    ""type"": ""image/png""
                }
            ]
        }";
        File.WriteAllText(manifestPath, manifestData);
        Debug.Log($"Overridden manifest.webmanifest using preset text");

        string baseTemplatePath = Path.GetDirectoryName(indexPath);

        // Copy TemplateData folder
        string templateDataPath = Path.Combine(baseTemplatePath, "TemplateDataTemplate");
        string destinationTemplateDataPath = Path.Combine(destinationPath, "TemplateData");
        CopyFolder(templateDataPath, destinationTemplateDataPath);

        // Copy js folder
        string jsPath = Path.Combine(baseTemplatePath, "js");
        string destinationJsPath = Path.Combine(destinationPath, "js");
        CopyFolder(jsPath, destinationJsPath);

        // Rename files in the "Build" folder
        string buildFolderPath = Path.Combine(destinationPath, "Build");
        if (Directory.Exists(buildFolderPath))
        {
            RenameFilesInBuildFolder(buildFolderPath, projectName);
        }
    }

    private static void CopyFolder(string sourcePath, string destinationPath)
    {
        if (Directory.Exists(destinationPath))
        {
            Directory.Delete(destinationPath, true);
        }
        Directory.CreateDirectory(destinationPath);

        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourcePath}");
        }

        foreach (FileInfo file in dir.GetFiles())
        {
            if (file.Extension.Equals(".meta"))
            {
                continue;
            }
            string tempPath = Path.Combine(destinationPath, file.Name);
            file.CopyTo(tempPath, true);
        }

        foreach (DirectoryInfo subdir in dir.GetDirectories())
        {
            string tempPath = Path.Combine(destinationPath, subdir.Name);
            CopyFolder(subdir.FullName, tempPath);
        }
    }

    private static void RenameFilesInBuildFolder(string buildFolderPath, string projectName)
    {
        DirectoryInfo dir = new DirectoryInfo(buildFolderPath);
        foreach (FileInfo file in dir.GetFiles())
        {
            if (file.Extension.Equals(".meta"))
            {
                continue;
            }

            string newFileName = projectName + file.Name.Substring(file.Name.IndexOf('.'));
            string newFilePath = Path.Combine(buildFolderPath, newFileName);
            file.MoveTo(newFilePath);
        }
    }
}
