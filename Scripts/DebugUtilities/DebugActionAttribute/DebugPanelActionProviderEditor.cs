#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace DebugUtilities
{
    [CustomEditor(typeof(DebugPanelActionProvider))]
    public class DebugPanelActionProviderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = target as DebugPanelActionProvider;
            if (!t) return;

            if (GUILayout.Button("Analyze assemblies"))
                t.LoadAssemblies();
        }
    }
}
#endif