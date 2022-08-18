using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    public GameObject inputField;
    public void GetTextField()
    {
        AlgorithmList.numberRepeat = Convert.ToInt32(inputField.GetComponent<Text>().text);
    }
}
