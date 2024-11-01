using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataBase
{

    private static IMongoCollection<BsonDocument> _collection;


    public DataBase(string url, string databaseName, string collection)
    {
        var client = new MongoClient(url);
        var database = client.GetDatabase(databaseName);

        _collection = database.GetCollection<BsonDocument>(collection);

    }

    public async void InsertDocument(DataTeste data)
    {
        try
        {
            BsonDocument document = data.ToBsonDocument();

            await _collection.InsertOneAsync(document);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao inserir documento: {ex.Message}");
            ToastUtil.ShowToastError("Erro ao inserir documento no banco");
        }
    }
    public void SalvarDadosEmJson(DataTeste dados)
    {
        string caminhoArquivo = @"C:\Users\gustt\OneDrive\Documentos\testefoguete\dados.json";
        try
        {
            string json = JsonUtility.ToJson(dados);

            File.WriteAllText(caminhoArquivo, json);

        }
        catch (Exception ex)
        {
            Debug.LogError("Erro ao salvar os dados: " + ex.Message);
        }
    }

    [System.Serializable]
    public class Registro
    {
        public long Tempo;
        public float Peso;
    }

    [System.Serializable]
    public class DataTeste
    {
        public long Tempo;
        public float PesoMaximo;
        public List<Registro> Registros;
    }
}