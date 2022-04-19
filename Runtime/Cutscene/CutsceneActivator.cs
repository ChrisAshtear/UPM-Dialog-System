using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActivator : MonoBehaviour
{
    public string sceneID;
    public bool triggerOnce = true;
    public bool destroyObj = true;

    public string triggeredOnlyByTag = "Player";
    //a version with & without object props - ifdef?
    //has to be in assembly definition to use it here.
    private ObjectProps props;

    private void Start()
    {
        props = GetComponent<ObjectProps>();
        bool found = props.propsN.TryGetValue("sceneID", out Property sID);
        if (found)
        {
            sceneID = sID.value;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeredOnlyByTag))
        {
            startEvent();
        }
    }

    private void startEvent()
    {
        if (!CutsceneSystem.activescenes)
        {
            CutsceneSystem.RunCutscene(sceneID);
            if (triggerOnce)
            {
                if (destroyObj) { Destroy(gameObject); }
                else { Destroy(this); }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggeredOnlyByTag))
        {
            startEvent();
        }
    }
}
