using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
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
	private MapGenerater MG;
	private GameObject[,] objs;
	private GameObject[] copys;
	private Sprite[] sprs;
	private int[,] attributes,kinds,kindscopy;
	private Transform transpar;

	//Save
	private static string SavePath = Application.streamingAssetsPath  + "/Data/SaveData" ;
	private string[] filenames;

	public void Start()
	{
		GetHeight = GetComponent<RectTransform>().sizeDelta.y;
		CCf = GameObject.Find ("Contents").GetComponent<ContentConf>();
		MG = new MapGenerater();
		transpar = GameObject.Find("Content").GetComponent<Transform>();
		sprs = CCf.sprs;
		width = CCf.width;
		height = CCf.height;
		colW = parent2.GetComponent<RectTransform>().rect.width/Screen.width;
		colH = parent2.GetComponent<RectTransform>().rect.height/Screen.height ;
		X = int.Parse(xtxt.text);
		Y = int.Parse(ytxt.text);
		dx = X;
		dy = Y;
		imagenum = sprs.Length;
		Array.Resize(ref filenames,imagenum);
		//Array.Resize(ref objs,X*Y);
		//ブロック生成
        objs = new GameObject[X,Y];
        kinds = new int[X,Y];
        attributes = new int[X,Y];
        kindscopy = new int[X,Y];
		for(int i = 0; i < X;++i){
			for(int j =0; j < Y; ++j){
				GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
				obj.GetComponent<Image>().sprite = null;
				obj.transform.SetParent(transpar,true);
				obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
				obj.name = "(" + i + "," + j+")";
				objs[i,j] =  obj;
                kinds[i,j] = -1;
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
		if(Input.GetKeyDown(KeyCode.G)){
            Generate();	
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
		if(xtxt.text != X.ToString() && xtxt.text != "" ){
			dx = int.Parse(xtxt.text);
			Array.Resize(ref copys, dx*Y);
			kindscopy = new int[dx,Y];
		}
		if(ytxt.text != Y.ToString() && ytxt.text != ""){
			dy = int.Parse(ytxt.text);
			Array.Resize(ref copys,X*dy);
			kindscopy = new int[X,dy];
		}
	}

	public void ChangeMapSize(){
		Debug.Log(dx + "," + X);

		if(dx != X){
			int gap = 0;
			/*for(int j = 0; j < Y;++j){
				if(X-dx>0){
					for(int i = 0; i < dx;++i){
						copys[i+(j*Y) - gap] = objs[i+(j*Y)];
						//kindscopy[i+(j*Y) - gap] = kinds[i+(j*Y) ];
					}
					gap += X - dx;
				}
				else if(dx > X){
					for(int i = 0; i < X;++i){
						copys[i+(j*Y)-gap] = objs[i+(j*Y)];
					}
					gap += dx-X;
				}
			}*/
			foreach(var obj in objs){
				    Destroy(obj);
			}
			kinds.Initialize();
			kinds = new int[dx,Y];
            objs = new GameObject[dx,Y];
            attributes = new int[dx,Y];
			gap =0;
			for(int j =0; j < Y; ++j){
				for(int i = 0; i < dx;++i){
					GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
					obj.GetComponent<Image>().sprite = null;
					obj.transform.SetParent(transpar,true);
					obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
					obj.name = "(" + i + "," + j+")";
					objs[i,j] =  obj;
					kinds[i,j] = -1;
				}
			}//ここまで
			/*if(X>dx){
				for(int j = 0; j < Y;++j){
					for(int i = 0; i < dx;++i){
						Debug.Log(i + "," + j);
						GameObject obj = Instantiate(copys[i+(j*Y) - gap]) as GameObject;
						obj.GetComponent<Image>().enabled = true;
						obj.GetComponent<LayoutElement>().enabled = true;
						obj.GetComponent<BoxCollider2D>().enabled = true;
						obj.GetComponent<MapFragConf>().enabled = true;
						obj.transform.SetParent(transpar,true);
						obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
						obj.transform.localScale = copys[i+(j*Y)-gap].transform.localScale;
						obj.name = "(" + j + "," + i+")";
						objs[i+(j*Y) - gap] =  obj;
						//kinds[i+(j*Y)-gap] = kindscopy[i+(j*Y)];
					}

					gap += X - dx;
				}
			}
			else if(dx > X){
				for(int j = 0; j < Y;++j){
					for(int i = 0; i < dx;++i){
						Debug.Log(i + "," + j);
						GameObject obj = Instantiate(copys[i+(j*Y)-gap]) as GameObject;
						obj.name = "(" + j + "," + i+")";
						obj.GetComponent<Image>().enabled = true;
						obj.GetComponent<LayoutElement>().enabled = true;
						obj.GetComponent<BoxCollider2D>().enabled = true;
						obj.GetComponent<MapFragConf>().enabled = true;
						obj.transform.SetParent(transpar,true);
						obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
						obj.name = "(" + j + "," + i+")";
						objs[i+(j*Y)] =  obj;
						//kinds[i*(j*Y)] = -1;
					}


					gap += dx-X;
				}
				for(int j = 0; j < Y;++j){
					for(int i = 0; i < X;++i){
						objs[i+(j*Y)].transform.localScale = copys[i+(j*Y)].transform.localScale;
						//kinds[i+(j*Y)] = kinds[i+(j*Y)];
					}
				}
			}*/
			copys.Initialize();
			X = dx;
		}
		if(dy != Y){
			int gap = 0;
			/*for(int j = 0; j < dy;++j){
				for(int i = 0; i < X;++i){
					copys[i+(j*Y) - gap] = objs[i+(j*Y)];
				}
				if(Y - dy > 0){
					gap += Y - dy;
				}
			}*/
			foreach(var obj in objs){
					Destroy(obj);
			}
            objs = new GameObject[X,dy];
            kinds = new int[X,dy];
            attributes = new int[X,dy];
			//Array.Resize(ref objs,dy*X);
			Debug.Log(objs.Length);
			gap =0;
			for(int j =0; j < dy; ++j){
				for(int i = 0; i < X;++i){
					GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
					obj.GetComponent<Image>().sprite = null;
					obj.transform.SetParent(transpar,true);
					obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
					obj.name = "(" + i + "," + j+")";
					objs[i,j] =  obj;
					kinds[i,j] = -1;
				}
			}//ここまで

			/*for(int j = 0; j < dy;++j){
				for(int i = 0; i < X;++i){
					Debug.Log(i + "," + j);
					GameObject obj = Instantiate(copys[i+(j*Y) - gap]) as GameObject;
					obj.GetComponent<Image>().enabled = true;
					obj.GetComponent<LayoutElement>().enabled = true;
					obj.GetComponent<BoxCollider2D>().enabled = true;
					obj.transform.SetParent(transpar,true);
					obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
					obj.transform.localScale = copys[i+(j*Y)-gap].transform.localScale;
					obj.name = "(" + j + "," + i+")";
					objs[i+(j*Y) - gap] =  obj;
					kinds[i+(j*Y)-gap] = kinds[i+(j*Y)];
				}
				if(Y - dy > 0){
					gap += Y - dy;
				}
			}*/
			copys.Initialize();
			Y = dy;
		}
		//マップサイズ変更
		/*if(dx > X){
			for(int i = X; i < dx;++i){
				for(int j = 0; j < Y;++j){
					GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
					obj.GetComponent<Image>().sprite = null;
					obj.transform.SetParent(transpar,true);
					obj.transform.position = new Vector3(0,0,z);
					obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH,z);
					obj.name = "(" + i + "," + j+")";
					objs[i + (j * Y)] =  obj;
                    kinds[i + (j * Y)] = -1;
				}
				++X;
				if(X == dx){
					break;
				}
			}
		}
		else if(dx < X){
			Debug.Log("X"+X);
			Debug.Log("dx"+dx);
			for(int i = X-1; i >= dx ;--i){
				for(int j = Y-1; j >= 0;--j){
					Destroy(objs[i+(j*Y)]);
					//objs[i+(j*Y)].GetComponent<Image>().sprite = null;
				}
				--X;
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
					obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
					obj.name = "(" + (i) + "," + j+")";
					objs[i + (j * Y)] =  obj;
                    kinds[i + (j * Y)] = -1;
				}
				++Y;
				if(Y == dy){
					break;
				}
			}
		}
		else if(dy < Y){
			for(int i = X-1; i >= 0;--i){
				for(int j = Y-1; j >= dy;--j){
					Destroy(objs[i+(j*Y)]);
				}
				--Y;
				if(Y == dy){
					break;
				}
			}
		}*/
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
						objs[x,y] = obj;
						var tmp = hoge.GetComponent<ImageConf>();
						var tmp2 = tg.ActiveToggles().FirstOrDefault().GetComponent<TogglesConf>();
						obj.GetComponentInChildren<Text>().text = tmp2.num.ToString();
						attributes[x,y] = tmp2.num;
						kinds[x,y] = tmp.kind;
					}
					else if(rightflag){
						obj.GetComponent<Image>().sprite = null;
						var objPos = obj.name;
						int n = objPos.IndexOf(",");
						int x = int.Parse(objPos.Substring(1,n-1));
						int y = int.Parse(objPos.Substring(n+1).Trim(')'));
						objs[x,y] = obj;
						attributes[x,y] = 0;
						kinds[x,y] = -1;
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
					sw.Write(attributes[j,i]+":"+kinds[j,i]+",");
				}
				sw.WriteLine("");
			}
			sw.Flush();
			sw.Close();
           MG.GenerateMap(kinds,X,Y, save.GetComponent<Dropdown>().value);
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
                    int j = i-(3+imagenum) ;
					SplitNumAndSet(ref attributes[j%X,j/Y],ref kinds[j%X,j/Y],strs[i]);
				}
				for(int i = 0; i < X;++i){
					for(int j =0; j < Y; ++j){
						if(objs[i,j] == null){
							GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
							objs[i,j] = obj;
						}
						if(kinds[i,j] > 0){
							objs[i,j].GetComponent<Image>().sprite = sprs[kinds[i,j]-1];
							objs[i,j].GetComponent<MapFragConf>().attribute = attributes[i,j];
						}
						else{
							objs[i,j].GetComponent<Image>().sprite = null;
						}
						objs[i,j].transform.SetParent(transpar,true);
						objs[i ,j].transform.localPosition = new Vector3((i) * width*colW , (GetHeight)-(j)*height*colH,z);
						objs[i,j].name = "(" + i + "," + j+")";
					}
				}
			}
		}
	}
	public void Clear(){
		for(int i = 0; i < X;++i){
            for(int j = 0; j < Y;++j){
			objs[i,j].GetComponent<Image>().sprite = null;
			objs[i,j].GetComponent<MapFragConf>().attribute = 0;
			kinds[i,j] =-1;
			attributes[i,j] = 0;
            }
		}
	}
	public void Visualize(){
		foreach(var obj in objs){
			obj.GetComponentInChildren<MapFragConf>().Visualize();
		}
	}
    public void Generate() {
        MG.GenerateMap(kinds, X, Y, 0);
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
		hoge = GameObject.Find ("SelectMapTip");
	}
	
	void IDragHandler.OnDrag(PointerEventData ped)
	{
		SetObject(ped);
	}
	void IPointerClickHandler.OnPointerClick(PointerEventData ped){
		hoge = GameObject.Find ("SelectMapTip");
		SetObject(ped);
	}
}
