using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
[Serializable]
public struct SceneEvent
{
    public string id;
    public string eventCode;
    public string eventArgs;
}
public struct Scene
{
    public string id;
    public List<SceneEvent> events;
}
///<summary>
///Direct C# representation of a dialog file
///</summary>
public struct ScenesFile
{
    // Attributes
    public string name;
    public string description;
    public string author;

    public string path;
    public string unityPath;

    public string webglPath;
    public string bgFolder;

    public Dictionary<string, Scene> scenes;

    //2nd constructor for using txt or json?
    public ScenesFile(XElement element, string assetPath)
    {
        // Attributes
        name = (string)element.Attribute("name");
        description = (string)element.Attribute("description");
        author = (string)element.Attribute("author");
        path = assetPath;
        unityPath = assetPath.Replace('\\', '/');
        webglPath = (string)element.Attribute("webglpath");
        bgFolder = (string)element.Attribute("bgFolder");
        // Elements
        scenes = element
            .Elements("sceneList")
            ?.Elements("scene")
            .Select(p => new Scene
            {
                id = (string)p.Attribute("id"),
                events = p.Elements("sceneEvent").Select(p=> new SceneEvent { id= (string)p.Attribute("id"), eventArgs = (string)p.Attribute("eventParameters"), eventCode = (string)p.Attribute("eventCode")  }).ToList(),
            })
            .ToDictionary(o => o.id, o => o);

    }


}