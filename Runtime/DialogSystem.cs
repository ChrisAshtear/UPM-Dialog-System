using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    //if in pc, get path info from this file and then try to open in streaming asssets.
    //need UnityEditor
    //Debug.Log(AssetDatabase.GetAssetPath(material));
    public TextAsset dialogFile;
    public float delayBetweenPages = 2f;
    public float delayBetweenLetters = 0.02f;
    public float delayBetweenFadeout = 2f;
    private static DialogFile dialog;

    public GameObject onscreenUI;

    private TMPro.TextMeshProUGUI txtmesh;
    public static DialogSystem ins;
    //is there a dialog currently running?
    public static bool activeDialog = false;

    private void Awake()
    {
        if (DialogSystem.ins != null)
        {
            Destroy(DialogSystem.ins);
        }
        else
        {
            DialogSystem.ins = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {
        //need to support streaming assets on desktop mode.
        if (!dialogFile) { Debug.LogError("Dialog System-Need a dialog file!"); }
        XElement dialogDoc = XDocument.Parse(dialogFile.text).Element("DialogFile");
        dialog = new DialogFile(dialogDoc, "");
        txtmesh = onscreenUI.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public static void DialogEvent(string id)
    {
        bool found = dialog.events.TryGetValue(id, out DialogEvent ev);
        if (!found) { Debug.LogError("Dialog System- Event not found"); }
        else if(!activeDialog)
        {
            ins.StartCoroutine("TypeDialog", ev);
        }
    }

    IEnumerator TypeDialog(DialogEvent ev)
    {
        DialogSystem.activeDialog = true;
        for(int i = 0; i< ev.dialogPages.Length;i++)
        {
            txtmesh.text = "";
            string text = ev.dialogPages[i];
            foreach (char c in text)
            {
                txtmesh.text += c;
                yield return new WaitForSeconds(delayBetweenLetters);
            }
            yield return new WaitForSeconds(delayBetweenPages);
        }


        yield return new WaitForSeconds(delayBetweenFadeout);
        txtmesh.text = "";
        DialogSystem.activeDialog = false ;
        yield break;
    }

}
