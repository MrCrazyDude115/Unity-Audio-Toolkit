# Unity Audio Toolkit
A lightweight and reusable audio toolkit for Unity.

# 1. Introduction
The Audio Toolkit is a simple audio system for Unity that lets developers set up and manage sounds without having to configure an AudioSource every time they want to play something. It uses SoundCue assets to store audio settings, such as volume, pitch, looping, 2D/3D audio, and mixer groups. These SoundCues can then be added to an AudioBank component and played using a unique identifier.

I originally made this toolkit for the GAD2010 Technical Design module at Teesside University, with the aim of making audio easier to set up for fellow students.

# 2. Features
- SoundCues: Store and reuse audio settings through ScriptableObject assets. Supports AudioClips and Unity's Audio Random Containers.
- AudioBanks: Add multiple sounds to a GameObject and play them using their identifiers.
- Different ways to play sounds: Trigger audio through C# scripts, UnityEvents in the Inspector, or the Play On Start option.
- 2D and 3D audio: Configure Spatial Blend, distance settings, and supported rolloff modes.
- Looping sounds: Start and stop continuous sounds.
- Automatic Cleanup: Temporary AudioSources are cleaned up when they are no longer needed.
- Audio Mixer Support: Includes a basic mixer with groups for Master, SFX, Music, Dialogue, and UI.

# 3. Installation
1) Open the Releases page and download the latest .unitypackage when available.
2) Open your Unity Project, Select Assets -> Import Package -> Custom Package, and choose the downloaded file.
3) Import the toolkit files into your project.

# 4. Quickstart
For a simple example, let's set up a sound that plays when a door opens.

1) Right-click in the Unity Project window and select Create -> Audio Toolkit -> Sound Cue.
2) Name the new SoundCue "DoorOpen" and assign a door-opening sound to its Audio Resource field.
3) Add an AudioBank component to your door GameObject.
4) Add an entry to the AudioBank. Set its Identifier to "Open" and assign it your DoorOpen SoundCue.
5) Trigger the sound through a UnityEvent by selecting AudioBank -> Play(string) and entering "Open", or call audioBank.Play("Open"); from your own C# script.

That's it! The AudioBank will create and configure a temporary child GameObject with an AudioSource when the sound is played, and delete it afterwards in an automated clean-up process.

For looping sounds, enable "Loop" on the SoundCue and use Stop("Identifier"); when you want the sound to stop.

# 5. Full Documentation
For a more detailed explanation of each setting and what you can do with SoundCues, AudioBanks, UnityEvents, and C# methods, looping sounds, and mixer setup, check the full Audio Toolkit Documentation in the files above.

# 6. Development Status
This is an early version of the toolkit made mainly for myself and fellow students taking the GAD2010 Technical Design module. It includes the main features needed to manage and play sounds in a Unity project, but I plan to add more functionality and improvements in the future.
