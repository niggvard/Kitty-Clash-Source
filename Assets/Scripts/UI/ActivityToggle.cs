using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityToggle : MonoBehaviour
{
    public GameObject target;
    public void Swap()
    {
        target.SetActive(!target.activeSelf);
    }
}
