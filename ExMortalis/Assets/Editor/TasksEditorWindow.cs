using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TasksEditorWindow : EditorWindow
{
    private List<Task> Tasks = new List<Task>();

    [MenuItem("Window/Task Manager")]
    public static void ShowWindow()
    {
        GetWindow<TasksEditorWindow>("Task Manager");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        // Add button to create a new task
        if (GUILayout.Button("Add Task"))
        {
            Tasks.Add(new Task());
        }

        // Display existing tasks
        foreach (var task in Tasks)
        {
            EditorGUILayout.BeginHorizontal();

            // Display task details
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Title", GUILayout.Width(50));
            EditorGUILayout.TextField(task.Title, GUILayout.Width(200));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Notes", GUILayout.Width(50));
            EditorGUILayout.TextArea(task.Description, GUILayout.Width(200));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Status", GUILayout.Width(50));
            EditorGUILayout.EnumPopup(task.Status, GUILayout.Width(100));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Priority", GUILayout.Width(50));
            EditorGUILayout.EnumPopup(task.Priority, GUILayout.Width(50));
            EditorGUILayout.EndVertical();
            // EditorGUILayout.TextField(task.Deadline, GUILayout.Width(100));
            // EditorGUILayout.TextField(task.AssignedTo, GUILayout.Width(100));

            // Edit and delete buttons
            if (GUILayout.Button("Edit"))
            {
                // Code to edit task
            }
            if (GUILayout.Button("Delete"))
            {
                Tasks.Remove(task);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}

public class Task
{
    public string Title;
    public string Description;
    public TaskStatus Status;
    public TaskPriority Priority;
    public string Deadline;
    public string AssignedTo;

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public enum TaskPriority
    {
        Urgent,
        Moderate,
        Low
    }
}
