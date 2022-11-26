using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PState 
{ RUN , SLOWRUN , SIDERUN , SLOWSIDERUN , SPRINT , JUMP , FEINT , JUKE , SPIN , SLIDE , SPRINTFEINT , FLIP , HIGHKNEE , 
    GAMEOVER , SLIP , HURDLE }


public abstract class PlayerState
{
    protected enum Event { ENTER , UPDATE , EXIT , GAMEOVER }


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
    protected float rawAcc;
    protected float rawSide;
    protected float startSide;

    protected float startTime;
    protected float animTime;


    private Coroutine celebrationCR;

    private bool catched = false;

    protected bool IsRaining
    {
        get { return player.gameManager.gameData.gameWeather == GameWeather.RAIN; }
    }

    readonly float rotationSpeed = 10f;

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
        rawAcc = Input.GetAxisRaw("Vertical");
        rawSide = Input.GetAxisRaw("Horizontal");
    }
    public virtual void Exit() { stage = Event.EXIT; } // Debug.Log(name + " -> " + nextState.name); }



    public PlayerState Process()
    {
        if (stage == Event.GAMEOVER) { return new GameOverPS(player); }

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
                Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * rotationSpeed);

            player.fPPlayer.transform.localRotation =
                Quaternion.Slerp(player.fPPlayer.transform.localRotation,
                    Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * rotationSpeed);
        }

    }
    protected void PlayerOrientation()
    {
        PlayerOrientation(false);
    }

    protected void SlowMotion(float time, float radius, int min)
    {
        int enemyNumber = player.playerManager.EnemyNumber(radius, out bool zombie);
        if (enemyNumber >= min * (zombie ? 2 : 1))
        {
            player.StartCoroutine(SlowmoCR(time));
            player.effects.SlowMoVolume(time+0.5f);
            player.playerManager.PlayerBigplay();
        }
    }
    private IEnumerator SlowmoCR(float time)
    {
        float max = 1;
        float min = 0.25f;
        float sample = 100;
        time += 0.5f;
        WaitForSeconds wait = new(time / sample);
        Time.timeScale = min;
        for (int i = 0; i < sample; i++)
        {
            Time.timeScale += (max - min) / sample;
            yield return wait;
        }
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
    protected void SetInt(string name, int value)
    {
        foreach (Animator a in animators)
        {
            a.SetInteger(name, value);
        }
    }
    protected void SetLayer(string name, float weight)
    {
        foreach (Animator a in animators)
        {
            a.SetLayerWeight(a.GetLayerIndex(name), weight);
        }
    }

    public void Catch()
    {
        if (!catched) player.StartCoroutine(CatchCR(0.5f));
        catched = true;
    }
    private IEnumerator CatchCR(float time)
    {
        SetTrigger("Catch");
        float weight = 0, sample = 50;
        for (int i = 0; i < sample; i++)
        {
            weight = Mathf.Lerp(weight, 1, 0.2f);
            SetLayer("Catch", weight);
            yield return new WaitForSeconds(time / sample);
        }
        SetLayer("Catch", 1);
        player.playerManager.FootballActive = true;
        yield return new WaitForSeconds(time * 3/2);
        SetLayer("Catch", 0);
        ResetTrigger("Catch");

        catched = false;
    }

    public void Flashlight(bool state)
    {
        SetLayer("Flashlight", state ? 1 : 0);
        player.playerManager.FlashlightActive = state;
    }

    public void SetWeapon(bool state, bool bigWeapon)
    {
        SetLayer(bigWeapon ? "BigWeapon Layer" : "SmallWeapon Layer", state ? 1 : 0);
        Flashlight(!(bigWeapon & state));
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
        SetLayer("BigWeapon Layer", 0);
        SetLayer("SmallWeapon Layer", 0);
    }

    public void TD(bool state)
    {
        if (state) celebrationCR = player.StartCoroutine(TDCoroutine());
        else
        {
            if (celebrationCR != null) player.StopCoroutine(celebrationCR);
            SetLayer("TD", 0);
        }
    }
    private IEnumerator TDCoroutine()
    {
        float weight = 0;
        while (weight < 0.99f)
        {
            weight = Mathf.Lerp(weight, 1, 0.2f);
            SetLayer("TD", weight);
            yield return new WaitForSeconds(0.01f);
        }
        SetLayer("TD", 1);
    }
    public void SetRandomCelebration()
    {
        int n = Random.Range(1, UD.celebrationNumber + 1);
        SetInt("TD Number", n);
        SetTrigger("TD");
    }

    public void Dead()
    {
        DeactivateWeaponLayer();

        SetLayer("Dead", 1);
        SetTrigger("Dead");

        stage = Event.GAMEOVER;
        Process();
    }
    public void Win()
    {
        SetTrigger("Win");
        stage = Event.GAMEOVER;
        Process();
    }
    public void Lose()
    {
        SetTrigger("Lose");

        stage = Event.GAMEOVER;
        Process();
    }
}

