using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(TileManager))]
public class TileManagerEditor : Editor
{
    private TileManager tm;
    private SerializedProperty ids;
    private SerializedProperty tiles;
    private int size;

    private void OnEnable()
    {
        tm = (TileManager)target;
        ids = serializedObject.FindProperty("tempId");
        tiles = serializedObject.FindProperty("tempTiles");
        size = ids.arraySize;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ShowArrayData();
    }

    GUIContent labelId = new GUIContent("id");
    GUIContent tileId = new GUIContent("tile");
    private void ShowArrayData()
    {
        EditorGUI.BeginChangeCheck();

        //Arra Id
        EditorGUILayout.BeginVertical("box");
        size = EditorGUILayout.IntField("Size", size);
        EditorGUILayout.EndVertical();

        if(size > 0)
        {
            //Array Data
            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < ids.arraySize; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(ids.GetArrayElementAtIndex(i), labelId);
                EditorGUILayout.PropertyField(tiles.GetArrayElementAtIndex(i), tileId);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck())
        {
            UpdateArray();
        }
    }

    void UpdateArray()
    {
        if(ids.arraySize != size)
        {
            ids.arraySize = size;
            tiles.arraySize = size;
        }

        serializedObject.ApplyModifiedProperties();
        SetSceneDirty(tm.gameObject);
    }

    void SetSceneDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}
