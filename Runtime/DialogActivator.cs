using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string dialogID;
    public bool triggerOnce = true;
    public bool destroyObj = true;

    public string triggeredOnlyByTag = "Player";
    //a version with & without object props - ifdef?
    //has to be in assembly definition to use it here.
    private ObjectProps props;

    private void Start()
    {
        props = GetComponent<ObjectProps>();
        bool found = props.propsN.TryGetValue("dialogID", out Property dID);
        if(found)
        {
            dialogID = dID.value;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(triggeredOnlyByTag))
        {
            startEvent();
        }
    }

    private void startEvent()
    {
        if(!DialogSystem.activeDialog)
        {
            DialogSystem.DialogEvent(dialogID);
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
