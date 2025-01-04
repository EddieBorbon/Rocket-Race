# 🚀 Rocket Race: A Musical Learning Game 🎵

**Rocket Race** is an interactive music game built with **Unity 6** where players compete in teams to correctly identify musical chords or instruments based on audio clues. The game helps players learn music by engaging them in quick decision-making while enhancing their musical ear.

## Features ✨
- **Modes**: Two modes are available:
  - **Chords** 🎶: Players must identify whether a chord is Major or Minor.
  - **Instruments** 🎸: Players must identify the correct musical instrument based on the sound played.
  
- **Team Play** 🤝: Players are grouped into teams. Each team competes to get the most correct answers.
  
- **Rounds & Timer ⏲️**: The game can be customized with the number of rounds and a countdown timer for each round.

- **Feedback ✅❌**: After each answer, players receive immediate feedback (Correct/Incorrect) and see the updated score.

- **Game Over 🎮**: The game ends after the specified number of rounds, with the winning team(s) displayed.

## Setup & Installation ⚙️

To run the game locally, follow these steps:

1. **Clone this repository**:
   - Go to your terminal and run: 
     `git clone https://github.com/your-username/rocket-race.git`

2. **Open the project in Unity**:
   - Open Unity Hub and click "Open", then select the folder where you cloned the repository.

3. **Build & Run**:
   - Once the project is opened, build and run the game by going to `File > Build Settings` and clicking `Build`.

## Play the Game 🎮
You can play the game directly here:  
[Play Rocket Race on Unity](https://play.unity.com/en/games/09fd063e-56a1-44a2-a653-6fa6864b28a4/rocket-race)  
Or on this link:  
[Play Rocket Race](https://eddieborbon.github.io/25-5-clock/)  

## Configuration ⚡

### Team Management 👥
- The game allows you to create and manage teams. Teams are selected from the dropdown in the settings panel.

### Mode Selection 🎮
- Players can choose between two modes in the settings:
  - **Chords Mode** 🎶: Identify whether the played chord is Major or Minor.
  - **Instruments Mode** 🎸: Identify the instrument based on the sound.

### Round Settings 🔄
- The number of rounds and round timer can be customized in the settings.

## Controls 🎛️

- **Play Sound** 🔊: Play the sound associated with the selected mode.
- **Answer Buttons** 🅾️❌: Select the correct answer from the displayed options.
- **Next Team** ⏩: After a round, move to the next team.
- **Restart Game** 🔁: Restart the game once it ends.

## Dependencies 📦
- **Unity 6**
- **TextMeshPro**: For displaying UI elements.
- **AudioManager**: For handling sound playback.

## Game Flow 🔄

1. **Settings Panel ⚙️**: Configure teams, round timer, and game mode.
2. **Play Button ▶️**: Start the game once the settings are configured.
3. **Gameplay 🎮**: Teams take turns identifying chords or instruments. Correct answers add to the team score.
4. **Game Over 🏁**: After all rounds are completed, the team(s) with the highest score win.

## Known Issues ⚠️
- Ensure that all buttons in the inspector are properly assigned.
- AudioManager, UIManager, and TeamInstanceManager must be correctly referenced in the script for proper game flow.

## License 📜
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
