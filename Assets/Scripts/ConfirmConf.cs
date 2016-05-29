using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class ConfirmConf : MonoBehaviour {
	//連携するGameObject
	public ToggleGroup toggleGroup;

	// Use this for initialization
	void Start()
	{
	}
	// Update is called once per frame
	void Update()
	{
	
	}
		
	public void onClick()
	{
		//Get the label in activated toggles
		string selectedLabel = toggleGroup.ActiveToggles().First().GetComponentsInChildren<Text>().First(t => t.name == "Label").text;
			
		Debug.Log("selected " + selectedLabel);
			
	}
}
