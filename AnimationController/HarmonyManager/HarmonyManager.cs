using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using Studio;
using System.Reflection;

namespace AnimationController
{
    /// <summary>
    /// This class holds collection for harmony patches and creates components that implement them.
    /// </summary>
    public class HarmonyManager : MonoBehaviour
    {
        public static HarmonyInstance HarmonyInstance { get; private set; }
        private MethodInfo mi_AddNode;
        public HarmonyAdd Adder { get; private set; }
        public HarmonySelect Selector { get; private set; }

        public static Action SceneLoadHappened;
        public static Action AnimLoadHappened;

        public void Init()
        {
            AttacherInit();
            SelectorInit();
            HarmonyInit();
        }

        private void HarmonyInit()
        {
            try
            {
                (HarmonyInstance = HarmonyInstance.Create(nameof(HarmonyManager))).PatchAll(typeof(HarmonyManager));   
                mi_AddNode = typeof(TreeNodeCtrl).GetMethod("AddNode", BindingFlags.Instance | BindingFlags.Public, null,
                    new[] { typeof(string), typeof(TreeNodeObject) }, null); //find AddNode
                HarmonyInstance.Patch(mi_AddNode, null, new HarmonyMethod(typeof(HarmonyManager), nameof(AddNodeName))); //patch AddNode
                AddNeoAddonPostfix();
                Logger.Create(GetType());
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Harmony manager: Harmony selection patch failed" + e);
            }

        }
        private void AttacherInit()
        {
            Adder = new GameObject("AttachController").AddComponent<HarmonyAdd>();
            Adder.transform.parent = gameObject.transform;
        }

        private void SelectorInit()
        {
            Selector = new GameObject("SelectController").AddComponent<HarmonySelect>();
            Selector.transform.parent = gameObject.transform;
        }


        private void OnDestroy()
        {
            UnityEngine.Debug.Log("Harmony manager: Harmony patch purge");
            try
            {
                foreach (var met in typeof(HarmonyManager).GetMethods().Where(x => x.IsStatic && x.IsPublic))
                    foreach (var attr in met.GetCustomAttributes(false))
                        if (attr is HarmonyPatch hp)
                        {
                            var original = AccessTools.Method(hp.info.originalType, hp.info.methodName, hp.info.parameter);
                            if (original != null) HarmonyInstance.RemovePatch(original, met);
                        }
                HarmonyInstance.RemovePatch(mi_AddNode, typeof(HarmonyManager).GetMethod(nameof(AddNodeName)));
                UnityEngine.Debug.Log("Harmony manager: purge success");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Harmony manager: Harmony patch purge failed: " + e);
            }
            Logger.Destroy(GetType());
        }

        #region harmony saveloadcounter patches + controller classes attach logic
        public static void AddNodeName(ref TreeNodeObject __result) => HarmonyAdd.Counter(true); //know when objects are added

        [HarmonyPostfix, HarmonyPatch(typeof(TreeNodeCtrl), "DeleteNode", new[] { typeof(TreeNodeObject) })] //know when objects are deleted 
        public static void DeleteNode(ref TreeNodeCtrl _node) => HarmonyAdd.Counter(false);

        [HarmonyPostfix, HarmonyPatch(typeof(TreeNodeCtrl), "AddSelectNode")] //know when objects are selected
        public static void AddSelectNode() => HarmonySelect.RefreshList(); 

        public static void AddNeoAddonPatch() => HarmonySelect.RefreshList();//know when user tries to select objects through studio neo addon     
        public static void AddNeoAddonPostfix()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "HSStudioNEOAddon");
            if (assembly == null) return;
            var tnoa = assembly.GetType("HSStudioNEOAddon.ObjMoveRotAssist.WorkspaceAssistMgr+TreeNodeObjectAssist");
            if (tnoa == null) return;
            var tns = tnoa.GetMethod("ToggleNodeSelect");
            if (tns == null) return;
            var somePostFix = new HarmonyMethod(typeof(HarmonyManager), nameof(HarmonyManager.AddNeoAddonPatch));
            HarmonyInstance.Patch(tns, null, somePostFix);
        }
        #endregion
        #region Load patches
        [HarmonyPostfix, HarmonyPatch(typeof(Studio.Studio), "LoadScene")] //inform loading system that scene is fully initialized
        public static void LoadScene()
        {
            SceneLoadHappened?.Invoke();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MPCharCtrl), "LoadAnime")] //inform GUI that user loaded some animation
        public static void LoadAnime()
        {
            AnimLoadHappened?.Invoke();
        }
        #endregion
    }
}

