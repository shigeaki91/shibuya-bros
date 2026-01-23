using UnityEngine;

public class CharacterData : ScriptableObject
{
    CharacterNames _characterName;
    GameObject _characterPrefab;

    public CharacterNames CharacterName => _characterName;
    public GameObject CharacterPrefab => _characterPrefab;
}