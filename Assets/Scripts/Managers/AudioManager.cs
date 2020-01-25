using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    
	public AudioSource themeSource;
	public AudioSource stepSource;

	public AudioSource soundSource;
	public AudioSource enemySource;
    public AudioSource shotSource;

	[SerializeField]
	private AudioClip soundTrack;

    [SerializeField]
    private AudioClip buttonPress;

	[SerializeField]
	private AudioClip step;

	[SerializeField]
	private AudioClip laserTriggered;

    [SerializeField]
    private AudioClip playerDiscovered;

	[SerializeField]
	private AudioClip playerSpottedSound;

    [SerializeField]
    private AudioClip[] playerHit;

    [SerializeField]
    private AudioClip gunShot;

    [SerializeField]
    private AudioClip gunShotMenu;

    [SerializeField]
    private AudioClip gameOver;

    [SerializeField]
    private AudioClip playerScream;

    [SerializeField]
    private AudioClip mainMenu;

    [SerializeField]
    private AudioClip victory;

    [SerializeField]
    private AudioClip laserDisable;

    [SerializeField]
    private AudioClip pickUp;

    void Awake () {
        MakeSingleton(this);
		
		themeSource = gameObject.AddComponent<AudioSource>();
		stepSource = gameObject.AddComponent<AudioSource>();
		soundSource = gameObject.AddComponent<AudioSource>();
		enemySource = gameObject.AddComponent<AudioSource>();
        shotSource = gameObject.AddComponent<AudioSource>();

		soundSource.loop = false;
		soundSource.clip = step;
		soundSource.volume = Save.GetSoundVolume();

        enemySource.loop = false;
        enemySource.volume = Save.GetEnemySoundVolume();

        shotSource.loop = false;
        shotSource.clip = step;
        shotSource.volume = Save.GetEnemySoundVolume();

        stepSource.loop = false;
		stepSource.clip = step;
		stepSource.volume = Save.GetSoundVolume();

        //Setting and Playing the soundtrack
        themeSource.loop = true;
        themeSource.volume = Save.GetMusicVolume();
        PlayMainMenu();
	}

    public void PlayStepSound(){
		stepSource.Play();
	}

	public void PlayLaserTriggeredSound(){
		soundSource.clip = laserTriggered;
		soundSource.Play();
	}

    public void PlayPlayerHit() {
        soundSource.clip = playerHit[Random.Range(0, playerHit.Length)];
        if(soundSource.clip)
            soundSource.Play();
    }

    public IEnumerator PlayPlayerDiscovered() {
		enemySource.clip = playerSpottedSound;
		enemySource.Play();
		yield return new WaitForSeconds(.5f);
        themeSource.clip = playerDiscovered;
        themeSource.Play();
		yield return null;
    }

    public void PlaySoundTrack() {
        themeSource.clip = soundTrack;
        themeSource.Play();
    }

    public void PlayMainMenu() {
        themeSource.clip = mainMenu;
        themeSource.Play();
    }

    public void PlayGunShot() {
        shotSource.clip = gunShot;
        shotSource.Play();
    }

    public void PlayGameOver() {
        if(themeSource.clip != gameOver) {
            themeSource.loop = false;
            themeSource.clip = gameOver;
            themeSource.Play();
        }
    }

    public void PlayLaserSwitch() {
        soundSource.clip = laserDisable;
        soundSource.Play();
    }

    public void PlayVictory(){
        themeSource.loop = false;
        themeSource.clip = victory;
        themeSource.Play();
    }

    public void PlayPlayerScream() {
        soundSource.clip = playerScream;
        soundSource.Play();
    }

    public void PlayButtonSound(){
        soundSource.clip = buttonPress;
        soundSource.Play();
    }

    public void PlayPickUp(){
        soundSource.clip = pickUp;
        soundSource.Play();
    }

    public void PlayConfirmSound(){
        soundSource.clip = gunShotMenu;
        soundSource.Play();
    }

    public IEnumerator MusicFadeOut(System.Action callback, float fadingSpeed = 1f, float timer=3){
        while(timer > 0){
            themeSource.volume -= 0.1f * Time.deltaTime * fadingSpeed;
            timer -= Time.deltaTime;
            yield return null;
        }
        if (callback != null) {
            callback();
        }
        themeSource.volume = GameManager.instance.musicSlider.value;
        yield return null;
    }
}
