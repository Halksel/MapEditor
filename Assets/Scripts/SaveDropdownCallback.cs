using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization;
using UniRx;
using UniRx.Triggers;

public class SaveDropdownCallback : MonoBehaviour {

	private int defaultnum = 3;
	[SerializeField] private int num;
	[SerializeField] private Dropdown saveDropDown;
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
		if (num <= defaultnum){
			num = defaultnum;
		}

		GetComponentInChildren<Text>().text = "Select&Save ";
		for(int i = defaultnum;i <= num+1; ++i){
			GetComponent<Dropdown>().options.Add(new Dropdown.OptionData());
		}
		var options = GetComponent<Dropdown>().options;
		options[0].text = "DummyData";
		for(int i = 1; i < num+1;++i){
			options[i].text = "SaveData" + i + ".csv";
		}
		options[num+1].text = "Create New...";
		//Refactor In
		saveDropDown.ObserveEveryValueChanged(x => x.value)
			.Subscribe( x => {
					using(StreamWriter sw = new StreamWriter(fi.OpenWrite(),Encoding.UTF8)){
						if (x >= num){
							sw.WriteLine(x);
						}
						else{
							sw.WriteLine(num);
						}
					}	
				});
	}
	//Refactor Out
	public void OnValueChanged(int result)
	{
		int n = GetComponent<Dropdown>().value;
		FileInfo fi = new FileInfo(InfoPath);
		using(StreamWriter sw = new StreamWriter(fi.OpenWrite(),Encoding.UTF8)){
			if (n >= num){
				sw.WriteLine(n);
			}
			else{
				sw.WriteLine(num);
			}
		}
	}
	string SetDefaultText(){
		return "C#あ\n";
	}
}

//http://qiita.com/WassyPG/items/5e4d3df219bba2a14f81