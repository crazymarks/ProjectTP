using System.Collections.Generic;
using UnityEngine;

public class AttachPrefab : MonoBehaviour {
	public GameObject prefab;

	void Start () {
		GameObject g = Instantiate (prefab);
		g.transform.SetParent(gameObject.transform);
		g.transform.position = gameObject.transform.position;
		g.name = prefab.name;
		Destroy (this);
	}
}
