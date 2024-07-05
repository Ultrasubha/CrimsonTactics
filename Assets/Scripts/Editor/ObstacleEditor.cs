using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditor : Editor
{
    private bool[] obstacles;
    private const int gridSize = 10;

    void OnEnable()
    {
        ObstacleData data = (ObstacleData)target;
        obstacles = data.obstacles;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Obstacle Grid", EditorStyles.boldLabel);
        
        for (int y = 0; y < gridSize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                obstacles[index] = EditorGUILayout.Toggle(obstacles[index], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Obstacle Data"))
        {
            SaveObstacleData();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SaveObstacleData()
    {
        ObstacleData data = (ObstacleData)target;
        data.obstacles = obstacles;
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
