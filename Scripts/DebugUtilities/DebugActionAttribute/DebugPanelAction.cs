using System;
using System.Linq;

namespace DebugUtilities
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class DebugPanelAction : Attribute
    {
        private string _displayName;

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName) && string.IsNullOrEmpty(ActionPath) == false)
                {
                    var parts = ActionPath.Split('/');
                    if (parts.Length == 0)
                        _displayName = "Unknown";
                    else
                        _displayName = parts.Last();
                }

                return _displayName;
            }
        }
        public string ActionPath { get; set; }
        public string ObjectType; // Initialized by provider
        public string MethodName; // Initialized by provider

        public DebugPanelAction()
        {
        }

        public DebugPanelAction(string actionPath)
        {
            ActionPath = actionPath;
        }
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DebugPanelProperty : Attribute
    {
        private string _displayName;

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName) && string.IsNullOrEmpty(ActionPath) == false)
                {
                    var parts = ActionPath.Split('/');
                    if (parts.Length == 0)
                        _displayName = "Unknown";
                    else
                        _displayName = parts.Last();
                }

                return _displayName;
            }
        }

        public string ActionPath { get; set; }
        public int PositiveStep { get; set; }
        public int NegativeStep { get; set; }
        public Type ObjectType { get; set; }
        public Type PropertyType { get; set; }

        public string PropertyName; // Initialized by provider

        public DebugPanelProperty()
        {
        }

        public DebugPanelProperty(string actionPath, int negativeStep = -1, int positiveStep = 1)
        {
            ActionPath = actionPath;
            NegativeStep = negativeStep;
            PositiveStep = positiveStep;
        }
    }
}