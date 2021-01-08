using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    public static string playerTag = "Player";
    public static string enemyTag = "Enemy";
    public static string shopNpcTag = "Npc";
    public static string keywordTag = "Keyword";
    public static string groundTag = "Ground";
    public static string questNPCTag = "QuestNPC";
    public static string blockTag = "Block";

    public static string msgLongDistance = "거리가 너무 멉니다.";
    public static string msgNotEnoughInventory = "인벤토리 공간이 부족합니다.";
    public static string msgWrongClassEquipItem = "선택된 캐릭터의 장비가 아닙니다.";
    public static string msgCanNotEquipItem = "장착이 불가능합니다.";
    public static string msgCooltime = "사용할 수 없습니다.";
    public static string msgNotEnoughGold = "골드가 부족합니다.";
    public static string msgNoTarget = "대상이 없습니다.";
    public static string msgGetKeword = "키워드 'keyword' 획득";
    public static string msgCanNotBlockEquip = "등록 불가합니다.";
    public static string msgNotEnoughBlock = "블록 개수가 부족합니다.";
    public static string msgEmptyBlockSlot = "등록된 블록이 없습니다.";
    public static string msgWrongSwipeBlock = "잘못된 스와이프 방향입니다.";
    public static string msgBlockExistObject = "소환 위치에 방해되는 물체가 있습니다";
    public static string msgEmptySkillSlot = "등록된 스킬이 없습니다.";
    public static string msgCanNotSkill = "쿨타임 입니다.";

    public static string block = "Block";
    public static string skill = "Skill";

    public static string ItemTypeToString(OptionType type)
    {
        string str = "";

        switch(type)
        {
            case OptionType.HP: str = "체력"; break;
            case OptionType.STR: str = "힘"; break;
            case OptionType.INT: str = "지력"; break;
            case OptionType.DEF: str = "방어력"; break;
            case OptionType.SPEED: str = "이동속도"; break;
        };

        return str;

    }
}
