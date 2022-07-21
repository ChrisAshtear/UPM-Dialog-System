# UPM-Dialog-System
#### Dialog & Cutscene system

## Cutscenes
cutscene is used by putting a cutscene system component in the UI. You point it to an xml file configured as such:

	<scenesFile name="" description="" author="" webglpath="" bgFolder="BG">
	<sceneList>
	<scene id="cs1">
 	<sceneEvent id="" eventCode="runDialog" eventParameters="artifact1"/>
 	<sceneEvent id="" eventCode="runDialog" eventParameters="death"/>
 	<sceneEvent id="" eventCode="changeBG" eventParameters="backgroundForest"/>
 	<sceneEvent id="" eventCode="playSound" eventParameters="MMXitemGet"/>
 	<sceneEvent id="" eventCode="changeMusic" eventParameters="poppy"/>
 	<sceneEvent id="" eventCode="runDialog" eventParameters="mines2"/>
 	</scene>
 	</sceneList>
 	</scenesFile>
 
 ### Cutscene Event codes
 
The following codes are supported:

- runDialog : starts a dialog system event. The event parameter is the dialogID to run.
- changeBG : change the background that covers gameplay. The parameter is the name of the background image in Resources/*bgFolder*
- playSound : play a sound. The parameter is the name of the sound file in Resources/SFX
- changeMusic : change the music being played. The paramater is the name of the music file in Resources/Music. The music will continue until changed again.
- changeLevel : change the Tile level. This does not work correctly right now. The parameter is the name of the TMX file in Resources/

## Dialog
cutscene is used by putting a dialog system component in the UI. You point it to an xml file configured as such:

    <DialogFile portraitFolder="portraits">
    <dialogList>
    <dialog id="end" text="... | Thanks for playing! |Credits: Code-Ryan, Concept/Art/Levels/Designs-Rhides, Music/SFX-Bongo." portrait="robin"></dialog>
    <dialog id="testroom1" text="Run over a tile to change levels. Mineshaftest on left, vcaves2 on right" portrait="robin"></dialog>
    <dialog id="artifactmines" text="We're onto something! |Head to the lower path, Rippy |&lt;rippy&gt;Roger that, Ranger Command!" portrait="robin"></dialog>
    <dialog id="artifact1" text="Nice find, Rippy! |&lt;tony&gt;Looks cool." portrait="robin"></dialog>
    </dialogList>
    </DialogFile>
 
### Dialog Event Fields

- id: is used to name the event so it can be started by a player colliding with an object with a dialog activator as a component. the id of that must match the desired dialog id.
- text: will display the written text. using a | will end the screen of text. To change portraits mid dialog, use ;portraitname; at the beginning of a new screen of dialog.
- portrait: this is the name of the image that will be shown in the portrait box during dialog. path is resources/*portraitFolder*/*portrait*.
