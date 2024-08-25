using UnityEngine;
using UnityEngine.UI;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField] private Transform transformCell;
    [SerializeField] private MainCell cellInventory;

    [SerializeField] private GridLayoutGroup grid;

    public MainCell[,] cells;

    public int sizeX, sizeY;

    public void Awake()
    {
        NewInvPanel();
    }

    public void NewInvPanel()
    {
        cells = new MainCell[sizeX, sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var newCell = Instantiate(cellInventory, transformCell);
                newCell.name = x + " " + y;
                newCell.x = x;
                newCell.y = y;
                newCell.isFree = true;
                newCell.inventory = this;
                cells[x, y] = newCell;
            }
        }

        grid.CalculateLayoutInputHorizontal();
        grid.CalculateLayoutInputVertical();
        grid.SetLayoutHorizontal();
        grid.SetLayoutVertical();
    }
}
