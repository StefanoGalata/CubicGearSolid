using UnityEngine;

public class EnemyManager : Singleton<EnemyManager> {
	private Vector3 resetPosition = new Vector3(1000,1000,1000);
	private Vector3 lastPlayerKnownPosition = new Vector3(1000,1000,1000);
   

	public GameObject[] enemies;

    public Vector3 LastPlayerKnownPosition {
        get { return lastPlayerKnownPosition; }
        set { 
				lastPlayerKnownPosition = value;
				if(value != resetPosition)
					NotifyEnemies();
			}
    }

    public Vector3 ResetPosition {
        get { return resetPosition; }
    }

	void Awake(){
		MakeSingleton(this);
		GetEnemies();
	}

    public void GetEnemies() {
        enemies = GameObject.FindGameObjectsWithTag("Patrol");
    }


    public void NotifyEnemies(){
		for(int i = 0; i<enemies.Length; i++){
			ShootingPatrolAI PatrolAI = enemies[i].GetComponent<ShootingPatrolAI>();
			if(PatrolAI){
				PatrolAI.OnNotify();
			}
		}
	}
}
