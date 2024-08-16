using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class Mensaje
{
    public string IdRemitente;
    public string Contenido;
    public string Hash;
}

public class MensajeHandler : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private string mensaje;
    [SerializeField] private bool tieneQueCorromperse;

    void Start()
    {
        var mensajeOriginal = new Mensaje
        {
            IdRemitente = ID.ToString(),
            Contenido = mensaje
        };

        mensajeOriginal.Hash = GenerarHash(mensajeOriginal.Contenido, tieneQueCorromperse);

        string mensajeSerializado = JsonUtility.ToJson(mensajeOriginal);
        Debug.Log("Mensaje serializado:");
        Debug.Log(mensajeSerializado);

        var mensajeDeserializado = JsonUtility.FromJson<Mensaje>(mensajeSerializado);
        Debug.Log("Mensaje deserializado:");
        Debug.Log("ID Remitente: " + mensajeDeserializado.IdRemitente);
        Debug.Log("Contenido: " + mensajeDeserializado.Contenido);

        bool esIntegro = VerificarIntegridad(mensajeDeserializado.Contenido, mensajeDeserializado.Hash);
        Debug.Log("¿El mensaje está corrupto?: " + !esIntegro);
    }

    private string GenerarHash(string contenido, bool tieneQueCorromperse)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contenido));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
                if (tieneQueCorromperse) break;
            }
            return builder.ToString();
        }
    }

    private bool VerificarIntegridad(string contenido, string hashOriginal)
    {
        string hashNuevo = GenerarHash(contenido, tieneQueCorromperse);
        return hashNuevo.Equals(hashOriginal);
    }
}
