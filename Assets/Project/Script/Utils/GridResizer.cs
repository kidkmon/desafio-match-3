using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class GridResizer : MonoBehaviour
    {
        // Resize the grid layout group based on the parent RectTransform and the number of columns
        public static void ResizeGrid(GridLayoutGroup gridLayoutGroup, RectTransform parentRectTransform, int columns)
        {
            float totalSpacing = gridLayoutGroup.spacing.x * (columns - 1);
            float availableWidth = parentRectTransform.rect.width - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right - totalSpacing;

            float cellSize = availableWidth / columns;

            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        }
    }
}
