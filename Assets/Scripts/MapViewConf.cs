using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
using System.Text;
using UniRx;
using UniRx.Triggers;

public class MapViewConf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,IDragHandler, IPointerClickHandler {
		
	public GameObject iconImage, parent,parent2,parent3;
	public InputField xtxt,ytxt;
	public int X = 10, Y = 10,dx,dy;
	private const int MX = 50,MY = 50;
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
	private int[,] attributes,kinds,kindsCopy;
	private Transform transpar;

	//Save
	private static string SavePath = Application.streamingAssetsPath  + "/Data/SaveData" ;
	private string[] filenames;

	//Refactor In
	[SerializeField] private Dropdown loadDropDown;
	[SerializeField] private Dropdown saveDropdown;
	[SerializeField] private Button clearButton;

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
		//ブロック生成
        objs = new GameObject[MX,MY];
        kinds = new int[MX,MY];
        attributes = new int[MX,MY];
        kindsCopy = new int[MX,MY];
		for(int i = 0; i < MX;++i){
			for(int j =0; j < MY; ++j){
				GameObject obj = Instantiate(iconImage,transform.position,transform.rotation) as GameObject;
				obj.GetComponent<Image>().sprite = null;
				obj.transform.SetParent(transpar,true);
				obj.transform.localPosition = new Vector3( (i) * width*colW , (GetHeight)-(j)*height*colH ,z);
				obj.name = "(" + i + "," + j+")";
				objs[i,j] = obj;
                kinds[i,j] = -1;
				objs[i,j].SetActive(false);
			}
		}
		for(int i = 0; i < X;++i){
			for(int j =0; j < Y; ++j){
				objs[i,j].SetActive(true);
			}
		}
			
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
			
		//Refactor In
		loadDropDown.ObserveEveryValueChanged(x => x.value).Subscribe( _ => LoadMap());
		saveDropdown.ObserveEveryValueChanged(x => x.value).Subscribe( _ => SaveMap());
		clearButton.OnPointerClickAsObservable().Subscribe( _ => Clear());

		var updateStream = this.UpdateAsObservable();

		//mouseStream
		updateStream.Select(_ => Input.GetMouseButton(0)).Subscribe(x => {
			leftflag = x;
		});
		updateStream.Select(_ => Input.GetMouseButton(1)).Subscribe(x => {
			rightflag = x;
		});
		// keyStream
		updateStream.Where(_ => Input.GetKeyDown(KeyCode.S)).Subscribe(_ => ChangeSave());
		updateStream.Where(_ => Input.GetKeyDown(KeyCode.D)).Subscribe(_ => ChangeLoad());
		updateStream.Where(_ => Input.GetKeyDown(KeyCode.F)).Subscribe(_ => Clear());
		updateStream.Where(_ => Input.GetKeyDown(KeyCode.V)).Subscribe(_ => Visualize());
		updateStream.Where(_ => Input.GetKeyDown(KeyCode.G)).Subscribe(_ => Generate());

		updateStream.Where(_ => xtxt.text != "").Where(_ => X.ToString() != xtxt.text).Subscribe(_ => dx = int.Parse(xtxt.text));
		xtxt.OnValueChangedAsObservable().ThrottleFrame(20).Where(x => x != "").Subscribe(x => {
			Array.Resize(ref copys,dx * Y);
			kindsCopy = new int[dx,Y];
			Debug.Log(dx);
		});
		updateStream.Where(_ => ytxt.text != "").Where(_ => Y.ToString() != ytxt.text).Subscribe(_ => dy = int.Parse(ytxt.text));
		ytxt.OnValueChangedAsObservable().ThrottleFrame(20).Where(x => x != "").Subscribe(x => {
			dy = int.Parse(ytxt.text);
			Array.Resize(ref copys,X*dy);
			kindsCopy = new int[X,dy];
			Debug.Log(dy);
		});
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
						var tmpSprite = hoge.GetComponent<Image>().sprite;
						var objPos = obj.name;
						int n = objPos.IndexOf(",");
						int x = int.Parse(objPos.Substring(1,n-1));
						int y = int.Parse(objPos.Substring(n+1).Trim(')'));
						objs[x,y].GetComponent<Image>().sprite = tmpSprite;
						var tmp = hoge.GetComponent<ImageConf>();
						var tmp2 = tg.ActiveToggles().FirstOrDefault().name;
						obj.GetComponentInChildren<Text>().text = tmp2;
						attributes[x,y] = int.Parse(tmp2);
						kinds[x,y] = tmp.kind;
					}
					else if(rightflag){
						var objPos = obj.name;
						int n = objPos.IndexOf(",");
						int x = int.Parse(objPos.Substring(1,n-1));
						int y = int.Parse(objPos.Substring(n+1).Trim(')'));
						objs[x,y].GetComponent<Image>().sprite = null;
						attributes[x,y] = 0;
						kinds[x,y] = -1;
					}
				}
			}
		}
	}
	public void ChangeMapSize(){
				Debug.Log ("dx = " + dx + "," + X);
		Debug.Log ("dy = " + dy + "," + Y);
		var tmpkind = kinds;
		var tmpattr = attributes;
		var tmpobjs = objs;
		kinds = new int[dx, dy];
		attributes = new int[dx, dy];
		for (int i = 0; i < Y; ++i) {
			for (int j = 0; j < X; ++j) {
				objs [j, i].SetActive (false);
			}
		}
		for (int i = 0; i < dy; ++i) {
			for (int j = 0; j < dx; ++j) {
				if (i < Y && j < X) {
					kinds [j, i] = tmpkind [j, i];
					attributes [j, i] = tmpattr [j, i];
				}
				objs[j, i].SetActive (true);
			}
		}
		X = dx;
		Y = dy;
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
			if (obj.activeSelf) {
				obj.GetComponentInChildren<MapFragConf>().Visualize ();
			}
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

	void IPointerEnterHandler.OnPointerEnter(PointerEventData ped){
		if(ped.pointerDrag == null) return;
	}
	
	void IPointerExitHandler.OnPointerExit(PointerEventData ped){
		if(ped.pointerDrag == null) return;
	}
	
	void IBeginDragHandler.OnBeginDrag(PointerEventData ped){
		hoge = GameObject.Find ("SelectMapTip");
	}
	
	void IDragHandler.OnDrag(PointerEventData ped){
		SetObject(ped);
	}
	void IPointerClickHandler.OnPointerClick(PointerEventData ped){
		hoge = GameObject.Find ("SelectMapTip");
		SetObject(ped);
	}
}
