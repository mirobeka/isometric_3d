using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public TimelineAsset gameOverScene;
    private PlayableDirector director;

    void Awake(){
        director = GetComponent<PlayableDirector>();
    }
    

    public void SetTimelinePlayable(TimelineAsset scene){
        director.playableAsset = scene;
        director.Play();
    }

    public void NextScene(){
        Debug.Log("Game Finished!");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReloadScene(){
        Debug.Log("Game Finished!");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GameOver(){
        if (gameOverScene != null){
            SetTimelinePlayable(gameOverScene);
        }
    }
}
