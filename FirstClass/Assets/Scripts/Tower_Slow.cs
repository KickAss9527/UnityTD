using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Slow : Tower {

	// Use this for initialization
	List<Enemy> arrAtkEnemy;

	void Start()
	{
		this.iAtkRadius = 30;
		this.arrAtkEnemy = new List<Enemy>{};
	}
	void Update()
	{
		
		Transform parent = GameObject.Find ("enemyParent").transform;
		for (int i = 0; i < parent.childCount; i++) 
		{
			Enemy em = parent.GetChild (i).GetComponent<Enemy> ();
			Enemy findEm = arrAtkEnemy.Find (
				delegate(Enemy obj) {
					return obj == em;
				}
			);
			if (isEnemyInAtkArea (em.transform)) 
			{
				if (!findEm) 
				{
					arrAtkEnemy.Add (em);
					em.fSpeed *= 0.6f;
				}
			} 
			else if(findEm)
			{
				arrAtkEnemy.Remove (em);
				em.fSpeed /= 0.6f;
			}
		}

	}
	protected override void attack(Transform enemyTrans)
	{
		Debug.Log ("slower");

	}
}
