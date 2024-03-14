#if UNITY_EDITOR
using UnityEngine;
using PampelGames.GoreSimulator;
using UnityEditor;
using Transendence.Core;

namespace Transendence.EditorTools
{
    [CustomEditor(typeof(GoreComponent))]
    public class GoreComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GoreComponent goreComponent = (GoreComponent)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Populate Gore Objects"))
            {
                PopulateGoreObjects(goreComponent);
                EditorUtility.SetDirty(goreComponent);
            }
        }

        private void PopulateGoreObjects(GoreComponent goreComponent)
        {
            Transform[] childTransforms = goreComponent.transform.GetComponentsInChildren<Transform>(true);

            int count = 0;
            foreach (Transform childTransform in childTransforms)
            {
                GoreBone goreObject = childTransform.GetComponent<GoreBone>();
                if (goreObject != null)
                {
                    count++;
                }
            }

            goreComponent.GoreObjects = new GoreBone[count];

            int index = 0;
            foreach (Transform childTransform in childTransforms)
            {
                GoreBone goreObject = childTransform.GetComponent<GoreBone>();
                if (goreObject != null)
                {
                    goreObject.gameObject.layer = LayerMask.NameToLayer("Limbs");
                    goreComponent.GoreObjects[index] = goreObject;
                    index++;
                }
            }
        }
    }
}
#endif

