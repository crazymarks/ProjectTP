using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsHelper : MonoBehaviour {
	int fps = 0;
	float timer = 0;
	int fpsResult = 0;

	void OnRenderObject()
	{
		fps += 1;

		if (Time.realtimeSinceStartup > timer + 1){
			timer = Time.realtimeSinceStartup;
			fpsResult = fps;
			fps = 0;
		}

		Text text = GetComponent<Text> ();

		if (Application.targetFrameRate > 0)
			text.text = "fps " + fpsResult.ToString() + "/" + Application.targetFrameRate;
		else
			text.text = "fps " + fpsResult.ToString();
	}
}
