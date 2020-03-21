using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public bool inRange = false;
    GameObject Player;
    Animator anim2d;

    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player");
        anim2d = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    private void Update() {
        if (inRange && Player.GetComponent<PlayerController>().running) {
            anim2d.SetBool("Alert", true);
        }

        float offset = Player.transform.position.x - transform.position.x;
        anim2d.SetFloat("PlayerPos", offset);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") 
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
            inRange = false;
    }
}
