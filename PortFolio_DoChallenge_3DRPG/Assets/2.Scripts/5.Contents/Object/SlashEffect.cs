using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    float damage;
    Rigidbody _rb;
    ParticleSystem _ps;

    public float Damage { get { return damage; } }

    void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _ps = GetComponentInChildren<ParticleSystem>();
        damage = 0;
    }

    public void StartEffect(Vector3 dir, float dmg, float power = 5f)
    {
        if(_rb== null || _ps == null)
            Init();

        StopCoroutine(ParticleEffect(dir, dmg, power));
        StartCoroutine(ParticleEffect(dir, dmg, power));
    }

    IEnumerator ParticleEffect(Vector3 dir,float dmg, float power = 5)
    {
        damage = dmg;
        if (dir != Vector3.zero)
        {
            _ps.Play();
            _rb.AddForce(dir * power, ForceMode.Impulse);
        }

        while (_ps.IsAlive(true))
        {
            yield return null;
        }
        gameObject.DestroyAPS();
    }
}
