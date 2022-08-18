using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public void SliderChangend()
    {
        StartWork.speedWorkAlgorithm = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>().value;
    }
}
