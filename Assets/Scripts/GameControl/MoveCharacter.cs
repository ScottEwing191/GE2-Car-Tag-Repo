// @nuget: Unity3D.SDK
// Ignore line above - required by DotNetFiddle

using System;
using UnityEngine;

public class MoveCharacter : MonoBehaviour {

	[SerializeField]
	private int linearSpeed;
	

	private void Awake() {
		//r = 42;
	}

	private void Update() {
		// BUG: It doesn't move smoothly across the screen

		transform.position = transform.position + new Vector3(linearSpeed * Time.deltaTime, 0f, 0f);
	}


}

public class RotateCharacter : MonoBehaviour {
	[SerializeField]
	private int rotateSpeed;

	private void Update() {

		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

	}
}