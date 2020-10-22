using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPanel : PanelBase
{
    public HPGroup hpGroup;
    public VitalSign vitalSign;
    public SkillGroup skillGroup;
    public WarningScreen warningScreen;
    public ComboText comboText;
    public ReturnButton returnButton;

    public override void Initialize()
    {
        CombatManager.onMapSet += Show;

        this.vitalSign.Initialize();
        this.hpGroup.Initialize();
        this.skillGroup.Initialize();

        CombatManager.onMapEnd += Hide;
    }

    public void SetPlayer(PlayerChar player)
    {
        this.vitalSign.SetPlayer(player);
        this.skillGroup.SetBaseData(player);
    }

    public void SetHPGroup(PlayerChar player, MobChar[] mobs)
    {
        this.hpGroup.SetHPGroup(player, mobs);
    }

    public void SetCombo()
    {
        this.comboText.Show(this.vitalSign.combo);
    }

    public void SetWarning()
    {
        this.warningScreen.StartWarningScreen();
    }

    public void ShowReturnButton()
    {
        this.returnButton.Show();
    }

    public void HideReturnButton()
    {
        this.returnButton.Hide();
    }
}
