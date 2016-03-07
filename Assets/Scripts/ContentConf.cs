using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class ContentConf : MonoBehaviour {
	[System.NonSerialized]
	public int width,height;
	private string path;
	public string[] files;
<<<<<<< HEAD
	private GameObject[] objs;
=======
>>>>>>> 07dbdf0... First Commit
	private Texture2D[] textures;

	public GameObject image;
	[System.NonSerialized]
	public Sprite[] sprs;
	public SizeSetter SS;

	byte[] LoadBytes(string path) {
		FileStream fs = new FileStream(path, FileMode.Open);
		BinaryReader bin = new BinaryReader(fs);
		byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
		bin.Close();
		return result;
	}

	void Awake(){
		//"C:\test"以下のファイルをすべて取得する
		//ワイルドカード"*"は、すべてのファイルを意味する
		path = Application.streamingAssetsPath+"/Images/";
		files = System.IO.Directory.GetFiles(Application.streamingAssetsPath+"/Images/","*.png",System.IO.SearchOption.TopDirectoryOnly);
		Array.Resize(ref textures,files.Length);
<<<<<<< HEAD
		Array.Resize(ref objs,files.Length);
=======
>>>>>>> 07dbdf0... First Commit
		int i = 0;
		foreach (var n in System.IO.Directory.GetFiles(Application.streamingAssetsPath+"/Images/","*.png",System.IO.SearchOption.TopDirectoryOnly)) {
			files[i] = n;
			textures[i] = new Texture2D(100,100);
			++i;
		}
		Array.Resize(ref sprs,i);

	}

	// Use this for initialization
	void Start () {
		for(int i = 0; i < files.Length ;++i){
<<<<<<< HEAD
			objs[i] = (GameObject)Instantiate(image,transform.position,transform.rotation);
			objs[i].gameObject.name = "Image (" + i+ ")" ;
			objs[i].transform.SetParent(transform,false);
=======
			GameObject obj = (GameObject)Instantiate(image,transform.position,transform.rotation);
			obj.gameObject.name = "Image (" + i+ ")" ;
			obj.transform.SetParent(transform,false);
>>>>>>> 07dbdf0... First Commit
			textures[i].LoadImage(LoadBytes(files[i]));
			width = textures[i].width;
			height = textures[i].height;
			SS.SetSize(width,height);
<<<<<<< HEAD
			objs[i].GetComponent<Image>().sprite = Sprite.Create(textures[i],new Rect(0,0,width,height),Vector2.zero);
			sprs[i] = objs[i].GetComponent<Image>().sprite ;
			objs[i].GetComponent<ImageConf>().kind = i;
=======
			obj.GetComponent<Image>().sprite = Sprite.Create(textures[i],new Rect(0,0,width,height),Vector2.zero);
			sprs[i] = obj.GetComponent<Image>().sprite ;
			obj.GetComponent<ImageConf>().kind = i;
>>>>>>> 07dbdf0... First Commit
		}
	}

	void Update(){
		
	}

	public void onClick (){
		Debug.Log("クリックされた") ;
	}
}
