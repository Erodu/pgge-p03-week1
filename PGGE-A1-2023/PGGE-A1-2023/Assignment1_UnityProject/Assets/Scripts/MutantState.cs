using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

public enum MutantStateType
{
    MOVEMENT = 0,
    ATTACK,
    RECHARGE,
    JUMP,
    CROUCH,
}

public class MutantState : FSMState
{
    protected Mutant mMutant = null;

    public MutantState(Mutant player)
        : base()
    {
        mMutant = player;
        mFsm = mMutant.mFsm;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public class MutantState_MOVEMENT : MutantState
    {
        public MutantState_MOVEMENT(Mutant player) : base(player)
        {
            mId = (int)(MutantStateType.MOVEMENT);
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            // For Student ---------------------------------------------------//
            // Implement the logic of player movement. 
            //----------------------------------------------------------------//
            // Hint:
            //----------------------------------------------------------------//
            // You should remember that the logic for movement
            // has already been implemented in PlayerMovement.cs.
            // So, how do we make use of that?
            // We certainly do not want to copy and paste the movement 
            // code from PlayerMovement to here.
            // Think of a way to call the Move method. 
            //
            // You should also
            // check if fire buttons are pressed so that 
            // you can transit to ATTACK state.

            mMutant.Move();

            for (int i = 0; i < mMutant.mAttackButtons.Length; ++i)
            {
                if (mMutant.mAttackButtons[i])
                {
                    mMutant.mAnimator.SetBool("Attack" + i.ToString(), true);

                    
                }
            }
        }
    }
}

public class MutantState_ATTACK : MutantState
{
    private int mAttackID = 0;
    private string mAttackName;

    public int AttackID
    {
        get
        {
            return mAttackID;
        }
        set
        {
            mAttackID = value;
            mAttackName = "Attack" + (mAttackID + 1).ToString();
        }
    }

    public MutantState_ATTACK(Mutant player) : base(player)
    {
        mId = (int)MutantStateType.ATTACK;
    }

    public override void Enter()
    {
        mMutant.mAnimator.SetBool(mAttackName, true);
    }

    public override void Exit()
    {
        mMutant.mAnimator.SetBool(mAttackName, false);
    }

    public override void Update()
    {
        //base.Update();

        //if (mMutant.mAttacksLeft == 0)
        //{
        //    mMutant.mFsm.SetCurrentState((int)MutantStateType.RECHARGE);
        //    return;
        //}

        //if (mMutant.mAttackButtons[mAttackID])
        //{
        //    mMutant.mAnimator.SetBool(mAttackName, true);
        //    //mMutant
        //}
    }
}