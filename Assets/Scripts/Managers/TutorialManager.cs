using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	public string[] hints;
	int currentHint=0;

	[SerializeField] GameObject TutorialScreen;
	[SerializeField] Text tutorialTxt;

	void Start(){
		tutorialTxt.text = hints[currentHint];
		TutorialScreen.SetActive(true);
		Time.timeScale = 0;
	}

	public void Next(){
		if(currentHint < hints.Length-1){
			currentHint++;
			tutorialTxt.text = hints[currentHint];
		}
		else {
			Time.timeScale=1;
			TutorialScreen.SetActive(false);
		}
	}


}
