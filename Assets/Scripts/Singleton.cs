using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{

	public static T instance;

	protected void MakeSingleton(T obj){
		if(instance == null)
			instance = obj;
		else if (instance != obj)
			Destroy(obj.gameObject);
	}
}
