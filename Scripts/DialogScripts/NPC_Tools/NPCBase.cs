using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class NPCBase: MonoBehaviour, I1WayDialog, I2WayDialog, IInteractable
{
    //PUBLIC VARS
    public string[] dialogArray;
    public float INTERACT_DISTANCE;
    public GameObject promptIcon;
    public GameObject dialogBox;
    public Sprite PortraitSprite;
    public Image PortraitImage;

    // ABSTRACT METHODS

    public abstract IEnumerator typewriterWrite(string text, float delay = 0.025f);

    public abstract bool checkInteractable();

    public abstract void interact();

    public abstract void show1WayDialog(string text);

    public abstract void check1WayDialogArray();

    public abstract void clear1WayDialog();
}
