using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimationController
{
    /// <summary>
    /// This class serves as root point for plugin, creates and initializes all necessary components.
    /// </summary>
    class AnimationController : MonoBehaviour
    {
        private HarmonyManager harmonyManager;
        private GUIManager guiManager;
        private SaveLoadManager extSaveHandler;

        void Awake()
        {
            HarmonyManagerInit();
            GUIManagerInit();
            SaveLoadHandlerInit();

            Logger.Create(GetType());
        }

        private void HarmonyManagerInit()
        {
            harmonyManager = new GameObject("HarmonyManager").AddComponent<HarmonyManager>();
            harmonyManager.transform.parent = gameObject.transform;
            harmonyManager.Init();
        }
        private void GUIManagerInit()
        {
            guiManager = new GameObject("GUIManager").AddComponent<GUIManager>();
            guiManager.transform.parent = gameObject.transform;
            guiManager.Init();

        }
        private void SaveLoadHandlerInit()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "HSExtSave");
            if (assembly == null)
            {
                UnityEngine.Debug.LogError("HSExtSave plugin not found. Scene states will not be saved.");
                //return;
            }
            else
            {
                extSaveHandler = new GameObject("SaveLoadHandler").AddComponent<SaveLoadManager>();
                extSaveHandler.transform.parent = gameObject.transform;
                extSaveHandler.Init();
            }
        }

        private void OnDestroy()
        {
            Logger.Destroy(GetType());
        }
    }
}
