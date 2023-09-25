using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Defines;

public class UI_Skill : UI_Base
{
    enum GameObjects
    {
        Skill,
        Cool_Img,
    }
    public SOSkill _skill;
    GameObject Skill_Obj;
    Image Cool_Img;

    void Start()
    {
        Init();

        ClearCool();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Skill_Obj = GetObject((int)GameObjects.Skill);
        Cool_Img = GetObject((int)GameObjects.Cool_Img).GetComponent<Image>();

        //BindEvent(Skill_Obj, (PointerEventData data) => { OnClicked(); }, UIEvent.Click);
        Managers._input.KeyAction -= OnKeyBoardEvent;
        Managers._input.KeyAction += OnKeyBoardEvent;
    }

    void OnKeyBoardEvent()
    {
        if (Input.GetKeyDown(_skill.key))
        {
            OnSkill();
        }
    }

    void ClearCool()
    {
        Cool_Img.fillAmount = 0;
        Cool_Img.gameObject.SetActive(false);
    }

    void SetCool()
    {
        if(Cool_Img.gameObject.activeSelf == false)
            Cool_Img.gameObject.SetActive(true);
        Cool_Img.fillAmount = 1;
    }

    public void OnSkill()
    {
        if (Cool_Img.fillAmount > 0)
            return;

        //스킬 처리
        PlayerCtrl._inst.SkillEffect(_skill._type, _skill);

        //스킬 쿨타임 처리
        StopCoroutine(Skill_Cool());
        StartCoroutine(Skill_Cool());
    }

    IEnumerator Skill_Cool()
    {
        float tick = 1.0f / _skill.cool;
        float t = 0;

        SetCool();

        while(Cool_Img.fillAmount > 0)
        {
            Cool_Img.fillAmount = Mathf.Lerp(1, 0, t);
            t += (Time.deltaTime * tick);

            yield return null;
        }
        ClearCool();
    }
}
