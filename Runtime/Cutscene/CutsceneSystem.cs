using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public delegate void EventProcessor(string arguments);//maybe add callback here?
public delegate void Callback(string arg);

public class CutsceneSystem : MonoBehaviour
{
    //if in pc, get path info from this file and then try to open in streaming asssets.
    //need UnityEditor
    //Debug.Log(AssetDatabase.GetAssetPath(material));
    public TextAsset scenesFile;
    public float delayBetweenPages = 2f;
    public float delayBetweenFadeout = 2f;
    private static ScenesFile scenes;

    public GameObject onscreenUI;
    public GameObject bgContainer;
    public Image bgImage;

    public UnityEvent onCutsceneStart;
    public UnityEvent onCutsceneEnd;

    public static CutsceneSystem ins;
    //is there a scenes currently running?
    public static bool activescenes = false;
    public static bool activeevent = false;

    private Dictionary<string, Sprite> bgList;
    private Dictionary<string, EventProcessor> eventProcessors;

    private void Awake()
    {
        if (CutsceneSystem.ins != null)
        {
            Destroy(CutsceneSystem.ins);
        }
        else
        {
            CutsceneSystem.ins = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {
        eventProcessors = new Dictionary<string, EventProcessor>();
        //need to support streaming assets on desktop mode.
        if (!scenesFile) { Debug.LogError("scenes System-Need a scenes file!"); }
        XElement scenesDoc = XDocument.Parse(scenesFile.text).Element("scenesFile");
        scenes = new ScenesFile(scenesDoc, "");

        Sprite[] sprites = Resources.LoadAll<Sprite>(scenes.bgFolder + "/");
        if (sprites.Length < 1) { Debug.LogError("scenes System- cant load images: " + scenes.bgFolder + "/. Does the folder have images imported as sprites?"); }
        bgList = sprites.ToDictionary(x => x.name, x => x);

        AddEventHandler("runDialog", Ev_Dialog);
        AddEventHandler("changeBG", Ev_changeBG);
    }

    public static void AddEventHandler(string eventName,EventProcessor handler)
    {
        if (!ins.eventProcessors.ContainsKey(eventName))
        {
            ins.eventProcessors.Add(eventName, handler);
        }//
    }

    public void Ev_Dialog(string dialogID)
    {
        activeevent = true;
        DialogSystem.DialogEvent(dialogID,false, AsyncEventComplete);
    }

    public static void RunCutscene(string id, bool overrideExisting = false)//if override is true, cancels any current sceness.
    {
        bool found = scenes.scenes.TryGetValue(id, out Scene ev);
        if (overrideExisting) { ins.StopCoroutine("RunScene"); activescenes = false; }
        if (!found) { Debug.LogError("Cutscenes System- Scene not found:" + id); }
        else if (!activescenes)
        {
            ins.StartCoroutine("RunScene", ev);
        }
    }

    public void AsyncEventComplete(string text)
    {
        activeevent = false;
    }

    private void Ev_changeBG(string name)
    {
        bool found = bgList.TryGetValue(name, out Sprite img);
        if (!found) { Debug.LogError("scenes System- cant find image: " + scenes.bgFolder + "/. Does the folder have images imported as sprites?"); return; }
        bgImage.sprite = img;
        bgContainer.SetActive(true);
    }

    public bool StartEvent(string eventCode, string eventArgs)
    {
        bool handlerFound = eventProcessors.TryGetValue(eventCode, out EventProcessor handler);
        if (!handlerFound) { Debug.LogError("CutsceneSystem- event handler not found for " + eventCode); return false; }
        
        handler.Invoke(eventArgs);
        return false;
    }

    IEnumerator RunScene(Scene scn)
    {
        CutsceneSystem.activescenes = true;
        bgContainer.SetActive(true);
        onCutsceneStart.Invoke();

        //fade to black
        //use a unity event handler for each supported function? or an override option?
        foreach(SceneEvent evnt in scn.events)
        {
            StartEvent(evnt.eventCode, evnt.eventArgs);
            //activeevent is used to signal for async event completion.
            while(activeevent)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(delayBetweenPages);

        }

        yield return new WaitForSeconds(delayBetweenFadeout);
        CutsceneSystem.activescenes = false;
        bgContainer.SetActive(false);
        onCutsceneEnd.Invoke();

        yield break;
    }

}
