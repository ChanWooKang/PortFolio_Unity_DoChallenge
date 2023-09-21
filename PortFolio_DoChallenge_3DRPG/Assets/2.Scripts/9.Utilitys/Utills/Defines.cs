using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    public enum CursorType
    {
        UnKnown,
        Hand,
        Attack,
    }

    public enum SceneType
    {
        UnKnown,
        MainScene = 0,
        InGameScene = 1,
    }

    public enum SoundType
    {
        BGM = 0,
        Effect,
        Max_Cnt
    }

    public enum LayerType
    {
        Floor = 6,
        Player = 7,
        Block = 8,
        MonsterDisable = 9,
        Monster = 10,
        RootItem = 11,
    }

    public enum TagType
    {
        Floor,
        Player,
        Monster,
        Interactive,
        Weapon,
        EquipSlot,
        ItemSlot,
    }

    public enum MouseEvent
    {
        Click,
        Press,
        PointerDown,
        PointerUp,
    }

    public enum UIEvent
    {
        Click,
        Drag,
        DragBegin,
        DragEnd,
        Drop,
    }

    public enum CameraMode
    {
        QuaterView,
    }

    public enum PoolType
    {
        Monster,
        RootItem,
        Effect,
    }

    public enum InteractType
    {
        Unknown = 0,
        RootItem
    }

    public enum StatType
    {
        HP = 0,
        MP,
        Damage,
        Defense,
        Max_Cnt
    }

    public enum PlayerState
    {
        Die,
        Idle,
        Move,
        Skill,
        Attack
    }

    public enum PlayerBoolType
    {

    }

    public enum MonsterType
    {
        Unknown = 0,
        Cactus,
        Mushroom,

        Max_Cnt
    }

    public enum MonsterState
    {
        Die,
        Idle,
        Patrol,
        Trace,
        Sense,
        Attack,
    }

    public enum ComboType
    {
        Hit1,
        Hit2,
        Hit3,
    }

    public enum ItemType
    {
        Unknown = 0,

        Gold,
        Potion,
        Etc,
        Equipment
    }

    public enum EquipmentType
    {
        Unknown = 0,
        Helm,
        Chest,
        Arm,
        Leg,
        Weapon,
        Shield,
        Max_Cnt
    }

    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> Make();
    }

    public interface IFSMState<T>
    {
        void Enter(T e);
        void Execute(T e);
        void Exit(T e);
    }
}
