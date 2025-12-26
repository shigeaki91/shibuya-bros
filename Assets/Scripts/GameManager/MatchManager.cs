using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public StageManager stage;
    public List<Character> characters;
    public float timelimit = 180f;

    public float timer;

    void Start()
    {
        timer = timelimit;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        foreach (var chara in characters)
        {
            if (stage.IsOutOfBounds(chara.transform))
            {
                Debug.Log(chara.characterName + " is dead!! ");
                chara.hp = 0f;
            }
        }

        CheckVictory();
    }

    void CheckVictory()
    {
        List<Character> alive = characters.FindAll(c => c.hp > 0);

        if (alive.Count == 1)
        {
            Debug.Log("Winner is " + alive[0].characterName + "!!");
            enabled = false;
        }
        else if (alive.Count == 0)
        {
            Debug.Log("It's a draw!");
            enabled = false;
        }
        else if (timer <= 0f)
        {
            Character highestHpChar = null;
            float highestHp = -1f;

            foreach (var chara in alive)
            {
                if (chara.hp > highestHp)
                {
                    highestHp = chara.hp;
                    highestHpChar = chara;
                }
            }

            if (highestHpChar != null)
            {
                Debug.Log("Time's up! Winner is " + highestHpChar.characterName + "!!");
                enabled = false;
            }
            else
            {
                Debug.Log("Time's up! It's a draw!");
                enabled = false;
            }
        }
    }
}
