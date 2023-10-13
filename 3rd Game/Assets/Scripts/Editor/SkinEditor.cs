using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Skin))]
[CanEditMultipleObjects]
public class SkinEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var skin = target as Skin;

        EditorGUI.BeginChangeCheck();

        skin.SkinName = EditorGUILayout.TextField("Skin Name",skin.SkinName);

        skin.mesh = (Mesh)EditorGUILayout.ObjectField("Mesh",skin.mesh, typeof(Mesh), true);

        skin.CustMaterials = EditorGUILayout.Toggle(new GUIContent("Cust Materials"
            , "this Will tell it if this Mesh needs Custom Materials meaning not just the Player Mat")
            , skin.CustMaterials);


        //The Material Array Showing Stuff

        if (skin.CustMaterials)
        {
            SerializedProperty mats = serializedObject.FindProperty("Materials");
            EditorGUILayout.PropertyField(mats, true);
        }
       

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
    
}
