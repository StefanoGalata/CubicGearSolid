using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : Singleton<GameManager> {
    
    private bool playerDied = false;
    private bool isLevelCompleted = false;

    bool isPlayingDiscoveredSoundTrack = false;

    public Slider musicSlider;
    public Slider soundsSlider;
    public Slider enemySoundSlider;

    int scoreIndexFound = -1;

    private float timer = 0;

    public bool PlayerDied {
        get { return playerDied; }
        set {
            playerDied = value;
            if(playerDied){
                PlayerHasDied();
            }
        }
    }

     public bool IsLevelCompleted {
        get { return isLevelCompleted; }
        set { isLevelCompleted = value; }

    }

    void Awake() {
        MakeSingleton(this);
        timer = 0;
        SetQuality(Save.GetQualitySettings());
    }

    void Start(){
        Slider[] sliders = UIManager.instance.soundMenu.GetComponentsInChildren<Slider>();
        if (sliders.Length <= 0) return;
        musicSlider = sliders[0];
        musicSlider.value = AudioManager.instance.themeSource.volume;

        soundsSlider = sliders[1];
        soundsSlider.value = AudioManager.instance.soundSource.volume;

        enemySoundSlider = sliders[2];
        enemySoundSlider.value = AudioManager.instance.enemySource.volume;
    }

    void Update() {
        if(playerDied || isLevelCompleted)
            return;

        UIManager.instance.UpdateUITime(timer);
        timer+=Time.deltaTime;
        if (EnemyManager.instance.LastPlayerKnownPosition != EnemyManager.instance.ResetPosition && !isPlayingDiscoveredSoundTrack) {
            isPlayingDiscoveredSoundTrack = true;
            //if is running music fade out we must stop it
            StopAllCoroutines();
            AudioManager.instance.themeSource.volume = musicSlider.value;
            StartCoroutine(AudioManager.instance.PlayPlayerDiscovered());
        }

        else if (EnemyManager.instance.LastPlayerKnownPosition == EnemyManager.instance.ResetPosition && isPlayingDiscoveredSoundTrack) {
            StartCoroutine(AudioManager.instance.MusicFadeOut(AudioManager.instance.PlaySoundTrack));
            isPlayingDiscoveredSoundTrack = false;
        }
    }

    void PlayerHasDied(){
        StopAllCoroutines();
        StartCoroutine(AudioManager.instance.MusicFadeOut(AudioManager.instance.PlayGameOver, 1.5f));
        UIManager.instance.StartCoroutine(UIManager.instance.GameOverFadeIn());
    }

    public IEnumerator LaserTriggered(Vector3 position, bool shouldPlaySound = true){
        if(shouldPlaySound)
            AudioManager.instance.PlayLaserTriggeredSound();
        yield return new WaitForSeconds(.2f);
        EnemyManager.instance.LastPlayerKnownPosition = position;
        yield return null;
    }

    public void TogglePause() {
        if(SceneManager.GetActiveScene().name == "MainMenu") return; //TODO REMOVE?
        AudioManager.instance.PlayButtonSound();
        UIManager.instance.pauseMenu.SetActive(!UIManager.instance.pauseMenu.activeSelf);
        if (UIManager.instance.pauseMenu.activeSelf || UIManager.instance.soundMenu.activeSelf || UIManager.instance.qualityMenu.activeSelf)
            Time.timeScale = 0;
        else{
            Time.timeScale = 1;  
        }
            
    }

    public void ToggleQualityMenu(){
        UIManager.instance.qualityMenu.SetActive(!UIManager.instance.qualityMenu.activeSelf);
    }

    public void SetQuality(int quality){
        QualitySettings.SetQualityLevel(quality);
        Save.SaveQualitySettings(quality);
    }

    public void Quit() {
        AudioManager.instance.PlayButtonSound();
        Debug.Log("Application quit");
        Application.Quit();
    }

    public void ToggleSound(){
        AudioManager.instance.PlayButtonSound();
        UIManager.instance.soundMenu.SetActive(!UIManager.instance.soundMenu.activeSelf);
        if(!UIManager.instance.soundMenu.activeSelf && (MainMenu.instance))
            MainMenu.instance.ToggleMainMenu(); //TODO REMOVE mainMenu
    }

    public void OnMusicValueChange(){
        AudioManager.instance.themeSource.volume = musicSlider.value;
        Save.SaveMusicVolume(musicSlider.value);
    }

    public void OnSoundValueChange(){
        AudioManager.instance.soundSource.volume = soundsSlider.value;
        AudioManager.instance.stepSource.volume = soundsSlider.value;
        Save.SaveSoundVolume(soundsSlider.value);
    }

    public void OnEnemySoundValueChange(){
        AudioManager.instance.enemySource.volume = enemySoundSlider.value;
        AudioManager.instance.shotSource.volume = enemySoundSlider.value;
        Save.SaveEnemySoundVolume(enemySoundSlider.value);
    }

    public void Retry(){
        UIManager.instance.Reset();
        AudioManager.instance.PlayConfirmSound();
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex,2f));
        UIManager.instance.gameOverScreen.GetComponent<CanvasGroup>().interactable = false;
    }

    void CheckScores(){
        float[] scores = Save.GetScores();
        for(int i=0; i<scores.Length; i++){
            if(UIManager.instance.Timer < scores[i]){
                UIManager.instance.ActivateInputField();
                string[] names = Save.GetPlayersName();
                string nameTmp = names[i];
                scoreIndexFound=i;
                float tmp = scores[i];
                Save.SaveScore("SCORE"+(i+1), UIManager.instance.Timer);
                Save.SavePlayerName("NAME"+(i+1), "");
                for(int j =i+1; j<scores.Length; j++){
                        Save.SaveScore("SCORE"+(j+1), tmp);
                        Save.SavePlayerName("NAME"+(j+1), nameTmp);
                        tmp = scores[j];
                        nameTmp = names[j];
                }
                break;
            }
        }
        UIManager.instance.UpdateUIScores();
    }

    public void SaveNameScores(string name){
        if(scoreIndexFound!=-1){
            Save.SavePlayerName("NAME"+(scoreIndexFound+1), name);
            //Debug.Log("NAME"+(scoreIndexFound+1));
            UIManager.instance.UpdateUIScores();
        }
    }

    public void LevelComplete(){
        isLevelCompleted = true;
        CheckScores();
        UIManager.instance.scoreScreen.SetActive(true);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnemyManager.instance.GetEnemies();
        EnemyManager.instance.LastPlayerKnownPosition = EnemyManager.instance.ResetPosition;
        AudioManager.instance.themeSource.loop = true;
        AudioManager.instance.themeSource.volume = musicSlider.value;
        AudioManager.instance.PlayMainMenu();
        UIManager.instance.DisableAllUI();
        UIManager.instance.gameOverScreen.GetComponent<CanvasGroup>().interactable = true;
        playerDied = false;
        isLevelCompleted = false;
        isPlayingDiscoveredSoundTrack = false;
        scoreIndexFound = -1;
        timer = 0;
        if(scene.name != "MainMenu"){ //do not remove for now
            AudioManager.instance.PlaySoundTrack();
            UIManager.instance.FindPlayerHealth();
            UIManager.instance.SetHealthSlider(1);
            UIManager.instance.ActivateMainUI();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public IEnumerator Load(int sceneIndex, float time = 0f){
        yield return new WaitForSeconds(time);
        SceneManager.sceneLoaded += OnSceneLoaded;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        UIManager.instance.loadingScreen.SetActive(true);
        //operation.allowSceneActivation = false;
        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress/.9f);
            UIManager.instance.loadingSlider.value = progress;
            yield return null;
        }
        //operation.allowSceneActivation = true;
        UIManager.instance.loadingScreen.SetActive(false);
    }

    public void Menu(float time=0){
        AudioManager.instance.PlayConfirmSound();
        UIManager.instance.gameOverScreen.GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(Load(0,time));
    }

}
