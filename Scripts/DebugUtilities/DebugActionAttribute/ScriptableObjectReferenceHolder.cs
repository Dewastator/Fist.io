using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilities
{
    /// <summary>
    /// Only purpose of this class is to keep references to the scriptable objects that need
    /// to live during whole game session, as the scriptable objects are unloaded during scene loadings
    /// </summary>
    public class ScriptableObjectReferenceHolder : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> objectsToPreserve;

        public List<ScriptableObject> ObjectsToPreserve => objectsToPreserve;

        public static ScriptableObjectReferenceHolder Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
    }
}