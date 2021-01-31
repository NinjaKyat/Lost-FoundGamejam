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
        foreach(var stat in player.playerStats.GetDictionary())
        {
            if (stat.Value > 0)
            {
                sb.AppendFormat("{0}: {1}\n", stat.Key, stat.Value);
            }
        }

        textUI.text = sb.ToString();
    }
}
