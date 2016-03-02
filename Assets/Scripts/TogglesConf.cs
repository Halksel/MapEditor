using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class TogglesConf : MonoBehaviour {

	public int num;
	private Toggle toggle1;
	private ToggleGroup toggleGroup1;

	// Use this for initialization
	void Start () {
		toggle1 = GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void OnClick()
	{
		Debug.Log(toggleGroup1.AnyTogglesOn());  // いずれかのトグルがオンになっているか
		if (toggleGroup1.AnyTogglesOn())
		{
			toggle1 = toggleGroup1.ActiveToggles().FirstOrDefault();  // チェックが付いているトグルを取得
			Debug.Log(toggle1.name); 
		}
	}
}
