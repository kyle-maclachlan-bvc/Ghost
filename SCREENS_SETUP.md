# Ghost — Start / Pause / Game Over Screens Setup

This guide wires up the three menu screens to the scripts I updated. Think of the
scripts as the wiring loom in a car: the harness is in place, you just need to plug
each connector (button, panel, text) into the right socket in the Inspector.

## What the scripts now do

- **`GameManager`** owns a `GameState` (`Start → Playing → Paused → GameOver`).
  - On scene load it shows the **Start screen** and runs nothing else.
  - `StartGame()` resets scores/timer and begins play.
  - **Esc** toggles Pause/Resume during play (sets `Time.timeScale` to 0/1, so the
    timer and ghost freeze).
  - `RestartGame()` reloads the current scene from scratch.
  - `QuitGame()` quits the build (and stops Play mode in the editor).
  - Mashing only counts while state is `Playing`, so presses on the menus are ignored.
- **`UIManager`** switches panels and updates the HUD. It exposes slots for the
  three screens, the HUD container, and the text fields.

`PlayerMasher` and `GhostController` are unchanged.

---

## Step 1 — Import the button + title sprites

Your art lives in `Assets/_Project/Art/Photoshop/`. For each PNG used in the UI
(`TitleScreen`, and all nine button images), select it and in the Inspector set:

- **Texture Type:** `Sprite (2D and UI)`
- Click **Apply**.

The nine button images are the three states for each button:

| Button  | Neutral            | Hover                   | Pressed                   |
|---------|--------------------|-------------------------|---------------------------|
| Start   | `StartButton`      | `StartButtonHover`      | `StartButtonPressed`      |
| Restart | `RestartButton`    | `RestartButtonHover`    | `RestartButtonPressed`    |
| Quit    | `QuitButton`       | `QuitButtonHover`       | `QuitButtonPressed`       |

---

## Step 2 — Make sure there's a Canvas + EventSystem

Open `Assets/_Project/Scenes/Levels/GhostLevel.unity`.

- If there's no **Canvas**: right-click in the Hierarchy → **UI → Canvas**. This also
  creates an **EventSystem** automatically. The EventSystem is what makes mouse
  hover/click work — without it the buttons do nothing.
- On the Canvas, set **Canvas Scaler → UI Scale Mode = Scale With Screen Size**,
  Reference Resolution `1920 × 1080`. This keeps the layout consistent at the arcade
  resolution.

> If your EventSystem was created for the new Input System, Unity may prompt you to
> replace the old `Standalone Input Module` with `InputSystemUIInputModule`. Accept it,
> since this project uses the new Input System.

---

## Step 3 — Build the four UI containers

Under the Canvas, create four empty UI objects (right-click Canvas → **Create Empty**,
or **UI → Panel** if you want a full-screen background). Name them exactly:

```
Canvas
├── HUD              <- timer + score texts (your existing gameplay UI)
├── StartScreen
├── PauseScreen
└── GameOverScreen
```

For each of `StartScreen`, `PauseScreen`, `GameOverScreen`, stretch its RectTransform
to fill the Canvas (anchor preset: hold **Alt+Shift** and click the bottom-right
stretch preset).

### HUD
Move your existing **Timer** and **Player 1 / Player 2** text objects under `HUD`
(or create three `TMP_Text` objects if they aren't there yet).

### StartScreen
1. Add an **Image** child, set its **Source Image** to `TitleScreen`, stretch to fill.
2. Add a **Button - TextMeshPro** child → this is the **Start** button (see Step 4).

### PauseScreen
1. Optional: add a semi-transparent **Image** as a dim background (black, alpha ~150).
2. Add two buttons: **Resume** (use the Start art, or any button art you like) and
   **Restart**, plus optionally **Quit**.

### GameOverScreen
1. Add a **TMP_Text** for the result message — name it so you can find it. Set it to
   center-aligned, large.
2. Add two buttons: **Restart** and **Quit**.

---

## Step 4 — Turn each button into a 3-state sprite button

For every button you created (Start, Resume, Restart, Quit):

1. Select the Button object. In the **Image** component, set **Source Image** to that
   button's **neutral** sprite (e.g. `StartButton`).
2. In the **Button** component:
   - **Transition:** `Sprite Swap`
   - **Highlighted Sprite:** the **Hover** sprite (e.g. `StartButtonHover`)
   - **Pressed Sprite:** the **Pressed** sprite (e.g. `StartButtonPressed`)
   - **Selected Sprite:** leave empty, or set it to the Hover sprite too.
3. Delete the child **Text (TMP)** object if your button art already includes the
   label (yours does).
4. Click the Image's **Set Native Size** so it matches the art's pixel size, then
   scale/position to taste.

`Sprite Swap` is what maps your three art states to Normal / Highlighted / Pressed —
no extra script needed.

> For **Resume**, you don't have dedicated art. Reuse the Start button art, or drop in
> a plain Unity sprite — the wiring in Step 6 is identical.

---

## Step 5 — Hook up the UIManager

Select the GameObject that has the **UIManager** component (likely your `GameManager`/
`UI` object). Drag into its Inspector slots:

- **Start Screen** → `StartScreen`
- **Pause Screen** → `PauseScreen`
- **Game Over Screen** → `GameOverScreen`
- **Hud** → `HUD`
- **Timer Text** → your timer `TMP_Text`
- **Player 1 Text / Player 2 Text** → the score texts (optional; safe if left empty)
- **Result Text** → the message text inside `GameOverScreen`

You can leave the three screens **active** in the editor while you build them —
`GameManager.Start()` calls `ShowStartScreen()` on play, which hides the others.

---

## Step 6 — Hook button clicks to GameManager

Each button has an **On Click ()** list at the bottom of its **Button** component.
Click **+**, drag the **GameManager** object into the object slot, then pick the
function from the dropdown:

| Screen          | Button   | Function (`GameManager → ...`)        |
|-----------------|----------|---------------------------------------|
| StartScreen     | Start    | `StartGame()`                         |
| PauseScreen     | Resume   | `ResumeGame()`                        |
| PauseScreen     | Restart  | `RestartGame()`                       |
| PauseScreen     | Quit     | `QuitGame()`                          |
| GameOverScreen  | Restart  | `RestartGame()`                       |
| GameOverScreen  | Quit     | `QuitGame()`                          |

Make sure you pick the functions under the **GameManager** (dynamic/`MonoScript`)
section, not a duplicate — they're plain public methods with no arguments.

---

## Step 7 — Add the scene to Build Settings

`RestartGame()` reloads the active scene by build index, so the scene must be in
**File → Build Settings → Scenes In Build**. Add `GhostLevel` if it isn't listed.

---

## Step 8 — Test

1. Press **Play**. You should land on the Start screen; the timer should not be
   counting.
2. Click **Start** → HUD appears, timer counts down, mashing works.
3. Press **Esc** → Pause screen appears, timer freezes. **Esc** again or **Resume**
   continues.
4. Let the timer hit 0 or a player reach the target → Game Over screen with the
   result message; **Restart** reloads, **Quit** stops Play mode.

---

## Notes / gotchas

- **Quit in the editor** just stops Play mode (that's the `#if UNITY_EDITOR` branch).
  In a real Windows build it calls `Application.Quit()`.
- Pause uses `Time.timeScale = 0`. If you later add animations or audio that must keep
  running while paused, set their update mode to **Unscaled Time**.
- If buttons highlight but clicks do nothing, you're missing an **EventSystem** or the
  **On Click** target wasn't assigned.
- If hover/press art doesn't change, the button **Transition** isn't set to
  `Sprite Swap`, or the sprite slots are empty.
