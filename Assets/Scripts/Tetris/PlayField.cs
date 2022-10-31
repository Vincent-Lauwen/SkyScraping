using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField] private int structureUpValue;
    [SerializeField] private int height;
    [SerializeField] private int width;

    public static Transform[,] Grid { get; private set; }

    private void Awake()
    {
        Grid = new Transform[width, height];
    }

    public void AddToGrid(TetrisBlock tetrisBlock)
    {
        foreach (Transform children in tetrisBlock.transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            Grid[roundedX, roundedY] = children;
        }
    }

    public void MoveUp(TetrisBlock tetrisBlock)
    {
        foreach (Transform childBlock in tetrisBlock.transform)
        {
            if (childBlock.position.y > structureUpValue)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (Grid[x, y] != null)
                        {
                            if (y > 0)
                            {
                                Grid[x, y - 1] = Grid[x, y];
                                Grid[x, y] = null;
                                Grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
                            }
                            else
                            {
                                Destroy(Grid[x, y].gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    
}

