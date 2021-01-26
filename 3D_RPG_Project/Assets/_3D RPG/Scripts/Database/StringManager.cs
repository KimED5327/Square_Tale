using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    public static string nickname = "Nickname";

    public static string playerTag = "Player";
    public static string enemyTag = "Enemy";
    public static string shopNpcTag = "Npc";
    public static string keywordTag = "Keyword";
    public static string groundTag = "Floor";
    public static string questNPCTag = "QuestNPC";
    public static string blockTag = "Block";
    public static string TriggerTag = "Trigger";
    public static string fieldItemTag = "DropItem";

    public static string msgLongDistance = "거리가 너무 멉니다.";
    public static string msgNotEnoughInventory = "인벤토리 공간이 부족합니다.";
    public static string msgWrongClassEquipItem = "선택된 캐릭터의 장비가 아닙니다.";
    public static string msgCanNotEquipItem = "장착이 불가능합니다.";
    public static string msgCooltime = "사용할 수 없습니다.";
    public static string msgNotEnoughGold = "골드가 부족합니다.";
    public static string msgNotEnoughItemCount = "판매량이 소유량보다 많습니다.";
    public static string msgNoTarget = "대상이 없습니다.";
    public static string msgGetKeword = "키워드 'keyword' 획득";
    public static string msgCanNotBlockEquip = "등록 불가합니다.";
    public static string msgNotEnoughBlock = "블록 개수가 부족합니다.";
    public static string msgEmptyBlockSlot = "등록된 블록이 없습니다.";
    public static string msgWrongSwipeBlock = "잘못된 스와이프 방향입니다.";
    public static string msgBlockAcquire = "모든 블록 10개 획득!";
    public static string msgBlockExistObject = "소환 위치에 방해되는 물체가 있습니다";
    public static string msgEmptySkillSlot = "등록된 스킬이 없습니다.";
    public static string msgCanNotSkill = "쿨타임 입니다.";
    public static string msgNotAccessChapter = "해금되지 않은 챕터입니다.";
    public static string msgAlreadyActivateTrigger = "이미 작동시킨 장치입니다.";
    public static string msgCanNotSwap = "스킬 쿨타임 일때 변경 할 수 없습니다.";
    public static string msgGetPureItem = "정화의 조각 획득!!";
    public static string msgNeedKey = "상자에 맞는 열쇠가 필요합니다.";

    public static string shopBuyMessage = "해당 아이템을 구매하시겠습니까?";
    public static string shopSellMessage = "해당 아이템을 판매하시겠습니까?";
    public static string shopObjectBuy = "상품 구매";
    public static string shopObjectSell = "상품 판매";
    public static string shopBuy = "구매";
    public static string shopSell = "판매";

    public static string block = "Block";
    public static string skill = "Skill";

    public static string BombBlock = "폭발 블록";

    public static string treasureKey = "Treasure";
    public static string GetTresureKey(int id)
    {
        return treasureKey + id;
    }

    public static string crystalKey = "Crystal";
    public static string GetCrystalKey(int id)
    {
        return crystalKey + id;
    }

    public static string eventKey = "Event";
    public static string GetEventKey(int id)
    {
        return eventKey + id;
    }

    public static string keywordKey = "Keyword";
    public static string GetKeywordKey(int id)
    {
        return keywordKey + id;
    }

    public static string mapNameVegonia = "Vegonia";
    public static string mapNameTown = "Town";
    public static string mapNameCroa = "Croa";
    public static string mapNameDelphinium = "Delphinium";
    public static string mapNameHyacinth = "Hyacinth";
    public static string mapNameLilyCastleFront = "LilyCastleFront";
    public static string mapNameLilyCastle = "LilyCastle";
    public static string mapNameLilyCastleBossRoom = "LilyCastleBossRoom";

    public static string GetMapKoreanName(string name)
    {
        if (name == mapNameTown)
            return "마을";
        if (name == mapNameCroa)
            return "크로아";
        if (name == mapNameVegonia)
            return "베고니아";
        if (name == mapNameDelphinium)
            return "델피늄";
        if (name == mapNameHyacinth)
            return "히아신스";
        if (name == mapNameLilyCastleFront)
            return "릴리의 성 정문";
        if (name == mapNameLilyCastle)
            return "릴리의 성";
        if (name == mapNameLilyCastleBossRoom)
            return "릴리의 방";
        else
            return "기타";


    }

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
