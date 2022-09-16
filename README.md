<p align="center">
		<img src="Assets/otherImages/logo.png" width="100" height="100">
</p>

# What is the Object Location Memory Task?
The object location memory (OLM) task was designed to investigate grid-like activity using fMRI and to assess spatial memory in humans inspired [Doeller et al. (2010)](https://pubmed.ncbi.nlm.nih.gov/20090680/). The task was primarily created to support our own research but is made available to the community at-large. Its design is general & configurabe without adapting the Unity3D code itself. The Unity3D code is shared under XXXX licence and can be used to make changes. However, several free assets and third-party resources are used in this task, which are excluded from this licence. A list of these resources can be found __here__. 

UXF

# Documentation
The idea of the task is participants who complete learn where objects are located within the environments during _encoding_ trials, performance can then be assessed during _retrieval_ trials. As an additional condition, we have added _control_ trials, where all spatial cues but the ground and the walls have been removed in order to provide an appropriate condition that can be used as a control that features tranlation without spatial cues necessary for navigation (i.e. landmarks). 

Each trial (in standard mode) with a cue period during which the target object is presented as an image on the screen with semi-transparent grey panel. This is followed by a delay period where the image of the target is replaced with a fixation marker. After this the participant can move and complete the task. This is followed by ITI period. The duration of the three periods (cue, delay & ITI) can be pre-configured for each trial. All trials end with the participant "collecting" the target object by running over/into it. 

## Trial types
### Encoding
The idea behind the _encoding_ trial is that the participant has to learn the object locations. The object is therefore present from the beginning and just has to be collected. 

### Control
_Control_ trials are identical to _encoding_ trials apart from the fact that all background objects (e.g. landmarks) and other spatial cues are removed to serve as a control condition that has translation but no map-based navigation.  

### Retrieval
In contrast to the two other trial types, the object is not visible from the beginning for _retrieval_ trials. Instead, participants are supposed to navigate to the location where they think the target object is hidden and press the confirmation key. 

## The environments
The OLM task comprises two main environments that are meant for experiments plus an additional simple enviroment that is meant to teach participants how this task works. The main enviroments have been optimised to run even on slightly weaker hardware.

IMAGE OF THE THREE ENVIRONMENTS

|             | Grassy Arena | Practice Arena | Desert Arena |
| ----------- | ----------- | ----------- | ----------- |
| Shape | Circle | Hexagon | Square |
| Dimensions | 180 vm diameter | 50 vm side length| 190 vm side length |

## The objects available
Objects were chosen based on being relatively symmetric along their vertical axes to avoid view-dependent confounds. Because at far distance, objects are still difficult to see, we added a large glowing-red arrow that rotates and jumps up and down above the objects whenever the object is visible. There are 14 objects in total a two sets of six meant for the two mean environments plus an object for control trials and an object for practice and to explain the task. 

|Object name | Object number | Meant for |
| ----------- | ----------- | ----------- |
| Drum | 7 | Desert arena |
| Basketball | 2 | Desert arena  |
| Pineapple | 12| Desert arena  | 
| Dice | 5 | Desert arena  |
| Cake | 3 | Desert arena |
| Lamp | 10 | Desert arena  |
| Barrel | 1 | Grassy Arena |
| Football | 8 | Grassy Arena |
| Pawn | 11 | Grassy Arena |
| Traffic cone | 4 | Grassy Arena |
| Donut | 6  | Grassy Arena |
| Vase | 13 | Grassy Arena |
| Gift | 9 | Control trials |
| Teddy bear | 14 | Only available in the practice arena|

All objects are available in all environments with the exception that the teddy is only available in the practice arena. You therefore do not have to stick to our object-enviroment assignment. 

Note these object have not been created by us and do not fall under our licencing terms. Please check out the credits to see where they can be found. 

## Movement
Because this task was primarily designed to be used inside an MRI scanner, movement might be relativel unintuitive. That is in order to move forward, the participants have to press the forward key. The forward movement then continued until the forward key was pressed again. Many video game applications require a continued button press for continued forward movement. Furthermore, before a new action can be started (e.g. turning to the left or to the right), the ongoing action has to be stopped. The rationale for this is that a) the use of MRI button boxes can not register prolonged button presses and b) we wanted movement to be only possible in straight lines. The speed of forward translation and rotation are set to a constant value. 

## How to configure the task?
The task was created with the explicit aim to make it as general and useful as possbile to other researchers even with they have no prior experiments with Unity3D. To use the task out of the box, no Unity3D is even needed. In order to change parameters of the task and in order to tell the OLM task what trials to present. You need to edit/paste the corresponding files into the _StreamingAssets_ folder of the build. Here is an overview of the files and what can/has to be configured in them. 

### Naturalistic vs. standard mode

## What hardware does the OLM task support?

## What platform does the OLM task support? 

## What is saved in this task?

## How to analyse the data?

## How to cite this work?

# Licence 



__Important notice:__ The repository contains various free assets that are not 
part of this licence. Please contact the copyright holders to make sure you have
permission if you want to re-use them. A list of these resources can be found __HERE__. 