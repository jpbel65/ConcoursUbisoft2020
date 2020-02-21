﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialP1 : Tutorial
{
    [SerializeField] private MonoBehaviour jumpScript;
    [SerializeField] private Text abilityTutorialText;
    
    // Start is called before the first frame update
    void Start()
    {
        abilityTutorialText.gameObject.SetActive(false);
        jumpScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            DesactivateTutorial();
        }
    }

    public override void ActivateTutorial()
    {
        abilityTutorialText.gameObject.SetActive(true);
        jumpScript.enabled = true;
        abilityTutorialText.text =
            "Appuyez sur A pour utilisé votre double saut pour atteindre la plaque sur la roche";
    }

    public override void DesactivateTutorial()
    {
        abilityTutorialText.gameObject.SetActive(false);
        this.enabled = false;
    }
}
