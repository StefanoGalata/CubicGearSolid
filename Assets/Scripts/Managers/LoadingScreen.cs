using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : Singleton<LoadingScreen> {
	void Awake(){
		MakeSingleton(this);
		DontDestroyOnLoad(gameObject);
	}
}
