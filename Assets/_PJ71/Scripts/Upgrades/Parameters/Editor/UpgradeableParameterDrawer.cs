using Main.Meta.Upgrades.Parameters;
using UnityEditor;
using UnityEngine;

namespace NS.Editor.Meta.Upgrades
{
    [CustomPropertyDrawer(typeof(IntUpgradeableParameter))]
    [CustomPropertyDrawer(typeof(FloatUpgradeableParameter))]
    public class UpgradeableParameterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float startWidth = position.width;
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var valueRect = new Rect(position.x, position.y, 70, position.height);
            var toggleRect =  new Rect(position.x + 75, position.y, 10, position.height);
            var parameterRect =  new Rect(position.x + 90, position.y, startWidth - position.x + 90, position.height);
            
            
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("_valueBase"), GUIContent.none);
          
            SerializedProperty isUpgradeableProp = property.FindPropertyRelative("_isUpgradeable");
            EditorGUI.PropertyField(toggleRect, isUpgradeableProp, GUIContent.none);
            if (isUpgradeableProp.boolValue)
            {
                EditorGUI.PropertyField(parameterRect, property.FindPropertyRelative("_parameterType"), GUIContent.none);
            }
            else
            {
                EditorGUI.LabelField(parameterRect, "Boostable");   
            }

            EditorGUI.EndProperty();
        }
    }
}