using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;
using UnityEngine.Assertions.Must;

public enum PlayerStateType
{
    MOVEMENT = 0,
    ATTACK,
    RECHARGE,
}

public class PlayerState : FSMState
{
    protected Player mPlayer = null;

    public PlayerState(Player player) 
        : base()
    {
        mPlayer = player;
        mFsm = mPlayer.mFsm;
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
}

public class PlayerState_MOVEMENT : PlayerState
{
    public PlayerState_MOVEMENT(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.MOVEMENT);
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

        mPlayer.Move();

        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i])
            {
                PlayerState_ATTACK attack = (PlayerState_ATTACK)mFsm.GetState((int)PlayerStateType.ATTACK);

                attack.AttackID = i;
                mPlayer.mFsm.SetCurrentState((int)PlayerStateType.ATTACK);

                //mPlayer.
            }
        }
    }

    //IEnumerator Coroutine_AttackLength()

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class PlayerState_ATTACK : PlayerState
{
    private int mAttackID = 0;
    private string mAttackName;
    private float mAttackAnimDuration;

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

    public PlayerState_ATTACK(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.ATTACK);
    }

    public override void Enter()
    {
        //mAttackAnimDuration = mPlayer
        mPlayer.mAnimator.SetBool(mAttackName, true);
    }
    public override void Exit()
    {
        mPlayer.mAnimator.SetBool(mAttackName, false);
    }
    public override void Update()
    {
        base.Update();

        // For Student ---------------------------------------------------//
        // Implement the logic of attack, reload and revert to movement. 
        //----------------------------------------------------------------//
        // Hint:
        //----------------------------------------------------------------//
        // 1. Transition to RELOAD
        // Notice that we have three variables, viz., 
        // mAmunitionCount
        // mBulletsInMagazine
        // mMaxAmunitionBeforeReload
        // You will need to make use of these variables while
        // implementing the transition to RELOAD.
        //
        // 2. Staying in ATTACK state
        // You should stay in ATTACK state as long as the 
        // Fire buttons are pressed. During ATTACK state
        // you should trigger the correct ATTACK animation
        // based on which button is pressed and shoot bullets.
        // Every bullet shot should reduce the count of mAmunitionCount
        // and mBulletsInMagazine.
        // Once mBulletsInMagazine reaches to 0 you should 
        // transit to RELOAD state.
        //
        // 3. Transition to MOVEMENT state
        // You should transit to MOVEMENT state when any of the 
        // following two situations happen.
        // First you have exhausted all your bullets, that means your
        // mAmunitionCount is 0 or if you do not press any of the
        // Fire buttons.
        // Discuss with your tutor if you find any difficulties
        // in implementing this section.        

        // For tutor - start ---------------------------------------------//
        //    Debug.Log("Ammo count: " + mPlayer.mAttacksCount + ", In Magazine: " + mPlayer.mAttacksLeft);
        //    if (mPlayer.mAttacksLeft == 0 && mPlayer.mAttacksCount > 0)
        //    {
        //        mPlayer.mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
        //        return;
        //    }

        //    if (mPlayer.mAttacksCount <= 0 && mPlayer.mAttacksLeft <= 0)
        //    {
        //        mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        //        mPlayer.NoAttacks();
        //        return;
        //    }

        if (mPlayer.mAttackButtons[mAttackID])
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
            mPlayer.mCurrentAttackID = mAttackID;
            AnimatorStateInfo stateInfo = mPlayer.mAnimator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;
            mPlayer.StartCoroutine(mPlayer.Coroutine_Attacking(mAttackID, animLength));
        }
        //else
        //{
        //    mPlayer.mAnimator.SetBool(mAttackName, false);
        //    mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        //}
        //    // For tutor - end   ---------------------------------------------//
        //}
    }
}

public class PlayerState_RECHARGE : PlayerState
{
    public float RechargeTime = 3.0f;
    float dt = 0.0f;
    public int previousState;
    public PlayerState_RECHARGE(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.RECHARGE);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetTrigger("Recharge");
        mPlayer.Recharge();
        dt = 0.0f;
    }
    public override void Exit()
    {
        //if (mFsm.GetCurrentState() != mPlayer.mFsm.GetState((int)PlayerStateType.MOVEMENT))
        //{
        //    mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        //}
        //mPlayer.isRecharging = false;
    }

    public override void Update()
    {
        dt += Time.deltaTime;
        if (dt >= RechargeTime)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }

    public override void FixedUpdate()
    {
    }
}