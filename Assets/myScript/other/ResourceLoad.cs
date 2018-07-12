using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct LoadRes
{
	public Sprite sprite0;
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;
}

public class ResourceLoad : MonoBehaviour {
    
	static string path = "myPrefabs/Fish/LoadRes/MR";
    
	void Start () {
		LoadRes res = new LoadRes();
		for (int i = 0; i < 4; ++i)
		{
			string str = path + i.ToString();

			//Resources.Load(str,typeof(Sprite)) as Sprite;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    


}
