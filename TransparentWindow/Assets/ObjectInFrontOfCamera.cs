using UnityEngine;
using System.Collections;

public class ObjectInFrontOfCamera : MonoBehaviour {

	public GameObject objectTarget;
	Vector3 screenPosition = new Vector3(Screen.width-(Screen.width/10),Screen.height/10,20);

	// Use this for initialization
	void Start () {

		if(objectTarget != null)
		{
			objectTarget.transform.position = GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
		}

	}
}
