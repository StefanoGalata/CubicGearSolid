using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
	// public static UIManager instance;

	public GameObject pauseMenu;
    
    public GameObject soundMenu;

    public GameObject gameOverScreen;
    public GameObject mainUI;
    public GameObject loadingScreen;
    public GameObject scoreScreen;
    public GameObject qualityMenu;

	public GameObject[] gameOverObjects;

    public Slider healthSlider;
    public Slider loadingSlider;

    [SerializeField] Text[] scoresTxt;
    [SerializeField]InputField scoreInputField;
    //private string scorePlayerName="";

    public Text[] ScoresTxt{
        get{return scoresTxt;}
    }

    [SerializeField] private Text timeTxt;
    int startingPlayerHealth = 1;
    private string niceTime="";

    public float Timer{
        get {
            char[] newChar = new char[niceTime.Length];
            for(int i=0; i<niceTime.Length; i++){
                newChar[i]=niceTime[i];
                if(niceTime[i]==':')
                    newChar[i]='.';
            }
            string newString = new string(newChar);
            return float.Parse(newString);
        }
    }

	void Awake(){
        MakeSingleton(this);
    }

    void Start(){
        niceTime="";
        DisableAllUI();
        FindPlayerHealth();
        var submitEvent = new InputField.SubmitEvent();
        //var valueChangeEvent = new InputField.OnChangeEvent();
        submitEvent.AddListener(OnEndEdit);
        //valueChangeEvent.AddListener(OnTextInputValueChange);
        scoreInputField.onEndEdit = submitEvent;
        //scoreInputField.onValueChanged = valueChangeEvent;
    }
	

    public void UpdateUITime(float timer){
        int minutes = Mathf.FloorToInt(timer / 60F);
     	int seconds = Mathf.FloorToInt(timer - minutes * 60);
     	niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeTxt.text="Time: " + niceTime;
    }

    public void ActivateInputField(){
        scoreInputField.gameObject.SetActive(true);
    }

	public IEnumerator GameOverFadeIn() {
        gameOverScreen.SetActive(true);
        Image gameOverImage = gameOverScreen.GetComponent<Image>();
        while (gameOverImage.color.a < 1) {
            gameOverImage.color = new Color(gameOverImage.color.r, gameOverImage.color.g, gameOverImage.color.b, gameOverImage.color.a+1 *.25f* Time.deltaTime);
			yield return null;
        }
		yield return new WaitForSeconds(4);
		ActivateGameOverButtons();
        yield return null;
    }

    public void DisableAllUI() {
        pauseMenu.SetActive(false);
        Image gameOverImage = gameOverScreen.GetComponent<Image>();
        gameOverImage.color = new Color(gameOverImage.color.r, gameOverImage.color.g, gameOverImage.color.b, 0);
        DisableGameOverButtons();
        gameOverScreen.SetActive(false);
        soundMenu.SetActive(false);
        if(loadingScreen)
            loadingScreen.SetActive(false);
        scoreScreen.SetActive(false);
        scoreInputField.gameObject.SetActive(false);
        qualityMenu.SetActive(false);
        DisableMainUI();
    }

    void ActivateGameOverButtons(){
		foreach(GameObject g in gameOverObjects)
			g.SetActive(true);
	}

    void DisableGameOverButtons() {
        foreach (GameObject g in gameOverObjects)
            g.SetActive(false);
    }

    public void OnEndEdit(string value){
        value = value.ToUpper();
        GameManager.instance.SaveNameScores(value);
    }

    public void UpdateUIScores(){
        float [] scores = Save.GetScores();
        string[] playerNames = Save.GetPlayersName();
        for(int i=0; i<scores.Length; i++){
            Debug.Log(scores[i]);
            if(scores[i]>=Mathf.Infinity){
                scoresTxt[i].text =(i+1).ToString()+ ") NONE";
            }
            else{
                scoresTxt[i].text =(i+1).ToString()+ ")"+playerNames[i]+" "+scores[i].ToString("00.00").Replace('.',':');
            }
        }
    }

    public void ActivateMainUI(){
        mainUI.SetActive(true);
    }

    public void DisableMainUI(){
        Reset();
        mainUI.SetActive(false);
    }

    public void FindPlayerHealth(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
            startingPlayerHealth = player.GetComponent<PlayerHealth>().StartingHealth;
    }

    public void SetHealthSlider(float amount){
        healthSlider.value = amount;
    }

    public void Reset(){
        SetHealthSlider(1);
    }

    public void UpdateHealth(float newVal){
        float actualFillAmount = Mathf.Clamp01(newVal/startingPlayerHealth);
        SetHealthSlider(actualFillAmount);
    }
}
