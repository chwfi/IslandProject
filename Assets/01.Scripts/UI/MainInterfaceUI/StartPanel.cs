using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private float _fadeTime;

    private void Start()
    {
        transform.GetComponent<Image>().DOFade(0, _fadeTime);    
    }
}
