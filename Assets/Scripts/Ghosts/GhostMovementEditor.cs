using UnityEditor;
using UnityEngine;

namespace Ghosts
{
    [CustomEditor(typeof(GhostMovement))]
    public class GhostMovementEditor : Editor
    {
        private SerializedProperty pointA;
        private SerializedProperty pointB;
        private SerializedProperty speed;
        private SerializedProperty movementEase;

        private void OnEnable()
        {
            pointA = serializedObject.FindProperty("pointA");
            pointB = serializedObject.FindProperty("pointB");
            speed = serializedObject.FindProperty("speed");
            movementEase = serializedObject.FindProperty("movementEase");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(pointA);
            EditorGUILayout.PropertyField(pointB);
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(movementEase);

            serializedObject.ApplyModifiedProperties();
        }
    }
}