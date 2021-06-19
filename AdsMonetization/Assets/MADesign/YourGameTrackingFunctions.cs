using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MADesign {
    public class YourGameTrackingFunctions
    {
        private const string TAG = "YourGameTrackingFunctions";

        public static IMATrackingImplement trackingImplement;

        public static void doTrackingImplement(string eventName, Dictionary<string, object> parameters)
        {

#if UNITY_EDITOR
            //string logParam = "";
            //foreach (KeyValuePair<string, object> kvp in parameters)
            //{
            //    logParam += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value) + "\n";
            //}

            //Debug.LogFormat("{0} - doTrackingImplement {1} - {2}", TAG, eventName, logParam);
#endif

            if (trackingImplement != null)
            {
                trackingImplement.doTracking(eventName, parameters);
            }
        }

        public static void doSetUserProperty(string key, string v)
        {
            if (trackingImplement != null)
            {
                trackingImplement.setUserProperty(key, v);
            }
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Splash Screen or Loading Screen
        // -------------------------------------------------------------------------
        public static void LoadingScene_trackingAwake()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("loading_awake", dataDic);
        }

        public static void LoadingScene_trackingStart()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("loading_start", dataDic);
        }

        public static void LoadingScene_trackingOnDestroy()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("loading_ondestroy", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Play Screen bao gồm: MenuScene và PlayScene
        // -------------------------------------------------------------------------
        public static void PlayScene_trackingAwake()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("play_awake", dataDic);
        }

        public static void PlayScene_trackingOnDestroy()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("play_ondestroy", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // MenuScene
        // -------------------------------------------------------------------------
        public static void PlayScene_MenuScene_callShow()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("play_call_show_menu", dataDic);
        }

        public static void PlayScene_MenuScene_clickPlayButton(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_click_play_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickRestoreIAPButton(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_click_restore_iap_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickBackgroundButton(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_click_background_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickSoundButton(int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["sound"] = isOn;
            doTrackingImplement("play_menu_click_sound_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickMusicButton(int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["music"] = isOn;
            doTrackingImplement("play_menu_click_music_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickLevelButton(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_click_level_button", dataDic);
        }

        public static void PlayScene_MenuScene_clickRemoveAdsButton(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_click_removeads_button", dataDic);
        }

        public static void PlayScene_MenuScene_callStartGame(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_menu_call_start_game", dataDic);
        }

        public static void PlayScene_MenuScene_gameReadyToPlay(int level, int loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["loading_interval_in_seconds"] = loading_interval_in_seconds;
            doTrackingImplement("play_menu_game_ready_to_play", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // InGame
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_readyToPlay(int level)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            doTrackingImplement("play_ingame_ready_to_play", dataDic);
        }

        public static void PlayScene_InGame_userFirstAction(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_user_first_action", dataDic);
        }

        public static void PlayScene_InGame_clickUndoButton(int level, int from_ready_to_play_interval_in_seconds, int remainUndoCount, bool action, string caseIndentifer = "Unknow")
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["undo_count"] = remainUndoCount;
            dataDic["action"] = action;
            dataDic["case_indentifer"] = caseIndentifer;
            doTrackingImplement("play_ingame_click_undo", dataDic);
        }

        public static void PlayScene_InGame_clickHintButton(int level, int from_ready_to_play_interval_in_seconds, int remainHintCount, bool action, string caseIndentifer = "Unknow")
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["hint_count"] = remainHintCount;
            dataDic["action"] = action;
            dataDic["case_indentifer"] = caseIndentifer;
            doTrackingImplement("play_ingame_click_hint", dataDic);
        }

        public static void PlayScene_InGame_clickRefreshButton(int level, int from_ready_to_play_interval_in_seconds, int remainRefreshCount, bool action, string caseIndentifer = "Unknow")
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["refresh_count"] = remainRefreshCount;
            dataDic["action"] = action;
            dataDic["case_indentifer"] = caseIndentifer;
            doTrackingImplement("play_ingame_click_refresh", dataDic);
        }

        public static void PlayScene_InGame_clickTutorialButton(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_click_tutorial", dataDic);
        }

        public static void PlayScene_InGame_clickMenuButton(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_click_menu", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Game Level Loading
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_dataLoad(int level, int data_loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = data_loading_interval_in_seconds;
            doTrackingImplement("play_ingame_data_load", dataDic);
        }

        public static void PlayScene_InGame_gamesceneLoad(int level, int data_loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = data_loading_interval_in_seconds;
            doTrackingImplement("play_ingame_gamescene_load", dataDic);
        }

        public static void PlayScene_InGame_levelNotFound(int level, int data_loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = data_loading_interval_in_seconds;
            doTrackingImplement("play_ingame_level_not_found", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Game Scene Loading
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_GameScene_Start_Load(int level, int data_loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = data_loading_interval_in_seconds;
            doTrackingImplement("play_ingame_gamescene_start_load", dataDic);
        }

        public static void PlayScene_InGame_GameScene_Loaded(int level, int data_loading_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = data_loading_interval_in_seconds;
            doTrackingImplement("play_ingame_gamescene_loaded", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Game Result Tracking
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_result(int level, int from_ready_to_play_interval_in_seconds, bool isWin, int levelPlayCount)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["isWin"] = isWin;
            dataDic["play_count"] = levelPlayCount;
            doTrackingImplement("play_ingame_result", dataDic);

            if (level < 61) {
                string eventName = string.Format("level_{0}", level);
                doTrackingImplement(eventName, dataDic);
            }
        }

        public static void PlayScene_InGame_win(int level, int from_ready_to_play_interval_in_seconds, int levelPlayCount)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["play_count"] = levelPlayCount;
            doTrackingImplement("play_ingame_win", dataDic);
        }

        public static void PlayScene_InGame_loose(int level, int from_ready_to_play_interval_in_seconds, int levelPlayCount)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["play_count"] = levelPlayCount;
            doTrackingImplement("play_ingame_loose", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // SecondChance Dialog
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_SecondChance_clickSecondChanceButton(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_sc_click_secondchance", dataDic);
        }

        public static void PlayScene_InGame_SecondChance_clickReplayButton(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_sc_click_replay", dataDic);
        }

        public static void PlayScene_InGame_SecondChance_EventTracking(int level, int from_ready_to_play_interval_in_seconds, string actionIndentifer) {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["actionIndentifer"] = actionIndentifer;
            doTrackingImplement("play_ingame_sc_event", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // SellectLevel Dialog
        // -------------------------------------------------------------------------
        public static void PlayScene_SelectLevelDialog_clickOnLevelItem(int level, int reachedMaxLevel)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["reachedMaxLevel"] = reachedMaxLevel;
            dataDic["isPlayingOldOne"] = level < reachedMaxLevel;
            doTrackingImplement("play_selectLevel_click_levelitem", dataDic);
        }

        public static void PlayScene_SelectLevelDialog_clickCloseButton()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("play_selectLevel_click_close", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Sellect Background Dialog
        // -------------------------------------------------------------------------
        public static void PlayScene_SelectBackgroundDialog_clickOnBackgroundItem(string backgroundId, int reachedMaxLevel)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["backgroundId"] = backgroundId;
            dataDic["reachedMaxLevel"] = reachedMaxLevel;
            doTrackingImplement("play_selectBackground_click_background", dataDic);
        }

        public static void PlayScene_SelectBackgroundDialog_clickCloseButton()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("play_selectBackground_click_close", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // InGame PausePopup
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_Pause_clickOnActionButton(string action, int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["action"] = action;
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("play_ingame_pause_action", dataDic);
        }

        public static void PlayScene_InGame_Pause_clickMusicButton(int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["music"] = isOn;
            doTrackingImplement("play_ingame_pause_click_music_button", dataDic);
        }

        public static void PlayScene_InGame_Pause_clickSoundButton(int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["sound"] = isOn;
            doTrackingImplement("play_ingame_pause_click_sound_button", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Sound + Music Settings
        // -------------------------------------------------------------------------
        public static void Global_musicSettingChanged (string source, int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["level"] = level;
            dataDic["music"] = isOn;
            doTrackingImplement("global_music_setting_change", dataDic);
        }

        public static void Global_soundSettingChanged(string source, int level, bool isOn)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["source"] = source;
            dataDic["level"] = level;
            dataDic["sound"] = isOn;
            doTrackingImplement("globel_sound_setting_change", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // LevelFailed popup
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_LevelFailed_Event(int level, int from_ready_to_play_interval_in_seconds, string actionIndentifer)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["actionIndentifer"] = actionIndentifer;
            doTrackingImplement("play_ingame_levelfailed_event", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // LevelComplete popup
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_Complete_Event(int level, int from_ready_to_play_interval_in_seconds, string actionIndentifer)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["actionIndentifer"] = actionIndentifer;
            doTrackingImplement("play_ingame_levelcomplete_event", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Tutorial popup
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_TutorialPopup_Event(int level, int from_ready_to_play_interval_in_seconds, string actionIndentifer)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            dataDic["actionIndentifer"] = actionIndentifer;
            doTrackingImplement("play_ingame_tutorialpopup_event", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // NewSkin popup
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_NewSkinPopup_Event(int level, string actionIndentifer)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["actionIndentifer"] = actionIndentifer;
            doTrackingImplement("play_ingame_newskinpopup_event", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Tutorial Flow
        // -------------------------------------------------------------------------
        public static void PlayScene_InGame_Tutorial_Begin_Event(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("tutorial_begin", dataDic);
        }

        public static void PlayScene_InGame_Tutorial_Complete_Event(int level, int from_ready_to_play_interval_in_seconds)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["interval_in_seconds"] = from_ready_to_play_interval_in_seconds;
            doTrackingImplement("tutorial_complete", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // IAP Dialog
        // -------------------------------------------------------------------------
        public static void PlayScene_IAP_Purchase_Helper_Success_Event(int level, string productId, int moreHelper)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["productId"] = productId;
            dataDic["moreHelper"] = moreHelper;
            doTrackingImplement("iap_helper_success", dataDic);
        }

        public static void PlayScene_IAP_Purchase_Helper_Failed_Event(int level, string reason)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["reason"] = reason;
            doTrackingImplement("iap_helper_failed", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Tooltip Tutorial
        // -------------------------------------------------------------------------
        public static void PlayScene_Tooltip_Tutorial_Event(int level, string action)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["action"] = action;
            doTrackingImplement("tooltip_tutorial", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // LevelComingSoonPopup
        // -------------------------------------------------------------------------
        public static void PlayScene_LevelComingSoonPopup_Event(int level, string action)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["level"] = level;
            dataDic["action"] = action;
            doTrackingImplement("coming_soon_popup", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // User Properties
        // -------------------------------------------------------------------------
        public static void setUserLevel(int level) {
            doSetUserProperty("user_reach_level", level.ToString());
        }

        public static void setUserSoundSetting(bool isOn)
        {
            doSetUserProperty("user_sound_setting", isOn ? "on" : "off");
        }

        public static void setUserMusicSetting(bool isOn)
        {
            doSetUserProperty("user_music_setting", isOn ? "on" : "off");
        }

        public static void setUserBackgroundSetting(string backgroundId)
        {
            doSetUserProperty("user_background_setting", backgroundId);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Error Tracking
        // -------------------------------------------------------------------------
        public static void error_GameEngine(string errorKey, Dictionary<string, object> moreInfomation = null) {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();

            if (moreInfomation != null && moreInfomation.Count > 0) {
                dataDic = new Dictionary<string, object>(moreInfomation);
            }

            dataDic["error"] = errorKey;

            doTrackingImplement("error_game_engine", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // FOR DEVELOPER
        // 
        // Try to reuse all functions above. If you cannot do this, please insert
        // more functions from here.
        // 
        // You can also remove unused above funtions. Clean and keep all your tracking
        // functions in this file.
        // -------------------------------------------------------------------------


        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Chia theo event_name.
        //
        // Result: Used for create Funnel.
        // -------------------------------------------------------------------------
        public static void LoadingScene_trackingAwake_()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("loading_awake", dataDic);
        }

        public static void GameScene_trackingAwake_()
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            doTrackingImplement("gamescene_awake", dataDic);
        }

        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Cùng event name, nhưng khác parameter truyền vào.
        //
        //  Result: Sẽ xác định được tỉ lệ phân hoá (%) của các params truyền vào.
        // -------------------------------------------------------------------------
        public static void Common_trackingAwake(string sceneName)
        {
            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            dataDic["sceneName"] = sceneName;
            doTrackingImplement("common_tracking_awake", dataDic);
        }


    }
}

