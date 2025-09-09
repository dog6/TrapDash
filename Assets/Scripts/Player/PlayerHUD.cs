using UnityEngine;


public class PlayerHUD : MonoBehaviour
{
    public HUDHandler hud;

    void Start()
    {
        hud = GameObject.FindWithTag("HUD").GetComponent<HUDHandler>();
    }

    int totalDeaths; // total amount of deaths in current level
    int moneyHeld; // amount of money player has collected

    public void ResetDeathCounter() => totalDeaths = 0;
    public void AddDeath() => totalDeaths++;
    public void SetMoney(int money) => moneyHeld = money;

    void UpdateDeathCounterLabel() => hud.SetDeathCounter(totalDeaths);
    void UpdateMoneyLabel() => hud.SetMoney(moneyHeld);

    void LateUpdate()
    {
        UpdateDeathCounterLabel();
        UpdateMoneyLabel();
    }

}