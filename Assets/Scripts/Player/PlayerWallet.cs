using UnityEngine;

[RequireComponent(typeof(PlayerHUD))]
[RequireComponent(typeof(PlayerSoundHandler))]
public class PlayerWallet : MonoBehaviour
{

    PlayerHUD playerHUD;
    PlayerSoundHandler playerSound;

    int moneyStored; // money stored in wallet

    void Start()
    {
        playerHUD = GetComponent<PlayerHUD>();
        playerSound = GetComponent<PlayerSoundHandler>();
    }
    public void PickupMoney(int value)
    {

        moneyStored += value;
        SaveState.PickupCoins(value);
        playerHUD.SetMoney(moneyStored);
        playerSound.PlayCoinPickupSFX();
    }



}