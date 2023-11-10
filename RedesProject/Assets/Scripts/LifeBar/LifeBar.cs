using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Transform _targetTransform;

    [SerializeField] float _yOffset;
    [SerializeField] Image _myFillableImage;

    public void SetTarget(TankController model)
    {
        _targetTransform = model.transform;

        model.OnLifeChange += UpdateBar;
    }
    
    void UpdateBar(float amount)
    {
        _myFillableImage.fillAmount = amount;
    }


    public void UpdatePosition()
    {
        transform.position = _targetTransform.position + Vector3.up * _yOffset;
    }


}
