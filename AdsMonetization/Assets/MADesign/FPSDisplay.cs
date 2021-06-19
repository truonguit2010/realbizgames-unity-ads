using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour {

	float deltaTime = 0.0f;

	private static FPSDisplay ins;

	public static FPSDisplay Instance {
		get 
		{ 
			if (ins == null) {
				GameObject go = new GameObject ("FPSDisplay");
				GameObject.DontDestroyOnLoad (go);
				ins = go.AddComponent<FPSDisplay> ();
				ins.init ();
			}
			return ins;
		}
	}

	private GUIStyle style;
	private Rect rect;

	private void init () {
		int w = Screen.width, h = Screen.height;
		rect = new Rect ();
		style = new GUIStyle();

		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100 + 15;
		style.normal.textColor = Color.green;
	}

	public void run() {

	}

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		float fps = 1.0f / deltaTime;

		float msec = deltaTime * 1000.0f;

		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

		GUI.Label(rect, text, style);
	}
}
