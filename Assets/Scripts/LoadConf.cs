using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class LoadConf : MonoBehaviour {
	//Refactor Out
	private MapViewConf MVC;

	//Refactor In
	public GameObject obj;
	[SerializeField] Button loadButton;
	[SerializeField] GameObject loadDropDown;
	[SerializeField] GameObject saveDropDown;
	void Start(){
		//Refactor Out
		MVC = obj.GetComponent<MapViewConf>();

		//Refactor In
		loadButton.OnPointerClickAsObservable()
			.Subscribe(_ => loadDropDown
				.ObserveEveryValueChanged(x => x.activeSelf)
				.FirstOrDefault()
				.Subscribe(x =>{
					loadDropDown.SetActive(!x);
					//saveDropDown.SetActive(x);
				}));
	}
	//Refactor Out
	public void ButtonPush() {
		MVC.LoadMap();
	}
}
