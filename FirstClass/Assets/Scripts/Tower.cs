using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	float fFireCoolDown = 0f;
	protected float fFireRate = 0.4f;
	int iDmg = 6;
	protected int iAtkRadius = 50;
	public GameObject prefBullet;
	// Use this for initialization
	void Start () {
	}

	bool canShoot(){return fFireCoolDown <= 0f;}
	void updateCoolDown(){
		if (fFireRate > 0) {
			this.fFireCoolDown -= Time.deltaTime;
		}
	} 

	void Update () 
	{
		updateCoolDown ();

		if (canShoot ()) {
			Transform parent = GameObject.Find ("enemyParent").transform;
			for (int i = 0; i < parent.childCount; i++) 
			{
				Transform em = parent.GetChild (i);
				if (!isEnemyInAtkArea (em))
					continue;
				attack (em);

			}
		}
	}

	protected bool isEnemyInAtkArea(Transform tran)
	{
		return Vector3.Distance (transform.position, tran.position) <= iAtkRadius;
	}

	protected virtual void attack(Transform enemyTrans)
	{
		Bullet obj = GameObject.Instantiate (prefBullet).GetComponent<Bullet> ();
		obj.transform.parent = this.gameObject.transform.parent;
		obj.iDmg = this.iDmg;
		Vector3 pos = this.gameObject.transform.position;
		pos.y += 10f;
		obj.transform.position = pos;
		obj.fly (enemyTrans.position);
		this.fFireCoolDown = this.fFireRate;
	}
}
