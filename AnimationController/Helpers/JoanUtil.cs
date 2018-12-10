using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace AnimationController
{
    /// <summary>
    /// THIS CLASS IS JOANS UIUTIL INSIDE THIS DLL BECAUSE I CANT FIGURE HOW TO HOT RELOAD IT 
    /// </summary>
    public static class JoanUtil
    {
        public static void SetRect(this RectTransform self, Vector2 anchorMin)
        {
            SetRect(self, anchorMin, Vector2.one, Vector2.zero, Vector2.zero);
        }
        public static void SetRect(this RectTransform self, Vector2 anchorMin, Vector2 anchorMax)
        {
            SetRect(self, anchorMin, anchorMax, Vector2.zero, Vector2.zero);
        }
        public static void SetRect(this RectTransform self, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin)
        {
            SetRect(self, anchorMin, anchorMax, offsetMin, Vector2.zero);
        }
        public static void SetRect(this RectTransform self, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            self.anchorMin = anchorMin;
            self.anchorMax = anchorMax;
            self.offsetMin = offsetMin;
            self.offsetMax = offsetMax;
        }

        public static void SetRect(this RectTransform self, RectTransform other)
        {
            self.anchorMin = other.anchorMin;
            self.anchorMax = other.anchorMax;
            self.offsetMin = other.offsetMin;
            self.offsetMax = other.offsetMax;
        }

        public static void SetRect(this RectTransform self, float anchorLeft = 0f, float anchorBottom = 0f, float anchorRight = 1f, float anchorTop = 1f, float offsetLeft = 0f, float offsetBottom = 0f, float offsetRight = 0f, float offsetTop = 0f)
        {
            self.anchorMin = new Vector2(anchorLeft, anchorBottom);
            self.anchorMax = new Vector2(anchorRight, anchorTop);
            self.offsetMin = new Vector2(offsetLeft, offsetBottom);
            self.offsetMax = new Vector2(offsetRight, offsetTop);
        }

        public static void SetRect(this Transform self, Transform other)
        {
            SetRect(self as RectTransform, other as RectTransform);
        }

        public static void SetRect(this Transform self, Vector2 anchorMin)
        {
            SetRect(self as RectTransform, anchorMin, Vector2.one, Vector2.zero, Vector2.zero);
        }
        public static void SetRect(this Transform self, Vector2 anchorMin, Vector2 anchorMax)
        {
            SetRect(self as RectTransform, anchorMin, anchorMax, Vector2.zero, Vector2.zero);
        }
        public static void SetRect(this Transform self, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin)
        {
            SetRect(self as RectTransform, anchorMin, anchorMax, offsetMin, Vector2.zero);
        }
        public static void SetRect(this Transform self, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            RectTransform rt = self as RectTransform;
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }

        public static void SetRect(this Transform self, float anchorLeft = 0f, float anchorBottom = 0f, float anchorRight = 1f, float anchorTop = 1f, float offsetLeft = 0f, float offsetBottom = 0f, float offsetRight = 0f, float offsetTop = 0f)
        {
            RectTransform rt = self as RectTransform;
            rt.anchorMin = new Vector2(anchorLeft, anchorBottom);
            rt.anchorMax = new Vector2(anchorRight, anchorTop);
            rt.offsetMin = new Vector2(offsetLeft, offsetBottom);
            rt.offsetMax = new Vector2(offsetRight, offsetTop);
        }

        public static Button LinkButtonTo(this Transform root, string path, UnityAction onClick)
        {
            Button b = root.Find(path).GetComponent<Button>();
            if (onClick != null)
                b.onClick.AddListener(onClick);
            return b;
        }

        public static Dropdown LinkDropdownTo(this Transform root, string path, UnityAction<int> onValueChanged)
        {
            Dropdown b = root.Find(path).GetComponent<Dropdown>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            return b;

        }

        public static InputField LinkInputFieldTo(this Transform root, string path, UnityAction<string> onValueChanged, UnityAction<string> onEndEdit)
        {
            InputField b = root.Find(path).GetComponent<InputField>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            if (onEndEdit != null)
                b.onEndEdit.AddListener(onEndEdit);
            return b;

        }

        public static ScrollRect LinkScrollViewTo(this Transform root, string path, UnityAction<Vector2> onValueChanged)
        {
            ScrollRect b = root.Find(path).GetComponent<ScrollRect>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            return b;

        }

        public static Scrollbar LinkScrollbarTo(this Transform root, string path, UnityAction<float> onValueChanged)
        {
            Scrollbar b = root.Find(path).GetComponent<Scrollbar>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            return b;

        }

        public static Slider LinkSliderTo(this Transform root, string path, UnityAction<float> onValueChanged)
        {
            Slider b = root.Find(path).GetComponent<Slider>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            return b;

        }

        public static Toggle LinkToggleTo(this Transform root, string path, UnityAction<bool> onValueChanged)
        {
            Toggle b = root.Find(path).GetComponent<Toggle>();
            if (onValueChanged != null)
                b.onValueChanged.AddListener(onValueChanged);
            return b;

        }


    }
}