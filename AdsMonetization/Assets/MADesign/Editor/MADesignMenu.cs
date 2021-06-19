using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MADesignMenu
{
	const string TAG = "MADesignMenu";

	[MenuItem("MADesign/Clear all PlayerPrefs")]
	private static void PlayerPrefsDeleteAll()
	{
		Debug.LogFormat("{0} - Clean all cache data", TAG);

		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
