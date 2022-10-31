using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisBlock : MonoBehaviour
{
    #region Fields
    private SpriteRenderer[] _spriteRenderers;
    private float _previousTime;
    private TetrisSystem tetrisSystem;
    #endregion

    #region Methods
    void Update()
    {
        MoveBlockDown();
    }

    public void OnInstanstiate(MoveButton leftBTN, MoveButton rightBtn, TetrisSystem tetrisSystem)
    {
        this.tetrisSystem = tetrisSystem;

        leftBTN.SetButtonListener(this);
        rightBtn.SetButtonListener(this);
        RandomColorPicker();
    }

    public void MoveBlockToRight()
    {
        transform.position += Vector3.right;
        if (!ValidMove())
            transform.position -= Vector3.right;
    }
    public void MoveBlockToLeft()
    {
        transform.position += Vector3.left;
        if (!ValidMove())
            transform.position -= Vector3.left;
    }
    private void MoveBlockDown()
    {
        if (Time.time - _previousTime > tetrisSystem.FallTime)
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                transform.position -= Vector3.down;
                this.enabled = false;
                this.tetrisSystem.OnBlockLanded(this);
            }
            _previousTime = Time.time;
        }
    }

    private void RandomColorPicker()
    {
        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            sprite.color = randomColor;
        }
    }

    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= PlayField.Grid.GetLength(0) || roundedY < 0)
            {
                return false;
            }
            if (PlayField.Grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
    #endregion
}
