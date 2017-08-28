using UnityEngine;  
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;  
using System.Net.Sockets;  
using System.IO;
using System.Text;
using System.Threading;
using System;

public class Server : Singleton<Server>  {

	// Use this for initialization

//	private void SocketOpened(object sender, MessageEventArgs e) {
//		//invoke when socket opened
//		Debug.Log ("open");
//	}
	private Socket clientSocket;
	void ReceiveSocket()  
	{  
		//在这个线程中接受服务器返回的数据  
		while (true)  
		{   
			if(!clientSocket.Connected)  
			{  
				//与服务器断开连接跳出循环  
				Debug.Log("Failed to clientSocket server.");  
				clientSocket.Close();  
				break;  
			}  
			try  
			{  
				//接受数据保存至bytes当中  
				byte[] bytes = new byte[4096];  
				//Receive方法中会一直等待服务端回发消息  
				//如果没有回发会一直在这里等着。  
				int i = clientSocket.Receive(bytes);  
				if(i <= 0)  
				{  
					clientSocket.Close();  
					break;  
				}     
				UTF8Encoding enc = new UTF8Encoding();
				Debug.Log(enc.GetString(bytes));
			}  
			catch (Exception e)  
			{  
				Debug.Log("Failed to clientSocket error." + e);  
				clientSocket.Close();  
				break;  
			}  
		}  
	}     

	public void launch () 
	{
		clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPAddress mIp = IPAddress.Parse("192.168.1.101");  
		IPEndPoint ip_end_point = new IPEndPoint(mIp, 8888);  

		try {  
			clientSocket.Connect(ip_end_point);  
			Debug.Log("connect to server");

			UTF8Encoding enc = new UTF8Encoding();

			clientSocket.Send(enc.GetBytes("gei server"));

			Thread th = new Thread(new ThreadStart(ReceiveSocket));
			th.IsBackground = true;
			th.Start();
		}
		catch{ Debug.Log ("coonect failed");}



//		string[] terrainStr = {
//			"XXXXXXXOOOOO", 
//			"XXXXXXXOOOOX",
//			"XXXXXXXOOOOX",
//			"XXXXXXXXOXXX",
//			"XXXOOOOOOXXX",
//			"XXXOXXXXXXXX",
//			"XOOOXXXXXXXX",
//			"XOOOXXXXXXXX",
//			"XOOOXXXXXXXX",
//			"OOOOXXXXXXXX",};
//		Vector2 teSize = new Vector2 (terrainStr[0].Length, terrainStr.Length);
//
//		GameObject terrainParent = GameObject.Find ("terrain");
//		GameObject tt = GameObject.Find ("groundTile");
//		float dis = tt.transform.localScale.x*1.05f;
//		for (int x = 0; x < teSize.x; x++) {
//			for (int y = 0; y < teSize.y; y++) {
//				if (terrainStr [y] [x] == 'X')
//					continue;
//				GameObject cube = GameObject.Instantiate (tt);
//				cube.transform.parent = terrainParent.transform;
//				cube.transform.position = new Vector3 ((teSize.x/2 - x)*dis, 0, (teSize.y/2-y)*dis);
//			}
//		}
//		GameObject.Destroy (tt);

	}

	// Update is called once per frame
	void Update () {
		
	}
}
