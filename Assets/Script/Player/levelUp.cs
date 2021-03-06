﻿using System.Collections;
using System.Collections.Generic;
using Script.Audio;
using UnityEngine;

public class levelUp : MonoBehaviour
{
    [SerializeField] private string playerTagToBeLeveledUp; // le tag du joueur qui sera level up 
    private bool alreadyTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerTagToBeLeveledUp == "Player1")
        {
            GetComponent<ParticleSystem>().startColor = new Color(204/255f, 28/255f, 138/255f);
        }
        else
        {
            GetComponent<ParticleSystem>().startColor = new Color(209/255f, 183/255f, 23/255f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(playerTagToBeLeveledUp) && !alreadyTrigger))
        {
            
            if (playerTagToBeLeveledUp == "Player1")
            {
                AudioManager.Instance.Play("lvlup", transform);
                other.GetComponent<Jump>().IncreaseJumpForce();
                other.GetComponent<TutorialP1>().ActivatejumpUgradeForceText();
                alreadyTrigger = true;
            }

            if (playerTagToBeLeveledUp == "Player2")
            {
                other.GetComponent<TelekinesisAbility>().increaseMaxWeight();
                other.GetComponent<TutorialP2>().activateHeavyUpdateText();
                alreadyTrigger = true;
            }
        }
    }
    
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(alreadyTrigger);
        } else if (stream.isReading)
        {
            alreadyTrigger = (bool) stream.ReceiveNext();
        }
    }
}
