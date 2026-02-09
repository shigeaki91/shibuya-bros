using UnityEngine;

[System.Serializable]
public struct CharaDataEntry
{
    public CharacterNames CharacterName;
    public Sprite CharacterSprite;
    public Sprite ReplacedSprite;
    public GameObject CharacterPrefab;
    public GameObject ReplacedPrefab;
    public string DisplayName;
}
[CreateAssetMenu(fileName = "CharacterDatas", menuName = "ScriptableObjects/CharacterDatas", order = 1)]
public class CharacterDatas : ScriptableObject
{
    [SerializeField] CharaDataEntry[] _charaDataEntries;

    public CharaDataEntry[] CharaDataEntries => _charaDataEntries;
}