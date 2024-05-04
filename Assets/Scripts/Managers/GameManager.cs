using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GridMapManager gridMapManager;
        [SerializeField] private BlockManager blockManager;
        private void Start()
        {
            SubscribeEvents();
            Initialize();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Initialize()
        {
            levelManager.Initialize();
            
            var cellSize = levelManager.GetGridCellSize();
            var gridMapSize = new Vector2Int(levelManager.GetGridMapBlockSettlement().GetLength(0),
                levelManager.GetGridMapBlockSettlement().GetLength(1));
            
            gridMapManager.Initialize(gridMapSize, cellSize);
            blockManager.Initialize(levelManager.GetGridMapBlockSettlement());
            uiManager.Initialize();
        }

        private void SubscribeEvents()
        {
            blockManager.OnBlocksSettled += inputManager.EnableInputs;
            blockManager.OnBlocksMoving += inputManager.DisableInputs;
            inputManager.OnClickConfirmed += blockManager.TouchDetected;
            uiManager.OnReloadButtonClicked += ReLoadGameScene;
        }

        private void Unsubscribe()
        {
            blockManager.OnBlocksSettled -= inputManager.EnableInputs;
            blockManager.OnBlocksMoving -= inputManager.DisableInputs;
            inputManager.OnClickConfirmed -= blockManager.TouchDetected;
            uiManager.OnReloadButtonClicked -= ReLoadGameScene;
        }

        private void ReLoadGameScene()
        {
            SceneManager.LoadScene(0);
        }

        private void GameFail()
        {
            inputManager.OnClickConfirmed -= blockManager.TouchDetected;
            inputManager.DisableInputs();
        }

        private void GameSuccess()
        {
            inputManager.OnClickConfirmed -= blockManager.TouchDetected;
            inputManager.DisableInputs();
        }
    }
}