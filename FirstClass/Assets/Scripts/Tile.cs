using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Tile : MonoBehaviour {

	public bool flgHasTower = false;
	// Use this for initialization
	void Start () {
		Debug.Log ("ttile");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick(BaseEventData eventData)
	{
		Debug.Log ("click tile");
	}

	public void evtSelect()
	{
		Debug.Log ("tile select");
	}

	public void evtUnselect(){
	}
}
