using Studio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


namespace AnimationController
{
    class SaveLoadManager : MonoBehaviour
    {
        private const string _pluginDir = "Plugins\\test\\";
        private const string _studioSavesDir = "StudioNEOScenes\\";
        //private SerializableDictionary<int, SaveState> LoadBuffer = new SerializableDictionary<int, SaveState>();
        //private bool loadSuccess;

        private SaveController Saver;
        private LoadController Loader;
        private ImportController Importer;


        public void Init()
        {
            SaveInit();
            LoadInit();
            ImportInit();

            HSExtSave.HSExtSave.RegisterHandler("AnimationController", null, null, Loader.OnSceneLoad, null, Saver.SaveProcedure, null, null);

            Importer.ImportFinished += OnImportFinished;
        }

        private void SaveInit()
        {
            Saver = new GameObject("SaveController").AddComponent<SaveController>();
            Saver.transform.parent = gameObject.transform;
        }

        private void LoadInit()
        {
            Loader = new GameObject("SaveController").AddComponent<LoadController>();
            Loader.transform.parent = gameObject.transform;
            Loader.Init();
        }

        private void ImportInit()
        {
            Importer = new GameObject("SaveController").AddComponent<ImportController>();
            Importer.transform.parent = gameObject.transform;
        }

        private void OnDestroy()
        {
            Console.WriteLine("SaveHandler destroyed");
            Importer.ImportFinished -= OnImportFinished;
        }

        private void OnImportFinished(SerializableDictionary<int, SaveState> shuffledStates)
        {
            Loader.DeployStates(shuffledStates);
        }



        #region Load

        //private void OnSceneLoad(string scenePath, XmlNode node)
        //{
        //    Console.WriteLine("OnSceneLoad");
        //    try
        //    {
        //        //scenePath = Path.GetFileNameWithoutExtension(scenePath) + ".sav";
        //        //string dir = _pluginDir + _studioSavesDir;
        //        //string path = dir + scenePath;
        //        //if (File.Exists(path))
        //        //{
        //        //    XmlDocument doc = new XmlDocument();
        //        //    doc.Load(path);
        //        //    node = doc;
        //        //}
        //        //if (node != null)
        //        //    node = node.FirstChild;
        //        //LoadProcedure(node);
        //        node?.FirstChild?.Act(x => LoadProcedure(node.FirstChild));
        //    }
        //    catch
        //    {
        //        Console.WriteLine("gotcha");
        //        loadSuccess = false;
        //    }
        //}

        //private void LoadProcedure(XmlNode node)
        //{
        //    Console.WriteLine("LoadProcedure");
        //    //Console.WriteLine(node.Name);
        //    //var nodes = node.ChildNodes;
        //    //foreach (XmlNode done in nodes)
        //    //{
        //    //    Console.WriteLine(done.Name);
        //    //}



        //    if (node == null || node.Name != "ControllerRoot")
        //    {
        //        Console.WriteLine("Root null");
        //        loadSuccess = false;
        //        return;
        //    }
        //    try
        //    {
        //        var state = node?.FirstChild.OuterXml;
        //        if (state == null || node.FirstChild.Name != "SaveHolder")
        //        {
        //            Console.WriteLine("SaveHolder null");
        //            loadSuccess = false;
        //            return;
        //        }
        //        try
        //        {
        //            SaveHolder holder = new SaveHolder();
        //            holder = XML<SaveHolder>.From(state);

        //            try
        //            {
        //                if (holder.Buffer == null)
        //                {
        //                    Console.WriteLine("holderbuffer null");
        //                    loadSuccess = false;
        //                    return;
        //                }
        //                LoadBuffer = holder.Buffer;
        //                if (LoadBuffer.Count == 0 || LoadBuffer == null)
        //                {
        //                    loadSuccess = false;
        //                }
        //                else
        //                {
        //                    loadSuccess = true;
        //                }
        //            }
        //            catch
        //            {
        //                Console.WriteLine("layer3");
        //            }
        //        }
        //        catch
        //        {
        //            Console.WriteLine("layer2");
        //        }

        //    }
        //    catch
        //    {
        //        Console.WriteLine("layer1");
        //    }

        //    //Console.WriteLine("-----------LOADING-----------");
        //    //foreach (KeyValuePair<int, SaveState> kvp in LoadBuffer)
        //    //{
        //    //    Console.WriteLine("-----------------------------");
        //    //    Console.WriteLine(kvp.Key);
        //    //    Console.WriteLine(kvp.Value.X);
        //    //    Console.WriteLine(kvp.Value.Y);
        //    //    Console.WriteLine(kvp.Value.CtrlType);
        //    //    Console.WriteLine("-----------------------------");
        //    //}
        //    //Console.WriteLine("-----------END---------------");
        //    //File.WriteAllText("out2.txt", state);
        //    Console.WriteLine(loadSuccess);
        //}

        //private void OnLoadFinished()
        //{
        //    if(loadSuccess)
        //    {
        //        DeployStates(LoadBuffer);
        //    }
        //    Console.WriteLine("Clearing buffer");
        //    LoadBuffer.Clear();
        //}
        //private void DeployStates(SerializableDictionary<int, SaveState> buffer)
        //{
        //    Console.WriteLine("Deploy states");
        //    foreach (var kvp in Studio.Studio.Instance.sceneInfo.dicObject)
        //    {
        //        if (buffer.ContainsKey(kvp.Key))
        //        {
        //            Console.Write("Found");
        //            switch (kvp.Value)
        //            {
        //                case OICharInfo x:
        //                    var charInfo = (OICharInfo)kvp.Value;
        //                    var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
        //                    var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == charInfo.charFile);
        //                    if (character == null) continue;
        //                    Console.Write(" Character");
        //                    LoadCharacter(character, kvp.Key, buffer);
        //                    break;
        //                case OIItemInfo y:
        //                    var itemInfo = (OIItemInfo)kvp.Value;
        //                    var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
        //                    var item = items.FirstOrDefault(zz => zz.itemInfo.dicKey == itemInfo.dicKey);
        //                    if (item == null) continue;
        //                    Console.Write(" Item");
        //                    LoadItem(item, kvp.Key, buffer);
        //                    break;
        //            }
        //        }
        //    }
        //}
        //private void LoadCharacter(OCIChar character, int id, SerializableDictionary<int, SaveState> kvp)
        //{
        //    var control = character.charInfo.gameObject.GetOrAddComponent<CharControl>();
        //    control.InitChar(character);
        //    if (control && control.CheckEntry)
        //    {
        //        Console.Write(" - Eligible, deploying on {0}", character.treeNodeObject.textName);

        //        kvp.TryGetValue(id, out SaveState value);
        //        control.CurrentX = value.X;
        //        control.CurrentY = value.Y;
        //    }
        //    else
        //    {
        //        Console.WriteLine(" - Incorrect");
        //    }
            
        //}
        //private void LoadItem(OCIItem item, int id, SerializableDictionary<int, SaveState> kvp)
        //{
        //    var control = item.objectItem.gameObject.GetOrAddComponent<CharControl>();
        //    control.InitItem(item);
        //    if (control && control.CheckEntry)
        //    {
        //        Console.Write(" - Eligible, deploying on {0}", item.treeNodeObject.textName);
        //        kvp.TryGetValue(id, out SaveState value);
        //        control.CurrentX = value.X;
        //        control.CurrentY = value.Y;
        //    }
        //    else
        //    {
        //        Console.WriteLine(" - Incorrect");
        //    }
            
        //}
#endregion
        #region Save
        //private void SaveProcedure(string scenePath, XmlTextWriter xmlWriter)
        //{
        //    xmlWriter.WriteStartElement("ControllerRoot");
        //    var saveWrapped = WrapStates();
        //    string data = XML<SaveHolder>.SerializeObject(saveWrapped);
        //    xmlWriter.WriteRaw(data);
        //    xmlWriter.WriteEndElement();

        //    //////////////////////////////////////////////////debug
        //    Console.WriteLine("-----------SAVING-----------");
        //    var sav = saveWrapped.Buffer;
        //    foreach (KeyValuePair<int, SaveState> kvp in sav)
        //    {
        //        Console.WriteLine("-----------------------------");
        //        Console.WriteLine(kvp.Key);
        //        Console.WriteLine(kvp.Value.X);
        //        Console.WriteLine(kvp.Value.Y);
        //        Console.WriteLine(kvp.Value.CtrlType);
        //        Console.WriteLine("-----------------------------");
        //    }
        //    Console.WriteLine("-----------END---------------");
        //    File.WriteAllText("out.txt", XML<SaveHolder>.SerializeObject(saveWrapped));
        //}
        ///// <summary>
        ///// Seeks eligible subjects, associates them with dictionary key, adds states into SaveState dictionary, wraps dictionary into SaveHolder
        ///// </summary>
        ///// <returns></returns>
        //private SaveHolder WrapStates()
        //{
        //    var SaveBuffer = new SerializableDictionary<int, SaveState>();
        //    var SaveHold = new SaveHolder();
        //    foreach (var kvp in Studio.Studio.Instance.sceneInfo.dicObject)
        //    {
        //        switch (kvp.Value)
        //        {
        //            case OICharInfo x:
        //                var charInfo = (OICharInfo)kvp.Value;
        //                var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
        //                var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == charInfo.charFile);
        //                if (character == null) continue;
        //                SaveCharacter(character, kvp.Key, SaveBuffer);
        //                break;
        //            case OIItemInfo y:
        //                var itemInfo = (OIItemInfo)kvp.Value;
        //                var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
        //                var item = items.FirstOrDefault(zz => zz.itemInfo.dicKey == itemInfo.dicKey);
        //                if (item == null) continue;
        //                SaveItem(item, kvp.Key, SaveBuffer);
        //                break;
        //        }
        //    }
        //    SaveHold.Buffer = SaveBuffer;
        //    return SaveHold;
        //}

        ///// <param name="id">ID = sceneInfo.dicObject Key</param>
        //private void SaveCharacter(OCIChar character, int id, SerializableDictionary<int, SaveState> sDic)
        //{
        //    var control = character.charInfo.gameObject.GetComponent<CharControl>();
        //    if (control && control.CheckEntry)
        //    {
        //        UnityEngine.Debug.Log("Currently saving " + character.treeNodeObject.textName);
        //        sDic.Add(id, SetState(OCICtrlType.Character, control.CurrentX, control.CurrentY));
        //    }
        //}
        ///// <param name="id">ID = sceneInfo.dicObject Key</param>
        //private void SaveItem(OCIItem item, int id, SerializableDictionary<int, SaveState> sDic)
        //{
        //    var control = item.objectItem.gameObject.GetComponent<CharControl>();
        //    if (control && control.CheckEntry)
        //    {
        //        UnityEngine.Debug.Log("Currently saving " + item.treeNodeObject.textName);
        //        sDic.Add(id, SetState(OCICtrlType.Object, control.CurrentX, control.CurrentY));
        //    }
        //}
        //private SaveState SetState(OCICtrlType type, float savedX, float savedY)
        //{
        //    SaveState state = new SaveState
        //    {
        //        CtrlType = type,
        //        X = savedX,
        //        Y = savedY
        //    };
        //    return state;
        //}
        #endregion

        //private void OnSceneImport(string scenePath, XmlNode node)
        //{
        //    scenePath = Path.GetFileNameWithoutExtension(scenePath) + ".sav";
        //    string dir = _pluginDir + _studioSavesDir;
        //    string path = dir + scenePath;
        //    if (File.Exists(path))
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path);
        //        node = doc;
        //    }
        //    if (node != null)
        //        node = node.FirstChild;
        //    int max = -1;
        //    foreach (KeyValuePair<int, ObjectCtrlInfo> pair in Studio.Studio.Instance.dicObjectCtrl)
        //    {
        //        if (pair.Key > max)
        //            max = pair.Key;
        //    }
        //    LoadDefaultVersion(node, max);
        //}

        //private void OnSceneSave(string scenePath, XmlTextWriter xmlWriter)
        //{
        //    int written = 0;
        //    xmlWriter.WriteStartElement("root");
        //    //xmlWriter.WriteAttributeString("version", HSPE.versionNum);
        //    SortedDictionary<int, ObjectCtrlInfo> dic = new SortedDictionary<int, ObjectCtrlInfo>(Studio.Studio.Instance.dicObjectCtrl);
        //    try
        //    {
        //        foreach (KeyValuePair<int, ObjectCtrlInfo> kvp in dic)
        //        {
        //            if (kvp.Value is OCIItem)
        //            {
        //                OCIItem ociItem = kvp.Value as OCIItem;
        //                if (ociItem != null)
        //                {
        //                    CharControl itemcontrol = ociItem.objectItem.gameObject.GetComponent<CharControl>();
        //                    if (itemcontrol)
        //                    {
        //                        UnityEngine.Debug.Log("Currently saving " + ociItem.treeNodeObject.textName);
        //                        xmlWriter.WriteStartElement("itemInfo");
        //                        xmlWriter.WriteAttributeString("name", ociItem.treeNodeObject.textName);
        //                        xmlWriter.WriteAttributeString("index", XmlConvert.ToString(kvp.Key));
        //                        //if (controller)
        //                        // {
        //                        written += SaveItem(ociItem, xmlWriter);
        //                        // }
        //                        xmlWriter.WriteEndElement();
        //                    }
        //                }
        //            }
        //            else if (kvp.Value is OCIChar)
        //            {
        //                OCIChar ociChar = kvp.Value as OCIChar;
        //                if (ociChar != null)
        //                {
        //                    CharControl charcontrol = ociChar.charInfo.gameObject.GetComponent<CharControl>();
        //                    if (charcontrol && charcontrol.CheckEntry) //we don't want to save irrelevant stuff
        //                    {
        //                        UnityEngine.Debug.Log("Currently saving " + ociChar.treeNodeObject.textName);
        //                        xmlWriter.WriteStartElement("characterInfo");
        //                        xmlWriter.WriteAttributeString("name", ociChar.treeNodeObject.textName);
        //                        xmlWriter.WriteAttributeString("index", XmlConvert.ToString(kvp.Key));

        //                        written += SaveChar(ociChar, xmlWriter);

        //                        xmlWriter.WriteEndElement();
        //                    }
        //                }
        //            }
        //        }
        //        xmlWriter.WriteEndElement();
        //    }
        //    catch (Exception e) { UnityEngine.Debug.Log("1" + e); }

        //}
        //private void LoadDefaultVersion(XmlNode node, int lastIndex = -1) //actual loading happens here
        //{
        //    if (node == null || node.Name != "root")
        //        return;
        //    //string v = node.Attributes["version"].Value;
        //    node = node.CloneNode(true);
        //    this.ExecuteDelayed(() =>
        //    {
        //        List<KeyValuePair<int, ObjectCtrlInfo>> dic = new SortedDictionary<int, ObjectCtrlInfo>(Studio.Studio.Instance.dicObjectCtrl).Where(p => p.Key > lastIndex).ToList();
        //        int i = 0;
        //        foreach (XmlNode childNode in node.ChildNodes)
        //        {
        //            switch (childNode.Name)
        //            {
        //                case "itemInfo":
        //                    OCIItem ociItem = null;
        //                    while (i < dic.Count && (ociItem = dic[i].Value as OCIItem) == null)
        //                        ++i;
        //                    if (i == dic.Count)
        //                        break;
        //                    if (ociItem.isAnime && ociItem.animator.parameterCount == 4)
        //                    {
        //                        LoadItem(ociItem, childNode);
        //                    }
        //                    ++i;
        //                    break;

        //                case "characterInfo":
        //                    OCIChar ociChar = null;
        //                    while (i < dic.Count && (ociChar = dic[i].Value as OCIChar) == null)
        //                        ++i;
        //                    if (i == dic.Count)
        //                        break;
        //                    LoadChar(ociChar, childNode);
        //                    ++i;
        //                    break;
        //            }
        //        }
        //    });
        //}
        ////itemload part
        //private void LoadItem(OCIItem ociItem, XmlNode node)
        //{
        //    CharControl controller = ociItem.objectItem.gameObject.GetComponent<CharControl>();
        //    if (controller == null)
        //        controller = ociItem.objectItem.gameObject.AddComponent<CharControl>();
        //    controller.ScheduleLoad(node);
        //}
        //private int SaveItem(OCIItem ociItem, XmlTextWriter xmlWriter)
        //{
        //    return ociItem.objectItem.gameObject.GetComponent<CharControl>().SaveXml(xmlWriter);
        //}
        ////charload part
        //private void LoadChar(OCIChar ociChar, XmlNode node)
        //{
        //    CharControl controller = ociChar.charInfo.gameObject.GetComponent<CharControl>();
        //    if (controller == null)
        //        controller = ociChar.charInfo.gameObject.AddComponent<CharControl>();
        //    controller.ScheduleLoad(node);
        //}
        //private int SaveChar(OCIChar ociChar, XmlTextWriter xmlWriter)
        //{
        //    return ociChar.charInfo.gameObject.GetComponent<CharControl>().SaveXml(xmlWriter);
        //}


        //private void CaseChar(int id, XmlNode node)
        //{
        //    var query = Studio.Studio.Instance.sceneInfo.dicObject.Where(x => x.Key == id). 
        //        Select(x => x.Value as OICharInfo).Where(x => x != null).FirstOrDefault();
        //    if (query == null) Console.WriteLine("query null");
        //    var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
        //    if (chars == null) Console.WriteLine("chars null");
        //    var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == query.charFile);
        //    if (character == null) Console.WriteLine("character null");
        //    var control = character.charInfo.gameObject.GetComponent<CharControl>();
        //    if (control == null) Console.WriteLine("control null");
        //    Console.WriteLine(control);
        //    if (control == null)
        //        control = character.charInfo.gameObject.AddComponent<CharControl>();

        //    control.ScheduleLoad(node);
        //}

        //private void CaseItem(int id, XmlNode node)
        //{
        //    var query = Studio.Studio.Instance.sceneInfo.dicObject.Where(x => x.Key == id).
        //                Select(x => x.Value as OIItemInfo).Where(x => x != null);
        //    if (query == null) Console.WriteLine("query null");
        //    var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
        //    if (items == null) Console.WriteLine("items null");
        //    Console.WriteLine("OITemInfo in query:");
        //    foreach (OIItemInfo no in query)
        //    {
        //        Console.WriteLine(no.dicKey);
        //    }
        //    Console.WriteLine("end____________________________________");
        //    Console.WriteLine("OITemInfo in items:");
        //    foreach (OCIItem no in items)
        //    {
        //        Console.WriteLine(no.itemInfo.dicKey);
        //    }
        //    Console.WriteLine("end____________________________________");
        //    //var item = items.FirstOrDefault(zz => zz.itemInfo.no == query.no);

        //    //if (item == null) Console.WriteLine("item null");
        //    //var control = item.objectItem.gameObject.GetComponent<CharControl>();
        //    //if (control == null) Console.WriteLine("control null");
        //    //if (control == null)
        //    //    control = item.objectItem.gameObject.AddComponent<CharControl>();

        //    //control.ScheduleLoad(node);
        //}

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Backspace))
        //    {
        //        //    var query = Studio.Studio.Instance.sceneInfo.dicObject.
        //        //Select(x => x.Value as OIItemInfo).Where(x => x != null);
        //        //    if (query == null) Console.WriteLine("query null");
        //        //    var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
        //        //    if (items == null) Console.WriteLine("items null");
        //        //    Console.WriteLine("OITemInfo in query:");
        //        //    foreach (OIItemInfo no in query)
        //        //    {
        //        //        Console.WriteLine(no.dicKey);
        //        //    }
        //        //    Console.WriteLine("end____________________________________");
        //        //    Console.WriteLine("OITemInfo in items:");
        //        //    foreach (OCIItem no in items)
        //        //    {
        //        //        Console.WriteLine(no.itemInfo.dicKey);
        //        //    }
        //        //    Console.WriteLine("end____________________________________");

        //    }
        //}

        //private void LoadProcedure(XmlNode node)
        //{
        //    if (node == null || node.Name != "root")
        //        return;
        //    //node = node.CloneNode(true);
        //    foreach (XmlNode childNode in node.ChildNodes)
        //    {
        //        int id = 0;
        //        switch (childNode.Name)
        //        {
        //            case "itemInfo":
        //                if (!int.TryParse(childNode.Attributes["ID"].Value, out id))
        //                    break;
        //                CaseItem(id, childNode);
        //                break;

        //            case "characterInfo":
        //                if (!int.TryParse(childNode.Attributes["ID"].Value, out id))
        //                    break;
        //                CaseChar(id, childNode);
        //                break;
        //        }
        //    }
        //}

        //    private void SaveProcedure(string scenePath, XmlTextWriter xmlWriter)
        //    {
        //        written = 0;
        //        xmlWriter.WriteStartElement("root");
        //        foreach (var kvp in Studio.Studio.Instance.sceneInfo.dicObject)
        //        {
        //            int id = kvp.Key;
        //            switch (kvp.Value)
        //            {
        //                case OICharInfo x:
        //                    var charInfo = (OICharInfo)kvp.Value;
        //                    var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
        //                    var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == charInfo.charFile);
        //                    if (character == null) continue;
        //                    SaveCharacter(character, id, xmlWriter);
        //                    break;
        //                case OIItemInfo y:
        //                    var itemInfo = (OIItemInfo)kvp.Value;
        //                    var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
        //                    var item = items.FirstOrDefault(zz => zz.itemInfo.no == itemInfo.no);
        //                    if (item == null) continue;
        //                    SaveItem(item, id, xmlWriter);
        //                    break;
        //            }
        //        }
        //        xmlWriter.WriteEndElement();
        //    }

        //    private void SaveCharacter(OCIChar character, int id, XmlTextWriter xmlWriter)
        //    {
        //        var control = character.charInfo.gameObject.GetComponent<CharControl>();
        //        if (control && control.CheckEntry)
        //        {
        //            UnityEngine.Debug.Log("Currently saving " + character.treeNodeObject.textName);
        //            xmlWriter.WriteStartElement("characterInfo");
        //            xmlWriter.WriteAttributeString("name", character.treeNodeObject.textName);
        //            xmlWriter.WriteAttributeString("ID", XmlConvert.ToString(id));
        //            written += control.SaveXml(xmlWriter);

        //            xmlWriter.WriteEndElement();

        //        }
        //    }
        //    private void SaveItem(OCIItem item, int id, XmlTextWriter xmlWriter)
        //    {
        //        var control = item.objectItem.gameObject.GetComponent<CharControl>();
        //        if (control && control.CheckEntry)
        //        {
        //            UnityEngine.Debug.Log("Currently saving " + item.treeNodeObject.textName);
        //            xmlWriter.WriteStartElement("itemInfo");
        //            xmlWriter.WriteAttributeString("name", item.treeNodeObject.textName);
        //            xmlWriter.WriteAttributeString("ID", XmlConvert.ToString(id));
        //            written += control.SaveXml(xmlWriter);
        //            xmlWriter.WriteEndElement();
        //        }
        //    }

    }
}
