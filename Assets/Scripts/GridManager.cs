using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int mineCount = 10;

    public GameObject cellPrefab;
    public GameObject gameOverPanel;
    public Transform gridParent;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;
    public Button restartButton;

    private Cell[,] cells;
    public bool gameEnded;
    private float timer;

    private int revealedSafeCells;
    private int totalSafeCells;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        GenerateGrid();
    }

    void Update()
    {
        if (!gameEnded)
        {
            timer += Time.deltaTime;
            timerText.text = $"Time: {Mathf.FloorToInt(timer)}s";
        }
    }

    void GenerateGrid()
    {
        // Clear old grid
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        cells = new Cell[width, height];
        revealedSafeCells = 0;
        totalSafeCells = width * height - mineCount;
        timer = 0f;
        gameEnded = false;

        gameOverPanel.SetActive(false);

        // Create new grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab, gridParent);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Init(x, y, this);
                cells[x, y] = cell;
            }
        }

        PlaceMines();
        CalculateNumbers();

        resultText.text = "";
    }

    void PlaceMines()
    {
        int placedMines = 0;
        while (placedMines < mineCount)
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

    void CalculateNumbers()
    {
        ForEachCell(cell =>
        {
            if (cell.isMine) return;

            int mineCountAround = 0;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = cell.x + dx;
                    int ny = cell.y + dy;

                    if (InBounds(nx, ny) && cells[nx, ny].isMine)
                        mineCountAround++;
                }
            }

            cell.neighborCount = mineCountAround;
        });
    }

    public void OnCellRevealed()
    {
        revealedSafeCells++;
        if (revealedSafeCells == totalSafeCells)
            GameOver(true);
    }

    public void GameOver(bool win)
    {
        gameOverPanel.SetActive(true);

        gameEnded = true;
        RevealEndState(win);
        resultText.text = win ? "You Win!" : "Game Over!";
    }

    void RevealEndState(bool win)
    {
        ForEachCell(cell =>
        {
            if (cell.isMine)
            {
                if (win)
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
                else
                {
                    if (!cell.isRevealed)
                        cell.RevealMine();
                }
            }
            else if (!win && cell.isFlagged)
            {
                cell.cellText.text = "X";
                cell.cellText.color = Color.red;
            }
        });
    }
    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }


    public bool InBounds(int x, int y) =>
        x >= 0 && y >= 0 && x < width && y < height;

    void ForEachCell(System.Action<Cell> action)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                action(cells[x, y]);
    }

    void RestartGame()
    {
        GenerateGrid();
    }
}
