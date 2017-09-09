using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	float fFireCoolDown = 0f;
	protected float fFireRate = 0.4f;
	int iDmg = 1;
	protected int iAtkRadius = 20;
	public GameObject prefBullet;
	public int tileId;
	public GameObject circle;
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
				break;

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
		obj.transform.position = pos;
		obj.fly (enemyTrans.position);
		this.fFireCoolDown = this.fFireRate;
	}

	public void evtSelect()
	{
		Circle cir = GameObject.Instantiate (circle).GetComponent<Circle> ();
		cir.transform.SetParent (transform);

		float scale = iAtkRadius / GameScene.Instance.getTileScale ();
		cir.setupScale (scale/2);
		cir.transform.localPosition = new Vector3 (0f, -1.11f, 0f);

		cir.gameObject.name = "circle";

	}
	public void unSelect()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Transform tr = transform.GetChild(i);
			if (tr.gameObject.name == "circle") 
			{
				Destroy (tr.gameObject);
			}
		}
	}
}
