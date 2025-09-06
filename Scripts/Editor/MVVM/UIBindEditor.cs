#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace STIGRADOR.MVVM.Editor
{
    [CustomEditor(typeof(UIBindBase), true)]
    public class UIBindEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var uiBind = (UIBindBase)target;
            
            var eventName = string.IsNullOrEmpty(uiBind.EventName) 
                ? uiBind.name 
                : uiBind.EventName;

            var enableField = string.IsNullOrEmpty(uiBind.EnableField) 
                ? $"{eventName}Enable" 
                : $"{uiBind.EnableField}Enable";

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Preview Finally Names", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("EventName", eventName);
            EditorGUILayout.TextField("EnableField", enableField);
            
            EditorGUILayout.Space();
            EditorGUILayout.TextField("EnableFieldEventName", $"On{enableField}Changed");
            EditorGUILayout.TextField("OnClickEventName", uiBind._oNClickEventName);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
#endif