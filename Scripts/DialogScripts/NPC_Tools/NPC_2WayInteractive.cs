using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_1WayInteractive : NPCBase
{

    //PRIVATE VARS
    [SerializeField] protected TMP_Text textLabel;
    protected GameObject player;
    protected int dialogStep;
    protected bool isTyping;


    // OVERRIDE METHODS
    public override IEnumerator typewriterWrite(string text, float delay = 0.025f)
    {
        if (!isTyping)
        {
            isTyping = true;
            int currentChar = 0;
            int length = text.Length;
            while (currentChar <= length)
            {
                string currentStr = text.Substring(0, currentChar);
                textLabel.text = currentStr;
                yield return new WaitForSeconds(delay);
                currentChar++;
            }
            dialogStep++;
            isTyping = false;
        }
    }

    public override bool checkInteractable()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 npcPos = new Vector2(transform.position.x, transform.position.y);
        return Vector2.Distance(playerPos, npcPos) < INTERACT_DISTANCE;
    }

    public override void interact()
    {
        show1WayDialog(dialogArray[dialogStep]);
    }

    public override void check1WayDialogArray()
    {
        if (dialogStep >= dialogArray.Length)
        {
            clear1WayDialog();
        }
        else if (dialogStep == 0)
        {
            show1WayDialog(dialogArray[dialogStep]);
        }
        else if (dialogStep > 0)
        {
            show1WayDialog(dialogArray[dialogStep]);
        }
    }

    public override void show1WayDialog(string text)
    {
        PortraitImage.sprite = PortraitSprite;
        dialogBox.SetActive(true);
        StartCoroutine(typewriterWrite(dialogArray[dialogStep]));
    }

    public override void clear1WayDialog()
    {
        PortraitImage.sprite = null;
        textLabel.text = "";
        dialogStep = 0;
        dialogBox.SetActive(false);
    }
}
