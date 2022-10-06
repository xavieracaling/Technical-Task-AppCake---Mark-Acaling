using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;
using Asyncoroutine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using EnumsType;
using System;
using TMPro;
using Newtonsoft.Json;
using Asyncoroutine;

public class AppManager : MonoBehaviour
{
    public Transform GIFCharacterScreen;
    public UniGifImage CharacterUNIgif;
    public GameObject PrefabImageGIF;
    public GameObject HorizontalGIF;
    public Transform ParentGIFContainer;
    public Transform SavedParentGIFContainer;
    public Scrollbar ScrollBarCharacters;
    public GameObject TestToAdd;
    public List<Sprite> SpriteConfigButtons;
    public ScrollRect MainSrollRect;
    public List<Sprite> HeartSprites;
    public GameObject LoadingScreen;
    public GameObject CardStillLoading;
    public bool StillLoadingCard;
    public GameObject GroupMain;
    bool ableToLoad;
    bool scrollLoading;
    bool finishLoadingGIF;
    public bool AbleToSwitch;
    [SerializeField]
    int numberOfLastInactives = 8;
    [SerializeField]
    int limit = 16;
    public EnumsType.Config CurrentConfig;
    public ButtonConfig CurrentConfigScript;

    public static AppManager Instance;
    public List<InfoGIF> CurrentSavesHeart;
    private void Awake() {
        Instance = this;
    }
    async void loadAllSavedDatas()
    {
        if(PlayerPrefs.HasKey("HeartSaves"))
        {
            LoadingScreen.SetActive(true);
               AppManager.Instance.LoadingScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Loading...";
            CurrentSavesHeart = JsonConvert.DeserializeObject<List<InfoGIF>>(PlayerPrefs.GetString("HeartSaves"));
            GameObject horizontal = Instantiate(HorizontalGIF,default,default,SavedParentGIFContainer);
            foreach (var item in CurrentSavesHeart)
            {
                if(horizontal.transform.childCount > 1)
                    horizontal = Instantiate(HorizontalGIF,default,default,SavedParentGIFContainer);
                GameObject gifPref = Instantiate(PrefabImageGIF,default,default,horizontal.transform);
                UniGifImage uniGF = gifPref.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                uniGF.CurrentConfig = item.Config;
                loadingProgressGIF(uniGF,item);
                uniGF.Hearted = true;
                uniGF.HeartImage.sprite = AppManager.Instance.HeartSprites[1];
                uniGF.transform.parent.parent.gameObject.name = $"{uniGF.CodeName}_{uniGF.NameGIF.text}";
                //gifModification(gifPref,item,uniGF);
            }
            await new WaitForSeconds(3f);
            LoadingScreen.SetActive(false);
            
        }
    }
    void gifModification(GameObject gifPref, InfoGIF item, 
                         UniGifImage uniGF)
    {
        Image i = gifPref.GetComponent<Image>();
        switch(item.Config)
        {
            case EnumsType.Config.Animations:
                i.color = new Color(0.6792453f,0.1762193f,5398791f,1f);
                uniGF.Play();
            break;

            case EnumsType.Config.Characters:
                i.color = Color.white;
                uniGF.Stop();
            break;
        }
        
    }
    [ContextMenu("CheckHeartLoaded")]
    public void CheckHeartLoaded()
    {
       
        foreach (Transform item2 in ParentGIFContainer)
        {
            foreach (Transform item3 in item2)
            {
                UniGifImage uniGifImage = item3.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                uniGifImage.Hearted = false;
                uniGifImage.HeartImage.sprite = AppManager.Instance.HeartSprites[0];

            }
            
        }
        foreach (var item in CurrentSavesHeart)
        {
            for(int x = 0; x < ParentGIFContainer.childCount; x++)
            {
                foreach (Transform item2 in ParentGIFContainer.GetChild(x))
                {
                    UniGifImage uniGifImage = item2.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                    if(uniGifImage.CodeName == item.CodeName && 
                        item.Config == AppManager.Instance.CurrentConfig)
                    {
                        uniGifImage.Hearted = true;
                        uniGifImage.HeartImage.sprite = AppManager.Instance.HeartSprites[1];
                    }
                }
            }
        }
    }
    public void instantiateSavedDataRealtime(InfoGIF infoGIF)
    {
        GameObject horizontal = null;
        if(SavedParentGIFContainer.childCount > 0)
        {
            horizontal = SavedParentGIFContainer.GetChild(SavedParentGIFContainer.childCount - 1).gameObject;
            if(horizontal.transform.childCount > 1)
                horizontal = Instantiate(HorizontalGIF,default,default,SavedParentGIFContainer);
            
        }
        else
        {
            horizontal = Instantiate(HorizontalGIF,default,default,SavedParentGIFContainer);
        }
        GameObject gifPref = Instantiate(PrefabImageGIF,default,default,horizontal.transform);
        UniGifImage uniGF = gifPref.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
        uniGF.CurrentConfig = infoGIF.Config;
        //gifModification(gifPref,infoGIF,uniGF);
        uniGF.Hearted = true;
        uniGF.HeartImage.sprite = AppManager.Instance.HeartSprites[1];
        loadingProgressGIF(uniGF,infoGIF);
    }
    public async void SwitchToGIFCharacterScreen(UniGifImage uniGifImage)
    {
        CharacterUNIgif.NameGIF.text = uniGifImage.NameGIF.text;
        CharacterUNIgif.M_loadOnStartUrl = uniGifImage.M_loadOnStartUrl;
        CharacterUNIgif.M_rawImage.texture = uniGifImage.M_rawImage.texture;
        CharacterUNIgif.gameObject.name = uniGifImage.gameObject.name;
        CharacterUNIgif.HeartImage.sprite = uniGifImage.HeartImage.sprite;
        CharacterUNIgif.Hearted = uniGifImage.Hearted;
        //StartCoroutine(uniGifImage.SetGifFromUrlCoroutine(CharacterUNIgif.M_loadOnStartUrl));
        switch(CurrentConfig)
        {
            case EnumsType.Config.Animations:
            await ParentGIFContainer.DOLocalMoveX(-361.7f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
            CharacterUNIgif.Play();
            break;

            case EnumsType.Config.Characters:
            await ParentGIFContainer.DOLocalMoveX(-361.7f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
            break;

            case EnumsType.Config.Saved:
            await SavedParentGIFContainer.DOLocalMoveX(-361.7f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
            break;
        }
        await GIFCharacterScreen.DOLocalMoveX(-8f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        // while(uniGifImage.nowState != UniGifImage.State.Playing)
        //     await new WaitForSeconds(0.3f);
        // uniGifImage.M_rawImage.enabled = true;
        // await uniGifImage.LoadingUI.DOScale(Vector2.zero, 0.2f).SetEase(Ease.OutFlash).AsyncWaitForCompletion();
        // uniGifImage.transform.DOScale(new Vector3(0.192571476f,0.192571476f,0.192571476f), 0.5f).SetEase(Ease.OutFlash);
    }
    public async void BackFromCharacter()
    {
        await GIFCharacterScreen.DOLocalMoveX(-550,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        switch(CurrentConfig)
        {
            case EnumsType.Config.Animations:
            await    ParentGIFContainer.DOLocalMoveX(0f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();;
            break;

            case EnumsType.Config.Characters:
            await    ParentGIFContainer.DOLocalMoveX(0f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();;
            break;

            case EnumsType.Config.Saved:
            await    SavedParentGIFContainer.DOLocalMoveX(0f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();;
            break;
        }
        GroupMain.SetActive(true);

    }
    public void DownloadButton()
    {
        Application.OpenURL("https://download1480.mediafire.com/8khnkbanf5cg/n9nbdvgpdi024xw/ExampleAnimation.fbx");
    }
    async Task loadingProgressGIF(UniGifImage uniGF, InfoGIF infoGIF)
    {
        uniGF.M_loadOnStartUrl = infoGIF.URLPicture;
        uniGF.CodeName = infoGIF.CodeName;
        uniGF.NameGIF.text = infoGIF.GIFLabel;
        
        while(uniGF.nowState != UniGifImage.State.Playing)
            await new WaitForSeconds(0.3f);
        uniGF.transform.localScale = Vector3.zero;
        uniGF.M_rawImage.enabled = true;
        await uniGF.LoadingUI.DOScale(Vector2.zero, 0.2f).SetEase(Ease.OutFlash).AsyncWaitForCompletion();
        uniGF.transform.DOScale(new Vector3(0.192571476f,0.192571476f,0.192571476f), 0.5f).SetEase(Ease.OutFlash);
    }
    async void Start()
    {
        loadAllSavedDatas();
        AbleToSwitch = true;
        CurrentConfig = EnumsType.Config.Characters;
        ableToLoad = false;
        finishLoadingGIF = false;
        HtmlWeb web = new HtmlWeb();
        HtmlDocument document2 =  new HtmlDocument();
        document2.Load("testlang.html");
        HtmlNode[] allNodes = document2.DocumentNode.Descendants().Where(w => w.HasClass("product-animation")).ToArray();
        
        Debug.Log(allNodes.Length + " ang kadamoon");
        bool produceHorizontalGIF = true;
        
        int currentImage = 0;
        
        //empty
        foreach (var item in allNodes)
        {
            currentImage ++;
            if(currentImage > limit)
                break;
            string urlImage = item.ChildNodes[0].ChildNodes[0].Attributes["src"].Value;
            string productName = item.ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerHtml;
            GameObject prefabHorizontalGIF = null;
            int countChild = 0;
            if(ParentGIFContainer.childCount > 0)
                countChild = ParentGIFContainer.GetChild(ParentGIFContainer.childCount-1).childCount;
            if(!produceHorizontalGIF)
            {
                prefabHorizontalGIF = ParentGIFContainer.GetChild(ParentGIFContainer.childCount - 1).gameObject;
            }
            else if(produceHorizontalGIF) 
            {
                prefabHorizontalGIF = Instantiate(HorizontalGIF,default,Quaternion.identity,ParentGIFContainer.transform);
                produceHorizontalGIF = false;
            }
            GameObject gifImage = Instantiate(PrefabImageGIF,default,Quaternion.identity,prefabHorizontalGIF.transform);
            gifImage.transform.localScale = Vector3.zero;
            UniGifImage uniGF = gifImage.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
            countChild = ParentGIFContainer.GetChild(ParentGIFContainer.childCount-1).childCount;

            uniGF.M_loadOnStartUrl = urlImage;
            uniGF.CodeName = $"c_{currentImage}";
            uniGF.NameGIF.text = productName;
            uniGF.transform.parent.parent.gameObject.name = $"{uniGF.CodeName}_{productName}";;
            uniGF.enabled = false;
            if(countChild > 1)
                    produceHorizontalGIF = true;
            if(currentImage <= (limit - numberOfLastInactives) )
            {
                await gifImage.transform.DOScale(new Vector3(1.14139998f,1.28550172f,1.14139998f), 0.4f).SetEase(Ease.OutFlash).AsyncWaitForCompletion();
            }
                
            else
            {
                gifImage.SetActive(false);
                gifImage.transform.parent.gameObject.SetActive(false);
            }
            
            
        }
        CheckHeartLoaded();
        ableToLoad = true;
        //show images
        loadGIFS(0);
    }
    async void loadGIFS(int yx)
    {
        for(int x = yx; x < returnActiveGameObjects(); x++)
        {
            foreach (Transform item in ParentGIFContainer.GetChild(x))
            {
                UniGifImage uniGF = item.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                uniGF.enabled = true;
                while(uniGF.nowState != UniGifImage.State.Playing)
                    await new WaitForSeconds(0.3f);
                uniGF.transform.localScale = Vector3.zero;
                uniGF.M_rawImage.enabled = true;
                await uniGF.LoadingUI.DOScale(Vector2.zero, 0.2f).SetEase(Ease.OutFlash).AsyncWaitForCompletion();
                uniGF.transform.DOScale(new Vector3(0.192571476f,0.192571476f,0.192571476f), 0.5f).SetEase(Ease.OutFlash);
                if(CurrentConfig == EnumsType.Config.Characters)
                    uniGF.Stop();
            }
            if(!ableToLoad && x ==  returnActiveGameObjects() - 1)
            {
                ableToLoad = true;
                finishLoadingGIF = true;
                break;
            }
        }
    }
    int returnActiveGameObjects() 
    {
        int val = 0;
        foreach (Transform item in ParentGIFContainer)
        {
            if(item.gameObject.activeSelf)
                val++;
        }
        return val;
    }
    public async Task MainSwitchConfig(EnumsType.Config buttonConfig, Image imageButtonConfig)
    {
        if(!AbleToSwitch) return;
        if(buttonConfig == CurrentConfig) return;
        if(CurrentConfig == EnumsType.Config.Saved)
            await SavedParentGIFContainer.DOLocalMoveX(-361.7f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        CurrentConfig = buttonConfig;
        CheckHeartLoaded();
        AbleToSwitch = false;
        if(CurrentConfigScript != null)
        {
          Image currentImage =  CurrentConfigScript.GetComponent<Image>();

          currentImage.GetComponent<Image>().sprite = AppManager.Instance.SpriteConfigButtons[0];
          currentImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.627451f,0.3411765f,0.9333334f);
          
          imageButtonConfig.sprite = AppManager.Instance.SpriteConfigButtons[1];
          imageButtonConfig.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
          CurrentConfigScript = imageButtonConfig.GetComponent<ButtonConfig>();
        }
        switch(buttonConfig)
        {
            case EnumsType.Config.Characters:
                await switchAnimationConfig(false);
            break;

            case EnumsType.Config.Animations:
                await switchAnimationConfig(true);
            break;
            
            case EnumsType.Config.Saved:
                await switchAnimationConfig();
            break;
        }

    }
    async Task 
    
    switchAnimationConfig(bool play = false)
    {
        float currentXPos = ParentGIFContainer.position.x;
        Debug.Log(currentXPos + " current");
        await ParentGIFContainer.DOLocalMoveX(-361.7f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();

        if(CurrentConfig != EnumsType.Config.Saved)
        {
            MainSrollRect.content = ParentGIFContainer.GetComponent<RectTransform>();
            foreach (Transform item in ParentGIFContainer)
            {
                foreach (Transform item2 in item)
                {
                    UniGifImage uniGifImage = item2.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                    Image i = item2.GetComponent<Image>();
                    if(play)
                    {
                        uniGifImage.Play();
                        i.color = new Color(0.6792453f,0.1762193f,5398791f,1f);
                    }
                    else 
                    {
                        uniGifImage.Stop();
                        i.color = Color.white;
                    }
                    
                }
            }
            await ParentGIFContainer.DOLocalMoveX(0f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
            
        }
        else 

        {
            MainSrollRect.content = SavedParentGIFContainer.GetComponent<RectTransform>();
            GameObject horizontal;
            if(SavedParentGIFContainer.childCount < 1 && 
                CurrentSavesHeart.Count > 0)
                loadAllSavedDatas();
            foreach (Transform item in SavedParentGIFContainer)
            {
                foreach (Transform item2 in item)
                {
                    UniGifImage uniGifImage = item2.transform.GetChild(0).GetChild(0).GetComponent<UniGifImage>();
                    Image i = item2.GetComponent<Image>();
                    switch(uniGifImage.CurrentConfig)
                    {
                        case Config.Characters:
                            i.color = Color.white;
                            uniGifImage.Stop();
                        break;

                        case Config.Animations:
                            i.color = new Color(0.6792453f,0.1762193f,5398791f,1f);
                            uniGifImage.Play();
                        break;
                    }
                }
            }
            await SavedParentGIFContainer.DOLocalMoveX(0f,0.4f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        }
        AbleToSwitch = true;
        
    }
    public async void CheckScroll()
    {
        if(CurrentConfig == EnumsType.Config.Saved) return;
        if(!ableToLoad) return;
            await CheckScrollIE();
    }
    public async Task CheckScrollIE()
    {
        if(ScrollBarCharacters.value <= 0.01f)
        {
            ableToLoad = false;
            int currentActiveGameObjects = returnActiveGameObjects();
            for(int x = currentActiveGameObjects; x < currentActiveGameObjects + 1; x++ )
            {
                Transform horizontal = ParentGIFContainer.GetChild(x);
                horizontal.gameObject.SetActive(true);
                foreach (Transform item in horizontal)
                {
                    item.gameObject.SetActive(true);
                    await item.DOScale(new Vector3(1.14139998f,1.28550172f,1.14139998f),0.4f).
                            SetEase(Ease.OutFlash).AsyncWaitForCompletion();
                }
                
            }
            if(finishLoadingGIF)
                loadGIFS(returnActiveGameObjects() - 1);
        }
    }
    void Update()
    {
        
    }
}
