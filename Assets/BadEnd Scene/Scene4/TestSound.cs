using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    public AudioClip collisionSound; // ���� ������������
    public AudioSource audioSource; // �������� �����

    void Start()
    {
        // �������� ��������� AudioSource
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Play");
        // ������������� ���� ��� ������������
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(collisionSound);
    }
}
