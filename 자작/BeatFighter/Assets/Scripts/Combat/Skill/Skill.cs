using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillHash
{
    JustOneShot,
    Start,
    Middle,
    Finish
}

public abstract class Skill : PoolObj
{
    protected CastInfo castInfo;
    private SkillInfo metaData;
    private JudgeRank judge;
    private int combo;

    private bool prepared;
    private bool isAlive;
    private bool hasDuration;
    private float startTime;
    private float lastTickTime;

    private ParticleSystem[] particleSystems;
    private AudioSource[] audioSources;

    protected virtual void OnOneShot() { }
    protected virtual void OnStart() { }
    protected virtual void OnMiddle() { }
    protected virtual void OnFinish() { }
    protected virtual void OnAlive() { }
    protected virtual void OnTick() { }
    protected virtual void OnExpired() { }

    private void Update()
    {
        if (!CombatManager.Instance.isCombat || !CombatManager.Targetable(this.castInfo.from)) Despawn();
        if (prepared)
        {
            if (isAlive)
            {
                OnAlive();
                if (hasDuration && metaData.duration > 0 && Time.time - lastTickTime >= metaData.tickTime)
                {
                    OnTick();
                    lastTickTime = Time.time;
                }
                if (hasDuration && metaData.duration > 0 && Time.time - startTime >= metaData.duration)
                {
                    OnExpired();
                }
            }
            else
            {
                if (CanDespawn()) base.Despawn();
            }
        }
    }

    /// <summary>
    /// 스킬을 준비하고 시전자의 캐스팅 애니메이션을 실행
    /// </summary>
    /// <param name="castInfo"></param>
    /// <param name="meta"></param>
    public void Prepare(CastInfo castInfo, SkillInfo metaData, JudgeRank judge, int combo)
    {
        this.castInfo = castInfo;
        this.metaData = metaData;
        this.judge = judge;
        this.combo = combo;
        this.prepared = false;
        castInfo.from.Play(metaData.stateName);
    }

    /// <summary>
    /// 실질적인 스킬 발동
    /// </summary>
    public void Initialize()
    {
        this.prepared = true;
        this.isAlive = true;
        this.hasDuration = false;
    }

    public void OnInitialized(SkillHash hash)
    {
        switch (hash)
        {
            case SkillHash.JustOneShot:
                OnOneShot();
                break;
            case SkillHash.Start:
                OnStart();
                break;
            case SkillHash.Middle:
                OnMiddle();
                break;
            case SkillHash.Finish:
                OnFinish();
                break;
        }
    }

    /// <summary>
    /// 지속시간이 있는 스킬의 타이머
    /// </summary>
    protected void StartTimer()
    {
        this.hasDuration = true;
        this.startTime = Time.time;
        this.lastTickTime = Time.time;
    }

    /// <summary>
    /// 스킬 디스폰
    /// </summary>
    protected new void Despawn()
    {
        this.isAlive = false;
        this.hasDuration = false;
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    /// <summary>
    /// 현재 파티클과 오디오의 재생 여부에 따라 디스폰 여부 결정
    /// </summary>
    /// <returns></returns>
    private bool CanDespawn()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i].isPlaying) return false;
        }
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying) return false;
        }

        return true;
    }

    protected void DoSkillDamage(BaseChar target)
    {
        if (!CombatManager.Targetable(target)) return;
        Stats stats = castInfo.from.stats;
        stats.AddSkillParameter(metaData, castInfo.from is PlayerChar ? PlayerData.GetSkillData(metaData.typeID).level : 1);
        castInfo.from.DoDamage(target, stats, judge, combo);
    }

    protected void DoSkillDamageAllEnemies()
    {
        foreach (BaseChar target in CombatManager.Instance.mobs)
        {
            DoSkillDamage(target);
        }
    }

    #region Effect
    /// <summary>
    /// 특정 스킬 이펙트 실행
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    protected Transform Play(string name, Vector3 position)
    {
        Transform child = transform.Find(name);
        if (child == null) return null;
        child.position = position;
        PlayChild(child);
        
        return child;
    }

    private void PlayChild(Transform child)
    {
        AudioSource[] audios = child.GetComponentsInChildren<AudioSource>();
        ParticleSystem[] particles = child.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].Play();
        }
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    /// <summary>
    /// 특정 스킬 이펙트 중지
    /// </summary>
    /// <param name="name"></param>
    protected void Stop(string name)
    {
        Transform child = transform.Find(name);
        if (child == null) return;
        StopChild(child);
    }

    private void StopChild(Transform child)
    {
        AudioSource[] audios = child.GetComponentsInChildren<AudioSource>();
        ParticleSystem[] particles = child.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < audios.Length; i++)
        {
            if (!audios[i].isPlaying) continue;
            audios[i].Stop();
        }
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }
    #endregion
}
