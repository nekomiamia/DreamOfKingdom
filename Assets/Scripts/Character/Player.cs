using System;
using UnityEngine;


public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;
    public int CurMana
    {
        get => playerMana.curValue;
        set => playerMana.SetValue(value);
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurMana = playerMana.maxValue;
    }

    /// <summary>
    /// EventFunc
    /// </summary>
    public void NewTurn()
    {
        CurMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurMana -= cost;
        if (CurMana <= 0)
        {
            CurMana = 0;
        }
    }

    public void NewGame()
    {
        CurHP = MaxHP;
        isDead = false;
        GetComponent<BuffHolder>()?.RemoveAllBuffs();
        gameObject.SetActive(false);
        NewTurn();
    }
    
    public void BackToMap()
    {
        isDead = false;
        GetComponent<BuffHolder>()?.RemoveAllBuffs();
        gameObject.SetActive(false);
        NewTurn();
    }
}
