using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSquareAnimation : MonoBehaviour
{
    [SerializeField] private float scaleDownDuration = 2f;
    [SerializeField] private float scaleUpDuration = 2f;
    [SerializeField] private float rotationDuration = 2f;

    private Transform pivot;
    private Transform corner;

    private Vector3[] positions;

    private Image[] images;


    [SerializeField] [ColorUsage(true)] private List<Color> colors;

    private int colorIndex = 0;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        pivot = transform.Find("Pivot");

        corner = pivot.Find("Corner");
        positions = new Vector3[4];

        images = new Image[4];


        images[1] = transform.GetChild(0).GetComponent<Image>();
        images[2] = transform.GetChild(1).GetComponent<Image>();
        images[3] = transform.GetChild(2).GetComponent<Image>();
        images[0] = transform.GetChild(3).GetChild(1).GetComponent<Image>();

        positions[0] = images[0].transform.position;
        positions[1] = images[1].transform.position;
        positions[2] = images[2].transform.position;
        positions[3] = images[3].transform.position;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = colors[colorIndex];
        }

        colorIndex++;

        while (true)
        {
            yield return StartCoroutine(SingleLoopCor());
        }
    }

    IEnumerator SingleLoopCor()
    {
        corner.localScale = Vector3.one;
        ChangeParentObjects(corner);
        yield return corner.DOScale(Vector3.one * 0.4f, scaleDownDuration).SetEase(Ease.InQuad).WaitForCompletion();
        Quaternion nextRot = pivot.rotation * Quaternion.AngleAxis(90, Vector3.back);
        pivot.DORotateQuaternion(nextRot, rotationDuration).SetEase(Ease.OutQuart);
        images[0].DOColor(colors[colorIndex], rotationDuration * 0.5f);

        for (int i = 1; i < images.Length; i++)
        {
            images[i].color = colors[colorIndex];
        }

        colorIndex++;
        colorIndex = colorIndex % colors.Count;

        yield return new WaitForSeconds(rotationDuration * 0.6f);

        ChangeParentObjects(transform);
        RotatePositions();

        for (int i = 1; i < 4; i++)
        {
            images[i].transform.position = positions[i];
        }

        for (int i = 1; i < 4; i++)
        {
            images[i].transform.DOScale(Vector3.one, scaleUpDuration).SetEase(Ease.OutQuart);
        }

        yield return new WaitForSeconds(scaleUpDuration);
    }

    private void ChangeParentObjects(Transform parent)
    {
        for (int i = 1; i < 4; i++)
        {
            images[i].transform.SetParent(parent);
            images[i].transform.rotation = Quaternion.identity;
        }
    }

    void RotatePositions()
    {
        Vector3 temp = positions[0];
        for (int i = 0; i <= 2; i++)
        {
            positions[i] = positions[i + 1];
        }

        positions[3] = temp;
    }
}