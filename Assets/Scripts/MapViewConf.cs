﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class MapViewConf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,IDragHandler, IPointerClickHandler {
		
	public GameObject iconImage, parent,parent2,parent3;
	public InputField xtxt,ytxt;
	public int X = 10, Y = 10,dx,dy;
	public ToggleGroup tg;
	public GameObject save,load;

	private float width { set; get; }
	private float height{ set; get; }
	private float colW,colH;//補正値
	private int imagenum;
	private float z = 0,GetHeight;
	private bool leftflag = false,rightflag = false;
	private GameObject hoge;
	private ContentConf CCf;
	private GameObject[] objs;
	private Sprite[] sprs;
	private int[] attributes,kinds;
	private Transform transpar;

	//Save
	private static string SavePath = Application.streamingAssetsPath  + "/Data/SaveData" ;
	private string[] filenames;

	public void Start()
	{
		GetHeight = GetComponent<RectTransform>().sizeDelta.y;
		CCf = GameObject.Find ("Contents").GetComponent<ContentConf>();
		transpar = GameObject.Find("Content").GetComponent<Transform>();
		sprs = CCf.sprs;
		width = CCf.width;
		height = CCf.height;
		colW = (parent2.GetComponent<RectTransform>().rect.width/parent3.GetComponent<RectTransform>().rect.width) * Screen.width;
		Debug.Log(Screen.width + "/" + width + "=" + Screen.width /width);
		Debug.Log("SH+"+Screen.height);
		Debug.Log(colW);
		Debug.Log(colW/width);
		colH = parent2.GetComponent<RectTransform>().rect.height / (Screen.height) ;
		X = int.Parse(xtxt.text);
		Y = int.Parse(ytxt.text);
		imagenum = sprs.Length;
		Array.Resize(ref filenames,imagenum);
		Array.Resize(ref objs,X*Y);
		Array.Resize(ref attributes,X*Y);
		Array.Resize(ref kinds,X*Y);
		//ブロック生成
		for(int i = 0; i < X;++i){
			for(int j =0; j < Y; ++j){
				GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
				obj.GetComponent<Image>().sprite = null;
				obj.transform.SetParent(transpar,true);
				obj.transform.localPosition = new Vector3( (i) * width , (GetHeight)-(j)*height ,z);
				var scale = obj.transform.localScale;
				/*var Vecx = realW/width; 
				var Vecy = realH/height; 
				obj.transform.localScale.Set( Vecx,Vecy,z);*/
				obj.name = "(" + i + "," + j+")";
				objs[i + (j * Y)] =  obj;
			}
		}//ここまで
			
		DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath+"/Images/");
		FileInfo[] info = dir.GetFiles("*.png",System.IO.SearchOption.TopDirectoryOnly);
		int fi = 0;
		foreach(FileInfo f in info)
		{
			if(f.Name[f.Name.Length-1] != 'a'){
				filenames[fi] = f.Name;
				++fi;
			}
		}
	}
	void Update() {
		if ( Input.GetKeyDown(KeyCode.S) ) {
			ChangeSave();
		}
		if ( Input.GetKeyDown(KeyCode.D) ) {
			ChangeLoad();
		}
		if ( Input.GetKeyDown(KeyCode.F) ) {
			Clear();
		}
		if( Input.GetMouseButton(0) ){
			leftflag = true;
			rightflag = false;
		}
		else{
			leftflag = false;
		}
		if( Input.GetMouseButton(1) ){
			rightflag = true;
			leftflag = false;
		} else {
			rightflag = false;
		}
		dx = int.Parse(xtxt.text);
		dy = int.Parse(ytxt.text);
	}

	public void ChangeMapSize(){
		Array.Resize(ref objs,dx*dy);
		Array.Resize(ref attributes,dx*dy);
		Array.Resize(ref kinds,dx*dy);
		//マップサイズ変更
		if(dx > X){
			for(int i = X; i < dx;++i){
				for(int j = 0; j < Y;++j){
					GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
					obj.GetComponent<Image>().sprite = null;
					obj.transform.SetParent(transpar,true);
					obj.transform.position = new Vector3(0,0,z);
					obj.transform.localPosition = new Vector3( (i) * width , (GetHeight)-(j)*height,z);
					obj.name = "(" + i + "," + j+")";
					objs[i + (j * Y)] =  obj;
				}
				++X;
				if(X == dx){
					break;
				}
			}
		}
		if(dy > Y){
			for(int j = Y; j < dy;++j){
				for(int i = 0; i < X;++i){
					GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
					obj.GetComponent<Image>().sprite = null;
					obj.transform.SetParent(transpar,true);
					obj.transform.position = new Vector3(0,0,z);
					obj.transform.localPosition = new Vector3( (i) * width , (GetHeight)-(j)*height ,z);
					obj.name = "(" + (i) + "," + j+")";
					objs[i + (j * Y)] =  obj;
				}
				++Y;
				if(Y == dy){
					break;
				}
			}
		}//ここまで
	}

	private void SetObject(PointerEventData ped){
		var pointer = new PointerEventData(EventSystem.current);

		Vector2 mousePos = Input.mousePosition;
		pointer.position = mousePos;

		var raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);

		if (raycastResults.Count > 0) {
			for(int i =0; i < raycastResults.Count;++i){
			GameObject obj = raycastResults[i].gameObject;
				if(obj.tag == "MapFragment"){
					if(leftflag){
						obj.GetComponent<Image>().sprite = hoge.GetComponent<Image>().sprite;
						var objPos = obj.name;
						int n = objPos.IndexOf(",");
						int x = int.Parse(objPos.Substring(1,n-1));
						int y = int.Parse(objPos.Substring(n+1).Trim(')'));
						objs[x + y*Y] = obj;
						var tmp = hoge.GetComponent<ImageConf>();
						var tmp2 = tg.ActiveToggles().FirstOrDefault().GetComponent<TogglesConf>();
						obj.GetComponentInChildren<Text>().text = tmp2.num.ToString();
						attributes[x + y*Y] = tmp2.num;
						kinds[x + y*Y] = tmp.kind;
					}
					else if(rightflag){
						obj.GetComponent<Image>().sprite = null;
						var objPos = obj.name;
						int n = objPos.IndexOf(",");
						int x = int.Parse(objPos.Substring(1,n-1));
						int y = int.Parse(objPos.Substring(n+1).Trim(')'));
						objs[x + y*Y] = obj;
						attributes[x + y*Y] = 0;
						kinds[x + y*Y] = 0;
					}
				}
			}
		}
	}
	public void SaveMap(){
		if(save.GetComponent<Dropdown>().value != 0){
			StreamWriter sw = new StreamWriter(SavePath+ save.GetComponent<Dropdown>().value+".csv",false);

			sw.Write(width+":");
			sw.Write(height+",");
			sw.Write(X+":");
			sw.Write(Y+",");
			sw.WriteLine (imagenum+",");
			foreach(string s in filenames){
				sw.Write(s+",");
			}
			sw.WriteLine("");
			for(int i =0; i < Y;++i){
				for(int j = 0; j < X;++j){
					sw.Write(attributes[j + i*Y]+":"+kinds[j+i*Y]+",");
				}
				sw.WriteLine("");
			}
			sw.Flush();
			sw.Close();
		}
	}
	public void LoadMap(){
		if(load.GetComponent<Dropdown>().value != 0){
			FileInfo fi = new FileInfo(SavePath+load.GetComponent<Dropdown>().value+".csv");
			if(fi.Exists){
				StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);
				string str = sr.ReadToEnd();
				string[] strs;
				strs = str.Split(new Char[] {','});
				SplitNumAndSet(ref CCf.width,ref CCf.height,strs[0]);
				width = CCf.width;
				height = CCf.height;
				X = int.Parse(xtxt.text);
				Y = int.Parse(ytxt.text);
				SplitNumAndSet(ref dx,ref dy,strs[1]);
				ChangeMapSize();
				xtxt.text = X.ToString();
				ytxt.text = Y.ToString();
				imagenum = int.Parse(strs[2]);
				for(int i = 3 + imagenum; i < strs.Length-1; ++i){
					SplitNumAndSet(ref attributes[i-(3+imagenum)],ref kinds[i-(3+imagenum)],strs[i]);
				}
				Array.Resize(ref objs,X*Y);
				for(int i = 0; i < X;++i){
					for(int j =0; j < Y; ++j){
						if(objs[i +(j*Y)] == null){
							GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
							objs[i +(j*Y)] = obj;
						}
						if(kinds[i +(j*Y)] != 0){
							objs[i +(j*Y)].GetComponent<Image>().sprite = sprs[kinds[i +(j*Y)]-1];
							objs[i +(j*Y)].GetComponent<MapFragConf>().attribute = attributes[i +(j*Y)];
						}
						else{
							objs[i +(j*Y)].GetComponent<Image>().sprite = null;
						}
						objs[i +(j*Y)].transform.SetParent(transpar,true);
						objs[i +(j*Y)].transform.localPosition = new Vector3((i) * width , (GetHeight)-(j)*height,z);
						objs[i +(j*Y)].name = "(" + i + "," + j+")";
					}
				}
			}
		}
	}
	public void Clear(){
		for(int i = 0; i < X*Y;++i){
			objs[i].GetComponent<Image>().sprite = null;
			objs[i].GetComponent<MapFragConf>().attribute = 0;
			kinds[i] =0;
			attributes[i] = 0;
		}
	}
	public void Visualize(){
		foreach(var obj in objs){
			obj.GetComponentInChildren<MapFragConf>().Visualize();
		}
	}
	private void SplitNumAndSet(ref int t1, ref int t2, string s){
		string[] tmps;
		tmps = s.Split(new Char[] {':'});
		t1 = int.Parse(tmps[0]);
		t2 = int.Parse(tmps[1]);
	}
	public void ChangeSave(){
		save.GetComponent<Dropdown>().value = 0;
		bool flag = save.activeSelf;
		save.SetActive(!flag);
		if(!flag){
			load.SetActive(false);
		}
	}
	public void ChangeLoad(){
		load.GetComponent<Dropdown>().value = 0;
		bool flag = load.activeSelf;
		load.SetActive(!flag);
		if(!flag){
			save.SetActive(false);
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData ped)
	{
		if(ped.pointerDrag == null) return;
	}
	
	void IPointerExitHandler.OnPointerExit(PointerEventData ped)
	{
		if(ped.pointerDrag == null) return;
	}
	
	void IBeginDragHandler.OnBeginDrag(PointerEventData ped){
		hoge = GameObject.Find ("Dragging Object");
	}
	
	void IDragHandler.OnDrag(PointerEventData ped)
	{
		SetObject(ped);
	}
	void IPointerClickHandler.OnPointerClick(PointerEventData ped){
		hoge = GameObject.Find ("Dragging Object");
		SetObject(ped);
	}
}
