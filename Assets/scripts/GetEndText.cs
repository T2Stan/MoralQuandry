using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetEndText : MonoBehaviour
{
    string EndNarrativ;
    int EndNum;

    Text Endtext;

    // Start is called before the first frame update
    void Start()
    {
        Endtext = gameObject.GetComponent<Text>();
        EndNum = 3;

        if (GameState.playerInventory.Love <= 0)
        {
            EndNum = 1;

        }
        else if (GameState.playerInventory.Hope <= 0)
        {
            EndNum = 2;
        }
        else if (GameState.playerInventory.Joy <= 0)
        {
            EndNum = 3;
        }
        else if (GameState.playerInventory.Parts <= 0)
        {
            EndNum = 4;
        }
        else
        {
            EndNum = 5;
        }


        if (EndNum == 1)
        {
            Endtext.text = "Big fights become scandals. Some even make the news. But that's just the symptom of an entire community that has woken up on the wrong side of the toybox. Resentment is contagious. You see it in the curled lip of a rubber duck, the tight fist of a teddy bear. Toys take sides just to have something to vent about. Peacemakers step up. Then step down again in frustration. You do what you can to thwart the tinderbox of rage building. But it isn't enough. The war spares no toy.";
        }
        else if (EndNum == 2)
        {
            Endtext.text = "With no hope in the future you promised, the toys turn against you. The plot begins as whispers quickly hushed as you walk by. You almost know what's coming when you are pulled from your bed by plastic arms in the dead of night. Chased off the island by a mob of angry toys, you start the long swim back to the mainland. When you can swim no further, you turn to see the Isle of Misfits one last time. On the horizon, you can just make out the lights in the village until they go out.";
        }
        else if (EndNum == 3)
        {
            Endtext.text = "At first, toys miss work shifts. Then, attendance at tea parties and castle bouncy time gets scarce. The toys say they're just tired. But their colors seem faded, squeakers and voiceboxes go silent. Sleeptime and waketime blur as toys shuffle around in a dreamlike stupor. Their legs and wheels drag over the pavement until they stop moving altogether. Graffiti in the town square, surrounded by the empty shells of the last toys who fell where they stood, declares -PLAYTIME IS OVER.-";
        }
        else if (EndNum == 4)
        {
            Endtext.text = "The beautiful ones are the first to go.The few replacement parts left are too clunky for their delicate limbs and mechanisms.Rainbow pots of paint have long - since dried to useless crust.So though empty eyesockets and cracked faces, they try their best to keep up a cheerful facade. After all, it's all they know how to be. Some toys last longer than others. But even preschool toys get sun-cracked and loose eventually. Brave souls offer to go to recycling, but everyone knows, their parts are too worn out to be any good. Misfit Isle becomes a living graveyard of fragmented toys wearing the best broken smiles they can muster, waiting in vain that they are not abandoned. Merely well-loved.";
        }
        else
        {
            Endtext.text = "The elders of the village declare vanity a sin, and everything shifts uncomfortably. Toys celebrate the freedom to be 'out of the box' and embrace their scars. But without the taboo of mismatch parts, soon the desire of every toy to survive above all takes a vicious edge. On the Night of Shattered Vanities, the streets sparkle with the tiny pieces of hundreds of broken mirrors that crunch underfoot. Weaker toys, coveted for their parts, are jumped by gangs of violent Frankensteined horrors. You flee when the toys turn upon the living.";
        }
        
        }
    }
