using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goTo(Vector3 position)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(
            gameObject.transform.DOMove(position, 1).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                Destroy(gameObject);
            }));

                

    }
}
