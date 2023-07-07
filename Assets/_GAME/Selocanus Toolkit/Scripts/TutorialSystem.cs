using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [Header("Tutorial Variables")]
    public bool TutorialSystemEnabled = true;
    public bool TutorialPause;
    public bool UpgradesEnabled = true;
    public bool FirstClick, SecondClick, ThirdClick, FourthClick, FifthClick;
    public bool TutorialCompleted;
    public bool GameStarted;
    [Header("Tutorial References")]
    public TMPro.TextMeshProUGUI Text1, Text2, Text3, Text4, Text5;
    public Image Icon1, IconArrow1, IconArrow2, IconArrow3, IconArrow4;
    private void Start()
    {
        //TutorialCompleted = ProgressManager.Instance.TutorialStatus();
        TutorialSystemEnabled = !TutorialCompleted;
        if (!TutorialSystemEnabled)
        {
            FirstClick = true;
            SecondClick = true;
            ThirdClick = true;
            FourthClick = true;
            FifthClick = true;
            UpgradesEnabled = true;
        }
    }

    public void ChangeTutorialPart()
    {
        Time.timeScale = 0f;
        if (TutorialSystemEnabled)
        {
            if (!Text1.gameObject.activeInHierarchy && !FirstClick)
            {
                Text1.gameObject.SetActive(true);
                Icon1.gameObject.SetActive(true);
                FirstClick = true;
            }
            else if ((Text1.gameObject.activeInHierarchy && !Text2.gameObject.activeInHierarchy) && (FirstClick && !SecondClick))
            {
                Text1.gameObject.SetActive(false);
                Icon1.gameObject.SetActive(false);

                IconArrow1.gameObject.SetActive(true);
                Text2.gameObject.SetActive(true);
                SecondClick = true;
            }
            else if ((Text2.gameObject.activeInHierarchy && !Text3.gameObject.activeInHierarchy) && (SecondClick && !ThirdClick))
            {
                Text2.gameObject.SetActive(false);
                IconArrow1.gameObject.SetActive(false);

                Text3.gameObject.SetActive(true);
                IconArrow2.gameObject.SetActive(true);
                ThirdClick = true;
            }
            else if (Text3.gameObject.activeInHierarchy && !Text4.gameObject.activeInHierarchy && (ThirdClick && !FourthClick))
            {
                Text3.gameObject.SetActive(false);
                IconArrow2.gameObject.SetActive(false);

                IconArrow3.gameObject.SetActive(true);
                Text4.gameObject.SetActive(true);
                FourthClick = true;
            }
            else if (Text4.gameObject.activeInHierarchy && (FourthClick && !FifthClick))
            {
                IconArrow3.gameObject.SetActive(false);
                Text4.gameObject.SetActive(false);

                Text5.gameObject.SetActive(true);
                IconArrow4.gameObject.SetActive(true);
                FifthClick = true;
            }
            else if (Text5.gameObject.activeInHierarchy && (FifthClick))
            {
                Text5.gameObject.SetActive(false);
                IconArrow4.gameObject.SetActive(false);

                TutorialCompleted = true;
                //ProgressManager.Instance.TutorialCompleted();
                Time.timeScale = 1f;
                CloseAllGUI();
            }
        }
    }

    public void ChangeTutorialPart(int number)
    {
        //if (ProgressManager.Instance.TutorialStatus()) return;
        Time.timeScale = 0;
        TutorialPause = true;
        switch (number)
        {
            case 1:
                Text1.gameObject.SetActive(true);
                Icon1.gameObject.SetActive(true);
                FirstClick = true;
                break;
            case 2:
                Text1.gameObject.SetActive(false);
                Icon1.gameObject.SetActive(false);
                Text2.gameObject.SetActive(true);
                IconArrow1.gameObject.SetActive(true);
                SecondClick = true;
                break;
            case 3:
                Text2.gameObject.SetActive(false);
                Text3.gameObject.SetActive(true);
                IconArrow1.gameObject.SetActive(false);
                IconArrow2.gameObject.SetActive(true);
                ThirdClick = true;
                break;
            case 4:
                Text3.gameObject.SetActive(false);
                IconArrow2.gameObject.SetActive(false);
                IconArrow3.gameObject.SetActive(true);
                Text4.gameObject.SetActive(true);
                FourthClick = true;
                //ProgressManager.Instance.TutorialCompleted();
                break;
            case 5:
                Text4.gameObject.SetActive(false);
                IconArrow3.gameObject.SetActive(false);
                TutorialCompleted = true;
                //ProgressManager.Instance.TutorialCompleted();
                break;
            default:
                CloseAllGUI();
                break;
        }
    }

    public void CloseAllGUI()
    {
        Time.timeScale = 1;
        Text1.gameObject.SetActive(false);
        Icon1.gameObject.SetActive(false);
        Text2.gameObject.SetActive(false);
        IconArrow1.gameObject.SetActive(false);
        Text3.gameObject.SetActive(false);
        IconArrow2.gameObject.SetActive(false);
        Text4.gameObject.SetActive(false);
        IconArrow3.gameObject.SetActive(false);
        Text5.gameObject.SetActive(false);
        IconArrow4.gameObject.SetActive(false);
        TutorialPause = false;
    }
}