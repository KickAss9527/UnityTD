using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour {
	public float fSpeed = 10.5f;
	int iCurIdx = 0;
	Vector3 vTargetPos;
	public int iHealthValue = 100;
	int iHealthCur = 0;
	public GameObject preBar;
	Sequence mySeq;
	Slider objSlider;
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

	void Awake () 
	{
		this.iHealthCur = this.iHealthValue;
		this.iCurIdx = 0;
		this.transform.position = getTargetPos (0);
	}

	void Start()
	{
		this.objSlider = Instantiate (preBar).GetComponent<Slider> ();
		this.objSlider.transform.SetParent(GameObject.Find ("CanvasMain").transform);
		this.updateHealthBar ();
	}

	public void updatePath()
	{
		DOTween.Kill(transform);
		Vector3 tmpPos;
		float distance = 9999999f;
		int idx = -1;
		for (int i = 0; i < GameManager.Instance.arrPath.Length; i++) {
			tmpPos = getTargetPos (i);
			float tmpDis = Vector3.Distance(tmpPos, transform.position);
			if (tmpDis <= distance) {
				idx = i;
				distance = tmpDis;
			}
		}
		this.iCurIdx = idx-1;
		this.move ();

	}

	void tweenMoveFunc()
	{
		if (iCurIdx > -99 && iCurIdx < GameManager.Instance.arrPath.Length) 
		{
			iCurIdx++;
			Vector3 pos = getTargetPos (iCurIdx);
			float dur = Vector3.Distance (pos, transform.position) / fSpeed;
			transform.DOMove (pos, dur).OnComplete (tweenMoveFunc);
		}

	}
	public void move(){
		this.tweenMoveFunc ();
	}
	bool checkMoveNext()
	{
		if (iCurIdx > -99 && iCurIdx < GameManager.Instance.arrPath.Length) 
		{
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
		if (checkMoveNext ()) {
		}
		else 
		{
			this.escapse ();
		}

		Vector2 player2DPos = Camera.main.WorldToScreenPoint (transform.position);
		player2DPos.y += 20f;
		this.objSlider.transform.position = player2DPos;
	}

	void OnTriggerEnter(Collider collider)
	{
		Bullet bul = collider.transform.GetComponent<Bullet> ();
		if (!bul)//撞到建筑物了
			return;
		this.iHealthCur -= bul.iDmg;
		Destroy (bul.gameObject);

		this.updateHealthBar ();
		this.checkDeath ();
	}

	void updateHealthBar()
	{
		float p = iHealthCur * 1f / iHealthValue;
		this.objSlider.value = p;
		Color co = Color.Lerp (Color.red, Color.green, p);
		this.objSlider.fillRect.transform.GetComponent<Image> ().color = co;
	}

	void checkDeath()
	{
		if (iHealthCur > 0)
			return;
		this.goDie ();

	}

	void goDie(){
		Destroy (this.objSlider.gameObject);
		Destroy (this.gameObject);
	}

	void escapse()
	{	
		this.goDie ();
	}
}
