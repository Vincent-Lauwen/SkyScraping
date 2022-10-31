using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vincent.Wanderlost.Code;

public class TetrisSystem : MonoBehaviour
{
    #region Fields
    [Header("Block Info")]
    [SerializeField] private float fallTime;
    [SerializeField] private int blockDamage;
    [SerializeField] private GameObject[] TetrisBlocksPrefabs;
    [SerializeField] private Transform SpawnPos;

    [Header("Script References")]
    [SerializeField] private StructureSystem structure;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PlayField playField;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private MoveButton leftButton, rightButton;

    public float FallTime { get => fallTime; }
    #endregion

    #region Methods
    public void StartSystem()
    {
        SetHealth();
        SpawnNewTetrisBlock();
    }

    public void StopSystem()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

    }

    public void OnBlockLanded(TetrisBlock tetrisBlock)
    {
        float currentHeight = structure.GetStructureHeight();

        playField.AddToGrid(tetrisBlock);
        if (structure.Collapsed(tetrisBlock, out float updatedHeight))
        {
            UpdateHealth(-blockDamage);
        }
        else
        {
            float UpdatedScore = updatedHeight - currentHeight;
            playField.MoveUp(tetrisBlock);
            scoreSystem.UpdateScore(UpdatedScore);
        }
        SpawnNewTetrisBlock();
    }

    private void SpawnNewTetrisBlock()
    {
        TetrisBlock tetrisBlock = Instantiate(TetrisBlocksPrefabs[Random.Range(0, TetrisBlocksPrefabs.Length)], SpawnPos.position, Quaternion.identity).GetComponent<TetrisBlock>();
        tetrisBlock.OnInstanstiate(leftButton, rightButton, this);
    }

    public void SetHealth()
    {
        Player.playerData.CurrentHealth = Player.playerData.Health;
        healthBar.SetHealth(Player.playerData.CurrentHealth);
    }

    public void UpdateHealth(int value)
    {
        Player.playerData.CurrentHealth += value;
        healthBar.UpdateHealth(value);

        if (Player.playerData.CurrentHealth <= 0)
        {
            scoreSystem.SetEndScore();
            SceneManager.LoadScene("GameOver");
        }
    }
    #endregion
}
