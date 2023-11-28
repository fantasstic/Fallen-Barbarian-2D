using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    public AudioClip collisionSound; // Звук столкновения
    public AudioSource audioSource; // Источник звука

    void Start()
    {
        // Получаем компонент AudioSource
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Play");
        // Воспроизводим звук при столкновении
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(collisionSound);
    }
}
