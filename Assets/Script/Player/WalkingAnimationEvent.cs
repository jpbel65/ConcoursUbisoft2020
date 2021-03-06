﻿using System;
using UnityEngine;

public enum FootPrintEnum
{
    LEFT,
    RIGHT
}
public class WalkingAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject rightFootPrint;
    [SerializeField] private GameObject leftFootPrint;

    private FootPrint[] footPrintLeftInit = new FootPrint[6];
    private FootPrint[] footPrintRightInit = new FootPrint[6];
    private FootPrintEnum footprintToSpawn = FootPrintEnum.LEFT;
    private int rightIndex = 0;
    private int leftIndex = 0;

    private void Start()
    {
        for (int i = 0; i < footPrintLeftInit.Length; i++)
        {
            footPrintLeftInit[i] = Instantiate(leftFootPrint).GetComponent<FootPrint>();
        }
        
        for (int i = 0; i < footPrintRightInit.Length; i++)
        {
            footPrintRightInit[i] = Instantiate(rightFootPrint).GetComponent<FootPrint>();
        }
    }

    private void SpawnFootPrint(FootPrintEnum footPrintEnum)
    {
        FootPrint footPrint;
        if (footPrintEnum == FootPrintEnum.LEFT)
        {
            footPrint =  GetLeftFootPrint();
            footprintToSpawn = FootPrintEnum.RIGHT;
        }
        else
        {
            footPrint = GetRightFootPrint();
            footprintToSpawn = FootPrintEnum.LEFT;
        }
        
        footPrint.gameObject.transform.position = transform.position;
        footPrint.gameObject.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        footPrint.Spawn();
    }

    public FootPrint GetRightFootPrint()
    {
       
        FootPrint footPrint = footPrintRightInit[rightIndex];
        
        rightIndex++;
        rightIndex %= footPrintRightInit.Length;
        
        return footPrint;
    }
    
    public FootPrint GetLeftFootPrint()
    {
        FootPrint footPrint = footPrintLeftInit[leftIndex];
        
        leftIndex++;
        leftIndex %= footPrintLeftInit.Length;
        
        return footPrint;
    }

    public void playStepSound(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.clip.name == "RunLeft" || evt.animatorClipInfo.clip.name == "RunRight")
        {
            if (Math.Abs(evt.animatorClipInfo.weight - 1f) > 0.01)
            {
                return;
            }
        }
        
        SoundsManager.instance.RandomizeSfx(); // joue les sons de pas aléatoirement 
        SpawnFootPrint(footprintToSpawn);
    }
}
