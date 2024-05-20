using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : NPC_1WayInteractive
{



    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogStep = 0;
        isTyping = false;
    }




    private void Update()
    {
        if (checkInteractable())
        {
            promptIcon.SetActive(true);
        }
        else
        {
            promptIcon.SetActive(false);
            if (!(dialogStep == 0))
            {
                textLabel.text = "";
                dialogStep = 0;
                dialogBox.SetActive(false);
            }
        }
        if (checkInteractable() && Input.GetKeyDown("e"))
        {
            interact();
        }
    }
}
