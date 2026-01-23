using UnityEngine;

class CameraLeader : MonoBehaviour
{
    public float DistancePToP;
    GameObject[] _players;

    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if (_players.Length < 2) return;

        Vector3 centerPoint = Vector3.zero;
        foreach (var player in _players)
        {
            centerPoint += player.transform.position;
        }
        centerPoint /= _players.Length;

        DistancePToP = (_players[0].transform.position - _players[1].transform.position).magnitude;

        Vector3 desiredPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
        transform.position = desiredPosition;
    }
}