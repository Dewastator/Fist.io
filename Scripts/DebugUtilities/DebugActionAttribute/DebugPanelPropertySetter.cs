using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DebugUtilities
{
    public class DebugPanelPropertySetter : MonoBehaviour
    {
        [SerializeField] private Text propertyName;
        [SerializeField] private Text propertyValue;
        private object _instance;
        private DebugPanelProperty _propertyInfo;

        public void Initialize(DebugPanelProperty methodInfo)
        {
            _propertyInfo = methodInfo;
            propertyName.text = methodInfo.DisplayName;

            if (_instance == null)
                _instance = FindObjectOfType(methodInfo.ObjectType);
            if (_instance == null)
                _instance = GetSystemOfType(methodInfo.ObjectType);

            UpdateText();
        }

        public void Plus()
        {
            var propertyInfo = _instance?.GetType().GetProperty(_propertyInfo.PropertyName);
            if (propertyInfo is { })
            {
                var value = (int)propertyInfo.GetValue(_instance);
                value += _propertyInfo.PositiveStep;
                propertyInfo.SetValue(_instance, value);
            }

            UpdateText();
        }

        public void Minus()
        {
            var propertyInfo = _instance?.GetType().GetProperty(_propertyInfo.PropertyName);

            if (propertyInfo is { })
            {
                var value = (int)propertyInfo.GetValue(_instance);
                value += _propertyInfo.NegativeStep;
                propertyInfo.SetValue(_instance, value);
            }

            UpdateText();
        }

        private void UpdateText()
        {
            var propertyInfo = _instance?.GetType().GetProperty(_propertyInfo.PropertyName);
            if (propertyInfo is null) return;
            var value = (int)propertyInfo.GetValue(_instance);
            propertyValue.text = value.ToString();
        }

        private object GetSystemOfType(Type type)
        {
            if (ScriptableObjectReferenceHolder.Instance == null) return null;
            
            var other = ScriptableObjectReferenceHolder.Instance.ObjectsToPreserve;
            var result = other.FirstOrDefault(x => x.GetType() == type);

            return result;
        }
    }
}