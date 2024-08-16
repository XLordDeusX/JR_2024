using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PackageProcessor : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _message;
    [SerializeField] private string _hash;
    [SerializeField] private bool _isSerialized;
    [SerializeField] private bool _isCorrupted;
    void Start()
    {
        DataPackage package = new DataPackage(_id.ToString(), _message, _hash);
        print($"ID: {_id} | Message: {_message} | Hash: {_hash}");
    }

    void Update()
    {
        if (_isSerialized) SerializePackage();
        else DeserializePackage();
    }

    void SerializePackage()
    {
        int newMessage = 0;
        int hashValue = 0;
        foreach (char character in _message)
        {
            newMessage += character;        //suma el valor ascii de cada caracter del mensaje
            Debug.Log($"Valor actual del mensaje: {newMessage}");
        }

        foreach (char character in _hash)
        {
            hashValue += character;
            Debug.Log($"Valor actual del hash: {hashValue}");
        }
        newMessage *= hashValue;     //le multiplica el valor ascii del hash
        Debug.Log($"Valor final del mensaje: {newMessage}");
    }

    void DeserializePackage()
    {

    }
}

public class DataPackage
{
    public DataPackage(string id, string content, string hash) { }
}
