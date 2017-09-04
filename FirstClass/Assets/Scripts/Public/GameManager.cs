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
}
