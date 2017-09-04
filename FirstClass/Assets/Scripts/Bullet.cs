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

		float disX = Vector2.Distance (new Vector2(vTraget.x, vTraget.y), 
			new Vector2(transform.position.x, transform.position.y));
		float angX = Mathf.Acos ((transform.position.y - vTraget.y) / disX);
		angX *= Mathf.Rad2Deg;
		int dirX = transform.position.x > vTraget.x ? -1 : 1;
		angX *= dirX;

		float disZ = Vector2.Distance (new Vector2(vTraget.z, vTraget.y), 
			new Vector2(transform.position.z, transform.position.y));
		float angZ = Mathf.Acos ((transform.position.y - vTraget.y) / disZ);
		angZ *= Mathf.Rad2Deg;
		int dirZ = transform.position.z > vTraget.z ? 1 : -1;
		angZ *= dirZ;

		this.transform.rotation = Quaternion.Euler(new Vector3 (angZ, 0, angX));
	}


}
