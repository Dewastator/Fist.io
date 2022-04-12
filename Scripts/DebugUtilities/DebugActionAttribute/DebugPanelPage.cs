using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilities
{
    public class DebugPanelPage : MonoBehaviour
    {
        public Dictionary<string, DebugPanelPage> Pages = new Dictionary<string, DebugPanelPage>();
        private Action _back;

        public void Initialize(Action back)
        {
            _back = back;
            Pages = new Dictionary<string, DebugPanelPage>();
        }

        public void Back()
        {
            _back?.Invoke();
        }

        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
        }
    }
}