using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;
[CustomEditor(typeof(CustomText))]
public class CustomTextEditor : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        CustomText component = (CustomText)target;
        base.OnInspectorGUI();
        component.id = EditorGUILayout.TextField("ID", component.id);
    }
}
#endif

public class CustomText : Text
{
    public string id;

    protected override void Start()
    {
        base.Start();
        SetText();
        OptionMode.languageChange += SetText;
    }

    public void SetText()
    {
        if (TableData.instance != null)
        {
            text = TableData.instance.GetString(id);
        }
    }
}
