using Core.TrackerSystem;
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

        [SerializeField] private GoalTracker goalTracker;
        [SerializeField] private MoveCountTracker moveCountTracker;
        [SerializeField] private TimeTracker timeTracker;
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
            var gridMapSize = new Vector2Int(levelManager.GetGridMapBlockSettlement().Length,
                levelManager.GetGridMapBlockSettlement()[0].Length);
            inputManager.EnableInputs();
            gridMapManager.Initialize(gridMapSize, cellSize);
            blockManager.Initialize(levelManager.GetGridMapBlockSettlement());
            uiManager.Initialize();
            goalTracker.SetGoals(levelManager.GetGoals());
            moveCountTracker.SetMoveCount(levelManager.GetMoveCount());
            timeTracker.StartTimer(levelManager.GetLevelTime());
        }

        private void SubscribeEvents()
        {
            blockManager.OnBlocksSettled += inputManager.EnableInputs;
            blockManager.OnBlocksMoving += inputManager.DisableInputs;
            inputManager.OnClickConfirmed += blockManager.TouchDetected;
            uiManager.OnReloadButtonClicked += ReLoadGameScene;
            goalTracker.OnGoalComplete += GameSuccess;
            moveCountTracker.OnNoMoveLeft += GameFail;
            timeTracker.OnTimeOver += GameFail;
        }

        private void Unsubscribe()
        {
            uiManager.OnReloadButtonClicked -= ReLoadGameScene;
            goalTracker.OnGoalComplete -= GameSuccess;
            UnsubscribeInputAndTrackerEvents();
        }

        private void  UnsubscribeInputAndTrackerEvents()
        {
            blockManager.OnBlocksSettled -= inputManager.EnableInputs;
            blockManager.OnBlocksMoving -= inputManager.DisableInputs;
            inputManager.OnClickConfirmed -= blockManager.TouchDetected;
            moveCountTracker.OnNoMoveLeft -= GameFail;
            timeTracker.OnTimeOver -= GameFail;
        }

        private void ReLoadGameScene()
        {
            SceneManager.LoadScene(levelManager.GetLevelIndex());
        }

        private void GameFail()
        {
            UnsubscribeInputAndTrackerEvents();
            inputManager.DisableInputs();
            inputManager.DisableInputs();
            timeTracker.StopTimeTracker();
            uiManager.OpenFailPanel();
        }

        private void GameSuccess()
        {
            goalTracker.OnGoalComplete -= GameSuccess;
            UnsubscribeInputAndTrackerEvents();
            inputManager.DisableInputs();
            timeTracker.StopTimeTracker();
            uiManager.OpenSuccessPanel();
            levelManager.IncreaseLevelIndex();
        }
    }
}