using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public int x, y;
    public bool isMine;
    public bool isRevealed;
    public bool isFlagged;
    public int neighborCount;

    public Button button;
    public TextMeshProUGUI cellText;

    public GridManager grid;

    public void Init(int x, int y, GridManager grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        button = GetComponent<Button>();
        cellText = GetComponentInChildren<TextMeshProUGUI>();

        ResetCell();
    }

    private void ResetCell()
    {
        isMine = false;
        isRevealed = false;
        isFlagged = false;
        neighborCount = 0;
        cellText.text = "";
        cellText.color = Color.black;
        button.interactable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (grid.gameEnded) return; // prevent clicks after game ends

        if (eventData.button == PointerEventData.InputButton.Right)
            ToggleFlag();
        else if (eventData.button == PointerEventData.InputButton.Left)
            HandleClick();
    }

    private void HandleClick()
    {
        if (isFlagged || isRevealed) return;

        if (isMine)
        {
            RevealMine();
            grid.GameOver(false);
        }
        else
        {
            Reveal();
        }
    }

    public void Reveal(bool recursive = true)
    {
        if (isRevealed) return;

        isRevealed = true;
        button.interactable = false;

        if (isMine)
        {
            RevealMine();
        }
        else if (neighborCount > 0)
        {
            SetNumber();
            grid.OnCellRevealed();
        }
        else
        {
            cellText.text = "";
            grid.OnCellRevealed();

            if (recursive)
                RevealAdjacentCells();
        }
    }

    private void SetNumber()
    {
        cellText.text = neighborCount.ToString();
        cellText.color = GetNumberColor(neighborCount);
    }

    public void RevealMine()
    {
        isRevealed = true;
        cellText.text = "💣";
        cellText.color = Color.red;
        button.interactable = false;
    }

    public void ToggleFlag()
    {
        if (isRevealed) return;

        isFlagged = !isFlagged;
        cellText.text = isFlagged ? "🚩" : "";
        cellText.color = isFlagged ? Color.red : Color.black;
    }

    private Color GetNumberColor(int number)
    {
        switch (number)
        {
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.red;
            case 4: return new Color(0.5f, 0, 0.5f);
            case 5: return new Color(0.5f, 0, 0);
            case 6: return Color.cyan;
            case 7: return Color.black;
            case 8: return Color.gray;
            default: return Color.black;
        }
    }

    private void RevealAdjacentCells()
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if (grid.InBounds(nx, ny))
                {
                    Cell neighbor = grid.GetCell(nx, ny);
                    if (!neighbor.isMine && !neighbor.isRevealed && !neighbor.isFlagged)
                    {
                        neighbor.Reveal(true);
                    }
                }
            }
        }
    }
}
