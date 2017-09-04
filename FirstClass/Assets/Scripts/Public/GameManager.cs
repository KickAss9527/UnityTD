using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager> {

	public string[] strTerrain;
	public int iStartTag;
	public int iEndTag;
	public int[] arrPath;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public int convertXY_ToId(Vector2 vec)
	{
		return (int)(vec.x + vec.y*strTerrain[0].Length);
	}
	public Vector2 convertId_ToXY(int idx)
	{
		return new Vector2 (idx%strTerrain[0].Length, idx/strTerrain[0].Length);
	}
	public void startGame()
	{
		Server.Instance.launch ();
	}
	public void playerReady()
	{
		Server.Instance.sendReady ();
	}
	public void setupConfig(string[] t, int start, int end, int[] path)
	{
		this.strTerrain = t;
		this.iStartTag = start;
		this.iEndTag = end;
		this.arrPath = path;
		SceneManager.LoadScene ("Game");
	}
	public void updateConfig(string[] t, int start, int end, int[] path)
	{
		this.strTerrain = t;
		this.iStartTag = start;
		this.iEndTag = end;
		this.arrPath = path;
		//todo path changed, do sth to show update
	}
	public void sendBuilding(int tileIdx)
	{
		Server.Instance.sendBuilding (tileIdx);	
	}

	public Quaternion caculateAng(Vector3 source, Vector3 target)
	{
		float disX = Vector2.Distance (new Vector2(target.x, target.y), 
			new Vector2(source.x, source.y));
		float angX = Mathf.Acos ((source.y - target.y) / disX);
//		Debug.Log (disX);
		angX *= Mathf.Rad2Deg;

		int dirX = source.x > target.x ? -1 : 1;
		angX *= dirX;

		float disZ = Vector2.Distance (new Vector2(target.z, target.y), 
			new Vector2(source.z, source.y));
		float angZ = Mathf.Acos ((source.y - target.y) / disZ);
		angZ *= Mathf.Rad2Deg;
		int dirZ = source.z > target.z ? 1 : -1;
		angZ *= dirZ;

		return Quaternion.Euler(new Vector3 (angZ, 0, 0*angX));
	}
}
