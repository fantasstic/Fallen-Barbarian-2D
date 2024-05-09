using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlay : MonoBehaviour
{
    [SerializeField] private SpineNew _scene;
    [SerializeField] private string _colliderTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == _colliderTag)
        {
            _scene.LickPlay = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _scene.LickPlay = false;
    }
}
