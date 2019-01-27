using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 
    public bool HasGameStarted { get; private set;  }
    public bool IsPaused { get; private set; }
    public bool ingame;

    public float gameTimer = 0;

    [SerializeField] Image filledBar;
    [SerializeField] Character character;

    [SerializeField] GameObject InGameGroup;
    [SerializeField] GameObject MenuGroup;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject EndingGroup;
    [SerializeField] AudioManager audio;

    [SerializeField] Button playButton;
    [SerializeField] Button mainMenuExitButton;
    [SerializeField] Button pauseMenuResumeButton;
    [SerializeField] Button pauseMenuQuitButton;
    [SerializeField] Button endMenuQuitButton; 


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
        ingame = true;
        audio.Stop("menu_music");
        audio.Play("intro");
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
        if (gameTimer >= 356) EndGame(); 
    }
    void EndGame()
    {
        IsPaused = true;
        audio.Stop("Musique ambiance");
        audio.Play("music_fin");
        EndingGroup.SetActive(true); 
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
        ingame = false;
    }

    private void Update()
    {
        UpdateFilledBar();
        UpdateTimer(); 
        if( !audio.IsPlaying("intro")&& ingame && !audio.IsPlaying("boucle_tortue"))
        {
            audio.Play("boucle_tortue");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame(); 
    }
}
