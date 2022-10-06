using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumsType;
using System.Threading.Tasks;
using UnityEngine.UI;
public class ButtonConfig : MonoBehaviour
{
    public EnumsType.Config CurrentButtonConfig;

    public AppManager MainAppManager;

    public async void SwitchConfig() => await MainAppManager.MainSwitchConfig(CurrentButtonConfig,this.GetComponent<Image>());
 

}
