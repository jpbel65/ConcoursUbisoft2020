﻿using UnityEngine;


    public class PuzzleGenerator : MonoBehaviour
    {
        public PositionSelector[] movableObjectList;
        public GameManager GameManager;
        // Start is called before the first frame update
        // on start : // for loop sur les objet et call "randomSelect"
        void Start()
        {
            foreach (var positionSelector in movableObjectList)
            {
                positionSelector.SetLocalPlayerName(GameManager.GetLocalPlayerName());
                // si player1, randomSelect
                positionSelector.RandomSelectPosition();
                // sinon rien (dans position select, on va envoyer en réseau la valeur de l'index)
            }
        }
        // recoit un call que c'Est une autre playthrough
        // set isPLaythrouhg = false
        // randomSelect
        
    }

