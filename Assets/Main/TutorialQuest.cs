using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using System;

public class TutorialQuest : MonoBehaviour
{
    private int _hitCount;

    private void OnEnable()
    {
        UFE.OnHit += OnHit;
    }

    private void OnHit(HitBox strokeHitBox, MoveInfo move, ControlsScript player)
    {
        Debug.Log(move.name);
    }

    private void OnDisable()
    {
        UFE.OnHit -= OnHit;

    }

   
}
