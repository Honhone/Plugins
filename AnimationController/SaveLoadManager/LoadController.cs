using Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace AnimationController
{
    class LoadController : MonoBehaviour
    {
        private SerializableDictionary<int, SaveState> LoadBuffer = new SerializableDictionary<int, SaveState>();
        private bool loadSuccess;

        public void Init()
        {
            HarmonyManager.SceneLoadHappened += OnLoadFinished;
        }
        private void OnDestroy()
        {
            HarmonyManager.SceneLoadHappened -= OnLoadFinished;
        }

        public void OnSceneLoad(string scenePath, XmlNode node)
        {
            Console.WriteLine("OnSceneLoad");
            try
            {
                //scenePath = Path.GetFileNameWithoutExtension(scenePath) + ".sav";
                //string dir = _pluginDir + _studioSavesDir;
                //string path = dir + scenePath;
                //if (File.Exists(path))
                //{
                //    XmlDocument doc = new XmlDocument();
                //    doc.Load(path);
                //    node = doc;
                //}
                //if (node != null)
                //    node = node.FirstChild;
                //LoadProcedure(node);
                node?.FirstChild?.Act(x => LoadProcedure(node.FirstChild));
            }
            catch
            {
                Console.WriteLine("gotcha");
                loadSuccess = false;
            }
        }

        private void LoadProcedure(XmlNode node)
        {
            Console.WriteLine("LoadProcedure");
            //Console.WriteLine(node.Name);
            //var nodes = node.ChildNodes;
            //foreach (XmlNode done in nodes)
            //{
            //    Console.WriteLine(done.Name);
            //}



            if (node == null || node.Name != "ControllerRoot")
            {
                Console.WriteLine("Root null");
                loadSuccess = false;
                return;
            }
            try
            {
                var state = node?.FirstChild.OuterXml;
                if (state == null || node.FirstChild.Name != "SaveHolder")
                {
                    Console.WriteLine("SaveHolder null");
                    loadSuccess = false;
                    return;
                }
                try
                {
                    SaveHolder holder = new SaveHolder();
                    holder = XML<SaveHolder>.From(state);

                    try
                    {
                        if (holder.Buffer == null)
                        {
                            Console.WriteLine("holderbuffer null");
                            loadSuccess = false;
                            return;
                        }
                        LoadBuffer = holder.Buffer;
                        if (LoadBuffer.Count == 0 || LoadBuffer == null)
                        {
                            loadSuccess = false;
                        }
                        else
                        {
                            loadSuccess = true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("layer3");
                    }
                }
                catch
                {
                    Console.WriteLine("layer2");
                }

            }
            catch
            {
                Console.WriteLine("layer1");
            }

            //Console.WriteLine("-----------LOADING-----------");
            //foreach (KeyValuePair<int, SaveState> kvp in LoadBuffer)
            //{
            //    Console.WriteLine("-----------------------------");
            //    Console.WriteLine(kvp.Key);
            //    Console.WriteLine(kvp.Value.X);
            //    Console.WriteLine(kvp.Value.Y);
            //    Console.WriteLine(kvp.Value.CtrlType);
            //    Console.WriteLine("-----------------------------");
            //}
            //Console.WriteLine("-----------END---------------");
            //File.WriteAllText("out2.txt", state);
            Console.WriteLine(loadSuccess);
        }

        private void OnLoadFinished()
        {
            if (loadSuccess)
            {
                DeployStates(LoadBuffer);
            }
            Console.WriteLine("Clearing buffer");
            LoadBuffer.Clear();
        }
        public void DeployStates(SerializableDictionary<int, SaveState> buffer)
        {
            Console.WriteLine("Deploy states");
            foreach (var kvp in Studio.Studio.Instance.sceneInfo.dicObject)
            {
                if (buffer.ContainsKey(kvp.Key))
                {
                    Console.Write("Found");
                    switch (kvp.Value)
                    {
                        case OICharInfo x:
                            var charInfo = (OICharInfo)kvp.Value;
                            var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
                            var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == charInfo.charFile);
                            if (character == null) continue;
                            Console.Write(" Character");
                            LoadCharacter(character, kvp.Key, buffer);
                            break;
                        case OIItemInfo y:
                            var itemInfo = (OIItemInfo)kvp.Value;
                            var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
                            var item = items.FirstOrDefault(zz => zz.itemInfo.dicKey == itemInfo.dicKey);
                            if (item == null) continue;
                            Console.Write(" Item");
                            LoadItem(item, kvp.Key, buffer);
                            break;
                    }
                }
            }
        }
        private void LoadCharacter(OCIChar character, int id, SerializableDictionary<int, SaveState> kvp)
        {
            var control = character.charInfo.gameObject.GetOrAddComponent<CharControl>();
            control.InitChar(character);
            if (control && control.CheckEntry)
            {
                Console.Write(" - Eligible, deploying on {0}", character.treeNodeObject.textName);

                kvp.TryGetValue(id, out SaveState value);
                control.CurrentX = value.X;
                control.CurrentY = value.Y;
            }
            else
            {
                Console.WriteLine(" - Incorrect");
            }

        }
        private void LoadItem(OCIItem item, int id, SerializableDictionary<int, SaveState> kvp)
        {
            var control = item.objectItem.gameObject.GetOrAddComponent<CharControl>();
            control.InitItem(item);
            if (control && control.CheckEntry)
            {
                Console.Write(" - Eligible, deploying on {0}", item.treeNodeObject.textName);
                kvp.TryGetValue(id, out SaveState value);
                control.CurrentX = value.X;
                control.CurrentY = value.Y;
            }
            else
            {
                Console.WriteLine(" - Incorrect");
            }

        }

    }
}
