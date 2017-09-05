using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float fSpeed = 700.0f;
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
		transform.rotation = Quaternion.LookRotation (vTraget - transform.position);
	}


}
