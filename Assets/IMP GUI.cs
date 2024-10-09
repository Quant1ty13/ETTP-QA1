using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMPGUI : MonoBehaviour
{
    public Animator animator;


   public void UIAnimation()
    {
        animator.SetTrigger("Start Ani");
    }
}
