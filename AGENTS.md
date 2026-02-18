# InternshipProjectV2 - Agent Guidelines

This document outlines the development environment, code style, and workflows for AI agents and developers working on the `InternshipProjectV2` Unity repository.

## 1. Environment & Setup

- **Unity Version:** `6000.3.8f1`
- **Scripting Backend:** IL2CPP (WebGL), Mono (Editor/Dev Builds).
- **Api Compatibility Level:** .NET Standard 2.1.
- **Primary Platform:** WebGL (builds are automated via GitHub Actions).
- **Project Structure:**
  - All custom code, assets, and prefabs **MUST** reside in `Assets/_Project/`.
  - `Assets/_Project/Scripts/`: C# source files.
  - `Assets/_Project/Data/`: ScriptableObject assets.
  - `Assets/_Project/Scenes/`: Unity scene files.
  - Do not modify `Assets/Plugins/` or root `Assets/` unless installing a new third-party package.

## 2. Build, Lint, & Test

### Building
- **In Editor:** `File > Build Settings > WebGL > Build`.
- **Command Line (CI/CD):**
  The project uses `game-ci/unity-builder` actions. To mimic a build locally via CLI (PowerShell):
  ```powershell
  & "C:\Program Files\Unity\Hub\Editor\6000.3.8f1\Editor\Unity.exe" -quit -batchmode -projectPath . -executeMethod UnityBuilder.Build -logFile build.log
  ```
  *(Note: Requires a static build method, likely handled by the CI action internally or a helper script).*

### Testing
- **Test Runner:** Open `Window > General > Test Runner`.
- **Run All Tests:** Click `Run All` in the Test Runner window.
- **Run Specific Test:** Right-click a test in the list and select `Run`.
- **Command Line Test:**
  ```powershell
  & "C:\Program Files\Unity\Hub\Editor\6000.3.8f1\Editor\Unity.exe" -runTests -batchmode -projectPath . -testResults results.xml -testPlatform EditMode
  ```

### Linting & Validation
- **Compiler:** Ensure zero warnings in the Unity Console.
- **Validation:** No specific linter (e.g., Roslyn) is enforced via CLI, but standard IDE formatting (VS Code/Rider) should be applied.

## 3. Code Style & Conventions

Adhere strictly to these rules to maintain consistency with existing code (e.g., `GameSessionSO.cs`, `InvestigationPanelUI.cs`).

### Naming Conventions
- **Classes/Structs/Enums:** `PascalCase` (e.g., `GameSessionSO`, `SuspectState`).
- **Methods:** `PascalCase` (e.g., `ResetSession()`, `RefreshDisplay()`).
- **Public Properties:** `PascalCase` (e.g., `IsActive`).
- **Private Fields:** `camelCase` (e.g., `suspectsInSession`).
- **Serialized Fields:** `camelCase` with `[SerializeField]` attribute.
  ```csharp
  [SerializeField] private float movementSpeed;
  [SerializeField] private List<SuspectSO> suspects;
  ```
- **Constants:** `UPPER_CASE` (e.g., `MAX_RETRIES`).
- **Parameters:** `camelCase` (e.g., `int suspectId`).

### Formatting
- **Indentation:** 4 spaces (NO tabs).
- **Braces:** Allman style (opening brace on a new line).
  ```csharp
  // Correct
  public void UpdateState()
  {
      if (isValid)
      {
          Process();
      }
  }

  // Incorrect
  public void UpdateState() {
      if (isValid) {
          Process();
      }
  }
  ```
- **Spacing:** Space after `if`, `for`, `foreach`, `while`.
- **Regions:** Avoid `#region` unless the file exceeds 300 lines.

### Unity Specifics
1.  **Inspector Exposure:**
    -   Do **not** use `public` fields for Inspector configuration.
    -   Use `[SerializeField] private` instead.
    -   Group related fields with `[Header("Title")]`.
    -   Use `[Tooltip("Description")]` for complex settings.

2.  **Null Checking:**
    -   Use Unity's equality operator for `UnityEngine.Object` derived classes.
    -   Example: `if (myComponent == null)` is preferred over `myComponent is null`.

3.  **UI Components:**
    -   **Text:** Always use `TMPro.TextMeshProUGUI` (via `using TMPro;`), never standard `UnityEngine.UI.Text`.
    -   **Buttons:** `UnityEngine.UI.Button`.

4.  **Coroutines:**
    -   Prefer `Coroutines` for frame-based delays or visual sequencing.
    -   Use `UniTask` or `async/await` only if the project dependencies explicitly support it (currently standard Coroutines are observed).

### Architecture & Patterns
- **ScriptableObjects (SO):**
  -   Use for data containers (`SuspectSO`) and shared game state (`GameSessionSO`).
  -   Menu Item path: `[CreateAssetMenu(fileName = "NewData", menuName = "Data/DataType")]`.
- **MonoBehaviours:**
  -   Keep logic modular. Avoid "God classes".
  -   Use `Awake()` for initialization and `Start()` for references that depend on other objects.
- **Events:**
  -   Prefer C# `actions` or `UnityEvents` for decoupling UI from logic.

### Error Handling
-   **Logging:** Use `Debug.Log`, `Debug.LogWarning`, `Debug.LogError`.
-   **Validation:** Use `Debug.Assert` or explicit checks in `Awake` to verify required references are assigned.
    ```csharp
    private void Awake()
    {
        if (displayDetailsText == null)
        {
            Debug.LogError($"{name}: Display Details Text is missing!", this);
        }
    }
    ```

## 4. Git & File Management
- **Meta Files:** ALWAYS commit `.meta` files. They contain GUIDs and import settings.
- **Scene Files:** Avoid merging scenes. Treat scene files as binary blobs.
- **Ignored Files:** Do not commit `Library/`, `Temp/`, `Logs/`, or `UserSettings/`.

## 5. Agent Workflow Checklist
1.  **Read:** Before editing, read related files (especially ScriptableObjects) to understand data flow.
2.  **Plan:** Check if a similar feature exists. Don't reinvent the wheel.
3.  **Implement:** Follow the naming and formatting rules above.
4.  **Verify:**
    -   Does the code compile?
    -   Are `[SerializeField]` variables assigned in the Inspector (simulated thought process)?
    -   Did you mix up `Update()` and `FixedUpdate()`? (Use `FixedUpdate` for Rigidbody physics).

---
*This file is auto-generated and maintained to assist automated coding agents.*
