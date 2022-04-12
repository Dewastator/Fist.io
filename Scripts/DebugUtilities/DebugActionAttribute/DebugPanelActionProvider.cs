using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;

namespace DebugUtilities
{
    [CreateAssetMenu(menuName = "Debug/DebugPanelActionProvider", fileName = "DebugPanelActionProvider", order = 0)]
    public class DebugPanelActionProvider : ScriptableObject
    {
        [SerializeField] private List<DebugPanelAction> methods = new List<DebugPanelAction>();
        [SerializeField] private List<DebugPanelProperty> properties = new List<DebugPanelProperty>();

        private string _path;
        private bool _isInitialized;

        private void OnDisable()
        {
            _isInitialized = false;
        }

        public IReadOnlyList<DebugPanelAction> Methods
        {
            get
            {
                if (_isInitialized == false)
                    Initialize();

                return methods;
            }
        }

        public IReadOnlyList<DebugPanelProperty> Properties
        {
            get
            {
                if (_isInitialized == false)
                    Initialize();

                return properties;
            }
        }

        public void Initialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

            var methodData = Resources.Load<TextAsset>("_Debug/Methods").text;
            var propertyData = Resources.Load<TextAsset>("_Debug/Properties").text;
            methods = JsonConvert.DeserializeObject<List<DebugPanelAction>>(methodData, settings);
            properties = JsonConvert.DeserializeObject<List<DebugPanelProperty>>(propertyData, settings);
            _isInitialized = true;
        }

        [ContextMenu("Analyze assemblies")]
        public void LoadAssemblies()
        {
            _path = Application.dataPath + "/Resources/_Debug";
            methods = new List<DebugPanelAction>();
            properties = new List<DebugPanelProperty>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                GetMethodsFromAssembly(assembly);
                GetPropertiesFromAssembly(assembly);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

            var methodData = JsonConvert.SerializeObject(methods, settings);
            File.WriteAllText(_path + "/Methods.json", methodData);

            var propertyData = JsonConvert.SerializeObject(properties, settings);
            File.WriteAllText(_path + "/Properties.json", propertyData);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.Log($"Found debug methods {Methods.Count}");
            Debug.Log($"Found debug properties {properties.Count}");
        }

        private void GetMethodsFromAssembly(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();
            foreach (var type in allTypes)
            {
                GetMethodsFromType(type);
            }
        }

        private void GetMethodsFromType(Type type)
        {
            var allMethods = type.GetMethods();

            foreach (var method in allMethods)
            {
                DebugPanelAction attribute = Attribute.GetCustomAttribute(method, typeof(DebugPanelAction)) as DebugPanelAction;

                if (attribute != null)
                {
                    attribute.MethodName = method.Name;
                    attribute.ObjectType = type.ToString();
                    methods.Add(attribute);
                }
            }
        }

        private void GetPropertiesFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                GetPropertiesFromType(type);
            }
        }

        private void GetPropertiesFromType(Type type)
        {
            var allProperties = type.GetProperties();

            foreach (var propertyInfo in allProperties)
            {
                DebugPanelProperty attribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DebugPanelProperty)) as DebugPanelProperty;

                if (attribute != null)
                {
                    attribute.PropertyName = propertyInfo.Name;
                    attribute.ObjectType = type;
                    attribute.PropertyType = propertyInfo.PropertyType;
                    properties.Add(attribute);
                }
            }
        }
    }
}