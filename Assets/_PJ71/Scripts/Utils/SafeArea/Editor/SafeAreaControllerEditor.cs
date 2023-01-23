using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SafeAreaController))]
public partial class SafeAreaControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var script = target as SafeAreaController;
		Draw(script);
	}

	public static void Draw(SafeAreaController script)
	{
		EditorGUILayout.LabelField("Runtime Debug Settings", EditorStyles.boldLabel);

		script.EditorSafeZone.Left = EditorGUILayout.FloatField("Left", script.EditorSafeZone.Left);
		script.EditorSafeZone.Right = EditorGUILayout.FloatField("Right", script.EditorSafeZone.Right);
		script.EditorSafeZone.Bottom = EditorGUILayout.FloatField("Bottom", script.EditorSafeZone.Bottom);
		script.EditorSafeZone.Top = EditorGUILayout.FloatField("Top", script.EditorSafeZone.Top);
	}
}
