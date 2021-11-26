using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectOptimizer : MonoBehaviour
{
    public GameObject verticalPanel;

    public void Optimize()
    {
        int horizontalPanelCount = verticalPanel.transform.childCount;

        for (int i = 0; i < horizontalPanelCount; i++)
        {
            Transform horizontalPanel = verticalPanel.transform.GetChild(i);

            RectTransform verticalRT = verticalPanel.GetComponent<RectTransform>();
            RectTransform horizontalRT = horizontalPanel.gameObject.GetComponent<RectTransform>();

            bool isBelowScreen = horizontalRT.position.y + (horizontalRT.rect.height / 2) < Screen.height;

            int beatmapCellCount = horizontalPanel.childCount;

            if (isBelowScreen) //Horizontal panel is below the screen
            {
                for (int j = 0; j < beatmapCellCount; j++)
                {
                    Transform beatmapCell = horizontalPanel.GetChild(j);
                    beatmapCell.gameObject.SetActive(false);
                }
            }
            else
            {
                for (int j = 0; j < beatmapCellCount; j++)
                {
                    Transform beatmapCell = horizontalPanel.GetChild(j);
                    beatmapCell.gameObject.SetActive(true);
                }
            }
        }
    }
}
