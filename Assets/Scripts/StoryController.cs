using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Ink.Runtime;

public class StoryController : MonoBehaviour
{
    [Header("Scene Refs")]
    public Image backgroundImage;
    public CharacterView char1;   // Alice
    public CharacterView char2;   // Ethan

    [Header("Ink Data")]
    public TextAsset inkJSONAsset;    // file JSON từ Ink
    public string char1Id = "Alice";
    public string char2Id = "Ethan";

    [Header("BG Pan (nếu có)")]
    public BGPanHorizontal bgPan;

    private Story inkStory;
    private CharacterView current = null;
    private CharacterView previous = null;
    private Coroutine typingCo;
    private bool typing;

    void Start()
    {
        char1.HideInstant();
        char2.HideInstant();

        inkStory = new Story(inkJSONAsset.text);
        Next();
    }

    public void OnClickNext()
    {
        if (typing) { SkipTyping(); return; }
        Next();
    }

    void Next()
    {
        if (inkStory.canContinue)
        {
            string text = inkStory.Continue().Trim();

            // Biến tạm
            string speakerName = "";
            Sprite portraitSprite = null;

            // Đọc tags
            foreach (string tag in inkStory.currentTags)
            {
                if (tag.StartsWith("speaker:"))
                {
                    speakerName = tag.Substring("speaker:".Length).Trim();
                    current = (speakerName == char1Id) ? char1 : char2;
                }
                else if (tag.StartsWith("bg:"))
                {
                    string bgName = tag.Substring("bg:".Length).Trim();
                    Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/" + bgName);
                    if (bgSprite) backgroundImage.sprite = bgSprite;
                }
                else if (tag.StartsWith("portrait:"))
                {
                    string portraitName = tag.Substring("portrait:".Length).Trim();
                    portraitSprite = Resources.Load<Sprite>("Portraits/" + portraitName);
                }
            }

            // Nếu chưa có speaker thì mặc định char1
            if (current == null) current = char1;
            var other = (current == char1) ? char2 : char1;

            // Focus background nếu có
            if (bgPan) bgPan.FocusById(current == char1 ? char1Id : char2Id);

            // Tắt người trước nếu đổi speaker
            if (previous != null && previous != current)
                previous.Exit();

            current.Dim(false);
            if (other.gameObject.activeSelf) other.Dim(true);
            if (!current.gameObject.activeSelf) current.Enter();

            // Gán tên, ảnh, nhưng chưa gán text (text sẽ gán qua typewriter)
            current.Bind(speakerName, portraitSprite, "");

            // Hiệu ứng typewriter với text từ Ink
            if (typingCo != null) StopCoroutine(typingCo);
            typingCo = StartCoroutine(Typewriter(current.dialogueText, text, 0.02f, current.sizer));

            previous = current;

        }
        else if (inkStory.currentChoices.Count > 0)
        {
            // TODO: hiển thị UI lựa chọn
            Debug.Log("Có lựa chọn:");
            for (int i = 0; i < inkStory.currentChoices.Count; i++)
            {
                Debug.Log($"[{i}] {inkStory.currentChoices[i].text}");
            }

            // Tạm thời tự chọn lựa chọn đầu tiên
            inkStory.ChooseChoiceIndex(0);
            Next();
        }
        else
        {
            EndSequence();
        }
    }

    IEnumerator Typewriter(TextMeshProUGUI tmp, string full, float delay, DialogueAutoSizer sizer)
    {
        typing = true;
        tmp.text = full;
        tmp.maxVisibleCharacters = 0;
        if (sizer) sizer.ResizeForVisible();

        for (int i = 0; i <= full.Length; i++)
        {
            tmp.maxVisibleCharacters = i;
            if (sizer) sizer.ResizeForVisible();
            yield return new WaitForSeconds(delay);
        }

        typing = false;
    }

    void SkipTyping()
    {
        if (typingCo != null) StopCoroutine(typingCo);
        current.dialogueText.maxVisibleCharacters = int.MaxValue;
        if (current.sizer) current.sizer.ResizeForVisible();
        typing = false;
    }

    void EndSequence()
    {
        if (char1.gameObject.activeSelf) char1.Exit();
        if (char2.gameObject.activeSelf) char2.Exit();
        Debug.Log("Hết Ink Story!");
    }
}
