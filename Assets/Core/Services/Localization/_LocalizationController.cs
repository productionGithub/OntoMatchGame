using UnityEngine;
using System.Collections.Generic;
using StarterCore.Core.Services.Navigation;
using Zenject;
using TMPro;

using StarterCore.Core.Utils;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.GameState;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarterCore.Core.Services.Network.Models;


using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Services.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        [Inject] private NavigationService _navService;
        [Inject] private MockNetService _netService;
        [Inject] private GameStateManager _gamestate;

        TranslationsModel _languageDictionary;

        async private void Start()
        {
            LocalesManifestModel manifestModel = await GetLocaleManifest();//Contains languages file paths

            //Test if game locale exists in manifestModel
            if (SearchLocaleMatch(manifestModel))
            {
                //If yes, get the corresponding language file
                _languageDictionary = await GetLocaleDictionary(_gamestate.Locale);
            }
            else
            {
                Debug.Log("Locale not found in language file, falling back to default language : " + _gamestate.DefaultLocale);
                _languageDictionary = await GetLocaleDictionary(_gamestate.DefaultLocale);
            }


        }

        public string GetTranslation(string key)
        {
            //Return the value corresponding to key, from Dictionary
            Debug.Log("======================>" + _languageDictionary.StaticText);//[_navService.CurrentSceneName]);

            string translated = "oo";
            if (translated.Equals(key))
            {
                return  translated;
            }
            else
            {
                Debug.LogError("[Localization Manager] Could not find following key in language dictionary : " + key);
                return "";
            }
        }

        public async UniTask<LocalesManifestModel> GetLocaleManifest()
        {
            Debug.Log("GET LOCALE MANIFEST ASYNC");
            var result = await _netService.GetLocalesManifestFile();
            Debug.Log("RESULT RETURNED IS %" + result.Locales.ToString());
            return result;
        }

        public async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            var result = await _netService.GetLocaleDictionary(locale);
            return result;
        }

        public bool SearchLocaleMatch(LocalesManifestModel m)
        {
            foreach (string l in m.Locales.Keys)
            {
                if (l.Equals(_gamestate.Locale))
                {
                    return true;
                }
            }
            return false;
        }

        //public async UniTaskVoid ChangeLocale(string locale)
        //{
        //    TranslationsModel t;//Dictionary of all static texts in a specific language

        //    LocalesManifestModel manifestModel = await GetLocaleManifest();
        //    Debug.Log("MANIFEST MODEL IS " + manifestModel.Locales);
        //    if (SearchLocaleMatch(manifestModel))
        //    {
        //        t = await GetLocaleDictionary(_gamestate.Locale);
        //        Debug.Log("==> Locale found in language file !" + t.StaticText["SigninScene"]["title-text"]);
        //        TranslateStaticText(t);
        //    }
        //    else
        //    {
        //        Debug.Log("Locale not found in language file, falling back to default language : " + _gamestate.DefaultLocale);
        //        t = await GetLocaleDictionary(_gamestate.DefaultLocale);
        //        TranslateStaticText(t);
        //    }
        //}


        public bool TranslateStaticText(TranslationsModel t)
        {
            TextMeshProUGUI[] _tmProUGUIList = FindObjectsOfType<TextMeshProUGUI>();

            //Parse scene names in JSON
            foreach (KeyValuePair<string, Dictionary<string, string>> snKey in t.StaticText)
            {
                //if scene name is current scene name
                if (snKey.Key.Equals(_navService.CurrentSceneName))
                {
                    //Parse all TMPro text fields
                    foreach (TextMeshProUGUI tmpObj in _tmProUGUIList)
                    {
                        Debug.Log("Translating --> : " + tmpObj.text);// _gamestate.DefaultLocale);

                        //Parse all JSON Key for language dictionary
                        int index = 0;// index needed for SetValue() method.
                        foreach (KeyValuePair<string, string> dicS in t.StaticText[snKey.Key])
                        {
                            //if field text matches Json key, update Filed content with JSon value
                            if (dicS.Key.Equals(tmpObj.text))
                            {
                                tmpObj.SetText(dicS.Value);
                                break;
                            }
                            index++;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}















/*
using UnityEngine;
using System.Collections.Generic;
using StarterCore.Core.Services.Navigation;
using Zenject;
using TMPro;

using StarterCore.Core.Utils;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.GameState;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarterCore.Core.Services.Network.Models;


using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Services.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        [Inject] private NavigationService _navService;
        [Inject] private MockNetService _netService;
        [Inject] private GameStateManager _gamestate;

        async void Start()
        {
            TranslationsModel t;//Dictionary of all static texts in a specific language

            LocalesManifestModel manifestModel = await GetLocaleManifest();
            if(SearchLocaleMatch(manifestModel))
            {
                //t = await GetLocaleDictionary(_gamestate.Locale);
                //Debug.Log("Locale path =*= " + manifestModel.Locales[_gamestate.Locale]);

                t = await GetLocaleDictionary(_gamestate.Locale);
            }
            else
            {
                Debug.Log("Locale not found in language file, falling back to default language : " + _gamestate.DefaultLocale);
                t = await GetLocaleDictionary(_gamestate.DefaultLocale);
            }
            TranslateStaticText(t);
        }

        private async UniTask<LocalesManifestModel> GetLocaleManifest()
        {
            var result = await _netService.GetLocalesManifestFile();
            return result;
        }

        private async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            var result = await _netService.GetLocaleDictionary(locale);
            return result;
        }

        private bool SearchLocaleMatch(LocalesManifestModel m)
        {
            foreach (string l in m.Locales.Keys)
            {
                if(l.Equals(_gamestate.Locale))
                {
                    return true;
                }
            }
            return false;
        }

        private void TranslateStaticText(TranslationsModel t)
        {
            TextMeshProUGUI[] _tmProUGUIList = FindObjectsOfType<TextMeshProUGUI>();

            //Parse scene names in JSON
            foreach (KeyValuePair<string, Dictionary<string, string>> snKey in t.StaticText)
            {
                //if scene name is current scene name
                if (snKey.Key.Equals(_navService.CurrentSceneName))
                {
                    //Parse all TMPro text fields
                    foreach (TextMeshProUGUI tmpObj in _tmProUGUIList)
                    {
                        //Parse all JSON Key for language dictionary
                        int index = 0;// index needed for SetValue() method.
                        foreach (KeyValuePair<string, string> dicS in t.StaticText[snKey.Key])
                        {
                            //if field text matches Json key, update Filed content with JSon value
                            if (dicS.Key.Equals(tmpObj.text))
                            {
                                tmpObj.text = dicS.Value;
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
        }
    }
}

*/