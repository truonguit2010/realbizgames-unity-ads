using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MAMonoBehaviour), true)]
public class MonoHandlerInspector : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		MAMonoBehaviour mono = (MAMonoBehaviour)target;
		mono.GUIEditor();

	}


}
