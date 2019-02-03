using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 
    public bool HasGameStarted { get; private set;  }
    public bool IsPaused { get; private set; }
    public bool Ingame;
    public bool IsGoodEnding
    {
        get
        {
            if (!GridManager.Instance) return true;
            return GridManager.Instance.Cells.Where(c => c.State == CellState.Dirty).ToList().Count < GridManager.Instance.Cells.Count / 2;  
        }
    }

    float gameTimer = 0;
    public float GameTimer { get { return gameTimer; } }

    [SerializeField] Image filledBar;
    [SerializeField] Character character;

    [SerializeField] GameObject InGameGroup;
    [SerializeField] GameObject MenuGroup;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject BadEndingGroup;
    [SerializeField] GameObject GoodEndingGroup;
    [SerializeField] GameObject CreditGroup; 
    [SerializeField] AudioManager audio;

    [SerializeField] Button playButton;
    [SerializeField] Button mainMenuExitButton;
    [SerializeField] Button pauseMenuResumeButton;
    [SerializeField] Button pauseMenuQuitButton;
    [SerializeField] Button endMenuQuitButton;
    [SerializeField] Button badEndMenuQuitButton;
    [SerializeField] Button creditButton;
    [SerializeField] Button backCreditButton; 


    void UpdateFilledBar()
    {
        if (!filledBar || !character) return;
        filledBar.fillAmount = Mathf.Lerp(filledBar.fillAmount, ((float)character.Energy / (float)character.MaxEnergy), Time.deltaTime * 10); 
    }

    void PlayGame()
    {
        if(MenuGroup)
            MenuGroup.SetActive(false);
        if (InGameGroup)
            InGameGroup.SetActive(true);
        HasGameStarted = true;
        GridManager.Instance.StartBehaviour();
        Ingame = true;
        audio.Play("narrator_start");
        audio.Stop("menu_music");
        audio.Play("Musique ambiance");
    }

    void ResumeGame()
    {
        if (PauseMenu)
            PauseMenu.SetActive(false);
        if (InGameGroup)
            InGameGroup.SetActive(true);
        IsPaused = false;
    }

    void PauseGame()
    {
        IsPaused = true; 
        if (InGameGroup)
            InGameGroup.SetActive(false); 
        if (PauseMenu)
            PauseMenu.SetActive(true); 
    }

    void UpdateTimer()
    {
        if (!HasGameStarted || IsPaused) return;
        gameTimer += Time.deltaTime;
        if (gameTimer >= 252) EndGame(); 
    }

    void EndGame()
    {
        IsPaused = true;
        audio.Stop("Musique ambiance");
        if (IsGoodEnding)
        {
            audio.Play("music_good_ending");
            if (GoodEndingGroup) ; 
            GoodEndingGroup.SetActive(true);
        }
        else
        {
            audio.Play("music_bad_ending");
            if (BadEndingGroup)
                BadEndingGroup.SetActive(true);
        }
    }

    public void LooseGame()
    {
        IsPaused = true;
        audio.Stop("Musique ambiance");
        audio.Play("music_bad_ending");
        if(BadEndingGroup)
            BadEndingGroup.SetActive(true);
    }

    public void ShowCredits()
    {
       if(CreditGroup && MenuGroup)
       {
            CreditGroup.SetActive(true);
            MenuGroup.SetActive(false); 
       }
    }

    public void HideCredits()
    {
        if (CreditGroup && MenuGroup)
        {
            CreditGroup.SetActive(false);
            MenuGroup.SetActive(true);
        }
    }

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if(!character)
            character = FindObjectOfType<Character>();
        if (playButton)
            playButton.onClick.AddListener(PlayGame);
        if (mainMenuExitButton)
            mainMenuExitButton.onClick.AddListener(() => Application.Quit());
        if (pauseMenuResumeButton)
            pauseMenuResumeButton.onClick.AddListener(ResumeGame);
        if (pauseMenuQuitButton)
            pauseMenuQuitButton.onClick.AddListener(() => Application.Quit());
        if (endMenuQuitButton)
            endMenuQuitButton.onClick.AddListener(() => Application.Quit());
        if(badEndMenuQuitButton)
            badEndMenuQuitButton.onClick.AddListener(() => Application.Quit());
        if (creditButton)
            creditButton.onClick.AddListener(ShowCredits);
        if (backCreditButton)
            backCreditButton.onClick.AddListener(HideCredits);
        Ingame = false;
    }

    private void Update()
    {
        UpdateFilledBar();
        UpdateTimer(); 
        if( !audio.IsPlaying("intro")&& Ingame && !audio.IsPlaying("boucle_tortue"))
        {
            audio.Play("boucle_tortue");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame(); 
    }
}
