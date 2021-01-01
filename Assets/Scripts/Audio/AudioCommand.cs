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
    private bool recognitionStarted = false;

    SpeechConfig config = SpeechConfig.FromSubscription("6f107236326042baaee35121c20a673f", "eastus");
    SpeechRecognizer recognizer;
    TaskCompletionSource<int> stopRecognition = new TaskCompletionSource<int>();

    public GameObject commandHandlerGb;
    CommandHandler commandHandler;

   

    string[] phrases = new string[] {"open menu","close menu","select",
        "open window", "close window", "select", "up","down","forward","back","next",
    "delete","minimize","maximize","grow","shrink","show cursor","hide cursor",
        "safe mode on","safe mode off", "show safe windows", "hide safe windows", "show id", "hide id", "show calendar"};


    public void Start()
    {
        commandHandler = commandHandlerGb.GetComponent<CommandHandler>();
        //commandHandler = new CommandHandler(IotMenuGb.GetComponent<IotMenu>(), windowMenuGb.GetComponent<WindowMenu>(), handObjectGb);
        recognizer = new SpeechRecognizer(config);
        initPhraseList();
        initAudioCommands();
        micPermissionGranted = true;
        message = "";
   
    //    startRecoButton.onClick.AddListener(ButtonClick2);//(ButtonClick);
        recognizer.Recognized += RecognizingHandler;
        ButtonClick2();
    }

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
        }
    }



    public async void ButtonClick2()
    {
        if (recognitionStarted)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            lock (threadLocker)
            {
                recognitionStarted = false;
            }
        }
        else
        {
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            lock (threadLocker)
            {
                recognitionStarted = true;
            }
        }
    }


    void Disable()
    {
        recognizer.Recognizing -= RecognizingHandler;
        recognizer.Dispose();
    }

    private void OnDestroy()
    {
        Disable();
    }


    public void initPhraseList()
    {
        var phraseList = PhraseListGrammar.FromRecognizer(recognizer);
        foreach (string i in phrases)
        {
            phraseList.AddPhrase(i);
        }

        foreach(string i in addNumbers())
        {
            phraseList.AddPhrase(i);
        }

        foreach(string i in addPercents())
        {
            phraseList.AddPhrase(i);
        }

        foreach(string i in addMenuOptions())
        {
            phraseList.AddPhrase(i);
        }
    }

    string[] addNumbers()
    {
        List<string> output = new List<string>();
        for(int i = 1; i < 21; i++)
        {
            output.Add(i.ToString());
        }
        return output.ToArray();
    }

    string[] addPercents()
    {
        List<string> output = new List<string>();
        for(int i = 10; i <= 200; i += 10)
        {
            output.Add(i.ToString() + " percent");
        }
        return output.ToArray();
    }

    string[] addMenuOptions()
    {
        List<string> output = new List<string>();
        MenuOption start = new Home();

        LinkedList<MenuOption> queue = new LinkedList<MenuOption>();


        output.Add(start.name);
        queue.AddLast(start);

        while (queue.Count > 0)
        {

            MenuOption s = queue.First.Value;
            queue.RemoveFirst();

            if(s.submenus == null)
            {
                continue;
            }

            foreach (var menu in s.submenus)
            {
                if (!output.Contains(menu.name))
                {
                    output.Add(menu.name);
                    queue.AddLast(menu);
                }
            }
        }

        return output.ToArray();
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

        audioCommands.Add("show id", delegate { commandHandler.showIds(); });
        audioCommands.Add("hide id", delegate { commandHandler.hideIds(); });

        //all of these only work if a window is currently selected
        audioCommands.Add("delete", delegate { commandHandler.delete(); });
        audioCommands.Add("grow", delegate { commandHandler.grow(); }); //each by 10%
        audioCommands.Add("shrink", delegate { commandHandler.shrink(); });

        audioCommands.Add("safe mode on", delegate { commandHandler.safeModeOn(); });
        audioCommands.Add("safe mode off", delegate { commandHandler.safeModeOff(); });

        audioCommands.Add("show safe windows", delegate { commandHandler.showSafeWindows(); });
        audioCommands.Add("hide safe windows", delegate { commandHandler.hideSafeWindows(); });

        audioCommands.Add("select", delegate { commandHandler.select(); });

        audioCommands.Add("show calendar", delegate { commandHandler.calendar(); });
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

        Debug.LogError(command + " command");

        if (audioCommands.ContainsKey(command))
        {
            Debug.LogError("found in audio commands, executing...");
            audioCommands[command].Invoke();
        } else
        {
            parseComplex(command);
        }
    }

    //there are all commands that are more complex and need further parsing
    void parseComplex(string command)
    {
        var split = command.Split();
        if (command.Contains("select"))
        {
            if(split.Length != 2)
            {
                commandHandler.audioError();
                return;
            }

            if (mode.currentMode == modes.IOT_MENU)
            {
                commandHandler.selectIotName(split[1]);
            }
            else if (mode.currentMode == modes.NONE)
            {
                int id;
                if (split.Length == 2 && int.TryParse(split[1], out id))
                {
                    bool outcome  = commandHandler.selectId(id);
                    if (!outcome)
                    {
                        commandHandler.commandError();
                    }
                    else
                    {
                        commandHandler.completed();
                    }
                }
            }

        } else if(command.Contains("grow") || command.Contains("shrink")){
            if(split.Length == 3)
            {
                int percent = getPercent(split[1], split[2]);
                //then check that it is valid, and grow/shrink it by that percent 
                if(WindowManager.currentWindow == null)
                {
                    commandHandler.selectError();
                }
                var window = WindowManager.currentWindow.GetComponent<Window>();
                if (command.Contains("grow"))
                {
                    WindowManager.resizeWindow(window, percent);
                } else
                {
                    WindowManager.resizeWindow(window, -percent);
                }
            }

        } else
        {
            commandHandler.audioError();
        }
    }

    int getPercent(string a, string b)
    {
        if(b.Trim() != "percent")
        {
            return 0;
        }

        int output = 0;
        if(!int.TryParse(a, out output))
        {
            return 0;
        }
        return output;
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

