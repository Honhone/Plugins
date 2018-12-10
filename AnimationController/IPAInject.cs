using System.Linq;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnimationController
{
    /// <summary>
    /// This class is entrypoint into game. Stolen completely from Keelhauled!
    /// </summary> 
    public class IPAInject : IEnhancedPlugin
    {
        public const string PLUGIN_NAME = "AnimationController";
        public const string PLUGIN_VERSION = "0.1a";
        public string Name => PLUGIN_NAME;
        public string Version => PLUGIN_VERSION;


        public string[] Filter => new string[]
        {
            "StudioNEO_32",
            "StudioNEO_64",
        };

        public static string[] SceneFilter = new string[]
        {
            "Studio",
        };

        public void OnLevelWasInitialized(int level)
        {

        }

        public void OnLevelWasLoaded(int level)
        {
            if (SceneManager.GetActiveScene().name != "Studio") return;
            StartMod();
        }

        public static void StartMod()
        {
            if (SceneFilter.Contains(SceneManager.GetActiveScene().name)) new GameObject(PLUGIN_NAME).AddComponent<AnimationController>();
        }

        public static void Bootstrap()
        {
            var gameobject = GameObject.Find(PLUGIN_NAME);
            if (gameobject != null) GameObject.DestroyImmediate(gameobject);

            StartMod();
        }

        public void OnUpdate() { }
        public void OnLateUpdate() { }
        public void OnApplicationQuit() { }
        public void OnFixedUpdate() { }
        public void OnApplicationStart() { }
    }
}