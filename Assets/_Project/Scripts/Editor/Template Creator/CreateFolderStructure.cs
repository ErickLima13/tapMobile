using UnityEditor;
using UnityEngine;

public class CreateFolderStructure : EditorWindow
{
    private static string rootFolderName = "_Project";
    private static bool createGitPlaceholder = true;

    [MenuItem("Tools/Create Default Folders")]
    private static void CreateWindow()
    {
        CreateFolderStructure window = ScriptableObject.CreateInstance<CreateFolderStructure>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 300);
        window.Show();
    }

    private static void CreateFolders()
    {
        /// ---- How To Use ---- \\\
        /// First, create your main folder like so:
        /// VirtualFolder rootFolder = new VirtualFolder("Assets", rootFolderName);
        /// This will create a folder with whatever name you entered in the window, which you can find in Unity under Assets -> Create Default Folders.
        /// In order to add sub folders to this, you simply call either AddSubFolder or AddSubFolders on it, like so:
        /// rootFolder.AddSubFolder("YourNewSubFolder");
        /// This will add a new folder into the root folder, which ultimately looks like this: Assets/RootFolder/YourNewSubFolder.
        /// You can call AddSubFolder again to add yet another sub folder to THAT sub folder, like this:
        /// rootFolder.AddSubFolder("YourNewSubFolder").AddSubFolder("YetAnotherFolder");
        /// Which will result in this structure: Assets/RootFolder/YourNewSubFolder/YetAnotherFolder.
        /// You can chain these as much as you'd like.
        /// Furthermore, you can add multiple folders at once by calling AddSubFolders, like this:
        /// rootFolder.AddSubFolders(new string[] {"FolderOne", "FolderTwo", "FolderThree"});
        /// Note that this does NOT return the folders, so you can't chain on these. 
        /// So you should probably only do this on the deepest part of your hierachy.
        /// Finally, below is an example implementation that will probably suit most people just fine.
        /// But feel free to change it to your liking.


        // Create the root folder which you can name
        VirtualFolder rootFolder = new VirtualFolder("Assets", rootFolderName);
        VirtualFolder noVersion = new VirtualFolder("Assets", "__NoVersionControl");

        // Add a bunch of folders into this "Resources" folder.

        rootFolder.AddSubFolder("Audio");
        rootFolder.AddSubFolder("Art").AddSubFolders(new string[] { "Animations", "Materials", "Models", "Shaders", "Sprites", "Textures", "Fonts" });

        // Add more top level folders.
        rootFolder.AddSubFolders(new string[] { "Scripts", "Prefabs", "Scenes", "Data" });
        rootFolder.AddSubFolders(new string[] { "Plugins", "Resources" });


        rootFolder.AddSubFolder("Scenes").AddSubFolders(new string[] { "Game", "Prototype" });
        rootFolder.AddSubFolder("Scripts").AddSubFolders(new string[] { "Editor" });
        rootFolder.AddSubFolder("Scripts").AddSubFolder("Runtime").AddSubFolders(new string[] { "Implementations", "Systems", "Utility" });


        // By calling Realize() on the root folder, it will automatically create every folder we've added to it or its sub folders!
        rootFolder.Realize(createGitPlaceholder);
        noVersion.Realize(createGitPlaceholder);

        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Enter name of the root folder: ");
        rootFolderName = EditorGUILayout.TextField("Root:", rootFolderName);
        createGitPlaceholder = EditorGUILayout.ToggleLeft("Placeholders", createGitPlaceholder);
        EditorGUILayout.HelpBox("When ticked, a .gitkeep file will be created in the folder to make Git keep it. This file will not be imported by Unity and will only be visible in the explorer.", MessageType.Info);


        if (GUILayout.Button("Generate"))
        {
            CreateFolders();
            this.Close();
        }
        EditorGUILayout.EndVertical();
    }
}
