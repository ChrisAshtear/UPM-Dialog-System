#Dialog & Cutscene system

##Cutscenes
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
 
 ###Cutscene Event codes
 
The following codes are supported:

- runDialog : starts a dialog system event. The event parameter is the dialogID to run.
- changeBG : change the background that covers gameplay. The parameter is the name of the background image in Resources/*bgFolder*
- playSound : play a sound. The parameter is the name of the sound file in Resources/SFX
- changeMusic : change the music being played. The paramater is the name of the music file in Resources/Music. The music will continue until changed again.
- changeLevel : change the Tile level. This does not work correctly right now. The parameter is the name of the TMX file in Resources/