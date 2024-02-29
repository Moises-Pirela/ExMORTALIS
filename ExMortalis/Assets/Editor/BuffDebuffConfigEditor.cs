using Transendence.Core;
using Transendence.Core.Configs;
using UnityEditor;

namespace Transendence.EditorTools
{
    [CustomEditor(typeof(BuffDebuffConfig))]
    public class BuffDebuffConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BuffDebuffConfig buffDebuffConfig = (BuffDebuffConfig)target;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"));
            if (EditorGUI.EndChangeCheck())
            {
                UpdateModifiers(buffDebuffConfig);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ApplicationType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DuplicationType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DurationSec"));
            
            

            //switch (buffDebuffConfig.DuplicationType)
            //{
            //    case BuffDuplicationType.Ignore:
            //        EditorGUILayout.PropertyField(serializedObject.FindProperty("FlatMagnitude"));
            //        break;
            //    case BuffDuplicationType.Stack:
            //        EditorGUILayout.PropertyField(serializedObject.FindProperty("PercentageMagnitude"));
            //        break;
            //}

            switch (buffDebuffConfig.ApplicationType)
            {
                case BuffApplicationType.Base:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("BaseMagnitude"));
                    break;
                case BuffApplicationType.Flat:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("FlatMagnitude"));
                    break;
                case BuffApplicationType.Percentage:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("PercentageMagnitude"));
                    break;
            }

            switch (buffDebuffConfig.DuplicationType)
            {
                case BuffDuplicationType.Ignore:
                    break;
                case BuffDuplicationType.Extend:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxDuration"));
                    break;
                case BuffDuplicationType.Stack:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StacksToApply"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxStacks"));
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateModifiers(BuffDebuffConfig buffDebuffConfig)
        {
            switch (buffDebuffConfig.ApplicationType)
            {
                case BuffApplicationType.Base:
                    buffDebuffConfig.PercentageMagnitude = 0f;
                    buffDebuffConfig.FlatMagnitude = 0f;
                    break;
                case BuffApplicationType.Flat:
                    buffDebuffConfig.PercentageMagnitude = 0f;
                    buffDebuffConfig.BaseMagnitude = 0f;
                    break;
                case BuffApplicationType.Percentage:
                    buffDebuffConfig.FlatMagnitude = 0f;
                    buffDebuffConfig.BaseMagnitude = 0f;
                    break;
            }
        }
    }
}
