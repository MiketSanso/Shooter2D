using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField] private Transform transformCell;
    [SerializeField] private MainCell cellInventory;

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
    } 

     /* TestRegion(DontGameplay)
    public void Update()
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (cells[x, y].isFree)
                {
                    cells[x, y].image.color = Color.white;
                }
                else
                {
                    cells[x, y].image.color = Color.red;
                }
            }
        }
    }
    */
}
