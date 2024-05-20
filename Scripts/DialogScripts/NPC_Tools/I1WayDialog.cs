using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I1WayDialog
{
    public IEnumerator typewriterWrite(string text, float delay = 0.025f);

    public void show1WayDialog(string text);

    public void clear1WayDialog();

    public void check1WayDialogArray();
}

