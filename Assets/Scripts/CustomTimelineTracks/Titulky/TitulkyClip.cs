using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitulkyClip : PlayableAsset
{
    [TextArea(3, 10)]
    public string titulkyText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TitulkyBehaviour>.Create(graph);

        TitulkyBehaviour titBehav = playable.GetBehaviour();
        titBehav.titulkyText = titulkyText;

        return playable;
    }
}


