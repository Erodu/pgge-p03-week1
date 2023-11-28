using PGGE.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant : MonoBehaviour
{
    [HideInInspector]
    public FSM mFsm = new FSM();
    public Animator mAnimator;
    public PlayerMovement mPlayerMovement;

    //public int mMaxAttacksBeforeRecharge = 10;

    //public int mAttacksLeft = 10;

    public bool[] mAttackButtons = new bool[1];

    private void Start()
    {
        
    }

    private void Update()
    {
        mFsm.Update();

        if (Input.GetButton("Fire1"))
        {
            mAttackButtons[0] = true;
        }
        else
        {
            mAttackButtons[0] = false;
        }
    }

    public void Move()
    {
        mPlayerMovement.HandleInputs();
        mPlayerMovement.Move();
    }

    public void Attack(int id)
    {
        //if (mAttackButtons[0])
    }
}
