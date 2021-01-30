using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textUI;
    [SerializeField]
    Player player;

    string[] statsToDisplay =
    {
        "health",
        "food",
        "movementSpeed",
        "attack"
    };

    // Update is called once per frame
    void Update()
    {
        var sb = new StringBuilder();
        foreach(var stat in statsToDisplay)
        {
            sb.AppendFormat("{0}: {1}\n", stat, player.playerStats.GetStat(stat));
        }

        textUI.text = sb.ToString();
    }
}
