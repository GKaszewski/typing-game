using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameLetter : MonoBehaviour {
    private Rigidbody2D _rb;
    private TextMeshPro _text;
    [SerializeField] private float lifeTime = 5f;
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _text = GetComponentInChildren<TextMeshPro>();
    }
    
    private void OnEnable() {
        StartCoroutine(DisableAfterTime());
    }

    private IEnumerator DisableAfterTime() {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
    
    public void Spawn(Vector3 position, Vector2 direction, float force, string letter) {
        gameObject.SetActive(true);
        transform.position = position;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        _text.text = letter;
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        if (!col.collider.CompareTag("Killzone")) return;
        gameObject.SetActive(false);
    }
}