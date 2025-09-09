using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoneyPickup : MonoBehaviour
{

    public int Value; // Value of this moneypickup
    Collider2D moneyCollider;

    void Start()
    {
        moneyCollider = GetComponent<Collider2D>();
        moneyCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.transform != null && col.CompareTag("Player"))
        {

            PlayerWallet wallet;
            if (col.transform.gameObject.TryGetComponent<PlayerWallet>(out wallet))
            {
                wallet.PickupMoney(Value);
                Destroy(this.gameObject);
            }

        }

    }


}