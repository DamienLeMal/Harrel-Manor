# Harrel-Manor
Harrell's Manor is a Tactical RPG game (Turn based strategy) that is divided into several parts, exploration phases in which the player progresses in the manor, and fight phases in which they fight against the entities that live in the house.

[Video Trailer](https://youtu.be/LICrbLJ959A?si=lw-_kNNu1Wlx_DOS)<br/>
[Itch Page](https://dexeat.itch.io/harrells-manor)
## My Work - Damien Le Mâl

<details>
  <summary>
    
  ### Exploration Mode / Fight Mode
    
  </summary>

  <p float="left">
      <img src="/HarrelManorReadMe/chtulu1.PNG?raw=true" width="49%x" />
      <img src="/HarrelManorReadMe/chtulu2.PNG?raw=true" width="49%x" />
  </p>

  <img align="right" src="/HarrelManorReadMe/astar.png?raw=true" height="100" />
  <img align="right" src="/HarrelManorReadMe/mapUnity.png?raw=true" height="100" />
  <img align="right" src="/HarrelManorReadMe/unknown.png?raw=true" height="100" />
  The game is separated in two differents phases, on for exploration and one for combat. We had to convert the level design to a table of different numbers representing each room of the game with path, walls and obstacles. 
  This allowed tile based mechanics for combat while still being able to move the character freely in exploration mode.
  <br/>
  <br/>
  For instance, I could implement A* algorithm to move the player and the ennemies inside the grid and also use the coordinates to choose wich tile was attacked during the fight.
  <br/>
  <br/>
</details>
<details>
  <summary>

  ### Ennemy AI
    
  </summary>
  <table>
    <tr>
      <td>The enemies in this game can only do two things in combat: moving and attacking. The goal was not to make the fights really difficult, so I relied solely on an evaluation function to choose different tiles based 
        on various criteria depending on the enemy's current state (defensive, regular, or offensive).</td>
      <td><img src="/HarrelManorReadMe/AIChangeState.PNG?raw=true" /></td>
    </tr>
    <tr>
      <td>Every tile is scored with 3 parameters : 
        <ul>
        <li>Distance </li>
        <li>A defense score </li>
        <li>An attack score</li>
        </ul>
        Each parameter is then used to chose between all reachable tiles wich one are the most : <br/>
        🔴 aggressive tile <br/>
        🟡 safest tile <br/>
        🟢 safest tile that can attack <br/>
        🔵 tile with the best attack / defense ratio
      </td>
      <td><img src="/HarrelManorReadMe/AIScoring.png?raw=true" /></td>
    </tr>
  </table>

  
  <img align="right" src="/HarrelManorReadMe/logigramIA.png?raw=true" width="40%x" />
  
  ```cs
  public void PlayTurn() {
    //Start
    EvaluateTiles();
    //Can I Attack Player ?
    int bestAtkScore = GetBestScore(Score.Attack);
    if (bestAtkScore > 0) {
        //Yes
        AttackLoop();
    }else{
        //No
        switch (state) {
            case EnnemyState.Regular :
                //Go to average tile
                break;
            case EnnemyState.Defensive :
                //Go to safest attack tile
                break;
            case EnnemyState.Aggressive :
                //Go to best attack tile
                break;
        }
    }
    AttackLoop();//Will do nothing if not enough ap
    if (entity.pm > 0) {
        //Go to safest tile
    }
    //End   
}
  ```  
</details>

<details>

<summary>

### Music and Sounds Integration
  
</summary>
<img src="/HarrelManorReadMe/mixer.PNG?raw=true" width="100%x" />
<br/>
<br/>
<img align="right" src="/HarrelManorReadMe/Sound.PNG?raw=true" width="40%x" />
I worked with two people for the sounds, a sound designer and a composer. We used the Unity audio system with the Audio Mixer to apply dynamic effects to the sounds, such as reverb and a low-pass filter. We also used it to transition between songs. 
<br/>
<br/>
The songs were written with the same length and beats so that we could simply fade in and out of different songs seamlessly. This allowed us to dynamically change the ambiance based on the game's state, such as when the player is low on health, when the 
pause menu is up, and, of course, to switch the music between exploration mode and fighting mode.
<br/>
<br/>
The ambient sounds were placed all around the level with 3D audio sources, so the player can hear them at different volumes depending on their relative distance.
<br/>
<br/>
Finally, the use of the Audio Mixer allowed me to add an option in the menu to control the volume of different types of sounds separately, such as the music and the sound effects.

</details>
