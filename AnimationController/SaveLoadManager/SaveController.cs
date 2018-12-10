using Studio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace AnimationController
{
    class SaveController : MonoBehaviour
    {
        public void SaveProcedure(string scenePath, XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("ControllerRoot");
            var saveWrapped = WrapStates();
            string data = XML<SaveHolder>.SerializeObject(saveWrapped);
            xmlWriter.WriteRaw(data);
            xmlWriter.WriteEndElement();

            //////////////////////////////////////////////////debug
            Console.WriteLine("-----------SAVING-----------");
            var sav = saveWrapped.Buffer;
            foreach (KeyValuePair<int, SaveState> kvp in sav)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine(kvp.Key);
                Console.WriteLine(kvp.Value.X);
                Console.WriteLine(kvp.Value.Y);
                Console.WriteLine(kvp.Value.CtrlType);
                Console.WriteLine("-----------------------------");
            }
            Console.WriteLine("-----------END---------------");
            File.WriteAllText("out.txt", XML<SaveHolder>.SerializeObject(saveWrapped));
        }
        /// <summary>
        /// Seeks eligible subjects, associates them with dictionary key, adds states into SaveState dictionary, wraps dictionary into SaveHolder
        /// </summary>
        /// <returns></returns>
        private SaveHolder WrapStates()
        {
            var SaveBuffer = new SerializableDictionary<int, SaveState>();
            var SaveHold = new SaveHolder();
            foreach (var kvp in Studio.Studio.Instance.sceneInfo.dicObject)
            {
                switch (kvp.Value)
                {
                    case OICharInfo x:
                        var charInfo = (OICharInfo)kvp.Value;
                        var chars = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIChar>();
                        var character = chars.FirstOrDefault(yx => yx.charInfo.chaFile == charInfo.charFile);
                        if (character == null) continue;
                        SaveCharacter(character, kvp.Key, SaveBuffer);
                        break;
                    case OIItemInfo y:
                        var itemInfo = (OIItemInfo)kvp.Value;
                        var items = Studio.Studio.Instance.dicObjectCtrl.Values.OfType<OCIItem>();
                        var item = items.FirstOrDefault(zz => zz.itemInfo.dicKey == itemInfo.dicKey);
                        if (item == null) continue;
                        SaveItem(item, kvp.Key, SaveBuffer);
                        break;
                }
            }
            SaveHold.Buffer = SaveBuffer;
            return SaveHold;
        }

        /// <param name="id">ID = sceneInfo.dicObject Key</param>
        private void SaveCharacter(OCIChar character, int id, SerializableDictionary<int, SaveState> sDic)
        {
            var control = character.charInfo.gameObject.GetComponent<CharControl>();
            if (control && control.CheckEntry)
            {
                UnityEngine.Debug.Log("Currently saving " + character.treeNodeObject.textName);
                sDic.Add(id, StateAssign(control));
            }
        }
        /// <param name="id">ID = sceneInfo.dicObject Key</param>
        private void SaveItem(OCIItem item, int id, SerializableDictionary<int, SaveState> sDic)
        {
            var control = item.objectItem.gameObject.GetComponent<CharControl>();
            if (control && control.CheckEntry)
            {
                UnityEngine.Debug.Log("Currently saving " + item.treeNodeObject.textName);
                sDic.Add(id, StateAssign(control));
            }
        }
        private SaveState StateAssign(CharControl control)
        {
            SaveState state = new SaveState
            {
                CtrlType = (OCICtrlType)control.controlType, //note to self: enums should match in both CharControl and SaveState
                X = control.CurrentX,
                Y = control.CurrentY
            };
            return state;
        }
    }
}
