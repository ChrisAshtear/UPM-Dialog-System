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
        // Attributes
        name = (string)element.Attribute("name");
        description = (string)element.Attribute("description");
        author = (string)element.Attribute("author");
        path = assetPath;
        unityPath = assetPath.Replace('\\', '/');
        webglPath = (string)element.Attribute("webglpath");
        portraitFolder = (string)element.Attribute("portraitfolder");
        // Elements
        events = element
            .Elements("dialogList")
            ?.Elements("dialog")
            .Select(p => new DialogEvent
            {
                id = (string)p.Attribute("id"),
                portrait = (string)p.Attribute("portrait"),
                dialogPages = ((string)p.Attribute("text")).Split('|'),
            })
            .ToDictionary(o => o.id, o => o);

    }


}