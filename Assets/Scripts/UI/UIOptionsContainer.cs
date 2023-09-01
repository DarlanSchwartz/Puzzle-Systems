using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptionsContainer : MonoBehaviour
{
    public string[] options;

    public int currentOption;

    public Text currentOptionUIOutput;

    public UnityEvent onValueChanged;

    private void Start()
    {
        UpdateUI();
    }

    public string currentOptionText
    {
        get
        {
            return options[currentOption];
        }
    }

    public void SetCurrentOption(string optionValue)
    {
        for (int i = 0; i < (options.Length); i ++)
        {
            if (optionValue == options[i])
            {
                currentOption = i;
                UpdateUI();
            }
        }
    }

    public void SetCurrentOptions(string[] optionsArray)
    {
        options = optionsArray;
        UpdateUI();
    }

    public void ChangeCurrentOption(bool more)
    {
        if(options.Length > 0)
        {
            if(more)
            {
                if ((currentOption + 1) <= options.Length -1)
                {
                    currentOption ++;
                    UpdateUI();
                }
                else if((currentOption + 1) > options.Length -1)
                {
                    currentOption = 0;
                    UpdateUI();
                }
            }
            else
            {
                if ((currentOption - 1) >= 0)
                {
                    currentOption --;
                    UpdateUI();
                }
                else if ((currentOption - 1) < 0)
                {
                    currentOption = options.Length -1;
                    UpdateUI();
                }
            }

            ManagerSound.ClickSound();

            onValueChanged.Invoke();
        }
    }

    void UpdateUI()
    {
        currentOptionUIOutput.text = options[currentOption];
    }
}
