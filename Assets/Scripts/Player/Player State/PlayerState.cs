using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PState { RUN , SLOWRUN , SIDERUN , SLOWSIDERUN , SPRINT , JUMP , FEINT , JUKE , SPIN , SLIDE , FLIP , DEAD , WIN , LOSE , SLIP }


public abstract class PlayerState
{
    protected enum Event { ENTER , UPDATE , EXIT , DEAD , WIN , LOSE }


    public PState name;

    protected Event stage;

    protected PlayerState nextState;


    public Player player;
    public PlayerController controller;
    public Animator[] animators = new Animator[2];


    protected PlayerAttributesSO att;
    protected PlayerUniversalDataSO UD;

    protected float acc;
    protected float side;
    protected float startSide;

    protected float startTime;
    protected float animTime;


    protected bool IsRaining
    {
        get { return player.gameManager.gameData.gameWeather == GameWeather.RAIN; }
    }


    public PlayerState(Player _player)
    {
        stage = Event.ENTER;

        player = _player;

        controller = player.controller;
        att = controller.playerAtt;
        UD = controller.playerUD;

        animators[0] = player.fPPlayer.animator;
        animators[1] = player.tPPlayer.animator;
    }


    public virtual void Enter() 
    { 
        stage = Event.UPDATE;
        startTime = Time.time;
    }
    public virtual void Update()
    {
        acc = controller.Acceleration;
        side = controller.Direction;
    }
    public virtual void Exit() { stage = Event.EXIT; Debug.Log(name + " -> " + nextState.name); }



    public PlayerState Process()
    {
        if (stage == Event.DEAD) { return new DeadPS(player); }
        if (stage == Event.WIN) { return new WinPS(player); }
        if (stage == Event.LOSE) { return new LosePS(player); }

        if (stage == Event.ENTER) Enter();
        else if (stage == Event.UPDATE) Update();
        else if (stage == Event.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    protected void PlayerOrientation(bool front)
    {
        Vector3 direction = front ? Vector3.forward : controller.Velocity;

        if (player.gameManager.GameOn && controller.Velocity != Vector3.zero)
        {
            player.tPPlayer.transform.localRotation =
            Quaternion.Slerp(player.tPPlayer.transform.localRotation,
                Quaternion.LookRotation(direction, Vector3.up), 0.02f);

            player.fPPlayer.transform.localRotation =
                Quaternion.Slerp(player.fPPlayer.transform.localRotation,
                    Quaternion.LookRotation(direction, Vector3.up), 0.02f);
        }
    }

    protected void PlayerOrientation()
    {
        PlayerOrientation(false);
    }

    // Animators functions

    protected void SetTrigger(string name)
    {
        foreach (Animator a in animators)
        {
            a.SetTrigger(name);
        }
    }
    protected void ResetTrigger(string name)
    {
        foreach (Animator a in animators)
        {
            a.ResetTrigger(name);
        }
    }
    protected void SetFloat(string name, float value)
    {
        foreach (Animator a in animators)
        {
            a.SetFloat(name, value);
        }
    }

    public void SetWeapon(bool state, bool bigWeapon)
    {
        foreach (Animator a in animators)
        {
            a.SetLayerWeight(a.GetLayerIndex(bigWeapon ? "BigWeapon Layer" : "SmallWeapon Layer"), state ? 1 : 0);
        }
    }

    public void Shoot(bool fireArm)
    {
        foreach (Animator a in animators)
        {
            a.SetTrigger(fireArm ? "Shoot" : "Cut");
        }
    }

    private void DeactivateWeaponLayer()
    {
        foreach (Animator a in animators)
        {
            a.SetLayerWeight(a.GetLayerIndex("BigWeapon Layer"), 0);
            a.SetLayerWeight(a.GetLayerIndex("SmallWeapon Layer"), 0);
        }
    }

    public void Dead()
    {
        DeactivateWeaponLayer();

        stage = Event.DEAD;
        Process();
    }
    public void Win()
    {
        stage = Event.WIN;
        Process();
    }
    public void Lose()
    {
        DeactivateWeaponLayer();

        stage = Event.LOSE;
        Process();
    }
}

