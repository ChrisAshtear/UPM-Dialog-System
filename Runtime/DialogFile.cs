using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
[Serializable]
public struct DialogEvent
{
    public string id;
    public string[] dialogPages;
    public string portrait;
}
///<summary>
///Direct C# representation of a dialog file
///</summary>
public struct DialogFile
{
    // Attributes
    public string name;
    public string description;
    public string author;

    public string path;
    public string unityPath;

    public string webglPath;
    public string portraitFolder;

    public Dictionary<string,DialogEvent> events;

    //2nd constructor for using txt or json?
    public DialogFile(XElement element, string assetPath)
    {
        string last_valid_id = "none";
        string last_valid_line = "none";
        // Attributes
        try
        {
            name = (string)element.Attribute("name");
            description = (string)element.Attribute("description");
            author = (string)element.Attribute("author");
            path = assetPath;
            unityPath = assetPath.Replace('\\', '/');
            webglPath = (string)element.Attribute("webglpath");
            portraitFolder = (string)element.Attribute("portraitFolder");

            
            // Elements
            events = element
                .Elements("dialogList")
                ?.Elements("dialog")
                .Select(p => ParseEvent(p))
                .Distinct()
                .ToDictionary(o => o.id, o => o);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            // Get stack trace for the exception with source file information
            name = null;
            description = null;
            author = null;
            path = null;
            unityPath = null;
            webglPath = null;
            portraitFolder = null;
            events = null;
        }
    }

    private static string[] ParseText(XAttribute text, string id)
    {
        string[] nullDialog = new string[] {$"NO DIALOG for ID {id}"};
        if(text!= null)
        {
            return ((string)text).Split('|');
        }
        else {
            Debug.LogWarning("Dialog: No dialog text found for " + id);
            return nullDialog; 
        }
    }

    public static DialogEvent ParseEvent(XElement a)
    {
        try
        {
            DialogEvent evt = new DialogEvent
            {
                id = (string)a.Attribute("id"),
                portrait = (string)a.Attribute("portrait"),
                dialogPages = ParseText(a.Attribute("text"), (string)a.Attribute("id")),
            };
            return evt;
        }
        catch (Exception e)
        {
            Debug.LogError("Dialog: Cannot parse Line " + a.ToString());
            return new DialogEvent { dialogPages = null, id = "bad", portrait = "none" };
        }
    }


}