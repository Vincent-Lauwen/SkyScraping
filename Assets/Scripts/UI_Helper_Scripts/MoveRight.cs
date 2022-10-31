using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MoveButton
{
    public override void SetButtonListener(TetrisBlock spawnedTetrisBlock)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(spawnedTetrisBlock.MoveBlockToRight);
    }
}
