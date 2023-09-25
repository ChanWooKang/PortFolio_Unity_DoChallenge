using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    TrailRenderer trail;

    private void Awake()
    {
        
        
    }

    void Init()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
    }

    public void WeaponEvent()
    {
        if (trail == null)
            Init();
        StopCoroutine(AttackEvent());
        StartCoroutine(AttackEvent());

    }

    IEnumerator AttackEvent()
    {
        yield return new WaitForSeconds(0.1f);
        trail.enabled = true;
        yield return new WaitForSeconds(0.4f);
        trail.enabled = false;
    }
}
