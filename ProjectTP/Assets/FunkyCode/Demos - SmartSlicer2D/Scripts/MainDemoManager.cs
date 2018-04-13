using System.Collections.Generic;
using UnityEngine;

public class MainDemoManager : MonoBehaviour {
	public RectTransform UIObject;
	public RectTransform UIBack;
	public GameObject[] demoScenes;
	public int currentSceneID = 0;

	private GameObject lastScene;
	private Vector3 startPosition;
	private GameObject currentScene = null;

	public void ResetScene()
	{
		Destroy (currentScene);
		SetScene (currentSceneID);
	}

	public void SetScene(int id)
	{
		currentSceneID = id;
		currentScene = Instantiate(demoScenes[id]);
		currentScene.transform.position = new Vector3 ((id + 1) * 50f, 0f, 0f);

		switch (BackgroundManager.GetDayState ()) {
			case BackgroundManager.DayState.day:
					BackgroundManager.SetDayState (BackgroundManager.DayState.night);
				break;
			case BackgroundManager.DayState.night:
				BackgroundManager.SetDayState (BackgroundManager.DayState.day);
				break;
		}

	}

	public void SetMainMenu()
	{
		lastScene = currentScene;
		currentScene = null;
	}

	void Start()
	{
		startPosition = UIObject.anchoredPosition;
	}

	void Update () {
		Camera mainCamera = Camera.main;
		if (currentScene != null) {
			UIBack.gameObject.SetActive (true);
			if (currentScene.activeSelf == false) 
				currentScene.SetActive (true);

			Vector3 position = mainCamera.transform.position;
			position.x = position.x * 0.95f + currentScene.transform.position.x * 0.05f;
			mainCamera.transform.position = position;

			position = UIObject.position;
			position.y = position.y * 0.95f + -500f * 0.05f;
			UIObject.position = position;
			if (UIObject.position.y < -350f)
				UIObject.gameObject.SetActive (false);

		} else {
			UIBack.gameObject.SetActive (false);
			if (lastScene != null) {
				if (Vector2.Distance (lastScene.transform.position, mainCamera.transform.position) > 40) {
					Destroy (lastScene);
					lastScene = null;
				}
			}

			Vector3 position = mainCamera.transform.position;
			position.x = position.x * 0.95f;
			mainCamera.transform.position = position;

			position = UIObject.anchoredPosition;
			position.x = position.x * 0.95f + startPosition.x * 0.05f;
			position.y = position.y * 0.95f + startPosition.y * 0.05f;
			UIObject.anchoredPosition = position;

			UIObject.gameObject.SetActive (true);
		}
	}
}
