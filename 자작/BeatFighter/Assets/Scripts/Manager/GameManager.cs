using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Combat combat;
    public SkillGroup skillGroup;
    public VitalSign vitalSign;

    [SerializeField]
    private float _noteSpeed;
    public float noteSpeed { get { return _noteSpeed; } set { _noteSpeed = value; } }

    protected override void Awake()
    {
        base.Awake();

        noteSpeed = _noteSpeed;
    }

    public void test()
    {
        combat.gameObject.SetActive(true);
        skillGroup.gameObject.SetActive(true);
        vitalSign.gameObject.SetActive(true);
        HPGroup.Instance.SetHP();
    }
}
