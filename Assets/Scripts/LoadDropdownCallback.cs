using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization;
using UniRx;
using UniRx.Triggers;

public class LoadDropdownCallback : MonoBehaviour {
	
	private int defonum = 3;
	[SerializeField] private int num;
	private static readonly string InfoPath = Application.dataPath + "/Resources/Data/Saveinfo.txt";

	public GameObject obj;

	void Start(){
		FileInfo fi = new FileInfo(InfoPath);
		try {
			// 一行毎読み込み
			using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
				num = int.Parse(sr.ReadToEnd()) ;
			}
		} catch (Exception e){
			// 改行コード
			SetDefaultText();
		}
		if (num <= defonum){
			num = defonum;
		}

		GetComponentInChildren<Text>().text = "Select&Load";
		for(int i = 2;i < num; ++i){
			GetComponent<Dropdown>().options.Add(new Dropdown.OptionData());
		}
		var options = GetComponent<Dropdown>().options;
		options[0].text = "DummyData" ;
		for(int i = 1; i < num+1;++i){
			options[i].text = "SaveData" + i + ".csv";
		}
	}
	string SetDefaultText(){
		return "C#あ\n";
	}
}

//http://qiita.com/WassyPG/items/5e4d3df219bba2a14f81