using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour {
	public Button btnReady;
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;
	// Use this for initialization
	void Start () {
		Server.Instance.launch ();
		this.btnReady.onClick.AddListener (onClick);
	}
	
	// Update is called once per frame
	void Update () {
		if (axes == RotationAxes.MouseXAndY) 
		{
			Debug.Log (Input.GetAxis ("Mouse Y"));
		}
	}

	private void onClick()
	{
		Text txt = this.btnReady.transform.Find("Text").GetComponent<Text> ();
		txt.text = "Waiting...";
		this.btnReady.enabled = false;

		Server.Instance.sendReady ();
	}
}
