using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentButtonController : ButtonController
{
    private IEnumerator buttonHitCoroutine;

    public void HitButton()
    {
        if(buttonHitCoroutine != null)
        {
            StopCoroutine(buttonHitCoroutine);
        }

        buttonHitCoroutine = ButtonAction();
        StartCoroutine(buttonHitCoroutine);
    }

    private IEnumerator ButtonAction()
    {
        ButtonPressAction();
        yield return new WaitForSeconds(0.1f);
        LiftButton();
    }

    private void LiftButton()
    {
        ButtonLiftAction();
    }
}
