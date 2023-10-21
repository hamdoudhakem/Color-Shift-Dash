using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(CameraMovement))]
[CanEditMultipleObjects]
public class CameraMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraMovement Cam = target as CameraMovement;

        EditorGUI.BeginChangeCheck();

        //The Custom Editor Label Warning
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold };
        EditorGUILayout.LabelField("This Is a Custom Editor", style, GUILayout.ExpandWidth(true));

        EditorGUILayout.Space();

        //All The Options and Variables
        Cam.UseTransparency = EditorGUILayout.Toggle(new GUIContent("Use Transparency",
            "If I should make the objects that comes between the player and camera (generaly doors or falling balls) Transparent")
            ,Cam.UseTransparency);

        if (Cam.UseTransparency)
        {
            SerializedProperty Types = serializedObject.FindProperty("ConcernedTypes");
            EditorGUILayout.PropertyField(Types, true);

            Cam.ConcernedLayers = EditorGUILayout.LayerField("Concerned Layers", Cam.ConcernedLayers);

            Cam.TransparWhiteMat = (Material)EditorGUILayout.ObjectField("Transpar White Mat", Cam.TransparWhiteMat, typeof(Material),true);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Position Related", EditorStyles.boldLabel);

        Cam.SideLimit = EditorGUILayout.FloatField(new GUIContent("Side Limit",
            "The Limit that the X axe of the Camera can't go beyond"), Cam.SideLimit);

        Cam.OutOfSight = EditorGUILayout.Slider(new GUIContent("Out Of Sight",
            "The Difference between the player X and camera X from which the player start going out of sight")
            , Cam.OutOfSight, 0, 15);

        Cam.Player = (Transform)EditorGUILayout.ObjectField("Player", Cam.Player, typeof(Transform),true);

        Cam.Offset = EditorGUILayout.Vector3Field(new GUIContent("Offset",
            "Offset Between the player Position and This Object Position"), Cam.Offset);

        Cam.MinHeight = EditorGUILayout.FloatField(new GUIContent("Min Height",
            "The Y value of the Lowest piece of Ground that the Camera shouldn't go below (default is 0)")
            ,Cam.MinHeight);
       
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
