using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager: MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int mineCount = 10;

    public GameObject cellPrefab;
    public Transform gridPanel;


    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    //Timer
    public TextMeshProUGUI timerText;
    private float timer = 0f;
    private bool timerRunning = false;

    //Flag
    public TextMeshProUGUI flagCountText;
    private int flagCount = 0;


    private Cell[,] cells;
    private bool gameEnded = false;

    void Start()
    {
        timer = 0f;
        timerRunning = true;

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        GenerateGrid();
    }

    private void Update()
    {
        if (timerRunning && !gameEnded)
        {
            timer += Time.deltaTime;
            if (timerText != null)
                timerText.text = $"Time: {Mathf.FloorToInt(timer)}s";
        }
    }

    void GenerateGrid()
    {
        gameEnded = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);


        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, gridPanel);
                cellObject.name = $"Cell {x} {y}";
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Init(x, y,this);
                cells[x, y] = cell;
            }
        }
        flagCount = 0;
        UpdateFlagCount(0);
        PlaceMines();
        CalculateNumbers();
    }

    void PlaceMines()
    {
        int placedMines = 0;
        while(placedMines<mineCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (!cells[x, y].isMine)
            {
                cells[x, y].isMine = true;
                placedMines++;
            }
        }
    }

    void RevealAllMines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = cells[x, y];
                if (cell.isMine && !cell.isRevealed)
                {
                    cell.RevealMine();
                }
                else if (!cell.isMine && cell.isFlagged)
                {
                    cell.cellText.text = "X"; 
                    cell.cellText.color = Color.red;
                }
            }
        }
    }
    void DisableAllCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x,y].button.interactable = false;
            }
        }
    }

    void CalculateNumbers()
    {
        for ( int x=0; x<width;x++)
        {
            for (int y = 0; y<height;y++)
            {
                if (cells[x,y].isMine) continue;

                int count = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx >= 0 && nx < width && ny >= 0 && ny < height && cells[nx, ny].isMine)
                        {
                            count++;
                        }
                    }
                }
                cells[x,y].adjacentMines= count;
            }
        }
    }

    public void RevealAdjacentCells(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; 

                int nx = x + dx;
                int ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;

                Cell cell = cells[nx, ny];

                if (cell.isRevealed || cell.isMine || cell.isFlagged) continue;

                cell.RevealWithoutRecursion();

                if (cell.adjacentMines == 0)
                {
                    RevealAdjacentCells(nx, ny);
                }
            }
        }
    }

    public void CheckWinCondition()
    {
        if (gameEnded) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = cells[x, y];
                if (!cell.isMine && !cell.isRevealed)
                    return;
            }
        }

        GameOver(true);
    }

    public void UpdateFlagCount(int delta)
    {
        flagCount += delta;
        if (flagCountText != null)
            flagCountText.text = $"Flags: {flagCount} / {mineCount}";
    }



    public void GameOver(bool win)
    {
        if (gameEnded) return;

        timerRunning = false;
        gameEnded = true;

        DisableAllCells();

        if (win)
        {
            Debug.Log("🎉 CONGRATULATIONS! YOU WIN! 🎉");

            
            if (gameOverText != null)
            {
                gameOverText.text = "🎉 YOU WIN! 🎉\nAll mines found!";
                gameOverText.color = Color.green;
            }

            RevealWinState();
        }
        else
        {
            Debug.Log("💥 BOOM! GAME OVER 💥");

            if (gameOverText != null)
            {
                gameOverText.text = "💥 GAME OVER 💥\nYou hit a mine!";
                gameOverText.color = Color.red;
            }

            RevealAllMines();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }


    void RevealWinState()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = cells[x, y];
                if (cell.isMine)
                {
                    if (!cell.isFlagged)
                    {
                        cell.cellText.text = "F";
                        cell.cellText.color = Color.green;
                    }
                    else
                    {
                        cell.cellText.color = Color.green;
                    }
                }
            }
        }
    }
    public void RestartGame()
    {
        foreach (Transform child in gridPanel)
        {
            Destroy(child.gameObject);
        }

        GenerateGrid();
    }
    public bool IsGameEnded()
    {
        return gameEnded;
    }

}