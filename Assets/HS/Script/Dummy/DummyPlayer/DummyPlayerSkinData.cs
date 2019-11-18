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
    private string script;
    public string Script { get { return script; } }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
}
