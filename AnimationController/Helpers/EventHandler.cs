using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AnimationController
{
    /// <summary>
    /// This class handles UI events.
    /// </summary>
    public class EventHandler : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, ISubmitHandler
    {
        #region delegates events and all sorts of black magicks

        public delegate void TextSubmit(BaseEventData submitEvent = null);
        public event TextSubmit onSubmit;
        public void OnSubmit(BaseEventData submitEvent)
        {
            this.onSubmit?.Invoke(submitEvent);
        }

        public delegate void PointerDrag(PointerEventData dragEvent = null);
        public event PointerDrag onDrag;
        public void OnDrag(PointerEventData dragEvent)
        {
            this.onDrag?.Invoke(dragEvent);
        }

        public delegate void PointerBeginDrag(PointerEventData dragbEvent = null);
        public event PointerBeginDrag onBeginDrag;
        public void OnBeginDrag(PointerEventData dragbEvent)
        {
            this.onBeginDrag?.Invoke(dragbEvent);
        }

        public delegate void PointerEndDrag(PointerEventData drageEvent = null);
        public event PointerEndDrag onEndDrag;
        public void OnEndDrag(PointerEventData drageEvent)
        {
            this.onEndDrag?.Invoke(drageEvent);
        }


        public delegate void PointerDown(PointerEventData downEvent = null);
        public event PointerDown onPointerDown;
        public void OnPointerDown(PointerEventData downEvent)
        {
            this.onPointerDown?.Invoke(downEvent);
        }

        public delegate void PointerUp(PointerEventData upEvent = null);
        public event PointerUp onPointerUp;
        public void OnPointerUp(PointerEventData upEvent)
        {
            this.onPointerUp?.Invoke(upEvent);
        }

        public delegate void PointerExit(PointerEventData exitEvent = null);
        public event PointerExit onPointerExit;
        public void OnPointerExit(PointerEventData exitEvent)
        {
            this.onPointerExit?.Invoke(exitEvent);
        }

        public delegate void PointerEnter(PointerEventData enterEvent = null);
        public event PointerEnter onPointerEnter;
        public void OnPointerEnter(PointerEventData enterEvent)
        {
            this.onPointerEnter?.Invoke(enterEvent);
        }
        #endregion

        /* example tooltip?
        private bool m_tooltipDisplayed = false;
        public RectTransform TooltipItem;
        private Vector3 m_tooltipOffset;

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_tooltipDisplayed = true;
            TooltipItem.transform.position = transform.position + m_tooltipOffset;
            TooltipItem.gameObject.SetActive(m_tooltipDisplayed);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_tooltipDisplayed = false;
            TooltipItem.gameObject.SetActive(m_tooltipDisplayed);
        }

        void Start()
        {
            //Offset the tooltip above the target GameObject
            m_tooltipOffset = new Vector3(0, TooltipItem.sizeDelta.y, 0);
            //Deactivate the tooltip so that it is only shown when you want it to
            TooltipItem.gameObject.SetActive(m_tooltipDisplayed);
        }*/
    }
}
