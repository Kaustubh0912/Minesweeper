# 🧱 Unity Minesweeper

A fully playable Minesweeper game built in Unity, combining classic gameplay with modern UI polish. Uses 2D arrays and DFS recursion to uncover tiles — great for showcasing both Unity and algorithmic knowledge.

---

## 🎮 Features

- ✅ 2D Minesweeper grid (customizable size and mine count)
- 🚩 Right-click flagging with emoji icons
- 💣 Bomb detection and recursive reveal using DFS
- 🧠 Win condition check for all safe tiles revealed
- 🧼 Game over overlay with restart option
- 🕐 Timer and 🚩 live flag counter
- 🎨 Colored numbers, emojis, and smooth UI

---

## 🛠️ Built With

- **Unity** (2D)
- **C#**
- **TextMeshPro**
- **Basic DSA concepts** (2D arrays, recursion)

---

## 🚀 Getting Started

### Prerequisites

- Unity Editor (any recent LTS version)
- TextMeshPro package (comes with Unity by default)
- A font that supports emojis (e.g., Noto Emoji or Segoe UI Emoji)

### How to Run

1. Clone this repository:
   ```bash
   git clone https://github.com/Kaustubh0912/Minesweeper.git

2. Open the project in Unity


3. Press ▶️ Play in the Unity Editor


---

⚙️ Customization

Modify values in the GridManager.cs:
```bash
public int width = 10;
public int height = 10;
public int mineCount = 10;

Update flag or bomb icons by changing ToggleFlag() or RevealMine() in Cell.cs.


---

🎯 Learning Goals

This project demonstrates:

DFS-based tile revealing logic

Clean UI using Unity's Canvas and TextMeshPro

Safe recursive logic using 2D arrays

Unity + DSA integration (a nice portfolio piece!)



---

🙌 Credits

Created with ❤️ by NOX

Inspired by the classic Windows Minesweeper


