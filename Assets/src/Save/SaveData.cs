using System;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine;
[Serializable]
public enum WatchDisplay
{
    Present,
    HoursAgo,
    YearAgo,
    Error,
    None,
}
[Serializable]
public enum Ending
{
    Normal_CaseClosed,
    Bad_Delusion,
    Secret_Freedom,
    None
}
// Contains persistent data for changing game environment
// and state of quicksaved game state
[Serializable]
public class SaveData
{
    #region Persistent Flags for ending & environment changes
    // State data that isn't reset on New Game
    // Reset this by clearing cache
    [Serializable]
    public class PersistentSave
    {
        // If Ai Aria is captured and you reconnect into sandbox
        public bool AriaGone;
        // No. of times player launched this game after first new game
        // Triggers new game tutorial sequences for new game players
        public int ContinueCount;
        public string PlayerID;
        // display slightly different title/continue/new game sequences
        // depending on ending
        [JsonConverter(typeof(StringEnumConverter))]
        public Ending CurrentEnding;
        public PersistentSave( 
            bool isAriaDead,
            string id,
            int continueCount, Ending previousEnding)
        {
            AriaGone = isAriaDead;
            PlayerID = id;
            ContinueCount = continueCount;
            CurrentEnding = previousEnding;
        }
        // Default ctor, for fresh game/clear cache
        public PersistentSave()
        {
            AriaGone = false;
            PlayerID = "05127899";
            ContinueCount = 0;
            CurrentEnding = Ending.None;
        }
        public PersistentSave(PersistentSave persistent)
        {
            AriaGone = persistent.AriaGone;
            PlayerID = persistent.PlayerID;
            ContinueCount = persistent.ContinueCount;
            CurrentEnding = persistent.CurrentEnding;
        }
    }
    #endregion
    public PersistentSave Persistent;
    // Freedom flag before reaching ending
    public bool FreedomRoute;
    public bool CheckedSubjectProfile;
    public int ChapterIndex;
    // Where is the player looking at
    public string CurrentlyViewing;
    public DateTime InitDate;
    // Check if player found and dragged out the Jamming device found in secret puzzle 
    // Did player resolve malware overtaking Aria?
    // Secret/True End Flag
    public WatchDisplay WatchState;
    public AriaState AriaStatus;
    // time of the simulation, watch displays it
    public WatchDisplay SimulationTime;
    public KDefenderGameState KDGameState;
    [AssetReferenceUILabelRestriction("thought")]
    [JsonConverter(typeof(AssetReferenceListJsonConverter))]
    public List<AssetReference> HeldThoughts;
    public List<ChatHistory> ChatHistories;
    [JsonIgnore]
    public string InitTime => InitDate.ToString("d").Replace(@"/", string.Empty);
    // New save file
    public SaveData()
    {
        ChapterIndex = 0;
        ChatHistories = new List<ChatHistory>();
        HeldThoughts = new List<AssetReference>();
        SimulationTime = WatchDisplay.Present;
        InitDate = DateTime.Now;
        CheckedSubjectProfile = false;
        FreedomRoute = false;
        WatchState = WatchDisplay.None;
        Persistent = new PersistentSave();
        CurrentlyViewing = "RoomRight";
        AriaStatus = AriaState.Default;
        KDGameState = new KDefenderGameState{ };
    }
    public SaveData(SaveData copy)
    {
        ChapterIndex = copy.ChapterIndex;
        ChatHistories = new List<ChatHistory>(copy.ChatHistories);
        HeldThoughts = new List<AssetReference>(copy.HeldThoughts);
        SimulationTime = copy.SimulationTime;
        CheckedSubjectProfile = copy.CheckedSubjectProfile;
        InitDate = copy.InitDate;
        FreedomRoute = copy.FreedomRoute;
        WatchState = copy.WatchState;
        Persistent = new PersistentSave(copy.Persistent);
        CurrentlyViewing = copy.CurrentlyViewing;
        AriaStatus = new AriaState(copy.AriaStatus);
        KDGameState = copy.KDGameState;
    }
}