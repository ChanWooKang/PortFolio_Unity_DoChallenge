using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defines;

public class UI_Info : UI_Base
{
    enum GameObjects
    {
        HP,
        MP,
        Exp,
        Level
    }

    Image hp_img;
    Image mp_img;
    Image exp_img;
    Text level_txt;

    PlayerCtrl player;

    float hp;
    float mp;
    float exp;

    void Start()
    {
        Init();
    }


    void Update()
    {
        SettingUI();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        hp_img = GetObject((int)GameObjects.HP).GetComponent<Image>();
        mp_img = GetObject((int)GameObjects.MP).GetComponent<Image>();
        exp_img = GetObject((int)GameObjects.Exp).GetComponent<Image>();
        level_txt = GetObject((int)GameObjects.Level).GetComponent<Text>();
    }

    void SettingUI()
    {
        if (player == null)
            player = PlayerCtrl._inst;

        hp = player._stat.HP / player._stat.MaxHP;
        mp = player._stat.MP / player._stat.MaxMP;
        exp = player._stat.EXP() / player._stat.TotalEXP();

        SetImage();
    }

    void SetImage()
    {
        level_txt.text = player._stat.Level.ToString();
        hp_img.fillAmount = hp;
        mp_img.fillAmount = mp;
        exp_img.fillAmount = exp;
    }
}
