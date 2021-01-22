# experiment-09
Casual Game Developer Technical Test

Unit Configuration:
![config](/Images/config.PNG)

Start Scene:
![Start](/Images/start_game.gif)

End Menu:
![End](/Images/end_game.gif)

Timeline:
![timeline](/Images/timeline.PNG)

External Packages (unused assets were deleted):
* Kenney's free resources: https://www.kenney.nl/. Some assets I got them from the itchio's Bundle for Racial Justice and Equality.

Time spent: please consider chores and personal time taken during the test period.
* PART 1: ~ 4-5 hours.
* PART 2: ~ 2 days 

Considerations:
* To test: open unity project and go to Scenes/UnitsScene, open it and press play. 
* Mostly a MVC project: I like to create models and data to populate a factory logic that will create the views/instances on gameplay, then a controller (ScenarioController) is in charge of the gameplay conditions and scene set up.
* Camera and UI setup targeting a 1920 x 1080 screen size.
* I created the config in a way we can modify the rules for Part 1 and not have to heavily recode the data structures.
* I focused in the gameplay behaviour and the code structure: I know I could use more "juicy" assets and ui animations to make the mini game more appealing but I don't think this was the intention of the test.
* I left a debug message for unit creation so you can check the randomness.