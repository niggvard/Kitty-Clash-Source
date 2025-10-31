using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private UnitSelectButton[] buttonList;

    private void Start()
    {
        var UnitList = SavesManager.SelectedUtins;
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].Setup(UnitList[i]);
        }

        //Dropper.Instance.SelectUnit(UnitList[0]);
        buttonList[0].GetComponent<Button>().onClick.Invoke();
    }
}
