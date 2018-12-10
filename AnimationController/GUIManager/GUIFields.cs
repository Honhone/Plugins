using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AnimationController
{
    /// <summary>
    /// This class handles GUI operations related to input fields.
    /// </summary>
    class GUIFields : MonoBehaviour
    {
        private InputField xInput;
        private InputField yInput;
        public Action<float> SliderYInput;
        public Action<float> SliderXInput;

        public void Init(Canvas _ui)
        {
            xInput = _ui.transform.LinkInputFieldTo("MainPanel/XYGroup/XInputField", null, XChange);
            yInput = _ui.transform.LinkInputFieldTo("MainPanel/XYGroup/YInputField", null, YChange);
            Logger.Create(GetType());
        }

        void OnDestroy()
        {
            Logger.Destroy(GetType());
        }
        #region Input methods
        private void XChange(string s)
        {
            if (float.TryParse(xInput.text, out float value))
            {
                if (value > 1)
                    value = 1;
                if (value < -1)
                    value = -1;                
                SliderXInput?.Invoke(value);
                Logger.Call(GetType(), "SliderXInput");
            }
        }
        private void YChange(string s)
        {
            if (float.TryParse(yInput.text, out float value))
            {
                if (value > 1)
                    value = 1;
                if (value < -1)
                    value = -1;
                SliderYInput?.Invoke(value);
                Logger.Call(GetType(), "SliderYInput");
            }
        }
        /// <summary>
        /// Sets the text fields values according to the arguments passed
        /// </summary>
        public void SetFieldVisualText(float x, float y)
        {
            xInput.text = x.ToString("0.0000");
            yInput.text = y.ToString("0.0000");
        }
        #endregion
    }
}
