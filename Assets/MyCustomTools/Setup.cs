using System.IO;
using UnityEditor;
using UnityEngine;

public static class Setup
{
    
    [MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault("_Project", "Animation", "Art", "Audio", "Data", "Scripts", "InputActions", "Prefabs",
            "Scenes", "ScriptableObjects", "Resources", "StreamingAssets", "Tests");
        
        // Scripts
        Folders.CreateDefault("_Project/Scripts","Editor","Runtime", "Tests");
        Folders.CreateDefault("_Project/Scripts/Runtime","AI","Character", "Combat", "Core", "Environment","Physics","Procedural", "UI", "Input");
        Folders.CreateDefault("_Project/Scripts/Runtime/Core","UpdatePublisher", "Predicates");
        Folders.CreateDefault("_Project/Scripts/Runtime/Character","Data","StateMachine");
        Folders.CreateDefault("_Project/Scripts/Runtime/Character/StateMachine","Transition","States");
        
        //Scenes
        Folders.CreateDefault("_Project/Scenes","Levels","Main", "Test");
        
        //Prefabs
        Folders.CreateDefault("_Project/prefabs","Characters","Effects", "Environment", "Systems", "UI");
        
        //Data
        Folders.CreateDefault("_Project/Data","GameSettings", "Environment");
        
        //Audio
        Folders.CreateDefault("_Project/Audio","Ambient", "Music", "SFX");
        
        //Art
        Folders.CreateDefault("_Project/Art","Animations", "Materials", "Models", "Textures","UI","VFX");
        
        
        
        
        
        
        
    }

    public class Folders
    {
        public static void CreateDefault(string root, params string[] folders)
        {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var folder in folders)
            {
                var path = Path.Combine(fullPath, folder);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }
    }
}