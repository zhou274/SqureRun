using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D rb;
	public GameObject scoreUI;
	private TextMeshProUGUI scoreText;
	public float positionX = -6;
	public float positionY = 0.6f;
	public bool up = true;
	public float speedUp = 0.01f;
	private AudioSource changeLinesSound;
	public float timer = 0;
	public int score = 0;

	private int pauseButtonX;
	private int pauseButtonY;


	void Start() {
		changeLinesSound = GameObject.Find ("changeLines").GetComponent<AudioSource> ();
		scoreText = scoreUI.GetComponent<TextMeshProUGUI> ();
		pauseButtonX = Screen.width / 10;
		pauseButtonY = Screen.height - (int)(Screen.height - Screen.height / 1.2f);
	}
	// Update is called once per frame
	void FixedUpdate () {
		speedUp += 0.00001f;
		positionX += (0.05f + speedUp);
		rb.GetComponent<Rigidbody2D> ().position = new Vector2 (positionX, positionY);
	}
	void Update() {
		timer += Time.deltaTime;
		if (timer >= 1) {
			timer = 0;
			score += 10;
			scoreText.text = "得分: " + score;
		}
		if (Input.GetMouseButtonDown (0)) {
			if ((Input.mousePosition.y > pauseButtonY && Input.mousePosition.x < pauseButtonX) == false) {//detect if player has not clicked on pause button on screen
				changeLinesSound.Play ();
				if (up) {
					up = false;
					positionY = -0.6f;
				} else {
					up = true;
					positionY = 0.6f;
				}
			}
		}


	}
	void OnTriggerEnter2D(Collider2D col) {
		GameObject.Find ("Canvas").GetComponent<MenuSelect> ().gameOver ();
		Destroy (col.gameObject);
	}
	public void Respawn()
	{
		StartCoroutine(CancelCollider());
	}
	IEnumerator CancelCollider()
	{
		transform.GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(2f);
        transform.GetComponent<BoxCollider2D>().enabled = true;
    }
}
