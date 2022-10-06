using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Asyncoroutine;
using DG.Tweening;
public class GIFModifier : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    public async void OnPointerClick(PointerEventData eventData)
    {
        UniGifImage uniGifImage = transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
        if(uniGifImage.M_rawImage !=  null)
        {
            if(uniGifImage.M_rawImage.texture != null)
            {
                AppManager.Instance.SwitchToGIFCharacterScreen(uniGifImage);
                AppManager.Instance.GroupMain.SetActive(false);
            }
            
        }
        else 
        {
            if(!AppManager.Instance.StillLoadingCard)
            {
                AppManager.Instance.StillLoadingCard = true;
                await AppManager.Instance.CardStillLoading.transform.DOScale(new Vector3(2.490803f,0.6619312f,1.904249f),0.3f).
                                                               SetEase(Ease.InFlash).AsyncWaitForCompletion();
                await new WaitForSeconds(1.5f);
                await AppManager.Instance.CardStillLoading.transform.DOScale(Vector3.zero,0.2f).
                                                               SetEase(Ease.InFlash).AsyncWaitForCompletion();
                AppManager.Instance.StillLoadingCard = false;

            }
            
            Debug.Log("texture is null");
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
