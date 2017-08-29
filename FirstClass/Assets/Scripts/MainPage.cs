using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour {
	public Button btnReady;
	// Use this for initialization
	void Start () {
		Server.Instance.launch ();
		this.btnReady.onClick.AddListener (onClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void onClick()
	{
		Text txt = this.btnReady.transform.Find("Text").GetComponent<Text> ();
		txt.text = "Waiting...";
		this.btnReady.enabled = false;

		Server.Instance.sendReady ();
	}
}
