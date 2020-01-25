using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save{

	public static void SaveMusicVolume(float value){
		value = Mathf.Clamp01(value);
		PlayerPrefs.SetFloat("MUSIC", value);
	}

	public static void SaveSoundVolume(float value){
		value = Mathf.Clamp01(value);
		PlayerPrefs.SetFloat("SOUND",value);
	}

	public static void SaveEnemySoundVolume(float value){
		value = Mathf.Clamp01(value);
		PlayerPrefs.SetFloat("ENEMYSOUND", value);
	}

	public static float GetMusicVolume(){
		return PlayerPrefs.GetFloat("MUSIC", .7f);
	}

	public static float GetSoundVolume(){
		return PlayerPrefs.GetFloat("SOUND", 1);
	}

	public static float GetEnemySoundVolume(){
		return PlayerPrefs.GetFloat("ENEMYSOUND", 1);
	}

	public static float[] GetScores(){
		float[] array = {PlayerPrefs.GetFloat("SCORE1", Mathf.Infinity), PlayerPrefs.GetFloat("SCORE2",Mathf.Infinity), PlayerPrefs.GetFloat("SCORE3", Mathf.Infinity)};
		return array;
	}

	public static void SaveScore(string key, float value){
		PlayerPrefs.SetFloat(key, value);
	}

	public static string[] GetPlayersName(){
		string[] array = {PlayerPrefs.GetString("NAME1", ""), PlayerPrefs.GetString("NAME2",""), PlayerPrefs.GetString("NAME3", "")};
		return array;
	}

	public static void SavePlayerName(string key, string value){
		PlayerPrefs.SetString(key, value);
	}

	public static void SaveQualitySettings(int index){
		PlayerPrefs.SetInt("QUALITY", index);
	}

	public static int GetQualitySettings(){
		return PlayerPrefs.GetInt("QUALITY", 3);
	}

}
