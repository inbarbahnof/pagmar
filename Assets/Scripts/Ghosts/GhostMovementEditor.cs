using UnityEditor;
using UnityEngine;

namespace Ghosts
{
    [CustomEditor(typeof(GhostMovement))]
    public class GhostMovementEditor : Editor
    {
        private SerializedProperty pointA;
        private SerializedProperty pointB;
        private SerializedProperty pauseAfterTarget;
        private SerializedProperty speed;

        private void OnEnable()
        {
            pointA = serializedObject.FindProperty("pointA");
            pointB = serializedObject.FindProperty("pointB");
            pauseAfterTarget = serializedObject.FindProperty("_pauseAfterTarget");
            speed = serializedObject.FindProperty("speed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(pointA);
            EditorGUILayout.PropertyField(pointB);
            EditorGUILayout.PropertyField(pauseAfterTarget);
            EditorGUILayout.PropertyField(speed);

            serializedObject.ApplyModifiedProperties();
        }
    }
}