using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SaveConf : MonoBehaviour {	
	//Refactor In
	public GameObject obj;
	[SerializeField] Button saveButton;
	[SerializeField] GameObject saveDropDown;
	[SerializeField] GameObject loadDropDown;
	void Start(){
		//Refactor In
		saveButton.OnPointerClickAsObservable()
			.Subscribe(_ => saveDropDown
				.ObserveEveryValueChanged(x => x.activeSelf)
				.FirstOrDefault()
				.Subscribe(x => {
					saveDropDown.SetActive(!x);
				}));
	}
}
