using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class Item : MonoBehaviour
{
    public SOItem itemSO;
    Rigidbody _rb;
    SphereCollider _colider;

    bool isShoot = false;
    int gold;

    public int Gold { get { return gold; } set { gold = value; } }
    void Start()
    {
        Init();
    }

    void Update()
    {
        if(_rb.isKinematic && isShoot)
            transform.Rotate(30.0f * Vector3.up * Time.deltaTime);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(Util.ConvertEnum(TagType.Floor)))
        {
            _rb.isKinematic = true;
            _colider.enabled = false;
        }
    }

    void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _colider = GetComponent<SphereCollider>();
        isShoot = false;
    }

    Vector3 RandomPointInSphere(float radius = 2)
    {
        Vector3 pos = Random.onUnitSphere;
        pos.y = 0.5f;
        float r = Random.Range(0, radius);
        return pos * r;
    }

    public void Shoot(float power)
    {
        if (_rb == null)
            Init();
        Vector3 dir = RandomPointInSphere();
        _rb.AddForce(dir * power, ForceMode.Impulse);
        isShoot = true;
    }

    public bool Root()
    {
        PlayerCtrl player = PlayerCtrl._inst;
        if (itemSO.iType == ItemType.Gold)
        {
            player.EarnMoney(gold);
            Despawn();
            return true;
        }
        else
        {
            if (InventoryManager._inst.CheckSlotFull(itemSO) == false)
            {
                InventoryManager._inst.AddInvenItem(itemSO);
                Despawn();
                return true;
            }
            else
            {
                Debug.Log("¿Œ∫•≈‰∏Æ∞° ≤À√°Ω¿¥œ¥Ÿ....");
                return false;
            }
        }

        
    }

    void Despawn()
    {
        gameObject.DestroyAPS();
        _rb.isKinematic = false;
        _colider.enabled = true;
        isShoot = false;
    }
}
