using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct VirtualFolder
{
    public string Root { get => root; private set => root = value; }
    public string Name { get => name; private set => name = value; }
    public List<VirtualFolder> SubFolders { get => subFolders; private set => subFolders = value; }

    string root;
    string name;
    List<VirtualFolder> subFolders;

    public VirtualFolder (string root, string name)
    {
        this.root = root;
        this.name = name;
        this.subFolders = new List<VirtualFolder>();
    }
    /// <summary>
    /// Adds a new sub folder to this folder and returns it.
    /// </summary>
    /// <param name="name">Name of the new subfolder.</param>
    /// <returns>The new virtual folder.</returns>
    public VirtualFolder AddSubFolder (string name)
    {
        var subFolder = new VirtualFolder(root + "/" + this.name + "/", name);
        subFolders.Add(subFolder);
        return subFolder;
    }

    /// <summary>
    /// Adds multiple new sub folders to this folder.
    /// </summary>
    /// <param name="names">An array of names for the sub folders.</param>
    public void AddSubFolders (string[] names)
    {
        foreach (var item in names)
        {
            AddSubFolder(item);
        }
    }
    /// <summary>
    /// Turns this folder (and all subfolders (and their subfolders)) into actual directories, provided they don't already exist.
    /// </summary>
    /// <param name="createGitDummyFile">Whether or not a dummy text file is created for Git, because Git ignores empty folders.</param>
    public void Realize(bool createGitDummyFile = false)
    {
        string fullPath = root + "/" + name;

        if (!Directory.Exists(fullPath))
        {
            CreateFolder(fullPath, createGitDummyFile);
        }
        if (subFolders != null && subFolders.Count > 0)
        {
            foreach (var folder in SubFolders)
            {
                folder.Realize(createGitDummyFile);
            }
        }
    }

    void CreateFolder (string fullPath, bool createGitDummyFile = false)
    {
        Directory.CreateDirectory(fullPath);
        if (createGitDummyFile)
        {
            File.Create(fullPath + "/.gitkeep");
        }
    }
}
