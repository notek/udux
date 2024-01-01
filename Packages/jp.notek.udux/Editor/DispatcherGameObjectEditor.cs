using UnityEditor;
using UnityEngine;

namespace JP.Notek.Udux.Editor
{
    public class DispatcherGameObjectEditor
    {
        [MenuItem("GameObject/Udux/Dispatcher", false, 10)]
        private static void Create()
        {
            var prefab = Resources.Load<GameObject>("Dispatcher");
            var gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(gameObject, "Dispatcher");
            Selection.activeObject = gameObject;
        }
    }
}