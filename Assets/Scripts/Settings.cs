using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Элементы UI")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public Button resetProgressBtn;
    public Button tutorialBtn;

    [Header("Панели")]
    public GameObject tutorialPanel; // Панель туториала
    private AudioListener _audioListener;

    private void Awake()
    {
        _audioListener = FindObjectOfType<AudioListener>();

        // Загружаем сохранённые настройки при старте
        LoadSettings();
    }

    // Вызывается из OnValueChanged у Slider (в инспекторе)
    public void OnVolumeChanged(float value)
    {
        if (_audioListener == null) return;
        AudioListener.volume = value;
        SaveSettings();
    }

    // Вызывается из OnValueChanged у Toggle (в инспекторе)
    public void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }

    // Кнопка «Сброс прогресса»
    public void ResetProgress()
    {
        // Здесь можно добавить подтверждение (диалог «Вы уверены?»)
        PlayerPrefs.DeleteAll(); // Удаляет все сохранения
        Debug.Log("[Settings] Прогресс сброшен.");

        // Если есть другие системы сохранений (JSON, файлы и т. п.) — их тоже нужно очистить отдельно.
    }

    // Кнопка «Руководство пользователя»
    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    // Кнопка «Закрыть» на панели туториала (назначить в инспекторе на эту функцию)
    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("IsFullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void CloseSettings()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 0) == 1;

        volumeSlider.value = volume;
        fullscreenToggle.isOn = isFullscreen;

        // Применяем сразу при загрузке
        AudioListener.volume = volume;
        Screen.fullScreen = isFullscreen;
    }
}
