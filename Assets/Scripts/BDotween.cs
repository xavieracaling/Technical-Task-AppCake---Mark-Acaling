using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using TMPro;
using UnityEngine.UI;
using EnumsType;
public class BDotween : MonoBehaviour
{
    public Ease EaseType;
    public float Seconds;
    [SerializeField]
    bool isText;
    Vector2 CurrentLocalScale;
    bool available;
    private void Awake() {
        available = true;
        CurrentLocalScale = transform.localScale;
    }

    public async void OnHover()
    {
        if(!available) return;
        available = false;
        transform.DOKill();
        await transform.DOScale(new Vector3(CurrentLocalScale.x * 1.3f,CurrentLocalScale.y * 1.3f,1f),Seconds).SetEase(EaseType).AsyncWaitForCompletion();
    }
    public void OnLeave()
    {
        transform.DOKill();
        transform.DOScale(new Vector3(CurrentLocalScale.x,CurrentLocalScale.y,1f),Seconds).SetEase(EaseType);
        if(!available)
            available = true;
    }
    public void OnDown()
    {

    }
    public void OnSelect()
    {
        if(isText && AppManager.Instance.AbleToSwitch && AppManager.Instance.CurrentConfig != EnumsType.Config.Saved)
        {
            transform.GetComponent<Image>().sprite = AppManager.Instance.SpriteConfigButtons[1];
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else 
        {
            
        }
    }
    public void Deselect()
    {
        if(isText)
        {
            transform.GetComponent<Image>().sprite = AppManager.Instance.SpriteConfigButtons[0];
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.627451f,0.3411765f,0.9333334f);
        }
    }
}
