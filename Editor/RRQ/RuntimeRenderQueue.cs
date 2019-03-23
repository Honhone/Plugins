using UnityEngine;
using UnityEditor;


/// <summary>
/// This tool detects changes made to "Set Render Queue" MonoBehaviour on selected renderer reflects them runtime.
/// How to use: throw on any null in your scene. Adjust SRQ runtime.
/// It is intended to work in conjunction with SRQPersist scriptable object.
/// REQUIRES ReflectionHelper to work!
/// </summary>
public class RuntimeRenderQueue : MonoBehaviour {

    private GameObject selectedObject;
    private Renderer selectedRenderer;
    private SetRenderQueue selectedSRQ;

    private int queueCache;
    private bool editEligible = false;
    
    // Selection logic
    void Update() {

        if (Selection.activeGameObject != selectedObject && Selection.activeGameObject != null)
        {
            selectedObject = Selection.activeGameObject;
            if (selectedObject.GetComponent<Renderer>() != null && selectedObject.GetComponent<SetRenderQueue>() != null)
            {
                selectedRenderer = selectedObject.GetComponent<Renderer>();
                //Debug.Log(selectedRenderer.material.renderQueue);
                queueCache = selectedRenderer.material.renderQueue;
                editEligible = true;
                //Debug.Log("editEligible " + editEligible);
            }
            else
            {
                editEligible = false;
                //Debug.Log("editEligible " + editEligible);
            }
        }
        if (editEligible)
        {
            if (queueCache != SRQGet(selectedObject))
            {                
                selectedRenderer.material.renderQueue = SRQGet(selectedObject);
                queueCache = SRQGet(selectedObject);
            }
        }
    }

    //SRQ query
    private int SRQGet(GameObject queueObject)
    {
        selectedSRQ = queueObject.GetComponent<SetRenderQueue>();
        var queueArray = ReflectionHelper.GetFieldValue(selectedSRQ, "m_queues") as int[];
        foreach (int queue in queueArray)
        {            
            return queue;
        }
        Debug.LogError("SRQGet in RuntimeRenderQueue RETURNS ZERO SOMETHING IS TERRIBLY WRONG");
        return 0;        
    }    
}
