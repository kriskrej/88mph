using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Destroyable))]
public class DestroyableEditor : Editor {
    SerializedProperty destroyableFragments;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Add all children"))
            AddAllChildren();
    }

    void AddAllChildren() {
        destroyableFragments = serializedObject.FindProperty("destroyableFragments");
        foreach (var child in GetChildren())
            AddDestroyableFragment(child);
        serializedObject.ApplyModifiedProperties();
    }

    void AddDestroyableFragment(Object fragment) {
        var index = destroyableFragments.arraySize;
        destroyableFragments.InsertArrayElementAtIndex(index);
        destroyableFragments.GetArrayElementAtIndex(index).objectReferenceValue = fragment;
    }

    List<GameObject> GetChildren() {
        return ((Destroyable) target).GetComponentsInChildren<FixedJoint>()
            .Select(x => x.gameObject)
            .ToList();
    }
}