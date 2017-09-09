using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MainPage : MonoBehaviour {

	public Button btnReady;

	// Use this for initialization

	float getBeizerValue(float start, float mid, float end, float t)
	{
		return Mathf.Pow ((1 - t), 2) * start + 2 * (1 - t) * t * mid + Mathf.Pow (t, 2) * end;
	}

	void Start () {
		GameManager.Instance.startGame();
		this.btnReady.onClick.AddListener (onClick);
		Transform tow = GameObject.Find ("Cube").transform;
//		DOTween.To (()=>tow.position, x=>tow.position = x, new Vector3(0, 100, 0), 3f);

		Vector3 start = new Vector3 (0f, 0f, 0f);
		Vector3 end = new Vector3 (200f, 200f, 0f);
		Vector3 mid = new Vector3 (0f, 200f, 0f);

		float fValue = 0;
		DOTween.To(()=>fValue, (v)=>{
			tow.position = new Vector3(getBeizerValue(start.x, mid.x, end.x, v), 
										getBeizerValue(start.y, mid.y, end.y, v),
										getBeizerValue(start.z, mid.z, end.z, v));
		}, 1f, 3f);

		
		tow.DORotate (new Vector3 (0, 0, -90), 3f);
	}

	// Update is called once per frame
	void Update () {
	}

	private void onClick()
	{
		Text txt = this.btnReady.transform.Find("Text").GetComponent<Text> ();
		txt.text = "Waiting...";
		this.btnReady.enabled = false;

		GameManager.Instance.playerReady ();
	}


}
