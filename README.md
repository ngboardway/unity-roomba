## Description
The Unity Roomba is a 3D simluation of a Roomba moving within a space. There are various obstacles such as furniture and rugs which cause the robot to turn in search of a viable path. There are also walls which keep the robot within a pre-defined area (Room).

Throughout the space there are a series of "dirt" particles which can be collected. Both the movement of the robot and its collection of these particles consume battery life, which is displayed throughout the simulation.

Once the robot has run out of viable moves forward, it displays statistics about the current simulation such as how much of the potential grid was explored (the potential is based on all locations which contain a tile or dirt, as furniture/ walls cannot be traversed).

Future iterations will examine the effectiveness of various pathing algorithms in different kinds of spaces. 


## Running the Roomba

If you have never downloaded Unity before start by downloading Unity Hub from [the website](https://unity.com/download) and running through the installer. Follow the instructions to sign in/ activate a license.

Next follow the instructions to install the 2019 LTS release. Future iterations of the project may involve upgrading, but we will keep to this LTS for now. This is notoriously a very finicky process and may take a few tries. At the minimum you will need to install build support for whichever OS you are running (mac vs. windows) and an editor. The installation steps will suggest one, but you can use whatever text editor you would like for writing scripts. 

Once Unity has been installed, make sure the repository has been cloned locally and is accessible. 

Go the project menu and click 'Add'. Navigate to where you cloned the repository and select folder. You may be prompted to update the project if you are using a newer version of Unity than how the project/ assets was built last. Click confirm to allow it to update what it needs.

Once you have opened the project, open the RoombaScene. 

If you have an issue with the reference to TextMeshPro, you may need to close all editors and remove/ re-install the package.