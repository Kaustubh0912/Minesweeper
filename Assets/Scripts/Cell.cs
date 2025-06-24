using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Media;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool isMine;
    public bool isRevealed;
    public bool isFlagged;
    public int adjacentMines;

    public Button button;
    public TextMeshProUGUI cellText;

    public GridManager grid;

    void Update()
    {
        if (grid.IsGameEnded()) return;

        if (Input.GetMouseButtonDown(1)) 
        {
            Vector2 mousePos = Input.mousePosition;
            RectTransform rt = GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mousePos))
            {
                ToggleFlag();
            }
        }
    }

    public void Init(int x , int y , GridManager grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        cellText = GetComponentInChildren<TextMeshProUGUI>();

        isMine = false;
        isRevealed = false;
        isFlagged = false;
        adjacentMines = 0;
        cellText.text = "";
        cellText.color = Color.black;
        button.interactable = true;
    }

    void OnClick()
    {
        if (grid.IsGameEnded()) return;

        if (isFlagged||isRevealed) return;

        if (isMine)
        {
            RevealMine();
            grid.GameOver(false);
        }
        else
        {
            Reveal();
            grid.CheckWinCondition();
        }
    }

    public void Reveal()
    {
        if (isRevealed) return;

        isRevealed = true;
        button.interactable = false;

        if(adjacentMines>0)
        {
            cellText.text = adjacentMines.ToString();
            cellText.color = GetNumberColor(adjacentMines);

        }
        else
        {
            cellText.text = "";
            grid.RevealAdjacentCells(x, y);
        }

    }

    public void RevealWithoutRecursion()
    {
        if (isRevealed) return;

        isRevealed = true;
        button.interactable = false;

        if (adjacentMines > 0)
        {
            cellText.text = adjacentMines.ToString();
            cellText.color = GetNumberColor(adjacentMines);
        }

        else
            cellText.text = "";
    }


    public void RevealMine()
    {
        isRevealed = true;
        cellText.text = "💣";
        cellText.color = Color.red;
        button.interactable = false;

        ColorBlock colors = button.colors;
        colors.disabledColor = Color.red;
        button.colors = colors;
    }

    public void ToggleFlag()
    {
        if (isRevealed || grid.IsGameEnded()) return;

        isFlagged = !isFlagged;

        if (isFlagged)
            grid.UpdateFlagCount(1);
        else
            grid.UpdateFlagCount(-1);

        cellText.text = isFlagged ? "🚩" : "";
        cellText.color = isFlagged ? Color.red : Color.black;
    }


    Color GetNumberColor(int number)
    {
        switch (number)
        {
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.red;
            case 4: return new Color(0.5f, 0, 0.5f); // Purple
            case 5: return new Color(0.5f, 0, 0); // Dark red
            case 6: return Color.cyan;
            case 7: return Color.black;
            case 8: return Color.gray;
            default: return Color.black;
        }
    }
}
