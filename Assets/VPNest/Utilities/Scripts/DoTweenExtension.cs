using DG.Tweening.Core;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;

public static class DoTweenExtension
{
    public static TweenerCore<float, float, FloatOptions> DOBlendShapeWeight(this SkinnedMeshRenderer renderer,
        int index, float target, float duration)
    {
        float value = renderer.GetBlendShapeWeight(index);
        return DOTween.To(() => value, x =>
        {
            value = x;
            renderer.SetBlendShapeWeight(index, value);
        }, target, duration).SetTarget(renderer);
    }
    
    public static TweenerCore<float, float, FloatOptions> DOAlpha(this Graphic graphic, float alpha, float duration)
    {
        float a = graphic.color.a;
        return DOTween.To(() => a, x =>
        {
            Color color = graphic.color;
            color.a = a = x;
            graphic.color = color;
        }, alpha, duration).SetTarget(graphic);
    }
}