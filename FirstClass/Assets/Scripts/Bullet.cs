using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float fSpeed = 1280.0f;
	public int iDmg = 0;
	bool flgFly = false;
	Vector3 vTargetPos;
	// Use this for initialization
	void Start () {
//		Debug.Log ("create bullet");
	}
	
	// Update is called once per frame
	void Update () {
		if (flgFly) 
		{
			transform.position = Vector3.MoveTowards (transform.position, vTargetPos, fSpeed * Time.deltaTime);
		}
	}

	public void fly(Vector3 vTraget)
	{
		this.vTargetPos = vTraget;
		this.flgFly = true;

		float dist = Vector3.Distance (transform.position, vTargetPos);
		float deltaY = transform.position.y - vTargetPos.y;

		float angY = Mathf.Cos (deltaY / dist);
		angY *= Mathf.Rad2Deg;

		Vector2 source = new Vector2 (transform.position.x, transform.position.z);
		Vector2 tar = new Vector2 (vTargetPos.x, vTargetPos.z);
		float angX = Mathf.Cos ((tar.y - source.y) / Vector2.Distance (source, tar));
		angX *= Mathf.Rad2Deg;
		if (angX > 45) {
			angX -= 90;
		}

		transform.Rotate (angX-90, 0, angY); //angY - 90);
//		Debug.Log (angX);
	}


}
