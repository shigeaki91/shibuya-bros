using UnityEngine;
using Unity.Cinemachine;

public class GroupTargetSetting : MonoBehaviour
{
    CinemachineTargetGroup _targetGroup;
    GameObject[] _players;

    void Start()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in _players)
        {
            _targetGroup.AddMember(player.transform, 1f, 4f);
        }
    }
}