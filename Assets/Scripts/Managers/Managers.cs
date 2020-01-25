using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers> {

	void Awake(){
		MakeSingleton(this);
		DontDestroyOnLoad(gameObject);
	}
}
