using Harmony;
using Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AnimationController
{
    /// <summary>
    /// This class handles attach of CharControl component to eligible objects
    /// Harmony implementation of Joan6694 save\load\import counter, and owns SaveLoad handler.
    /// </summary>

    public struct Indexer
    {
        public int ObjectCount;
        public int LastObjectCount;
        public int LastIndex;

        public Indexer(int currentObj, int lastObj, int lastInd)
        {
            ObjectCount = currentObj;
            LastObjectCount = lastObj;
            LastIndex = lastInd;
        }
    }

    public class HarmonyAdd : MonoBehaviour
    {
        public Action<CharControl> controlAdded;
        private static HarmonyAdd adder;

        private Indexer index = new Indexer(0, 0, 0);
        //public Action<Indexer> IndexerUpdateWave;


        void Awake()
        {
            adder = this;
            Logger.Create(GetType());
        }
        void OnDestroy()
        {
            Logger.Destroy(GetType());
        }

        public static void Counter(bool optype) //joan6694 save/load counter + controller attach
        {
            //adder._objectCount = Studio.Studio.Instance.dicObjectCtrl.Count;
            adder.index.ObjectCount = Studio.Studio.Instance.dicObjectCtrl.Count;
            if (adder.index.ObjectCount != adder.index.LastObjectCount)
            {
                if (optype)
                {
                    UnityEngine.Debug.Log("Optype ADD");
                    adder.OnObjectAdded(); //controller class attach
                }
                else
                {
                    UnityEngine.Debug.Log("Optype DELETE");
                }
                adder.index.LastIndex = Studio.Studio.Instance.sceneInfo.CheckNewIndex();
                UnityEngine.Debug.Log("Counter log: LastIndex: [" + adder.index.LastIndex + "], ObjCount: [" + adder.index.ObjectCount + "], LastObjCount: [" + adder.index.LastObjectCount + "]");
                //adder.IndexerUpdateWave?.Invoke(adder.index);
            }
            adder.index.LastObjectCount = Studio.Studio.Instance.dicObjectCtrl.Count;
        }

        private void OnObjectAdded() //controller class attach 
        {
            foreach (KeyValuePair<int, ObjectCtrlInfo> kvp in Studio.Studio.Instance.dicObjectCtrl)
            {
                CharControl control;
                switch (kvp.Value)
                {
                    case OCIItem y:
                        if (CheckItem(y))
                        {
                            control = y.objectItem.gameObject.GetOrAddComponent<CharControl>();
                            control.InitItem(y);
                        }
                        break;
                    case OCIChar z:
                        control = z.charInfo.gameObject.GetOrAddComponent<CharControl>();
                        control.InitChar(z);
                        break;
                }
            }
        }
        private bool CheckItem(OCIItem item)
        {
            bool result = item.isAnime && item.animator.parameterCount == 4;
            return result;
        }


        //var query = Studio.Studio.Instance.dicObjectCtrl.Where(x => x.Key >= adder.index.LastIndex).
        //    Select(x =>
        //    {
        //        if (x.Value is OCIItem y && y.isAnime && y.animator.parameterCount == 4)
        //            return y.objectItem.gameObject;
        //        if (x.Value is OCIChar z)
        //            return z.charInfo.gameObject;
        //        return null;
        //    }).Where(y => y != null);

        //foreach (GameObject go in query)
        //{
        //    go.GetOrAddComponent<CharControl>();

        //    //if (go.GetComponent<CharControl>() == null)
        //    //{
        //    //    var charControl = go.AddComponent<CharControl>();
        //    //    Logger.Call(adder.GetType(), "CharControl creation");
        //    //}               
        //}


    }
}
