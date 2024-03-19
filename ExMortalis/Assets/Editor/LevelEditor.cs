using System.Collections;
using System.Collections.Generic;
using EFPController;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    private GameObject prefabToPlace;
    private Vector3 ghostPosition = Vector3.zero;
    private bool IsPlacing;
    Event e;

    private Vector2 scrollPosition;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }

    [MenuItem("Window/Level Editor/Set tower floors")]
    public static void SetFloors()
    {
        GameObject floorPrefab =  AssetDatabase.LoadAssetAtPath<GameObject>("Assets/@Game/Prefabs/GroundFloor.prefab");

        for (int i = 1; i < 13; i++)
        {
            GameObject floor =  GameObject.Instantiate(floorPrefab);
            floor.name = $"Floor_{i}";
            floor.transform.localPosition = new Vector3(0, 7.69f * i, 0);
        }
    }

    private void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void OnGUI()
    {
        e = Event.current;

        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Define categories
        string[] categories = new string[] { "Interactables", "Enemies", "Pickups", "Keys", "VFX" };

        foreach (string category in categories)
        {
            GUILayout.Label(category, EditorStyles.boldLabel);

            // Load prefabs from the category folder
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/@Game/Prefabs/" + category });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (GUILayout.Button(prefab.name))
                {
                    SceneView sceneView = SceneView.lastActiveSceneView;
                    if (sceneView != null)
                    {
                        prefabToPlace = prefab;
                        IsPlacing = true;
                    }
                }
            }
        }

        Handles.color = new Color(1, 1, 1, 0.5f);
        Handles.DrawWireCube(ghostPosition, Vector3.one);

        EditorGUILayout.EndScrollView();

    }

    private void Update()
    {
        if (IsPlacing && prefabToPlace != null)
        {

            if (e.type == EventType.MouseMove)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                if (plane.Raycast(ray, out float enter))
                {
                    ghostPosition = ray.GetPoint(enter);
                    ghostPosition.y = 0; // Snap to ground level
                }
            }

            // Confirm placement with a mouse click
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                // Instantiate the prefab at the final position
                GameObject prefab = PrefabUtility.InstantiatePrefab(prefabToPlace) as GameObject;
                prefab.transform.position = ghostPosition;
                prefab.transform.rotation = Quaternion.identity;
                prefabToPlace = null;
                IsPlacing = false;
            }
        }
    }
}
