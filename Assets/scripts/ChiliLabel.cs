using UnityEngine;
using TMPro;
using System;

public class ChiliLabel : MonoBehaviour
{
    public TextMeshProUGUI textLabel;

    public void SetText(ChiliData data)
    {
        string heatText = data.scoville.ToString("N0") + " SHU";

        textLabel.text = $"{data.name} (Gen {data.generation - 4})\n" +
                         $"<color=red>{heatText}</color>\n" +
                         $"<color=yellow>smokieness: {Math.Round(data.smokiness, 2)}</color>\n" +
                         $"<color=yellow>sweetness: {Math.Round(data.sweetness, 2)}</color>\n" +
                         $"<color=yellow>fruitiness: {Math.Round(data.fruitiness, 2)}</color>\n" +
                         $"<color=yellow>bitterness: {Math.Round(data.bitterness, 2)}</color>";
    }

    
}