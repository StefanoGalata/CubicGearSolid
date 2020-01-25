using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingPatrolAI : MonoBehaviour ,IPatrol {

	[SerializeField] private float patrollingSpeed = 2f;
	[SerializeField] private float chasingSpeed = 5f;
	[SerializeField] private float chaseWaitTime = 5f;
	[SerializeField] private float patrolWaitTime = 2f;
	[SerializeField] Transform[] patrolWaypoints;
	[SerializeField] private float shootingStoppingDistance = 5.5f;
	[SerializeField] private float patrollingStoppingDistance = .5f;
	[SerializeField] private float rotationSpeed = .3f;
	[SerializeField] private float reloadingTime;
	[SerializeField] private Transform firingPoint;
    [SerializeField] private LayerMask layersDetectedByShoot;
    [SerializeField] private int ShootDamage = 1;
	[SerializeField] private GameObject muzzleFlash;

	private delegate void State();
	private State currentState;

    private Vector3 startingPos;
    private Quaternion startingRot;

	private EnemySight enemySight;
	private NavMeshAgent nav;
	private Animator anim;

	private float chaseTimer;
	private float patrolTimer;
	private float shootingTimer = 0;

	private int wayPointIndex = 0;

    void Start() {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (patrolWaypoints.Length <= 0) { 
            startingPos = transform.position;
            startingRot = transform.rotation;
        }
		currentState += Patrol;
		nav.isStopped = true;
	}

	void Update(){
		anim.SetBool("IsMoving",!nav.isStopped);
		if (GameManager.instance.PlayerDied) {
            nav.isStopped = true;
            return;
        }
		nav.stoppingDistance = patrollingStoppingDistance;
		currentState();
	}

	void RotateToPlayer(){
		Quaternion newQuat = Quaternion.LookRotation(GetComponent<EnemySight>().currentPlayerPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newQuat, rotationSpeed);
	}

    Vector3 GetNextWaypointPosition() {
        //wayPointIndex = (wayPointIndex == patrolWaypoints.Length - 1) ? 0 : wayPointIndex+1;
		if(wayPointIndex == patrolWaypoints.Length-1)
			wayPointIndex = 0;
		else
			wayPointIndex +=1; 
        return patrolWaypoints[wayPointIndex].position;
    }

    public void Attack(){
        if (!GameManager.instance.PlayerDied) {
            RotateToPlayer();
            SetNextDestination(EnemyManager.instance.LastPlayerKnownPosition);
        }
    
        if (nav.remainingDistance < shootingStoppingDistance && enemySight.playerInSight){
			nav.isStopped = true;
			shootingTimer+=Time.deltaTime;
			if(shootingTimer>reloadingTime){
				shootingTimer = 0;
				anim.SetTrigger("IsShooting");
			}
		}
        else{
			//currentState -= Attack;
			//currentState += Chase;
			currentState = Chase;
		}
	}

	public void Chase(){
		if(enemySight.playerInSight)
			RotateToPlayer();
        nav.speed = chasingSpeed;
        SetNextDestination(EnemyManager.instance.LastPlayerKnownPosition);
        nav.isStopped = false;
		if(enemySight.playerInSight && nav.remainingDistance < shootingStoppingDistance){
			currentState += Attack;
			currentState -= Chase;
		}
		else if(nav.remainingDistance < nav.stoppingDistance){
			nav.isStopped = true;
		 	chaseTimer += Time.deltaTime;
		 	if(chaseTimer >= chaseWaitTime){
		 		EnemyManager.instance.LastPlayerKnownPosition = EnemyManager.instance.ResetPosition;
				chaseTimer = 0f; 
				patrolTimer = 0f;
				nav.stoppingDistance = patrollingStoppingDistance;
				SetNextDestination(patrolWaypoints[wayPointIndex].position);
				nav.isStopped = false;
				//currentState -= Chase;
				//currentState += Patrol;
				currentState = Patrol;
		 	}
		}
		else
			chaseTimer = 0f;
	}

	public void Patrol(){
		nav.speed = patrollingSpeed;
		if(nav.remainingDistance < nav.stoppingDistance){
			nav.isStopped = true;
			patrolTimer += Time.deltaTime;
			if(patrolTimer >= patrolWaitTime){
				patrolTimer = 0f;
                Vector3 pos;
                if (patrolWaypoints.Length > 0)
                    pos = GetNextWaypointPosition();
                else {
                    pos = startingPos;
                    if (nav.isStopped)
                        transform.rotation = startingRot;
                }
                SetNextDestination(pos);
            }
		}

		else{
			patrolTimer = 0f;
		}

		if(nav.isStopped && nav.remainingDistance > nav.stoppingDistance)
			nav.isStopped = false;

	}

	void Shoot(){
        RaycastHit hit;
		GameObject muzzleFlashParticle = Instantiate(muzzleFlash, firingPoint.position, firingPoint.rotation);
		if(Physics.Raycast(transform.position + transform.up, (EnemyManager.instance.LastPlayerKnownPosition-transform.position).normalized, out hit,GetComponent<SphereCollider>().radius, layersDetectedByShoot)) {
            if(hit.collider.tag == "Player") {
                hit.collider.GetComponent<PlayerHealth>().TakeDamage(ShootDamage);
            }
        }
        AudioManager.instance.PlayGunShot();
		Destroy(muzzleFlashParticle, 1f);
	}

	void SetNextDestination(Vector3 dest){
		nav.destination = dest;
	}

	public void OnNotify(){
		if(currentState == Patrol){
			currentState += Chase;
			currentState -= Patrol;
		}
	}


}
