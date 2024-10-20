using UnityEngine;

public class WeightHandler
{
    private float _maxWeight;
    private float _currentMaxWeight;
    private DataBase.DataTeste _dataTeste;

    public WeightHandler(float maxWeight)
    {
        _maxWeight = maxWeight;
        _currentMaxWeight = 0;
        _dataTeste = new DataBase.DataTeste { Registros = new() };
    }

    public float CalculateFillAmount(float weight)
    {
        return Mathf.Lerp(0f, 1f, weight / _maxWeight);
    }

    public float ReturnMaxWeight(float weight)
    {
        if (_currentMaxWeight < weight)
        {
            _currentMaxWeight = weight;
            _dataTeste.PesoMaximo = _currentMaxWeight;
        }
        return _currentMaxWeight;
    }

    public void ResetMaxWeight()
    {
        _currentMaxWeight = 0;
        _dataTeste = new DataBase.DataTeste { Registros = new() };
    }
}
