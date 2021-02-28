using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class GameManager : Singleton<GameManager>
{
    private PlayableDirector director;

    void Awake(){
        director = GetComponent<PlayableDirector>();
    }
    

    public void SetTimelinePlayable(TimelineAsset scene){
        director.playableAsset = scene;
        director.Play();
    }

    public void FinishGame(){
        Debug.Log("Game Finished!");
    }

    public void GameOver(){
        Debug.Log("Game Over!");
    }
}
