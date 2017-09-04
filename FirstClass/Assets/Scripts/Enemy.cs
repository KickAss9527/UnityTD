using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float fSpeed = 10.5f;
	int iCurIdx = 0;
	Vector3 vTargetPos;
	// Use this for initialization

	Vector3 getTargetPos(int idx)
	{
		if (idx >= GameManager.Instance.arrPath.Length)
			return new Vector3();
		
		GameObject terrainParent = GameObject.Find ("terrain");
		int targetTag = GameManager.Instance.arrPath [idx];
		GameObject tile = terrainParent.transform.Find (targetTag.ToString ()).gameObject;
		Vector3 pos = tile.transform.position;
		pos.y = 2.5f;
		return pos;
	}

	void Start () 
	{

	}
	public void move()
	{
		transform.position = getTargetPos (0);
		vTargetPos = transform.position;
		this.iCurIdx = -1;
		this.checkMoveNext ();
	}
	bool checkMoveNext()
	{
		if (iCurIdx > -99 && iCurIdx < GameManager.Instance.arrPath.Length) 
		{
			Vector3 myPos = transform.position;
			if (myPos == vTargetPos) 
			{
				iCurIdx++;
				if (iCurIdx >= GameManager.Instance.arrPath.Length) {
					iCurIdx = -99;
					return false;
				} else {
					vTargetPos = getTargetPos (iCurIdx);
				}
			}
			return true;
		} 
		else 
		{
			iCurIdx = -99;
			return false;
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if (checkMoveNext()) 
		{
//			Debug.Log (transform.position);
//			Debug.Log (vTargetPos);
//			Debug.Log ("----");
			transform.position = Vector3.MoveTowards (transform.position, vTargetPos, fSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Bullet bul = collider.transform.GetComponent<Bullet> ();
		Debug.Log ("boom dmg : "+ bul.iDmg);
	}
}
