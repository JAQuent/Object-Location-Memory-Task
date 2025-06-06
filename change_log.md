- Changes for version 0.2
    - Removed the billboard.
- Changes for version 0.3
    - Used marker of the middle screen for both delay plus navigation but the same to reduce change. Make transparent.
    - Replaced the cue words with images and only disabled the panel after the delay period is over.
    - Speed and durations now have to be provided in the input .csv.
    - Added the functionality to have control trials: To have control trial simply set trialType to control and object 9.
    - Add message after Block 1 and before control trial block
    - Removed progress in terms of trials and blocks
    - Remove things that are very close
    - Found symmetric objects replaced asymmetric objects so that all objects are more or less symmetric. Definition: not clear-cut mirror symmetry but whether people would clearly remember the orientation of an object like it would be with objects that have face & backside (e.g. microwave).
    - Made Skybox & background disappear on control trials
- **Elevated to Version 1.0 of development.**
    - Unity changes:
        - Now a experimenter-closed screen precedes the message screens, which is ended if space is pressed.
        - Also, movement now is not possible during ITI.
        - Sun in desert environment was made less bright (intensity from 4 to 2.5).
        - Practice objects were made slightly larger. Times 1.1 on scale.
        - KEYS now are 1, 2, 3, 4 plus S & Space.
    - Non-Unity based changes to the paradigm:
        - If using equally spaced steps that can divide the number 30, then there is always overlap with correct angle that why I will for now draw the initial heading angle from uniform distribution from 0 to 360.
        - 5 control trials were added to Practice II.
        - Change the trial structure to: encoding (30), retrieval (15), control (15), retrieval (15), control (15), retrieval (15), control (15), retrieval (15), control (15), “encoding” (30). This was done so that we can examine whether the memory task is crucial for the grid-signal to arise but at the same time we didn't want to overtrain the subjects.
        - To interleave with run2, the same halfs of the control trials were used but with a different shuffling.
        - Offset locations didn’t seem to be used in previous version.
- Version 1.1
    - Changed label from fMRI to main task.
    - Opened with Unity 2021.3.1f1c1
    - Created an extra sun that is rotated to remove extra spatial cues.
    - Move the house that is too close to the arena.
    - Now, every S is saved the log file but only the run start time is saved to main result.
    - Saving extra time points to allow how long each trial component is.
    - Saving screen resolution in log file.
    - Saving data & time.
    - For behavioural version use HJKL keys.
    - Thought about adding the run tracker time to the position tracker but decided not to in order to save performance cost.
    - Improve instructions.
- Version 1.2
    - Fixed small bug that after trial there was a tiny forward jerk, which was because the movement was only reset after movement was evaluated & only if movement was allowed. I moved this outside the if statement.
    - Added more comments and move long bits of code into separate functions to enhance the readability of the code
    - Remove the condition that retrieval is only possible for block 2 onwards. Here I replace every condition tied to block num and replaced with encoding trial type.
    - Changed the way the next trial is started by first checking if there is a new trial.
    - Added an endscreen of x seconds during which the task can not be closed so that the data can be uploaded.
- Version 1.3
    - Added which platform is used.
    - Added a canvas scaler.
    - Add HTTPost configuration
    - Make sure that the TheEnd() method does not produce error but closes sessions and trials correctly.
    - Add the possibility to change the key assignments.
- Version 1.4
    - Fixed UI start up issue.
- Version 1.5
    - Fixed wrong camera height
    - Change shape of objects to avoid falling over.
    - Had to change Physics > Default Max Depenetration Velocity to 20.
    - Fixed that main task did have hardcoded .json.
    - Disabled practice button for this version.
- Version 1.6
    - Added a new continuous mode.
    - Lowered graphic settings.
    - Add feedback relative to start location also save that. This is presented as a warning with a corresponding icon. Euclidean distance from start to end as well as if warning is shown is saved now.
- Version 1.7
    - Added reponsePixx functionality
    - I removed unnecessary stuff for instance extra settings for English and Chinese so that to switch between languages you just edit the .json files.
- Version 1.8
    - For fMRI testing add sound to notify the end of the run/block. Also add possibility to deactivate sound. Add different sound modes. 1 = all (object & message sound 2 = message only 3 = none.
    - Fix responsePIXX issue: While during simulation it worked flawlessly, with the responsepixx device in the lab no button press was registered. To fix this, Reset address to base address will only be done if a button was pressed.
    - Fixed welcome screen so it can handle a larger variety of aspect ratios.
    - Make messages larger so they can be read in the MRI scanner.
- Version 1.9
    - Make practice useable & add to UI.
    - Rename to grassy, practice & desert arena. Changed this for the input files as well and added feature that names only need to start on OLM_grassy for instance.
    - Log which version of the programme is used.
    - Add FPS_counter as a regular feature.
    - Changed grassy’s skybox to a skybox from the Free HDR Sky Version 1.0 - May 06, 2016 asset.
    - Changed to Bokeh Depth of Field which is a bit slower
    - Fixed that button cannot be pressed to early anymore (during cue & delay).
    - Changed some material and changed from linear to gamma colour space.
    - Now destroy object instead of just setting it inactive.
- Version 2.0
    - Changed to medium quality settings but added 4x Anti Aliasing in
    - Baked the reflection probe for grassy, practice & desert
    - Disabled wind for grassy scene.
    - Set sun to static and hard shadows.
    - Set UXF UI Canvas scaler to shrink.
    - Changed welcome screen instructions from billboard to normal canvas image.
    - Since reflection probes are baked now, they have to be disabled.
    - Change the Max size of texture all to 512 In the file.
    - Delete some unnecessary files
    - Fixed the donut (actually)
    - Removed culling LOD level for the trees.
- Version 2.1
    - Deactivated movement at first again.
    - Added functionality that pressing R rotates the subject by 180 degrees. Which is only available when the session started.
    - Add AudioTrialCounter that plays how many trials are left in the current block as a sequence of tones. This is activated by pressing backspace.
- Version 2.1.1
    - Fix that control suns had soft shadows. Now, they have hard shadow like the main suns. Also set the control sun to static and to 89 x-rotation because this is cause the flickering
    - Fixed issues with the responsePixx interface:
    - That confirm button pressed carried over.
    - That you could abort movement by pressing another button instead of ending it.
    - Make the responsePixx system spit out unknown key codes that can be used for debugging.
- Version 3.0.0
    - Changed normal bias of suns to 0 to remove the holes.
    - Added additional collider triggers to check if participants leave the arena. If that happens they are teleported back into the correct area based on the current 2D vector towards the centre. This is implemented via *measuresAgainstHittingWall.cs* and *outerTriggerController.cs.* Terrain colliders were also removed.
    - Renamed object folder for classical olm to make everything tidier.
    - Added custom rotation mode that can be activated by adding objectRotationSpeed to the input .json
    - Added the following new objects:
    - Animals:
    - chimpanzee, deer, dog, elephant, hippo, kangaroo, polar bear, tiger, zebra
    - Tools:
    - drill, hammer, isolation tape, cutter knife, measurement tape, pliers, saw, turnscrew, wrench
    - Music instruments:
    - clarinet, guitar, harmonica, keyboard, piano, saxophone, trumpet, violin
    - Fruits:
    - apple, banana, clementine, lemon, pear, pomegranate, strawberry, watermelon
    - Reused from classical OLM: drum & pineapple.
    - Added the possibility to give feedback. If showFeedback is set to true in .json and will look for feedback criteria in .csv: feedback_critVal1 & feedback_critVal2.
    - Added version to welcome screen.
    - Added a FPS check that can be aborted by pressing F1. The messages and the FPS criterium is customisable and load via normal text files. If these are not provided, defaults are chosen to make it fully backward compatible. The measured FPS is saved to the log at the beginning of the session.
    - Added the possibility to have a constant cue image of the current object at the bottom of the screen.
    - Made small changes to the metallicness of the drum to make it less shiny.
    - Considered whether to change the original pineapple and drum or add new version for this but decided against it because all future data collection will be new projects.
    - Checked if input files for 2.1.1 are still compatible. So far it seems that is the case.
    - Added the possibility to shuffle trials in certain blocks by providing the block numbers of those blocks *shuffleBlocks* in the .json file.
    - Added the possibility to change the movement from an actions have to be stopped and an actions don’t have to be stopped for moving around. In .json it is changed by *actionNeedToBeEnded*.
    - Added a new way to present so called block messages. One way is to set a column in the input .csv called *messageToDisplay*, which can actually present block messages even within a trial however when randomising the trial order this doesn’t work any more. Now, it can be set that the last trial of block displays the message that is provided as *blockMessages* in the .json. If *lastTrial_inBlockMessage* is true in the .json file, *messageToDisplay* is ignored.
- Version 3.0.1
    - Fixed bug: that when *actionNeedToBeEnded* is enabled that the Boolean whether the participant is moving or not was not working.
- Version 4.0.0
    - Attempting to allow WebGL builds of the task, which includes major overhauls of a lot of things. 
        - Get .json from the internet & load .csv via WebRequest: .json for UXF need to be downloaded instead of acquired from UXF. Made custom changes to UXF CSVExperimentBuilder with conditional compiling so that it downloads .csv via WebRequest when built for WebGL.
        - Add conditional compiling for FPS counter: When using WebGL, files are downloaded via WebRequest. 
        - Add WebGL to whichPlatform() method.
        - Add studyID logic for Welcome Script for WebGL: When the task is built for WebGL, the studyID is provided by the participants via a text field will a) look up the studyID in online .json file. The link for that is found in the streamingAssets folder in the scene. This .json can be remotely edit to add more studyIDs. Based on the studyID two important things are set 1) it is the scene that need to be loaded (desert, grassy or practice) and 2) the UXF .json. For now, I decided to hard code instructions for submitting the study ID to avoid having to complicated .json files. 
        - Updating UXF_UI_controllScript: 1) add support for WebGL experiments. 2) fix bug that the script still tries to change a component even if it is not enabled. Now a component is skipped when the field is set to "" in the .json. 3) added new field to study dict to also allow download of UI .jsons. 4) fixed bug that UXF script doesn't work if [UXF_Rig] is set inactive by the FPS counter. This was done by changing the sort order and remove the corresponding line in the FPS script. 5) the script also handles other necessary changes of the UI for local vs. web experiments (mainly setting jsonURL). 6) study ID and UXF_settings_url are log to the session.
        - Make it configurable if the FPS test is run: When running via WebGL, the study dictionary contains a boolean for the FPS test. For local experiments, this information is provided via the welcome.json. As another change, the FPS counter is set to in-active when the scene is opened.
        - Added a progress bar that can be toggled on and off via the UXF settings .json: This progress bar is relative to the total number of trials. 
        - Made sure that the other scenes also work: 1) fixed that ResponsePixxInterface.cs was active bey default in the desert scene. 2) set up the UXF UI controll script correctly. 3) Change Session always 1 for each scene. 
        - Fixed that the WebGL can be started before it is ready. Now no S is necessary. 
        - Small change how the application closes.
        - Fixed problems with the practice scene: changed the player to the prefab again. 
        - Fixed that encoding and control trials saved distance and end position.
        - Improved a confusing comment. 
        - Removed unnecessary name spacces from scripts. 
        - Optimise for WebGL and general performance 1: Following this tutorial https://www.youtube.com/watch?v=j0DN9P8e7dc. Mostly only madee the max size of the textures smaller and change some input stuff for models. 
        - Fix that log message in welcome script & setting name for experiment
        - Fix basketball rotating around wrong axis
        - Fixed error that line break characters messed with reading .csv (& other changes): now "\r" is replaced by an empty string. Also added that the cursor is visible again at the end. Additionally, I also changed made it so that the rotation speed variable is used again instead a hard coded value.
        - Automatically set full screen to true in WebGL and check this at each start of a trial.
        - Fix remaining download error message mentioning CSV.
        - Fix that control trials always used wrong skybox but also made the code simpler by removing boolean displaying background.
- Version 4.0.1
    - Fixing problem around the FPS counter: a) making sure that the FPS counter is started as soon as the scene opens. b) draw the FPS screen over the UXF UI.
- Internal changes:
    - Added a script AngleCalculator.cs: The script calculates the angle between two objects relative to the player. It is meant to check the viewing angle based on two objects estimating the angle between them. I used "cubes" with 0.1 x 1 x 0.1 dimensions and estimated the viewing angle to be 90 degrees with a setting of field of view 60 and 16:9 aspect ratio.
    - Added create_Screenshots.cs & prefar: This script takes camera positions and object positions to create screenshots while the camera moves through the environment. 
- Version 4.0.2
    - Fix bug that startupText is not loaded in Windows.
    - Deleted unnecessary files in StreamingAssets folder.
- Version 4.0.3
    - Fix bug that only the first block was actually shuffled. 