using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol {
	void Patrol();
	void Chase();
	void Attack();
	void OnNotify();
}
