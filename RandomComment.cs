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
        "What is this?\nAnother line.",
        "Who am I?\nAnother line.",
        "This is a random comment\nAnother line.",
        "Random chance for this one too.\nAnother line.",
        "I don't know what I'm supposed to do.\nAnother line.",
        "Beware the Hivemind.\nAnother line."
    };

    void Start () {
        commentBox = transform.parent.gameObject;
        textMesh = GetComponent<TextMesh>();
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

    string GetRandomComment()
    {
        return comments[Random.Range(0, comments.Count)];
    }

    
}
