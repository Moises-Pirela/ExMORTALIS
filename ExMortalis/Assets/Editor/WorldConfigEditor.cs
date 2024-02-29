using Transendence.Core.Configs;
using UnityEditor;
using UnityEngine;

namespace Transendence.EditorTools
{
    [CustomEditor(typeof(WorldConfig))]
    public class WorldConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WorldConfig worldConfig = (WorldConfig)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Populate Buff/Debuff Configs"))
            {
                PopulateBuffDebuffConfigs(worldConfig);
            }

            if (GUILayout.Button("Populate Ability Configs"))
            {
                PopulateAbilityConfigs(worldConfig);
            }
        }

        private void PopulateBuffDebuffConfigs(WorldConfig worldConfig)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BuffDebuffConfig)}");


            worldConfig.BuffDebuffConfigs = new BuffDebuffConfig[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                worldConfig.BuffDebuffConfigs[i] = AssetDatabase.LoadAssetAtPath<BuffDebuffConfig>(assetPath);
                worldConfig.BuffDebuffConfigs[i].Id = i;
            }

            EditorUtility.SetDirty(worldConfig);
            AssetDatabase.SaveAssets();

            Debug.Log("Buff/Debuff Configs Populated Successfully!");
        }

        private void PopulateAbilityConfigs(WorldConfig worldConfig)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(AbilityConfig)}");

            worldConfig.AbilityConfigs = new AbilityConfig[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                worldConfig.AbilityConfigs[i] = AssetDatabase.LoadAssetAtPath<AbilityConfig>(assetPath);
                worldConfig.AbilityConfigs[i].Id = i;
            }

            EditorUtility.SetDirty(worldConfig);
            AssetDatabase.SaveAssets();

            Debug.Log("Ability Configs Populated Successfully!");
        }
    }
}
