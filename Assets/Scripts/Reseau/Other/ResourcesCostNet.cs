using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesCostNet : MonoBehaviour
{
    TransformerUINet transformerUI;
    Image[] imagesResources = new Image[2];
    Text[] textResources = new Text[2];
    int nbDifferentResourcesNeeded = 0;
	// Use this for initialization
	void Start ()
    {
        transformerUI = GetComponentInParent<TransformerUINet>();
        imagesResources = GetComponentsInChildren<Image>();
        textResources = GetComponentsInChildren<Text>();
    }

    public void SetResourcesImages()
    {
        if (transformerUI.GetCurrentMenu() == 1)
        {
            if (PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetMultipleElement())
            {
                imagesResources[0].sprite = PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0];
                imagesResources[1].enabled = true;
                imagesResources[1].sprite = PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[1];
                textResources[1].enabled = true;

                switch (PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0].name)
                {
                    case "BoisSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                        break;
                    case "FerSansCadre" :
                        textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                        break;
                    case "DiamondSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                        break;
                    case "MithrilSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                        break;
                    case "CraneSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                        break;

                    default:
                        break;
                }

                switch (PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[1].name)
                {
                    case "BoisSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                        break;
                    case "FerSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                        break;
                    case "DiamondSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                        break;
                    case "MithrilSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                        break;
                    case "CraneSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                imagesResources[0].sprite = PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0];
                imagesResources[1].enabled = false;
                textResources[1].enabled = false;

                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDustRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveTrapRecipe(transformerUI.GetCurrentRecipe() - 1).GetDustRessources();
                }
            }
        }
        else if(transformerUI.GetCurrentMenu() == 2)
        {
            if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMultipleElement())
            {
                imagesResources[0].sprite = PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0];
                imagesResources[1].enabled = true;
                imagesResources[1].sprite = PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[1];
                textResources[1].enabled = true;

                switch (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0].name)
                {
                    case "BoisSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                        break;
                    case "FerSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                        break;
                    case "DiamondSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                        break;
                    case "MithrilSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                        break;
                    case "CraneSansCadre":
                        textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                        break;

                    default:
                        break;
                }

                switch (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[1].name)
                {
                    case "BoisSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                        break;
                    case "FerSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                        break;
                    case "DiamondSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                        break;
                    case "MithrilSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                        break;
                    case "CraneSansCadre":
                        textResources[1].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                        break;

                    default:
                        break;
                }

            }
            else
            {
                imagesResources[0].sprite = PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).iconResource[0];
                imagesResources[1].enabled = false;
                textResources[1].enabled = false;

                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetWoodRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetIronRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDiamondRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetMithrilRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetBonesRessources();
                }
                if (PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDustRessources() > 0)
                {
                    textResources[0].text = ": " + PeonNet.instance.GiveWeaponRecipe(transformerUI.GetCurrentRecipe() - 1).GetDustRessources();
                }
            }
        }
    }
}
