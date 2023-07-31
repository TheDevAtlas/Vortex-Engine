using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Figure", menuName = "Figure/SpawnManagerScriptableObject", order = 1)]
public class FigureObject : ScriptableObject
{
    public string NAME;
    public int ID;
    public int VID;
    public int START_HEALTH;
    public int MAX_HEALTH;
    public Texture FIGURE_SPRITE;
    public Element ELEMENT;
    public Type TYPE;
}

public enum Element
{
    Air, 
    Earth, 
    Dark, 
    Fire, 
    Life, 
    Light, 
    Magic, 
    Tech, 
    Undead, 
    Water
}

public enum Type
{
    Skylander,
    Giant,
    Lightcore,
    EonsElite,
    SwapForce,
    TrapTeam,
    Supercharger,
    Imagionator,
    Sensei
}
