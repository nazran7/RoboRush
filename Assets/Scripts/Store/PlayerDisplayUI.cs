using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayUI : MonoBehaviour
{
    //store controller component
    [SerializeField] private StoreController storeController;
    //all upgrade sprites
    [SerializeField] private SpriteRenderer[] legsSR;
    [SerializeField] private Sprite[] legSprites;
    [SerializeField] private SpriteRenderer rightArmSR;
    [SerializeField] private Sprite[] rightArmSprites;
    [SerializeField] private SpriteRenderer leftArmSR;
    [SerializeField] private Sprite[] leftArmSprites;
    [SerializeField] private SpriteRenderer bodySR;
    [SerializeField] private Sprite[] bodySprites;


    private void Start()
    {
        //subscribe on ui update event
        storeController.OnUpgradeBuy += DisplayParts;
        Initialization();
    }
    private void OnDisable()
    {
        storeController.OnUpgradeBuy -= DisplayParts;
    }
    //ui update method for every bodypart
    private void DisplayParts(SaveSystem.Type type, int currentLevel)
    {
        if (type == SaveSystem.Type.jetPackLevel)
        {
            foreach (SpriteRenderer sr in legsSR)
            {
                sr.sprite = legSprites[currentLevel];
            }
        }
        else if (type == SaveSystem.Type.boostLevel)
        {
            leftArmSR.sprite = leftArmSprites[currentLevel];
        }
        else if (type == SaveSystem.Type.healthLevel)
        {
            bodySR.sprite = bodySprites[currentLevel];
        }
        else if (type == SaveSystem.Type.weaponLevel)
        {
            rightArmSR.sprite = rightArmSprites[currentLevel];
        }
    }
    //load current upgrade level
    private void Initialization()
    {
        foreach (SpriteRenderer sr in legsSR)
        {
            sr.sprite = legSprites[SaveSystem.LoadData(SaveSystem.Type.jetPackLevel)];
        }

        leftArmSR.sprite = leftArmSprites[SaveSystem.LoadData(SaveSystem.Type.boostLevel)];
        bodySR.sprite = bodySprites[SaveSystem.LoadData(SaveSystem.Type.healthLevel)];
        rightArmSR.sprite = rightArmSprites[SaveSystem.LoadData(SaveSystem.Type.weaponLevel)];
    }

}
