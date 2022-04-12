using UnityEngine.UI;
using UnityEngine;
using System;

namespace DebugUtilities
{
    public class DebugPanelActionInvoker : MonoBehaviour
    {
        [SerializeField] private Text methodNameText;
        private object _instance;

        public void Initialize(DebugPanelAction methodInfo)
        {
            methodNameText.text = methodInfo.DisplayName;
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_instance == null)
                    _instance = GetSystemOfType(GetType(methodInfo.ObjectType));
                if (_instance == null)
                    _instance = FindObjectOfType(GetType(methodInfo.ObjectType));


                var method = _instance.GetType().GetMethod(methodInfo.MethodName);
                method?.Invoke(_instance, null);
            });
        }

        private Type GetType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var t = assembly.GetType(typeName);
                if (t != null)
                    return t;
            }

            return null;
        }

        private object GetSystemOfType(Type type)
        {
            if (ScriptableObjectReferenceHolder.Instance == null) return null;
            var objects = ScriptableObjectReferenceHolder.Instance.ObjectsToPreserve;
            foreach (var x in objects)
            {
                if (x == null) continue;
                if (x.GetType() == type)
                    return x;
            }

            return null;
        }
    }
}