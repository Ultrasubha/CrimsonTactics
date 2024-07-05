using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tileInfoText;

    public void UpdateTileInfo(int x, int y)
    {
        tileInfoText.text = $"Tile Position: {x}, {y}";
    }

    public void ClearTileInfo()
    {
        tileInfoText.text = "";
    }
}
