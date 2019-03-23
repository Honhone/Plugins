using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// This tool is intended to work with RuntimeRenderQueue MonoBehaviour.
/// It will iterate through SRQ MonoBehaviours under the hierarchy, record their runtime values and restore them when runtime ends.
/// The process is manual.
/// Select hierarchy root that contains renderers and use menu items in [HoneySelect].
/// Save\Load functions REQUIRE "Set Render Queue" MonoBehaviour on your renderers to work. 
/// If it does not find one, such renderer will be SKIPPED on save\load!
/// REQUIRES ReflectionHelper to work!
/// </summary>
public class SRQPersist : ScriptableObject
{
    private static Dictionary<int, int> SaveHold = new Dictionary<int, int>();

    [MenuItem("HoneySelect/Save Render Queues")]
    static void SaveSRQ()
    {
        SaveHold.Clear();
        if (Selection.activeGameObject != null)
        {            
            var SelSet = Selection.activeGameObject.GetComponentsInChildren<SetRenderQueue>();
            if (SelSet.Length > 0)
            {
                foreach (var Sel in SelSet)
                {
                    SaveHold.Add(Sel.GetInstanceID(), SRQGet(Sel));
                    Debug.Log("Saved Render Queue ID: " + Sel.GetInstanceID() + " | Value: " + SRQGet(Sel));
                }
            }
            else
            {
                Debug.Log("SRQPersist: Nothing was saved.");
            }
        }
        else
        {
            Debug.Log("SRQPersist: Nothing was saved.");
        }
    }

    [MenuItem("HoneySelect/Load Render Queues")]
    static void LoadSRQ()
    {      
        if (Selection.activeGameObject != null)
        {
            if (SaveHold.Count != 0)
            {
                var SelSet = Selection.activeGameObject.GetComponentsInChildren<SetRenderQueue>();
                if (SelSet.Length > 0)
                {
                    //Debug.Log("Loading in process");
                    foreach (var Sel in SelSet)
                    {
                        if (SaveHold.ContainsKey(Sel.GetInstanceID()))
                        {
                            int value;
                            if (SaveHold.TryGetValue(Sel.GetInstanceID(), out value))
                            {
                                SRQSet(Sel, value);
                            }
                            Debug.Log("Restored Render Queue ID: " + Sel.GetInstanceID() + " | Value: " + value);
                        }
                    }
                    SaveHold.Clear();
                }
            }
            else
            {
                Debug.Log("SRQPersist: Nothing was loaded.");
            }
        }
    }

    private static int SRQGet(SetRenderQueue queueObject)
    {
        var queueArray = ReflectionHelper.GetFieldValue(queueObject, "m_queues") as int[];
        foreach (int queue in queueArray)
        {
            return queue;
        }
        Debug.LogError("SRQGet in SRQPersist RETURNS ZERO SOMETHING IS TERRIBLY WRONG");
        return 0;
    }

    private static void SRQSet(SetRenderQueue queueObject, int value)
    {
        queueObject.SetFieldValue("m_queues", new int[] { value });
    }
}
