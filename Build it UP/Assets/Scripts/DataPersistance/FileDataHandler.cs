using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using PlayFab;
using PlayFab.ClientModels;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private string filename = "";
    private UploadFileHandler fileHandler;
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = UnityEngine.JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file:" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = UnityEngine.JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
        filename = Application.persistentDataPath + "/report" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv";
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Nombre jugador; Nombre Actividad; Errores; Aciertos");
        tw.Close();

        tw = new StreamWriter(filename, true);
        if (TextDisplay.nombre == null)
            tw.WriteLine("null" + ";" + data.NombreJuego + ";" + data.respuestasIncorrectas + ";" + data.respuestasCorrectas);
        else
            tw.WriteLine(TextDisplay.nombre + ";" + data.NombreJuego + ";" + data.respuestasIncorrectas + ";" + data.respuestasCorrectas);
        //tw.WriteLine("Chica de prueba" + ";" + data.respuestasIncorrectas + ";" + data.respuestasCorrectas);
        tw.Close();

        this.fileHandler = new UploadFileHandler();
        //fileHandler.UploadFile(filename);
        fileHandler.SendEmail(filename, TextDisplay.nombre, ButtonExit.emailMonitor);

    }
}
