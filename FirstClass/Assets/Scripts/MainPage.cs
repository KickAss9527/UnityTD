using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour {

	public Button btnReady;
	float dt;
	Vector2 prevVec;
	// Use this for initialization
	void Start () {
		GameManager.Instance.startGame();
		this.btnReady.onClick.AddListener (onClick);
;
	}
	
	// Update is called once per frame
	void Update () {
		int radius = 180;
		dt += Time.deltaTime;
		Transform round = GameObject.Find ("Cube").transform;
		round.position = new Vector3 (Mathf.Sin (dt+Mathf.PI*0.5f) * radius, 0, Mathf.Cos (dt+Mathf.PI*0.5f) * radius);

		Transform bul = GameObject.Find ("bullet").transform;
		bul.rotation = GameManager.Instance.caculateAng (bul.position, round.position);
//		bul.rotation = Quaternion.Euler (Mathf.Sin (dt) * Mathf.Rad2Deg, 0, Mathf.Cos (dt) * Mathf.Rad2Deg);
		Vector2 vec = new Vector2 (bul.rotation.x, bul.rotation.z);
		Debug.Log (vec);
		prevVec = vec;
	}

	private void onClick()
	{
		Text txt = this.btnReady.transform.Find("Text").GetComponent<Text> ();
		txt.text = "Waiting...";
		this.btnReady.enabled = false;

		GameManager.Instance.playerReady ();
	}


}
