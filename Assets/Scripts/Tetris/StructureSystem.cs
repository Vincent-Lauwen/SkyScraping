using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vincent.Wanderlost.Code;
using System.Threading.Tasks;
using System;

public class StructureSystem : MonoBehaviour
{
    [Header("Instability Info")]
    [SerializeField] private int instabilityHeightCheckValue;
    [SerializeField] private double instabilityAcceptence;
    [SerializeField] private AnimationClip[] fallingBlockAnimations;

    public float GetStructureHeight()
    {
        List<Transform> filledArea = GetFilledStructureSpaces();
        if (filledArea.Count != 0)
        {
            Bounds structureSize = StructureSize(filledArea);
            return structureSize.max.y+1;
        }
        return default;
        
    }

    public bool Collapsed(TetrisBlock tetrisBlock, out float updatedHeight)
    {
        //Get the used spaces from the grid
        List<Transform> filledArea = GetFilledStructureSpaces();

        //Get the structure total used grid spaces for calculation
        Bounds structureSize = StructureSize(filledArea);
        double totalArea = GetTotalStructureSpaces(filledArea, structureSize);

        updatedHeight = structureSize.max.y+1;

        if (structureSize.max.y > instabilityHeightCheckValue)
        {
            double emptyArea = totalArea - Convert.ToDouble(filledArea.Count);
            double InstabilityPercentage = emptyArea / totalArea * 100;

            if (instabilityAcceptence < InstabilityPercentage)
            {
                StartCoroutine(CreateStructureCollapse(tetrisBlock));
                return true;
            }
        }
        return false;
    }

    IEnumerator CreateStructureCollapse(TetrisBlock tetrisBlock)
    {
        foreach (Transform block in tetrisBlock.transform)
        {
            Animation FallingBlockAnim = block.transform.GetComponentInChildren<Animation>();
            FallingBlockAnim.Play(fallingBlockAnimations[UnityEngine.Random.Range(0, Mathf.RoundToInt(fallingBlockAnimations.Length))].name);
        }

        yield return new WaitForSeconds(3);
        Destroy(tetrisBlock.gameObject);
    }

    private double GetTotalStructureSpaces(List<Transform> filledArea, Bounds structureSize)
    {
        double totalArea = 0;
        //Get the information on how width and long the structure now is and places the gridspaces for this in a list.
        for (float x = structureSize.min.x; x <= structureSize.max.x; x++)
        {
            for (float y = structureSize.min.y; y <= structureSize.max.y; y++)
            {
                //Go trough every gridspace from the given structure's width and length and check if the gridspace is occupied. We also check when a gridspace is not occupied but has a block 
                Transform gridspace = filledArea.Find(tag => tag.position.x == x && tag.position.y == y);
                if (!gridspace)
                {
                    //empty space in structure. Check if it is not outer air
                    if (filledArea.Any(tag => tag.position != null && tag.transform.position.y > y && tag.transform.position.x == x))
                    {
                        totalArea++;
                    }     
                }
                else
                {
                    totalArea++;
                }
            }
        }
        return totalArea;
    }

    private Bounds StructureSize(List<Transform> usedGridSpaces)
    {
        Bounds structureSize = new Bounds();
        float max_X = usedGridSpaces.Max(tag => tag.position.x);
        float min_X = usedGridSpaces.Min(tag => tag.position.x);
        float max_Y = usedGridSpaces.Max(tag => tag.position.y);
        float min_Y = usedGridSpaces.Min(tag => tag.position.y);

        structureSize.SetMinMax(new Vector2(min_X, min_Y), new Vector2(max_X, max_Y));
        return structureSize;
    }

    private List<Transform> GetFilledStructureSpaces()
    {
        List<Transform> filledArea = PlayField.Grid.OfType<Transform>().Where(tag => tag != null).ToList();
        return filledArea;
    }
}
