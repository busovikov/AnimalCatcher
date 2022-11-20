using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AnimalContainer))]
public class AnimalContainerEditor : Editor
{
    private SerializedProperty rows;
    private SerializedProperty active;
    private SerializedProperty upgrade;
    private SerializedProperty containerMenu;
    private SerializedProperty follower;
    private SerializedProperty circle;

    protected virtual void OnEnable()
    {
        rows = serializedObject.FindProperty("rows");
        active = serializedObject.FindProperty("active");
        upgrade = serializedObject.FindProperty("upgradeDef");
        containerMenu = serializedObject.FindProperty("containerMenu");
        follower = serializedObject.FindProperty("follower");
        circle = serializedObject.FindProperty("selectionCircle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawValues();
        
        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawValues()
    {
        EditorGUILayout.PropertyField(active, new GUIContent("Active", "If active, will receive the animals"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(rows, new GUIContent("Rows", "Number of rows available for filling in the container"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(upgrade, new GUIContent("Upgrade", "Definition of upgrade for container"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(containerMenu, new GUIContent("Container menu", "Container menu"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(follower, new GUIContent("Follower", "Allow container to follow"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(circle, new GUIContent("Circle", "Selection circle"));
    }
}
