using UnityEngine;
using System.Collections.Generic;

public class RandomComment : MonoBehaviour {

    GameObject commentBox;
    TextMesh textMesh;
    float displayTime = 5f;

    const int maxCharsPerLine = 24; // 24 seemed nice

    /*
    List<string> comments = new List<string>()
    {
        "What is this?asdsadsadsadsadsa  wd wdaw dwdw",
        "Who am I?asdsadsadsadsadasdasd dw dwad wd",
        "This is a random commentsadsadsadsad dw dw dw ",
        "Random chance for this one too. dwd w dw",
        "I don't know what I'm supposed to do. dw dw dw dw",
        "Beware the Hivemind.asdsadsadsadsad dw dw dw dw"
    };
    */
    List<string> comments = new List<string>()
    {
        "Now, where did i put those keys.",
        "What should we eat today,\nsomething fast and simple.",
        "I must be coming sick\n*sneeze*, only 1 more\nday, i just got to\nhold on till then.",
        "Should i take phone, tablet\nor computer, and if\ncomputer what kind?",
        "Thank goodness christmas\nis over,small children\nare just impossible.",
        "Was total miracle that i\nwas at work in time\ntoday, i really need\na new alarm clock.",
        "Really cant wait the new\nmovies hit the theaters\n, kids will love it.",
        "That new tv and audio\nsystem really made everyone\nhappy in the family.",
        "Damn this flu is killing me.",
        "This might actually work.",
        "This is just purely wrong.",
        "Am i ready for this?",
        "Hope the bank is still open\nafter my work .",
        "My alcohol problem is\ngetting out of hand,\nits already on the floor.",
        "Does this plant look\ndifferent from yesterday,\nshould i report this or not?",
        "This is just not may day.",
        "I really need some extra\nwork hours, im so behind\non bills.",
        "Early mornings are not\nmy strong sides .",
        "Strange, its so damn\nquiet out here, did\nthe planet die \nor something ? .",
        "Im so hungry.",
        "I just want a nice,\neasy life. What's wrong\nwith that?",
        "I'm too old to start again.",
        "Going try to avoid\nyearly employee interview,\nboss might notice i dont\nhave proper insurance .",
        "I will pack my bags and\nmove to alaska, \nI'm done with this .",
        "Just crossed my mind, \nI have never seen my\n boss or higher staff,\n thats odd.",
        "I didnt agree to this \ncrap when i took the job.",
        "The pace here, its \nreally killing me, \nwhat next 18 hours \nin a day hmmm? .",
        "I'm so bored.",
        "Na naa naa na na.",
        "As a child, i was been\ntold dolls were for girls..",
        "That summer seemed\nto last forever .",
    };

    void Start () {
        commentBox = transform.parent.gameObject;
        textMesh = GetComponent<TextMesh>();
	}

    int num = 0;
    void Update()
    {
        // Testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            num++;
            if (num >= comments.Count) num = 0;
            GameObject.Find("Assistant Lacount").GetComponentInChildren<RandomComment>().GetComment(num);
        }

    }
    
    void Hide()
    {
        textMesh.text = "";
        commentBox.SetActive(false);
    }

    public void NewComment(string comment, float displayTime = 5f)
    {
        CancelInvoke();
        string text = comment;

        if (comment.Length > maxCharsPerLine) text = "";
        while (comment.Length > maxCharsPerLine)
        {
            text += comment.Substring(0, maxCharsPerLine - 1);
            comment = comment.Substring(maxCharsPerLine);
            text += "\n";
        }

        commentBox.SetActive(true);
        textMesh.text = text;
        Invoke("Hide", displayTime);
    }

    public void NewRandomComment(float displayTime = 5f)
    {
        CancelInvoke();
        string comment = GetRandomComment();

        string text = comment;

        /*
        if (comment.Length > maxCharsPerLine) text = "";
        while (comment.Length > maxCharsPerLine)
        {
            text += comment.Substring(0, maxCharsPerLine - 1);
            comment = comment.Substring(maxCharsPerLine);
            text += "\n";
        }
        */
        commentBox.SetActive(true);
        textMesh.text = text;
        Invoke("Hide", displayTime);
    }

    void GetComment(int index)
    {
        CancelInvoke();
        commentBox.SetActive(true);
        textMesh.text = comments[index];
        //Invoke("Hide", displayTime);
    }

    string GetRandomComment()
    {
        return comments[Random.Range(0, comments.Count)];
    }

    
}
