using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenResolutions : MonoBehaviour
{
    Resolution[] resolutions;
    Resolution currentResolution;
    public Dropdown dropdownMenu;
    void Start()
    {
        Resolution oldResolution = new Resolution();
        oldResolution.height = Screen.height;
        oldResolution.width = Screen.width;

        resolutions = Screen.resolutions;

        int countList = 0;
        dropdownMenu.value = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            
            bool strUnique = true;
            if (resolutions[i].refreshRate == 60)
            {
                dropdownMenu.options[countList].text = ResToString(resolutions[i]);
                if (oldResolution.width == resolutions[i].width && oldResolution.height == resolutions[i].height)
                {
                    dropdownMenu.value = i;
                }
                dropdownMenu.options.Add(new Dropdown.OptionData(dropdownMenu.options[countList].text));

                countList++;
            }
            else
            {
                if (resolutions[i].refreshRate == 59)
                {
                    foreach (Dropdown.OptionData option in dropdownMenu.options)
                    {
                        string str = ResToString(resolutions[i]);
                        if (str == option.text)
                        {
                            strUnique = false;
                        }
                    }
                    if (strUnique)
                    {
                        dropdownMenu.options[countList].text = ResToString(resolutions[i]);
                        if (oldResolution.width == resolutions[i].width && oldResolution.height == resolutions[i].height)
                        {
                            dropdownMenu.value = i;
                        }
                        dropdownMenu.options.Add(new Dropdown.OptionData(dropdownMenu.options[countList].text));

                        countList++;
                    }
                }
            }
        }

        for (int i = 0; i < dropdownMenu.options.Count; i++)
        {
            
            if (dropdownMenu.options[i].text == ResToString(oldResolution))
            {
                dropdownMenu.value = i;
                dropdownMenu.itemText.text = dropdownMenu.options[i].text;
            }
            //else
            //{
            //    dropdownMenu.value = dropdownMenu.options.Count - 1;
            //}
        }

        dropdownMenu.options.RemoveAt(dropdownMenu.options.Count - 1);
        SetResolution(oldResolution);
    }


    private void Update()
    {
        //dropdownMenu.value = 16;
    }

    public void OnResolutionValueChange()
    {
        // Modifier rotation de la fleche
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate == 60)
            {
                if (ResToString(r) == dropdownMenu.options[dropdownMenu.value].text)
                {
                    currentResolution = r;
                }
            }
            else
            {
                if (r.refreshRate == 59)
                {
                    if (ResToString(r) == dropdownMenu.options[dropdownMenu.value].text)
                    {
                        currentResolution = r;
                    }
                }
            }
            SetResolution(currentResolution);
        }
    }


    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    public void SetResolution(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
