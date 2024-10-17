using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    [Serializable]
    public class DataTeste
    {
        public long Tempo { get; set; }
        public float PesoMaximo { get; set; }
        public List<(long, float)> Registros { get; set; }
    }
}