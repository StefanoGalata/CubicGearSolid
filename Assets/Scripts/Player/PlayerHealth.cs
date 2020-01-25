using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private int startingHealth = 5;
    [SerializeField] private GameObject bloodParticleObj;

    private int health;

    public int StartingHealth{
        get {return startingHealth;}
    }

    void Start() {
        health = startingHealth;
        gameObject.tag = "Player";
    }

    IEnumerator PlayBloodParticle(){
        bloodParticleObj.SetActive(true);
        ParticleSystem bloodParticle = bloodParticleObj.GetComponent<ParticleSystem>();
        bloodParticle.Play();
        yield return new WaitForSeconds(bloodParticle.main.duration);
    }


    public void TakeDamage(int dmg) {
        GetComponent<PlayerInput>().StopCoroutine("Freeze");
        StartCoroutine(GetComponent<PlayerInput>().Freeze(.4f));
        StartCoroutine(PlayBloodParticle());
        health -= dmg;
        if(health <= 0) {
            AudioManager.instance.PlayPlayerScream();
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<Animator>().SetTrigger("IsDead");
            gameObject.tag = "Untagged";
            GameManager.instance.PlayerDied = true;
        }
        else{
            GetComponent<Animator>().SetTrigger("Hit");
            AudioManager.instance.PlayPlayerHit();
        }
        UIManager.instance.UpdateHealth(health);
    }
}
