using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnimationController
{
    /// <summary>
    /// This class handles GUI operations related to buttons.
    /// </summary>

    class GUIButtons : MonoBehaviour
    {

        public Action<float, float> SliderButtonPressed;

        public void Init(Canvas _ui)
        {
            Button SliderJumperL = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_L", -1, 0);
            Button SliderJumperR = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_R", 1, 0);
            Button SliderJumperT = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_T", 0, 1);
            Button SliderJumperD = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_D", 0, -1);
            Button SliderJumperTR = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_TR", 1, 1);
            Button SliderJumperTL = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_TL", -1, 1);
            Button SliderJumperDL = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_DL", -1, -1);
            Button SliderJumperDR = ConstructSliderButton(_ui, "MainPanel/SliderGroup/Btn_DR", 1, -1);
            Button SliderResetButton = ConstructSliderButton(_ui, "MainPanel/XYGroup/Btn_ResetPos", 0, 0);
        }


        private Button ConstructSliderButton(Canvas ui, string path, float setx, float sety)
        {
            Button button = ui.transform.Find(path).GetComponent<Button>();
            button.onClick.AddListener(() => SliderButtonPressed?.Invoke(setx, sety));
            return button;
        }

        ///???
        //private Button ConstructEventButton(Canvas ui, string path, Action onClick)
        //{
        //    Button button = ui.transform.Find(path).GetComponent<Button>();
        //    button.onClick.AddListener(() => onClick);
        //    return button;
        //}

    }
}
