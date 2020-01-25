using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : Singleton<UI> {

	void Awake(){
		MakeSingleton(this);
		DontDestroyOnLoad(gameObject);
	}
}
