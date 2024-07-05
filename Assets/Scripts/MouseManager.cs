using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Camera mainCamera;
    public UIManager uiManager;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tileInfo = hit.transform.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                uiManager.UpdateTileInfo(tileInfo.x, tileInfo.y);
            }
        }
        else
        {
            uiManager.ClearTileInfo();
        }
    }
}
