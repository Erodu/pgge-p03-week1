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

        // Switch to the ATTACK state.
        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i])
            {
                PlayerState_ATTACK attack = (PlayerState_ATTACK)mFsm.GetState((int)PlayerStateType.ATTACK);

                attack.AttackID = i;
                mPlayer.mFsm.SetCurrentState((int)PlayerStateType.ATTACK);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class PlayerState_ATTACK : PlayerState
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

    public PlayerState_ATTACK(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.ATTACK);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetBool(mAttackName, true);
    }
    public override void Exit()
    {
        mPlayer.mAnimator.SetBool(mAttackName, false);
    }
    public override void Update()
    {
        base.Update();

        if (mPlayer.mAttackButtons[mAttackID])
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
            mPlayer.mCurrentAttackID = mAttackID;
            // Retrieve attack animation length.
            AnimatorStateInfo stateInfo = mPlayer.mAnimator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;
            mPlayer.StartCoroutine(mPlayer.Coroutine_Attacking(mAttackID, animLength));
        }
    }
}

public class PlayerState_RECHARGE : PlayerState
{
    public float RechargeTime = 5.0f;
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

    }

    public override void Update()
    {
        dt += Time.deltaTime;
        if (dt >= RechargeTime)
        {
            //Debug.Log("Recharge End");
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
            mPlayer.isRecharging = false;
        }
    }

    public override void FixedUpdate()
    {
    }
}