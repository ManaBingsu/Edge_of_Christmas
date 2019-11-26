using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinData", menuName = "Data/PlayerSkinData")]
public class DummyPlayerSkinData : ScriptableObject
{
    [SerializeField]
    private int index;
    public int Index { get { return index; } }

    [SerializeField]
    private string skinName;
    public string SkinName { get { return skinName; } }

    [TextArea]
    [SerializeField]
    private string story;
    public string Story { get { return story; } }

    [TextArea]
    [SerializeField]
    private string abilityScript;
    public string AbilityScript { get { return abilityScript; } }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
}
