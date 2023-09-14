<p align="center">
        <img src="nonUnity_folder/images/banner.png" width="960" height="180">
</p>

# What is the Object Location Memory Task?

The object location memory (OLM) task was designed to investigate grid-like activity using fMRI on one hand and on the other hand to assess spatial memory in humans inspired by [Doeller et al. (2010)](https://pubmed.ncbi.nlm.nih.gov/20090680/). The task was primarily created to support our own research but is made available to the community at-large so you can use it for your research. The design of the task is kept general & configurable so it can be used without the need to adapt the Unity3D code itself. However, the Unity3D code is shared alongside the build versions so further development and customisation are possible. While the task is shared under GPL-3.0 license, several free assets and third-party resources are used, which are excluded from this license. A list of these resources can be found __here__. 

The task is programmed using the wonderful _Unity Experiment Framework (UXF)_ package. More documentation can be found on their [website](https://immersivecognition.com/unity-experiment-framework/), their [wiki](https://github.com/immersivecognition/unity-experiment-framework/wiki) and in their corresponding paper [Brookes et al. (2020)](https://link.springer.com/article/10.3758/s13428-019-01242-0). 

To do

- banner
- Add description of manual rotation
- add description of AudioTrialCounter
- New features

# Documentation

The idea of the task is participants, who complete it, learn where a number of objects are located within environments during the _encoding_ trials. Learning & performance is assessed on _retrieval_ trials. As an additional condition, we have added _control_ trials, where all spatial cues apart from the ground and the walls are removed in order to provide an appropriate condition that can be used as a control as it features translation without spatial cues necessary for navigation  and orientation (i.e. landmarks). 

In standard mode, each trial starts with a cue period during which the target object is presented as an image on the screen on top of a semi-transparent grey panel. This is followed by a delay period where the image of the target is replaced with a fixation marker. After this, the participant can move and complete the task. This is followed by a stationary ITI period. The duration of the three periods (cue, delay & ITI) can be configured separately for each trial. All trials end with the participant "collecting" the target object by running over/into it. Unless the experiment is set to continuous mode, which only makes sense for encoding trials because no cue is presented apart from on the first trial, the participant is teleported to a pre-specified location. 

If specified, a message (e.g. with further instructions) can be displayed after a trial. However, before the actual message is displayed, a standard pause display is presented that is closed by pressing the space bar. The actual message is closed soon as the letter S is pressed or an MRI scanner sends an S to the task computer (more on this below). 

In order for the task to begin after filling out the UXF start up menu, the program is also waiting for an "S". 

## Trial types

### Encoding

The idea behind the _encoding_ trial is that the participant has to learn the object locations. The object is therefore presented from the beginning and just has to be collected. 

### Control

_Control_ trials are identical to _encoding_ trials apart from the fact that all background objects (e.g. landmarks) and other spatial cues are removed to serve as a control condition that has translation but is not based on "map navigation". Note even the sun orientation is changed so it shines orthogonal to the plane of the ground removing the spatial cues provided by the shadows. 

### Retrieval

In contrast to the two other trial types, the object is not visible from the beginning for _retrieval_ trials. Instead, participants are supposed to navigate to the location where they think the target object is hidden and then press the confirmation key after which the object will appear providing feedback to the participants. After that, the object only needs to be collected like on the other trials. 

## The environments

The OLM task comprises two main environments that are meant for experiments plus an additional simple environment that is meant to teach participants how this task works. The main environments have been optimised to run even on slightly weaker hardware including on laptop using integrated GPUs. 

<p align="center">
        <img src="nonUnity_folder/images/bird_view.png" width="1016" height="317">
</p>

|            | Grassy Arena    | Practice Arena    | Desert Arena       |
| ---------- | --------------- | ----------------- | ------------------ |
| Shape      | Circle          | Hexagon           | Square             |
| Dimensions | 180 vm diameter | 50 vm side length | 190 vm side length |

## The objects available

Objects were chosen based on being relatively symmetric along their vertical axes to avoid view-dependent confounds and sourced from the internet from various websites. Because at far distance, objects are still difficult to see, we added a large glowing-red arrow that rotates and jumps up and down above the objects whenever the object is visible. There are 14 objects in total a two sets of six meant for the two main environments plus an object for control trials and another object for practicing and explaining the task. 

| Object name        | Object number | Meant for environment                                         | To be used for | Category            | Version |
|:------------------ | -------------:|:------------------------------------------------------------- |:-------------- |:------------------- |:------- |
| Barrel             | 1             | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Basketball         | 2             | Desert arena                                                  | Classical      |                     | v2.1.1  |
| Cake               | 3             | Desert arena                                                  | Classical      |                     | v2.1.1  |
| Traffic cone       | 4             | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Dice               | 5             | Desert arena                                                  | Classical      |                     | v2.1.1  |
| Donut              | 6             | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Drum               | 7             | Desert arena                                                  | Classical      | Musical instruments | v2.1.1  |
| Football           | 8             | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Gift               | 9             | Control trials                                                | Classical      |                     | v2.1.1  |
| Lamp               | 10            | Desert arena                                                  | Classical      |                     | v2.1.1  |
| Pawn               | 11            | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Pineapple          | 12            | Desert arena                                                  | Classical      | Fruits              | v2.1.1  |
| Vase               | 13            | Grassy Arena                                                  | Classical      |                     | v2.1.1  |
| Teddy bear         | 14            | Practice but also available in other environments (> v3.0.0). | Practice       |                     | v2.1.1  |
| Apple              | 15            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Banana             | 16            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Chimpanzee         | 17            |                                                               | Categorical    | Animals             | v3.0.0  |
| Clarinet           | 18            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Cutter knife       | 19            |                                                               | Categorical    | Tools               | v3.0.0  |
| Deer               | 20            |                                                               | Categorical    | Animals             | v3.0.0  |
| Dog                | 21            |                                                               | Categorical    | Animals             | v3.0.0  |
| Drill              | 22            |                                                               | Categorical    | Tools               | v3.0.0  |
| Elephant           | 23            |                                                               | Categorical    | Animals             | v3.0.0  |
| Guitar             | 24            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Hammer             | 25            |                                                               | Categorical    | Tools               | v3.0.0  |
| Harmonica          | 26            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Hippo              | 27            |                                                               | Categorical    | Animals             | v3.0.0  |
| Insulating tape    | 28            |                                                               | Categorical    | Tools               | v3.0.0  |
| Kangaroo           | 29            |                                                               | Categorical    | Animals             | v3.0.0  |
| Keyboard for music | 30            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Lemon              | 31            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Measurement tape   | 32            |                                                               | Categorical    | Tools               | v3.0.0  |
| Orange             | 33            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Pear               | 34            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Piano              | 35            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Pliers             | 36            |                                                               | Categorical    | Tools               | v3.0.0  |
| Polar bear         | 37            |                                                               | Categorical    | Animals             | v3.0.0  |
| Pomegranate        | 38            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Saw                | 39            |                                                               | Categorical    | Tools               | v3.0.0  |
| Saxophone          | 40            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Strawberry         | 41            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Tiger              | 42            |                                                               | Categorical    | Animals             | v3.0.0  |
| Trumpet            | 43            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Turnscrew          | 44            |                                                               | Categorical    | Tools               | v3.0.0  |
| Violin             | 45            |                                                               | Categorical    | Musical instruments | v3.0.0  |
| Watermelon         | 46            |                                                               | Categorical    | Fruits              | v3.0.0  |
| Wrench             | 47            |                                                               | Categorical    | Tools               | v3.0.0  |
| Zebra              | 48            |                                                               | Categorical    | Animals             | v3.0.0  |

All objects are available in all environments with the exception that the teddy is only available in the practice arena. You therefore do not have to stick to our object-environment assignment. 

Again: Note that these objects have not been created by us and do not fall under our licensing terms. Please check out the credits to see where they can be found.  HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEERRRRRRRRRRRRRRRRRRRRRRRREEEEEEEEEEEEEEEe

## Movement & keys

Because this task was primarily designed to be used inside an MRI scanner, movement might be relatively unintuitive. That is in order to move forward, the participants have to press the forward key. The forward movement then continued until the forward key is pressed again. Many video game applications require a continued button press for continued forward movement. Furthermore, before a new action can be started (e.g. turning to the left or to the right), the ongoing action has to be stopped. The rationale for this is that a) the use of MRI button boxes can not register prolonged button presses and b) we wanted movement only to be possible in straight lines. The speed of forward translation and rotation are set to a constant value that can be changed trial-by-trial. 

The default keys are: "W" for forward, "A" for left, "D" for right, "L" for confirm, "S" for start and "space" for skipping the experimenter messages. 

If the participants hits a wall and the collision is shaking the player character so that it might fall over, a script will righten the character immediately and log this with a timestamp. 

## How to configure the task?

The task was created with the explicit aim to make it as general and useful as possible to other researchers even with they have no prior experience with Unity3D. Because of this no Unity3D is even needed to use the task out of the box. In order to change parameters of the task and in order to tell the OLM task what trials it should present. You only need to edit/paste the corresponding input files into the _StreamingAssets_ folder of the build. Here is an overview of the files and what can/has to be configured in them. 

HEEEEEEEEEEEEEEEEERRRRRRRRRRRRRRRRRRRRRRRRREEEEEEEEEEEEEE you find an example configuration of the task. 

### The `welcome.json` file

The three different environments are accessed via the welcome UI. The welcome UI is configured with the `welcome.json` file. In this file you can specify:

- _button1Label_, _button2Label_, _button3Label_  = Strings for the the labels of the buttons that load the grassy, practice and desert arena respectively. Even if you do not show one of the buttons the numbering is still fixed (i.e. you need to use _button3Label_ for the desert arena).  
- _button1Show_, _button1Show_, _button1Show_  = Boolean that controls whether or not the buttons will be available to the participants. This can be used in order to prevent participants opening the wrong arena. 
- _title_ = String with which you can change the title. The default would be "Object Location Memory Task".
- _billboardText_  = String which controls what instructions are displayed on there. 

Next you have to provide the corresponding input files for the experiment itself.  These are specific to the environments. In principle, three files are needed for each environment: a .csv file that controls trial-by-trial behaviour, a .json file starting with OLM_ plus the name of the arena (i.e. grassy, practice, desert) and a .json file that controls the UXF start up menu UI. Further suffixes can be added making it possible to choose from several.json files that for instance follow the pattern OLM_grassy\*.json but caution should be exercised here if more than one file following the same pattern is placed into the _StreamingAssets_ folder.

### The trial .csv file

A valid .csv files needs to contain the following columns but extra columns can be added to make analysis easier:

- *trial\_num* = Number of the trial (special UXF name check their documentation). This will determine the order in which the trials are presented to the participant. 
- *block\_num* = Number of the block (special UXF name check their documentation).
- _targets_ = The target given as the object number from 1 - 13 (see above how these numbers correspond to the object names). Object 14 is only available in the practice arena. 
- *start\_x* = Start location of the player where they are teleported to. In continuous mode, only relevant on trial 1. 
- *start\_z* = Start location of the player where they are teleported to. In continuous mode, only relevant on trial 1. 
- *start\_yRotation* = Start rotation/heading angle of the player in degrees (0 - 360). In continuous mode, only relevant on trial 1.
- *object\_x* = Object location of the target. 
- *object\_z* = Object location of the target. 
- _cue_ = Period the cue is presented in seconds.
- _delay_ = Delay period after the cue was presented in seconds. 
- _ITI_ = inter-trial-interval (ITI) in seconds. 
- _trialType_ = Trial type i.e. "encoding", "retrieval" or "control". The spelling and capitalisation is important.  
- _speedForward_ = Forward speed in vm/s. 
- _rotationSpeed_ = Rotation speed in degrees/s.
- _messageToDisplay_ = Integer indicating whether a messages should be displayed after the trial (yes if >= 0, no if -1). Numbers above -1 are used as the index of the message from the list specified by the corresponding .json file (see below). 

### The main .json file

In the main .json file several things can and have to be configured in order for the task to run. Here is a complete list:

- _targetFrameRate_ = An integer that can cap the frames per second (FPS) at which the task is presented. If no cap is wished, simply choose a very high value (e.g. 9999).
- *trial\_specification\_name* = The file name of the .csv file (see above). This is a UXF field that is needed. 
- _continuousMode_ = Specification whether or not to use continuous (see below). Possible values: false/true. 
- _soundMode_ = This controls the sound in the experiment. Possible values: 1 = all sounds (collection & message sound), 2 = only plays sound when messages are displayed and 3 = no sound. soundMode = 2 can for instance be useful if MRI-operators wish to be notified when run/block is over.  
- _warningCriterium_ = The minimum distance a participant should move before pressing the confirmation button. 
- _warningMessage_ = The warning message that will be displayed if the participant moves less than the criterium before pressing the confirmation button. This is relative to the starting position not absolute moved distance.
- _waitForExperimenter_ = The pause message that is presented before the actual messages. This is mainly meant for fMRI data collection where the MRI operator wants to reset the recording. This message is skipped by pressing space bar. 
- _blockMessage_ = A list of strings for each message that the experimenter wants to be displayed. The correct message is chosen by the _messageToDisplay_ variable from the .csv file, which serves as an index starting with 0. 
- _objectNames_ = A list of strings to rename the objects. This needs to be specified but only has consequences for the results. Default: ["barrel", "basketball", "cake", "cone", "dice", "donut", "drum", "football", "gift", "lamp", "pawn", "pineapple", "vase"]. Also note that, for practice another entry has to be added (e.g. "teddy").
- _useHTTPPost_ = Specification whether or not data should be send a server using HTTPPost see ([here](https://github.com/immersivecognition/unity-experiment-framework/wiki/HTTP-POST-setup)). Possible values: false/true. If false, then no countdown message is shown because it is not needed.
- _endCountDown_ = Integer of how many seconds at the end of the experiment should be waited to allow data to be send to the server. This was added because currently there is no way to know when the web request is completed. We typically set this to 60 seconds just to make sure. 
- _endMessage_ = String for message to be displayed at the end of the experiment as part of the countdown from _endCountDown_ to zero. After that, the task closes automatically. 
- _url_ = String for the url of the HTTPPost server.
- _username_ = String for the username for the HTTPPost server.
- _password_ = String for the password for the HTTPPost server. __BECAREFUL not share a sensitive password with participants as they can also just see the password in the folder.__ 
- _changeKeys_ = Specification whether you want to change the default keys from W, A, D plus L to something else. Possible values: false/true. 
- _keys_ = A list of strings for the four keys necessary for the task. Please check [here](https://docs.unity3d.com/ScriptReference/KeyCode.html) to get the correct names. Please do __not__ use "S" or "space bar" as they are reserved keys.
- _useResponsePixx_ = Specification whether you want to use a responsePixx button box. This needs to be configured separately (see below). 

### The start up menu .json file

The .json files needed to control the start menus are called: `startupText_grassy.json`, `startupText_practice.json` and `startupText_desert.json`.

<p align="center">
        <img src="nonUnity_folder/images/example_menu.PNG" width="960" height="540">
</p>

The here is an image and the corresponding file as there is not much else to explain:

```
{
    "chromeBar": "Startup",
    "instructionsPanelContent1": "Welcome to OLM task! ",
    "instructionsPanelContent2": "You could use this space to display some instructions to the researcher or the participant.",
    "expSettingsprofile": "Experiment settings profile",
    "localPathElement": "Local data save directory",
    "localPathElement_placeHolder": "Press browse button to select...",
    "participantID": "Participant ID",
    "participantID_placeholder": "Enter text...",
    "sessionNumber": "Session number",
    "termsAndConditions": "Please tick if you understand the instructions and agree for your data to be collected and used for research purposes.<color=red>*</color>",
    "beginButton": "Begin session."
}
```

### Extra: How to configure the responsePIXX button box interface?

The responsePixx button box is configured via `responsePixx.json`. You have to configure the following values:

- _yellowCode_ = Button code.
- _redCode_ = Button code
- _blueCode_ = Button code.
- _greenCode_ = Button code. 
- _deviceType_ = Possible values: 1 = DATAPixx, 2 = DATAPixx2, 3 = DATAPixx3, 4 = VIEWPixx, 5 = PROPixx Controller, 6 = PROPixx, 7 = TRACKPixx.
- _dinBuffAddr_ = Address of the buffer. Default is 12000000. 
- _dinBuffSize_ = Size of the buffer. Default is 1000.

For more documentation please check with the company itself. The responsePixx integration is very basic but it works for us. 

### Continuous vs. standard mode

Mainly to create video versions of this task, a continuous mode is available. In this mode, participants are not teleported at the end but the next trial starts immediately without any interruption (hence the name). This also means that there are no cue, delay and ITI periods apart for the first trial. As a consequence, starting locations and cue/delay values only need to be specified for the first trial. For subsequent trials, the can be left as non-specified. 

Since there is no cue period showing what object is the target, the mode is not suited for retrieval. 

## What platforms/hardware/language does the OLM task support?

The OLM task was mainly developed for Windows but it can also be build for macOS. Though there are some issue and it is difficult to one build version for macOS that works for all different version of this operating system. The macOS build should therefore be regard as __highly experimental__!. A WebGL version is planned and should be feasible if certain changes to the code are made (especially with regard to how the .json and .csv files are accessed by Unity3D). 

The standard input device is the keyboard. So any button box that translate button presses into key presses should work. As noted above, responsePixx from [vpixx.com](vpixx.com) is also integrated in this task. 

The task has been professionally optimised to run even on slightly weaker hardware but this should be tested before running the experiment. When run on laptops, it is probably sensible to have it plugged in because most systems slow down drastically. Furthermore, changes to the _Graphic settings_ of windows can help as well. Here _high performance_ mode is recommended. 

The task was created to work with Chinese and English text. Other languages with different character sets should also work but this was not tested. 

## Special comments on using the task for fMRI research.

This task was primarily created to investigate grid-like activation patterns during virtual navigation using fMRI. In order to create fMRI runs in the cleanest way, it is important to use UXF's "block_num" and to display a message at the end of the last trial of the preceding run. This is because before the actual message is displayed. A standard pause message is shown, which gives the MRI operator enough time to handle the MRI data collection (e.g. manually stopping the run). An experimenter then has to press "space" to set the task into a state where it waits for the first S to be received by the data collection computer. This will reset the timer for the run itself. Note if you want to discard the first volumes you should configure with the scanner so that no S is send at that time. However, future version of this task could make this configurable as well. If that is something that might be of interest to you, submit a _Feature Request_ to this repository. For the first block, the task also only starts as soon as the first S is send by the scanner.  

Apart from this, what is saved from this task is also optimised run fMRI-based spatial memory/navigation studies. 

## What is saved in this task?

The data is saved by UXF so more information on this can be found in their documentation. Only the basics and the unique aspects of this task are covered.

### Trial results

The main results including the behavioural performance be found in `trial_results.csv` file for each participant. Also note that any column that is included in the .csv input file is also copied to the trial results. The following columns are saved:

- _ppid_ = The participant ID. 
- _end_x_ = End position at the end of the trial or when pressing the confirmation button.
- _end_z_ = End position at the end of the trial or when pressing the confirmation button.
- _euclideanDistance_ = This is the main performance measure: The distance between the end position and the target/object position in vm. 
- _objectName_ = Name of the object as specified in the .json file. 
- _runStartTime_ = Time point in seconds when the block/run started. This is when the first S of the block/run is pressed/arrives.
- _objectNumber_ = Number of the object.
- _navStartTime_ = Time point in seconds at which participant is able move after cue and delays and starts navigating. 
- _navEndTime_ = End of the trial in seconds. This is a duplicate of _end_time_.  
- _navTime_ = The total time between the start of navigation and the end of the trial in seconds. 
- _timesObjectPresented_ = The number of times an object was the target. All trial types count. 
- _confirmButtonTime_ = Time point when the confirm button is pressed. 
- _movedDistance_ = Distance between start and end position in vm. 
- _warningShown_ = Was a warning shown that the participant did not move enough?
- _player_movement_location_0_ = Path to the tracking file of this trial. 

### Tracking of position & rotation

For each trial, the position and rotation of the participant is tracked for each frame. For this UXF's standard position/rotation has been adapted to add a boolean whether there forward translation. The files all follow the same name convention (e.g. `player_movement_T001.csv`) and are saved for each trial separately. The columns of the tracking files are:

- _time_ = Time of the frame in seconds. 
- _pos_x_, _pos_y_ & _pos_z_ = Position at that frame. 
- _rot_x_, _rot_y_ & _rot_z_ = Rotation at the frame. x & z should mostly be unchanged unless the participants runs into the wall. 
- _moving_ = A boolean whether the participant is moving forward. 

This file can used to calculate the FPS for each trial because the position is recorded on reach trial. 

### Log

Everything else is saved in the `log.csv` file. Some of the things saved in the log are redundant but it makes it easier to search for something. Here is an incomplete list of things saved in the log:

- Session start time as date: E.g. "Session start time 9/2/2022 10:23:31 AM"
- Screen resolution: E.g. "1920 x 1080 @ 60Hz"
- Platform used: E.g. "Platform used is UNITY_STANDALONE_WIN"
- Trigger time: E.g. "A trigger was send 9/2/2022 10:24:15 AM Run time: 52.66564"
- Cue start: E.g. "Cue start of trial 1"
- Cue end: E.g. "Cue end of trial 1"
- Delay start: E.g. "Start of delay period of trial 1"
- Delay end: E.g. "End of delay period of trial 1"
- ITI start: E.g. "Start of ITI period of trial 1"
- ITI end: E.g. "End of ITI period of trial 1"
- Wait for experimenter: E.g. "Waiting for experimenter to press space."
- Experimenter pressed space: E.g. "Experimenter pressed space bar."
- The participant hit the wall and had to be rightened: E.g. "Attempting to righten the player."
- Movement: "forwardKey was pressed.", "leftTurn was pressed." and "rightTurn was pressed."

## How to analyse the data?

TBA

## How to cite this work?

TBA

# License

The OLM task is licensed under the GPL-3.0 license.

__Important notice:__ The repository contains various free assets that are not 
part of this license. Please contact the copyright holders to make sure you have
permission if you want to re-use them. A list of these resources can be found __HERE__. 