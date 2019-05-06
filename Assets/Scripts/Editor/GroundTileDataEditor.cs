using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GroundTile
{
    [CustomEditor(typeof(GroundTileData))]
    public class GroundTileDataEditor : Editor
    {
        private GroundTileData gta;
        private SerializedProperty tiles;
        //private SerializedProperty arraySize;

        private void OnEnable()
        {
            gta = (GroundTileData)target;
            tiles = serializedObject.FindProperty("tiles");
            //arraySize = tiles.FindPropertyRelative("Array.size");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowArrayData();
        }

        void ShowArrayData()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box");
            //EditorGUILayout.PropertyField(arraySize);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(tiles, true);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        //void RecalculateStok()
        //{
        //    int sum = 0;
        //    for (int i = 0; i < gta.tileHeightZone.Length; i++)
        //    {
        //        sum += gta.tileHeightZone[i];
        //    }

        //    stock = gta.mapHeight - sum;
        //    //Debug.Log(stock);
        //}
    }
}