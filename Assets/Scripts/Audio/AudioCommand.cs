using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine.UI;
using System.Text;

public class AudioCommand : MonoBehaviour
{
    // Hook up the two properties below with a Text and Button object in your UI.
    //  public Text outputText;
    public Button startRecoButton;


    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

    SpeechConfig config = SpeechConfig.FromSubscription("6f107236326042baaee35121c20a673f", "eastus");
    SpeechRecognizer recognizer;
    public GameObject commandHandlerGb;
    CommandHandler commandHandler;

    string[] phrases = new string[] {"open menu","close menu","select",
        "open window", "close window", "select", "up","down","forward","back","next",
    "delete","minimize","maximize","grow","shrink","show cursor","hide cursor",
        "safe mode on","safe mode off", "show safe windows", "hide safe windows"};


    public void Start()
    {
        commandHandler = commandHandlerGb.GetComponent<CommandHandler>();
        //commandHandler = new CommandHandler(IotMenuGb.GetComponent<IotMenu>(), windowMenuGb.GetComponent<WindowMenu>(), handObjectGb);
        recognizer = new SpeechRecognizer(config);
        initPhraseList();
        initAudioCommands();
        micPermissionGranted = true;
        message = "";
        startRecoButton.onClick.AddListener(ButtonClick);
    }


    public void initPhraseList()
    {
        var phraseList = PhraseListGrammar.FromRecognizer(recognizer);
        foreach (string i in phrases)
        {
            phraseList.AddPhrase(i);
        }
    }

    public async void ButtonClick()
    {
        lock (threadLocker)
        {
            waitingForReco = true;
        }

        var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

        string newMessage = string.Empty;
        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            newMessage = result.Text;
        }
        else if (result.Reason == ResultReason.NoMatch)
        {
            newMessage = "NOMATCH: Speech could not be recognized.";
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(result);
            newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
        }

        lock (threadLocker)
        {
            message = newMessage;
            waitingForReco = false;
        }
    }
    



    Dictionary<string, Action> audioCommands = new Dictionary<string, Action>();


    public void initAudioCommands()
    {
        audioCommands.Add("open menu", delegate { commandHandler.openMenu(); });
        audioCommands.Add("close menu", delegate { commandHandler.closeMenu(); });
        audioCommands.Add("next", delegate { commandHandler.next(); });
        audioCommands.Add("back", delegate { commandHandler.back(); });

        audioCommands.Add("open window", delegate { commandHandler.openWindow(); });
        audioCommands.Add("close window", delegate { commandHandler.closeWindow(); });
        audioCommands.Add("up", delegate { commandHandler.up(); });
        audioCommands.Add("down", delegate { commandHandler.down(); });

        audioCommands.Add("show cursor", delegate { commandHandler.showCursor(); });
        audioCommands.Add("hide cursor", delegate { commandHandler.hideCursor(); });

        //all of these only work if a window is currently selected
        audioCommands.Add("delete", delegate { commandHandler.delete(); });
        audioCommands.Add("grow", delegate { commandHandler.grow(); }); //each by 10%
        audioCommands.Add("shrink", delegate { commandHandler.shrink(); });

        audioCommands.Add("safe mode on", delegate { commandHandler.safeModeOn(); });
        audioCommands.Add("safe mode off", delegate { commandHandler.safeModeOff(); });

        audioCommands.Add("show safe windows", delegate { commandHandler.showSafeWindows(); });
        audioCommands.Add("hide safe windows", delegate { commandHandler.hideSafeWindows(); });

        audioCommands.Add("select", delegate { commandHandler.select(); });
    }

    void parseMessages(string command)
    {
         command = command.ToLower();
        var sb = new StringBuilder();

        foreach (char c in command)
        {
            if (!char.IsPunctuation(c))
                sb.Append(c);
        }

        command = sb.ToString().Trim();

        Debug.LogError("after parsed: " + command);
        if (audioCommands.ContainsKey(command))
        {
            Debug.LogError("found in audio commands, executing...");
            audioCommands[command].Invoke();
        } else
        {
            commandHandler.audioError();
        }
    }

    public void Update()
    {

        lock (threadLocker)
        {
            if (startRecoButton != null)
            {
                startRecoButton.interactable = !waitingForReco && micPermissionGranted;
            }
            if(message != "")
            { 
                parseMessages(message);
                message = "";
            }
           
        }
    }

}

