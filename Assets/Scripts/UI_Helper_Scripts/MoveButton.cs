using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MoveButton : MonoBehaviour
{
    #region Fields
    [SerializeField] protected Button _button;
    #endregion

    #region Methods
    public abstract void SetButtonListener(TetrisBlock spawnedTetrisBlock);
    #endregion
}
