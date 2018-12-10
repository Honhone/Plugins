using Harmony;
using Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimationController
{
    /// <summary>
    /// This class handles implementations of patched selection. It provides access to selected CharControl components across entire plugin.
    /// </summary>
    public class HarmonySelect : MonoBehaviour
    {
        public static HarmonySelect Instance { get; private set; }
        public CharControl SelectTarget { get; private set; }
        public List<CharControl> SelectTargetList { get; private set; }
        private CharControl lastSelected;

        public event Action<CharControl> SelectChanged;
        public event Action SelectNone;
        public event Action<CharControl> SelectSomething;

        private void Awake()
        {
            SingletonInit();
            SelectTargetList = new List<CharControl>();
            Logger.Create(GetType());
        }
        private void SingletonInit()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                DestroyImmediate(this);
            }
        }
        void OnDestroy()
        {
            Logger.Destroy(GetType());
        }

        public static void RefreshList() //refreshing list thanks to essu
        {
            Instance.SelectTargetList.Clear();
            foreach (var soc in Studio.Studio.Instance.treeNodeCtrl.selectObjectCtrl)
            {
                if (soc == null) continue;
                if (soc is OCIItem x && x != null) Instance.SelectTargetList.Add(x.objectItem.gameObject.GetComponent<CharControl>());
                if (soc is OCIChar y && y != null) Instance.SelectTargetList.Add(y.charInfo.gameObject.GetComponent<CharControl>());
                else continue;
            }
            Instance.Selector();
        }
        private void Selector()
        {
            lastSelected = SelectTarget;
            SelectTarget = SelectTargetList.FirstOrDefault();
            if (SelectTarget == null)
            {
                SelectNone?.Invoke();
                UnityEngine.Debug.Log("Invoke nothing");
            }
            else if (SelectTarget != lastSelected)
            {
                SelectChanged?.Invoke(SelectTarget);
                UnityEngine.Debug.Log("Invoke unique selected");
            }
            else
            {
                SelectSomething?.Invoke(SelectTarget);
                UnityEngine.Debug.Log("Invoke non-unique selected");
            }
        }
    }

}
