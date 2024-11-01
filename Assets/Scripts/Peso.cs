using UnityEngine;
using System;

public class WeightHandler
{
    private float _maxWeight;
    private float _currentMaxWeight;

    public WeightHandler(float maxWeight)
    {
        _maxWeight = maxWeight;
        _currentMaxWeight = 0;
    }

    public float CalculateFillAmount(float weight)
    {
        return Mathf.Lerp(0f, 1f, weight / _maxWeight);
    }

    public float ReturnMaxWeight(float weight = 0f)
    {
      if(weight == 0f) return _currentMaxWeight;

      return _currentMaxWeight = Math.Max(_currentMaxWeight, weight);

    }
    public void SetMaxWeight(float weight)
    {
        _currentMaxWeight = weight;
    }
    public void ResetMaxWeight()
    {
        _currentMaxWeight = 0;
    }
}
