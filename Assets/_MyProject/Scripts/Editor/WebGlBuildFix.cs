using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class WebGlBuildFix
{
    private static string projectName = "QoomonQuest";
    
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget _target, string _destinationPath)
    {
        if (_target != BuildTarget.WebGL)
        {
            return;
        }
        
        string _indexTemplatePath = AssetDatabase.GetAssetPath(Resources.Load<TextAsset>("WebTemplate/indexTemplate"));
        File.Copy(_indexTemplatePath, _destinationPath + Path.DirectorySeparatorChar + "index.html", true);
        Debug.Log($"Overridden index.html using template from {_indexTemplatePath}");

        string _templateDataPath = _indexTemplatePath.Replace("indexTemplate.html", "TemplateDataTemplate");
        string _destinationTemplateDataPath = _destinationPath + Path.DirectorySeparatorChar + "TemplateData";
        
        if (Directory.Exists(_destinationTemplateDataPath))
        {
            Directory.Delete(_destinationTemplateDataPath, true);
        }
        Directory.CreateDirectory(_destinationTemplateDataPath);
        
        CopyDirectoryContents(_templateDataPath, _destinationTemplateDataPath);
        Debug.Log($"Overridden TemplateData using template from {_templateDataPath}");
        
        // Call the rename method for the "Build" folder
        string _buildFolderPath = Path.Combine(_destinationPath, "Build");
        if (Directory.Exists(_buildFolderPath))
        {
            RenameFilesInBuildFolder(_buildFolderPath, projectName);
        }
    }

    private static void CopyDirectoryContents(string _sourceDirName, string _destDirName)
    {
        DirectoryInfo _dir = new DirectoryInfo(_sourceDirName);
        if (!_dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + _sourceDirName);
        }

        DirectoryInfo[] _dirs = _dir.GetDirectories();
        FileInfo[] _files = _dir.GetFiles();
        
        foreach (FileInfo _file in _files)
        {
            if (_file.Extension.Equals(".meta"))
            {
                continue;
            }

            string _tempPath = Path.Combine(_destDirName, _file.Name);
            _file.CopyTo(_tempPath, true);
        }

        foreach (DirectoryInfo _subDir in _dirs)
        {
            string _tempPath = Path.Combine(_destDirName, _subDir.Name);
            Directory.CreateDirectory(_tempPath);
            CopyDirectoryContents(_subDir.FullName, _tempPath);
        }
    }

    private static void RenameFilesInBuildFolder(string _buildFolderPath, string _projectName)
    {
        DirectoryInfo _dir = new DirectoryInfo(_buildFolderPath);
        FileInfo[] _files = _dir.GetFiles();

        foreach (FileInfo _file in _files)
        {
            if (_file.Extension.Equals(".meta"))
            {
                continue;
            }

            string _newFileName = _projectName + _file.Name.Substring(_file.Name.IndexOf('.'));
            string _newFilePath = Path.Combine(_buildFolderPath, _newFileName);
            _file.MoveTo(_newFilePath);
        }
    }
}