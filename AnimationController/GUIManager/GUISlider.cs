using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AnimationController
{
    /// <summary>
    /// This class handles GUI operations related to 2D slider.
    /// </summary>
    class GUISlider : MonoBehaviour
    {
        private RectTransform Slider2DPanel;
        private Vector2 sliderval;
        private float DistanceX;
        private float DistanceY;
        private float DistanceXCorrected;
        private float DistanceYCorrected;
        private RectTransform rt;

        EventHandler SliderPanelHandler;
        public Action<float, float> SliderMoving;

        public void Init(Canvas _ui)
        {
            //huge thanks to Essu who wrote this code
            rt = _ui.transform.Find("MainPanel/SliderGroup/SliderField/mainMask/SliderHandle").GetComponent<RectTransform>();
            Slider2DPanel = (RectTransform)_ui.transform.Find("MainPanel/SliderGroup/SliderField");

            DistanceY = (Slider2DPanel.rect.width) / 2;
            DistanceX = (Slider2DPanel.rect.height) / 2;
            DistanceXCorrected = DistanceX - rt.sizeDelta.x / 2f;
            DistanceYCorrected = DistanceY - rt.sizeDelta.y / 2f;
            SliderPanelHandler = Slider2DPanel.gameObject.AddComponent<EventHandler>();
            SliderPanelHandler.onPointerDown += SliderHandler; 
            SliderPanelHandler.onDrag += SliderHandler;
            Logger.Create(GetType());
        }
        void OnDestroy()
        {
            SliderPanelHandler.onPointerDown -= SliderHandler;
            SliderPanelHandler.onDrag -= SliderHandler;
            Logger.Destroy(GetType());
        }

        #region Slider logic
        /// <summary>
        /// Inversed operation, performs purely to update 2D slider handle position 
        /// </summary>
        public void SliderHandleMove(float x, float y)
        {
            var lp = rt.localPosition;
            lp.x = x * DistanceXCorrected;
            lp.y = y * DistanceYCorrected;
            rt.localPosition = lp;
        }
        private void SliderHandler(PointerEventData data)
        {
            var lp = rt.localPosition;
            var trueDelta = Input.mousePosition - rt.position;
            rt.transform.Translate(trueDelta);
            lp.x = Mathf.Clamp(rt.localPosition.x, -DistanceXCorrected, DistanceXCorrected);
            lp.y = Mathf.Clamp(rt.localPosition.y, -DistanceYCorrected, DistanceYCorrected);
            sliderval.x = lp.x / DistanceXCorrected;
            sliderval.y = lp.y / DistanceYCorrected;
            rt.localPosition = lp;
            SliderMoving?.Invoke(sliderval.x, sliderval.y);
            UnityEngine.Debug.Log("Slider position : " + sliderval.x + ", Y: " + sliderval.y);
        }
        #endregion
    }
}
