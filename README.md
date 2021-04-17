# Proton

The goal of Proton, is to solve a common problem of mine, which is having to switch back and forth between tabs. Although a second monitor might fix this, there are several
advantages to using Proton instead. 

First, bigger is better, and its hard to beat the size of a projector. Second, the system allows you to use any empty or unused space in your room, rather than taking up
precious real-estate on your desk. Third, it gives you more freedom in terms of where and how you work. Often, I like to switch between working sitting down or 
standing up. With Proton, there is little to no adjustmant required. There is no need to find a place to place my monitors, or find a surface for my mouse to avoid hunching over,
you can simply use it identical as to when you are sitting. The intuitive controls make it easy to use (more on that below), and the option of voice commands make it even easier.

<b>For a quick demo, check out the youtube video: https://youtu.be/-rruIseN1SM</b>

## Setup Instructions
1. Download the project as a zip file, and open it in Unity
2. Build it onto whatever platform you like
3. Download Kinect SDK for Windows 2.0
4. Plug any projector into your computer
5. Plug a Kinect sensor into your computer (you may need a Windows adapter)
6. Just run the project and done!

## Controls:
There are 2 main ways to control the system, with gestures and with voice commands. Lets start with the gestures: <br>

### IoT Menu
To open make a fist with your left hand. Next you can select any option by moving your left hand in a clockwise, or counter clockwise motion slowly.
To select an option, make another fist. Finally, to close the menu simply move your hand quickly in a straight line in any direction (as if you were swiping the menu off the screen).

### Window Menu
To open, hold a fist in your right hand (for about 2 seconds). Next, you can select any option by moving your right hand either up or down. To select an option, make a fist with your right hand.
Then, that window will appear, and the menu will disappear. Keeping your right hand in a fist, you can move the window around to any location you like. To resize the window, make a fist with your left hand (while keeping your right hand in a fist)
and move your hands closer together or farther apart, to exit the resizing mode, simply release your left hand's fist. Finally, to stop moving the window, simply release your right hand's fist.

### Selecting a Window
To select a window, quickly make a fist with your right hand (for less than 2 seconds), and a cursor will appear. Next, move the cursor over whichever window you would like to select, and make a fist, the window is now selected.
You can now resize the window, or change its position (same gestures as mentioned above). To delete a window, simply move your right hand quickly in a straight line, in any direction (again, as if you are swiping it off the screen).
If you opened the cursor by mistake, simply do the deleting gesture (swiping quickly in any direction), and it will disappear.
<br>

Next, lets review the voice commands:

### Iot Menu:
To open, simply say "Open menu". Next, to select an option say "Select x", where you replace 'x' with whatever the option is called. To close the menu say "Close menu".
You can also say "next", or "back" to select the next or previsou option, and then choose the current option by saying "select". 

### Window Menu
To open, say "Open window". Next, say "up" or "down" to change the current selected window, to choose the current window, say "select". To close the menu, say "Close window".

### Manipulating windows
As above, you can manipulate windows by first selecting them, than completing some action (like resizing). This can be down with the cursor, or voice commands. To show the cursor, say
"show cursor", to hide it say "hide cursor". You can also select any window, by saying "show id", this will show a number from 1 to n, where n is the number of windows, below each window, this is their id.
You can then say "select x" where x is a valid window id. This window is now selected. You can then resize the window by saying "grow by x percent" or "shrink by x percent" where x is replaced with a valid number. To delete a window
just say "delete".
<br>

Safe mode windows: Finally, there are "safe mode windows", which you can think of as Incognito tabs. While in safe mode, if the system detects another person in the room, it will hide any safe mode windows.
To turn on safe mode, say "safe mode on". Any windows opened after this will automatically be in safe mode. To turn off safe mode say "safe mode off". If you choose to hide, or show safe mode tabs, you can say
"hide safe mode", or "show safe mode" respectivaly. 

<b>Note about software</b>
It is still a work in progress, and not guarenteed to be stable.
