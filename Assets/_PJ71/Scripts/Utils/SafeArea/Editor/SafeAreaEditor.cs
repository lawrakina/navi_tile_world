using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SafeArea))]
public partial class SafeAreaEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (!Application.isPlaying) return;

		var controller = SafeAreaController.Instance;
		if (controller == null) return;

		EditorGUILayout.Separator();
		SafeAreaControllerEditor.Draw(controller);
	}
}
