using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavigationMenu : MonoBehaviour
{
    public VisualElement primaryNav;
    public VisualElement secondaryNav;
    [SerializeField] private SceneAsset testSceneAsset;
    private string testSceneName;

    void Start()
    {
        if (testSceneAsset != null)
        {
            testSceneName = testSceneAsset.name;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        primaryNav = root.Q<VisualElement>("PrimaryNav");
        secondaryNav = root.Q<VisualElement>("SecondaryNav");

        if (primaryNav == null)
        {
            Debug.LogError("PrimaryNav element not found");
            return;
        }

        if (secondaryNav == null)
        {
            Debug.LogError("SecondaryNav element not found");
            return;
        }

        var primaryButtons = primaryNav.Query<Button>().ToList();
        foreach (var button in primaryButtons)
        {
            button.RegisterCallback<ClickEvent>(evt => OnPrimaryButtonClick(button));
        }

        var secondaryButtons = secondaryNav.Query<Button>().ToList();
        foreach (var button in secondaryButtons)
        {
            button.RegisterCallback<ClickEvent>(evt => OnSecondaryButtonClick(button));
        }

        var testButton = root.Q<Button>("Btn_TEST");
        if (testButton != null)
        {
            testButton.RegisterCallback<ClickEvent>(evt => OnTestSceneButtonClick());
        }
        else
        {
            Debug.LogError("Btn_TEST element not found");
        }
    }

    void OnPrimaryButtonClick(Button clickedButton)
    {
        var primaryButtons = primaryNav.Query<Button>().ToList();
        foreach (var button in primaryButtons)
        {
            button.RemoveFromClassList("text-glow-white");
            button.AddToClassList("text-lg");
            button.AddToClassList("text-opacity");
        }
        clickedButton.AddToClassList("text-glow-white");

        var secondaryButtons = secondaryNav.Query<Button>().ToList();
        foreach (var button in secondaryButtons)
        {
            button.RemoveFromClassList("text-lg");
            button.RemoveFromClassList("text-opacity");
            button.RemoveFromClassList("text-glow-white");
        }
    }

    void OnSecondaryButtonClick(Button clickedButton)
    {
        var secondaryButtons = secondaryNav.Query<Button>().ToList();
        foreach (var button in secondaryButtons)
        {
            button.RemoveFromClassList("text-glow-white");
            button.AddToClassList("text-lg");
            button.AddToClassList("text-opacity");
        }
        clickedButton.AddToClassList("text-glow-white");

        var primaryButtons = primaryNav.Query<Button>().ToList();
        foreach (var button in primaryButtons)
        {
            button.RemoveFromClassList("text-lg");
        }
    }

    void OnTestSceneButtonClick()
    {
        LoadTestScene();
    }

    public void LoadTestScene()
    {
        SceneManager.LoadScene(testSceneName);
    }
}