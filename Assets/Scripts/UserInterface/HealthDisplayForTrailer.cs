
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthDisplayForTrailer : HealthDisplay
{

    
    public override void Init(bool isPlayer1)
    {
        base.Init(isPlayer1);
        StartCoroutine(EmphasizePlayer1());
    }
    public IEnumerator EmphasizePlayer1()
    {
        StartCoroutine(ScalePunch(Player1HealthText.transform));
        StartCoroutine(ScalePunch(Player1HealthBar.transform));

        StartCoroutine(ColorFlash(Player1HealthBarImage, GetEmphasizeColor(Player1HealthBarImage)));


        yield return new WaitForSeconds(0.5f);
        EmphasizePlayer2();
    }
    public void EmphasizePlayer2()
    {
        StartCoroutine(ScalePunch(Player2HealthText.transform));
        StartCoroutine(ScalePunch(Player2HealthBar.transform));

        StartCoroutine(ColorFlash(Player2HealthBarImage, GetEmphasizeColor(Player2HealthBarImage)));
    }
    IEnumerator ScalePunch(Transform target)
    {
        Vector3 original = Vector3.one;
        Vector3 big = original * 1.2f;

        float t = 0f;

        // 커지기
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(original, big, t / 0.15f);
            yield return null;
        }

        t = 0f;

        // 돌아오기
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(big, original, t / 0.15f);
            yield return null;
        }
    }
    IEnumerator ColorFlash(Image img, Color targetColor)
    {
        Color original = img.color;
        float t = 0f;

        // 강조
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(original, targetColor * 1.3f, t / 0.15f);
            yield return null;
        }

        t = 0f;

        // 복귀
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(targetColor * 1.3f, original, t / 0.3f);
            yield return null;
        }
    }
    Color GetEmphasizeColor(Image img)
    {
        Color baseColor = img.color;
        return baseColor * 1.3f; // 살짝 밝게
    }
}