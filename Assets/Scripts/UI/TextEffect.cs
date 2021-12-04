using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    TMP_Text textMesh;

    Mesh mesh;

    Vector3[] vertices;
    Color[] colors;

    public Gradient rainbow;
    public float gradientTime;

    List<int> wordIndexes;
    List<int> wordLengths;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);

        StartCoroutine(GradientTimer());

        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
    }

    // Update is called once per frame
    void Update()
    {
        //textMesh.ForceMeshUpdate();

        // /*PER WORD*/
        // // for (int w = 0; w < wordIndexes.Count; w++)
        // // {
        // //     int wordIndex = wordIndexes[w];
        // //     Vector3 offset = Wobble(Time.time + w);

        // //     for (int i = 0; i < wordLengths[w]; i++)
        // //     {
        // //         TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];

        // //         int index = c.vertexIndex;
        // //         for (int o = 0; o < 4; o++)
        // //         {
        // //             vertices[index + o] += offset;
        // //             colors[index + o] = rainbow.Evaluate(Time.time + (vertices[index + o].x * .0001f));
        // //         }
        // //     }
        // // }

        // /*PER CHARACTER*/
        // // for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        // // {
        // //     TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

        // //     //Vector3 offset = Wobble(Time.time + i);

        // //     int index = c.vertexIndex;
        // //     for (int j = 0; j < 4; j++)
        // //     {
        // //         //vertices[index + j] += offset;
        // //         colors[index + j] = rainbow.Evaluate(Mathf.Repeat(gradientTime + (vertices[i].x * .0001f), 1f));
        // //     }
        // // }

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    colors[i] = rainbow.Evaluate(Mathf.Repeat((i * gradientTime) + (vertices[i].y * .0001f), 1f));
        //}

        // mesh.vertices = vertices;
        //mesh.colors = colors;
        //textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 1f), Mathf.Cos(time * 15f));
    }

    IEnumerator GradientTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.003f);

            if (gradientTime >= 1f)
                gradientTime = 0f;

            gradientTime += 0.0001f;
        }
    }
}
