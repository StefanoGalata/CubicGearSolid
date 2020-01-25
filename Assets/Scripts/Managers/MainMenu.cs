using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	
	public static MainMenu instance;
	void Awake(){
		if(!instance)
			instance = this;
	}

	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject credits;

	public void DisableInteraction(){
		mainMenu.transform.parent.GetComponent<CanvasGroup>().interactable = false;
	}

	public void PlayGame() {
		UIManager.instance.Reset();
		AudioManager.instance.PlayConfirmSound();
		AudioManager.instance.themeSource.volume = 0;
		StartCoroutine(GameManager.instance.Load(1,1f));
	}

	public void ToggleSoundMenu(){
		GameManager.instance.ToggleSound();
	}

	public void ToggleMainMenu(){
		mainMenu.SetActive(!mainMenu.activeSelf);
	}

	public void ToggleCredits(){
		credits.SetActive(!credits.activeSelf);
	}

	public void Quit() {
        AudioManager.instance.PlayButtonSound();
        Debug.Log("Application quit");
        Application.Quit();
    }

	public void PlayButtonSound(){
		AudioManager.instance.PlayButtonSound();
	}

}