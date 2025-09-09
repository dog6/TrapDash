using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


// Static Element
// Players HUD
[RequireComponent(typeof(UIDocument))]
public class HUDHandler : MonoBehaviour
{

    private UIDocument pakUI;

    private UIDocument ui;
    private VisualElement root;
    private Label deathCounterLabel, moneyLabel, timerLabel;


    public void ShowPAKUI(bool state)
    {
        Debug.Log($"Setting PAKUI to {pakUI.enabled}");
        pakUI.enabled = state;
    }

    void Start()
    {
        pakUI = GameObject.FindWithTag("PAK").GetComponent<UIDocument>();
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        ReferenceUIElements();
        ShowPAKUI(false);
    }

    void ReferenceUIElements()
    {
        deathCounterLabel = root.Q<Label>("DeathCounter");
        moneyLabel = root.Q<Label>("Money");
        timerLabel = root.Q<Label>("Timer");
    }


    public void SetDeathCounter(int totalDeaths) => deathCounterLabel.text = $"Deaths: {totalDeaths}";
    public void SetMoney(int moneyHeld) => moneyLabel.text = $"${moneyHeld}";

    public void SetTime(string timeString) => timerLabel.text = $"Time: {timeString}";

}
