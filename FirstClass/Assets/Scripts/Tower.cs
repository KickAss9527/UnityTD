using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	float fFireCoolDown = 0f;
	float fFireRate = 0.4f;
	public GameObject prefBullet;
	// Use this for initialization
	void Start () {
		

	}

	bool canShoot(){return fFireCoolDown <= 0f;}
	// Update is called once per frame
	void Update () 
	{
		if (fFireRate > 0) {
			this.fFireCoolDown -= Time.deltaTime;
		}

		if (canShoot ()) {
			Transform parent = GameObject.Find ("enemyParent").transform;
			for (int i = 0; i < parent.childCount; i++) {
				Transform em = parent.GetChild (i);
				Bullet obj = GameObject.Instantiate (prefBullet).GetComponent<Bullet> ();
				obj.transform.parent = this.gameObject.transform.parent;
				obj.iDmg = 5;
				Vector3 pos = this.gameObject.transform.position;
				pos.y = 40f;
				obj.transform.position = pos;
				obj.fly (em.position);

				this.fFireCoolDown = this.fFireRate;

			}
		}


	}
}
