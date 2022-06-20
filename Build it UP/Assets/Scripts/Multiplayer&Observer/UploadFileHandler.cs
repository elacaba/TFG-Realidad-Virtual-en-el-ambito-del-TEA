using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Text;
using PlayFab.Internal;
using System.Net.Mail;
using System.ComponentModel;
using System.IO;

public class UploadFileHandler
{
    public string filename = "";
    private LevelLoader levelLoader;
    private string entityId = null;//LevelLoader.instance.entityId;
    private string entityType = null;//LevelLoader.instance.entityType;
    public int GlobalFileLock = 0;
    private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _tempUpdates = new Dictionary<string, string>();

    // Start is called before the first frame update

    public UploadFileHandler()
    {
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UploadFile(string filename)
    {
        this.filename = filename;

        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { this.filename }
        };
        Debug.Log("a ver la request:" + request.Entity + request.FileNames);
        PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload, OnInitFailed);
    }

    void OnInitFailed(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { this.filename },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(this.filename); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }
    void OnInitFileUpload(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        Debug.Log("response" + response);
        string payloadStr;
        if (!_entityFileJson.TryGetValue(this.filename, out payloadStr))
            payloadStr = "{}";
        var payload = Encoding.UTF8.GetBytes(payloadStr);

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );

        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    void FinalizeUpload(byte[] data)
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { this.filename },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, OnUploadSuccess, OnSharedFailure);
        GlobalFileLock -= 1; // Finish SimplePutCall
        void OnSharedFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
            GlobalFileLock -= 1;
        }
    }

    void OnUploadSuccess(PlayFab.DataModels.FinalizeFileUploadsResponse result)
    {
        Debug.Log("File upload success: " + this.filename);
        GlobalFileLock -= 1; // Finish FinalizeFileUploads
    }

    void OnSharedFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }

    public void SendEmail(string filename, string nameJugador, string emailSender)
    {
        // Command-line argument must be the SMTP host.
        SmtpClient client = new SmtpClient("smtp.office365.com", 587);
        client.Credentials = new System.Net.NetworkCredential(
            "eva.tfg@hotmail.com",
            "4gu4c4t3");
        client.EnableSsl = true;
        // Specify the email sender.
        // Create a mailing address that includes a UTF8 character
        // in the display name.
        MailAddress from = new MailAddress(
            "eva.tfg@hotmail.com",
            "Unity",
            System.Text.Encoding.UTF8);
        // Set destinations for the email message.
        MailAddress to = new MailAddress(emailSender);
        // Specify the message content.
        MailMessage message = new MailMessage(from, to);
        message.Body = "Report del jugador " + nameJugador + " con fecha y hora " + DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss");
        message.BodyEncoding = System.Text.Encoding.UTF8;
        if (filename != null && !filename.Equals(""))
            if (File.Exists(filename))
                message.Attachments.Add(new Attachment(filename));
        message.Subject = "Report de " + nameJugador + " día " + DateTime.UtcNow.ToString("dd/MM/yyyy");
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        // Set the method that is called back when the send operation ends.
        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        // The userState can be any object that allows your callback
        // method to identify this send operation.
        // For this example, the userToken is a string constant.
        string userState = "test message1";
        client.SendAsync(message, userState);
    }

    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;

        if (e.Cancelled)
        {
            Debug.Log("Send canceled " + token);
        }
        if (e.Error != null)
        {
            Debug.Log("[ " + token + " ] " + " " + e.Error.ToString());
        }
        else
        {
            Debug.Log("Message sent.");
        }
    }
}
