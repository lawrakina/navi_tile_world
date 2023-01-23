using System;
using System.Collections.Generic;
using EventSystem.Runtime.Core.Managers;
using UnityEngine;
using UnityEngine.EventSystems;


namespace NavySpade.pj77.Tutorial
{
    public class HideAfterClick : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}