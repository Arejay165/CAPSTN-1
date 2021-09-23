using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;// To be able to customize unity inspector (Currently in use for string list converted into drop down list)
                  // Using this will prevent the game from building
                  // Need to platformdefineddirective so that the game can be built
                  // Look at line 

public class ListToPopupAttribute : PropertyAttribute
{
    public Type myType;
    public string propertyName;
    public ListToPopupAttribute(Type p_myType, string p_propertyName)
    {
        myType = p_myType;
        propertyName = p_propertyName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer
{

    public override void OnGUI(Rect p_position, SerializedProperty p_property, GUIContent p_label)
    {
        ListToPopupAttribute atb = attribute as ListToPopupAttribute;
        List<string> stringList = null;

        if (atb.myType.GetField(atb.propertyName) != null)
        {
            stringList = atb.myType.GetField(atb.propertyName).GetValue(atb.myType) as List<string>;
        }

        if (stringList != null && stringList.Count != 0)
        {
            int selectedIndex = Mathf.Max(stringList.IndexOf(p_property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(p_position, p_property.name, selectedIndex, stringList.ToArray());
            p_property.stringValue = stringList[selectedIndex];
        }
    }
}
#endif

