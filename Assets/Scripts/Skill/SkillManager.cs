using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    //private Player player;

    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public BlackHole_Skill blackHole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    public Parry_Skill parry { get; private set; }
    public Dodge_Skill dorge { get; private set; }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        //player = PlayerManager.instance.player;

        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackHole = GetComponent<BlackHole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
        parry = GetComponent<Parry_Skill>();
        dorge = GetComponent<Dodge_Skill>();

        //InitializeSkills();
    }

    //private void InitializeSkills()
    //{
    //    dash.Initialize(player);
    //    clone.Initialize(player);
    //    sword.Initialize(player);
    //    blackHole.Initialize(player);
    //    crystal.Initialize(player);
    //    parry.Initialize(player);
    //    dorge.Initialize(player);
    //}
}
