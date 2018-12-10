using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AnimationController
{
    /// <summary>
    /// This class handles GUI operations and creates components that implement various parts of GUI.
    /// </summary>
    class GUIManager : MonoBehaviour
    {
        #region Variables
        private Canvas _ui;

        //Components
        private GUISlider uiSlider;
        private GUIButtons uIButtons;
        private GUIFields uiFields;
        #endregion

        #region Init + MonoBehaviour methods
  
        public void Init()
        {
            BootStrap();
            CanvasInit();
            SliderInit();
            ButtonInit();
            InputInit();
            _ui.gameObject.SetActive(false);

            HarmonySelect.Instance.SelectChanged += OnChange;
            HarmonySelect.Instance.SelectNone += OnNothingSelected;
            HarmonySelect.Instance.SelectSomething += OnNothingChanged;
            HarmonyManager.AnimLoadHappened += OnAnimLoadHappened;

            Logger.Create(GetType());
        }
        private void OnDestroy()
        {
            HarmonySelect.Instance.SelectChanged -= OnChange;
            HarmonySelect.Instance.SelectNone -= OnNothingSelected;
            HarmonySelect.Instance.SelectSomething -= OnNothingChanged;
            HarmonyManager.AnimLoadHappened -= OnAnimLoadHappened;


            uiSlider.SliderMoving -= MassMove;
            uIButtons.SliderButtonPressed -= InvertMassMove;
            uiFields.SliderYInput -= MassSetY;
            uiFields.SliderXInput -= MassSetX;
            Logger.Destroy(GetType());
        }

        private void BootStrap() //purge unwanted objects
        {
            var gameobject1 = GameObject.Find("StudioScene/Canvas Main Menu/03_00_Anime Control/ControllerCanvas(Clone)");
            if (gameobject1 != null) DestroyImmediate(gameobject1);
            var gameobject2 = GameObject.Find("ControllerCanvas");
            if (gameobject2 != null) DestroyImmediate(gameobject2);
        }
        #endregion

        #region Component initialization
        private void CanvasInit()
        {
            AssetBundle uibundle = AssetBundle.LoadFromMemory(Properties.Resources.testui);
            _ui = Instantiate(uibundle.LoadAsset<GameObject>("ControllerCanvas")).GetComponent<Canvas>();
            uibundle.Unload(false);
            RectTransform uiTransform = GameObject.Find("StudioScene").transform.Find("Canvas Main Menu/03_00_Anime Control/").GetComponent<RectTransform>();
            _ui.transform.SetParent(uiTransform, false);
            _ui.transform.localScale = uiTransform.localScale;
            _ui.transform.SetRect(Vector2.zero, Vector2.zero, new Vector2(150f, 0f));
        }
        private void SliderInit()
        {
            uiSlider = new GameObject("SliderController").AddComponent<GUISlider>();
            uiSlider.transform.parent = gameObject.transform;

            uiSlider.Init(_ui);
            uiSlider.SliderMoving += MassMove;
        }

        private void ButtonInit()
        {
            uIButtons = new GameObject("ButtonsController").AddComponent<GUIButtons>();
            uIButtons.transform.parent = gameObject.transform;

            uIButtons.Init(_ui);
            uIButtons.SliderButtonPressed += InvertMassMove;
        }
        private void InputInit()
        {
            uiFields = new GameObject("InputFieldController").AddComponent<GUIFields>();
            uiFields.transform.parent = gameObject.transform;

            uiFields.Init(_ui);
            uiFields.SliderYInput += MassSetY;
            uiFields.SliderXInput += MassSetX;
        }
        private void OptionInit()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Link events
        //linked to HarmonySelect
        private void OnChange(CharControl select)
        {
            GUICheck(select);
        }
        private void OnNothingChanged(CharControl select)
        {
            GUICheck(select);
        }
        private void OnNothingSelected()
        {
            _ui.gameObject.SetActive(false);
        }
        //linked to AnimLoad
        private void OnAnimLoadHappened()
        {
            var select = HarmonySelect.Instance.SelectTarget;
            GUICheck(select);
        }
        #endregion

        #region GUI logic
        private void GUICheck(CharControl control)
        {
            if (control.CheckEntry)
            {
                _ui.gameObject.SetActive(true);
                control.SetAccess(true);
                GUIValuesUpdate(control.CurrentX, control.CurrentY);
            }
            else
            {
                control.SetAccess(false);
                _ui.gameObject.SetActive(false);
            }
        }
        private void GUIValuesUpdate(float x, float y)
        {
            uiSlider.SliderHandleMove(x, y);
            uiFields.SetFieldVisualText(x, y);
            Logger.Call(GetType(), "GuiUpdate");
        }
        #endregion

        #region Movement events
        //linked to GUISlider
        private void MassMove(float x, float y) 
        {
            foreach (CharControl charControl in HarmonySelect.Instance.SelectTargetList)
            {
                charControl.TreeMove(x, y);
            }
            uiFields.SetFieldVisualText(x, y);
            Logger.Call(GetType(), "MassMove");
        }

        //linked to buttons, moves slider rectangle instead
        private void InvertMassMove(float x, float y) 
        {
            foreach (CharControl charControl in HarmonySelect.Instance.SelectTargetList)
            {
                charControl.TreeMove(x, y);
            }
            GUIValuesUpdate(x, y);
            Logger.Call(GetType(), "InvertMassMove");
        }

        //linked to input fields
        private void MassSetY(float y) 
        {
            foreach (CharControl charControl in HarmonySelect.Instance.SelectTargetList)
            {
                charControl.CurrentY = y;
            }
            GUIValuesUpdate(HarmonySelect.Instance.SelectTarget.CurrentX, y);
            Logger.Call(GetType(), "MassSetY");
        }
        private void MassSetX(float x)
        {
            foreach (CharControl charControl in HarmonySelect.Instance.SelectTargetList)
            {
                charControl.CurrentX = x;
            }
            GUIValuesUpdate(x, HarmonySelect.Instance.SelectTarget.CurrentY);
            Logger.Call(GetType(), "MassSetX");
        }
        #endregion

        #region Option events
        //placeholder
        //cute gui skin with cats and hearts TBA
        #endregion

        #region Wander events
        //placeholder
        #endregion

        #region Playback events
        //placeholder
        #endregion
    }
}
