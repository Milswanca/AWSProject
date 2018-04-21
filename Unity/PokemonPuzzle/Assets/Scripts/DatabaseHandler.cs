using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseHandler : MonoBehaviour
{
    public class ErrorResult
    {
        public ErrorResult(int _errorNum, string _errorMsg)
        {
            ErrorNumber = _errorNum;
            ErrorMessage = _errorMsg;
        }

        public int ErrorNumber;
        public string ErrorMessage;
    }

    public delegate void SuccessFailReturn(bool success, ErrorResult message);
    public delegate void CollectionReturn(bool success, ErrorResult message, NameValueCollection collection);

    public static bool LoggedIn { get; private set; }
    public static string Username { get; private set; }
    private static string Password;

    private static NameValueCollection cachedProfileInfo = null;
    private static NameValueCollection pendingProfileInfo = null;

    private static string SendScoreURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/SendScore.php?";
    private static string CreateAccountURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/CreateAccount.php?";
    private static string LoginURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/Login.php?";
    private static string ChangePasswordURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/ChangePassword.php?";
    private static string GetProfileURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/GetProfile.php?";
    private static string UpdateProfileURL = "puzzledserver-env4.us-west-2.elasticbeanstalk.com/UpdateProfile.php?";
    private static string PrivateKey = "pm36YVRuGh";

    public static DatabaseHandler Get()
    {
        return MasterManager.instance.DatabaseHandler;
    }

    public void CreateAccount(string _username, string _password, SuccessFailReturn _finishedEvent)
    {
        string url = CreateAccountURL + "Username=" + WWW.EscapeURL(_username) + "&Password=" + _password;
        AddHashToURL(ref url, _username + _password);

        StartCoroutine(SuccessFailRoutine(url, new SuccessFailReturn[] { _finishedEvent }));
    }

    public void Login(string _username, string _password, SuccessFailReturn _finishedEvent)
    {
        if(LoggedIn)
        {
            _finishedEvent(false, new ErrorResult(-1, "Already Logged In"));
            return;
        }

        string url = LoginURL + "Username=" + WWW.EscapeURL(_username) + "&Password=" + _password;
        AddHashToURL(ref url, _username + _password);

        Username = _username;
        Password = _password;
        StartCoroutine(SuccessFailRoutine(url, new SuccessFailReturn[] { OnLoggedIn, _finishedEvent }));
    }

    public void Logout()
    {
        if(!LoggedIn) { return; }

        Username = "";
        Password = "";
        LoggedIn = false;
        cachedProfileInfo = null;
    }

    private void OnLoggedIn(bool _success, ErrorResult _errorMessage)
    {
        if(_success)
        {
            LoggedIn = true;

            GetMyProfileInfo(OnRetrievedMyProfile);
        }
    }

    public void PostScore(int _score, SuccessFailReturn _finishedEvent)
    {
        string url = SendScoreURL + "Username=" + WWW.EscapeURL(Username) + "&Password=" + Password + "&Score=" + _score;
        AddHashToURL(ref url, Username + Password + _score);
        StartCoroutine(SuccessFailRoutine(url, new SuccessFailReturn[] { _finishedEvent }));
    }

    //Changes account password
    //@_newPassword: New account password
    //@_finishedEvent: Password change Callback
    public void ChangePassword(string _newPassword, SuccessFailReturn _finishedEvent)
    {
        string url = ChangePasswordURL + "Username=" + WWW.EscapeURL(Username) + "&Password=" + Password + "&NewPassword=" + _newPassword;
        AddHashToURL(ref url, Username + Password + _newPassword);

        StartCoroutine(SuccessFailRoutine(url, new SuccessFailReturn[] { _finishedEvent }));
    }

    public void GetProfileInfo(string _playerName, CollectionReturn _finishedEvent)
    {
        if(_playerName == null)
        {
            _finishedEvent(false, new ErrorResult(-1, "Invalid player name"), null);
            return;
        }

        string url = GetProfileURL + "ProfileName=" + WWW.EscapeURL(_playerName);
        AddHashToURL(ref url, _playerName);

        StartCoroutine(CollectionRoutine(url, new CollectionReturn[] { _finishedEvent }));
    }

    public void GetMyProfileInfo(CollectionReturn _finishedEvent)
    {
        if(cachedProfileInfo != null)
        {
            _finishedEvent(true, null, cachedProfileInfo);
            return;
        }

        GetProfileInfo(Username, _finishedEvent);
    }

    public void UpdateProfileInfo(string _greeting, int _displayPic, SuccessFailReturn _finishedEvent)
    {
        string url = UpdateProfileURL + "Username=" + WWW.EscapeURL(Username) + "&Password=" + Password + "&Greeting=" + _greeting + "&DisplayPic=" + _displayPic;
        AddHashToURL(ref url, Username + Password + _greeting + _displayPic);

        pendingProfileInfo = new NameValueCollection();
        pendingProfileInfo.Add("Username", Username);
        pendingProfileInfo.Add("Greeting", _greeting);
        pendingProfileInfo.Add("DisplayPic", _displayPic.ToString());

        StartCoroutine(SuccessFailRoutine(url, new SuccessFailReturn[] { _finishedEvent, UpdatedProfileInfo }));
    }

    private void UpdatedProfileInfo(bool _success, ErrorResult _error)
    {
        if (_success)
        {
            cachedProfileInfo = pendingProfileInfo;
        }
    }

    private void OnRetrievedMyProfile(bool _success, ErrorResult _error, NameValueCollection _nameValueCollection)
    {
        cachedProfileInfo = _nameValueCollection;
    }

    //Routine for all success or fail HTTP callbacks
    //@_url: WWW url request
    //@_finishedEvents: Events to call after the HTTP request completes, this is an array so we can auto track things such as logging in
    private IEnumerator SuccessFailRoutine(string _url, SuccessFailReturn[] _finishedEvents)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://" + _url))
        {
            yield return request.SendWebRequest();

            //See if theres an error
            ErrorResult error = GetErrorFromWWWText(request);

            if (error != null)
            {
                MasterManager.instance.PopupManager.QueuePopup(EPopupTypes.SingleTextSingleOption, new PopupSingleTextSingleOption.Data("Network Error!", error.ErrorMessage, "Okay"));
            }

            //Call the callbacks
            foreach (SuccessFailReturn i in _finishedEvents)
            {
                if(i == null) { continue; }

                i((error == null) ? true : false, error);
            }
        }
    }

    //Routine for all retrieval callbacks
    //@_url: WWW url request
    //@_finishedEvents: Events to call after the HTTP request completes, this is an array so we can auto track things such as logging in
    private IEnumerator CollectionRoutine(string _url, CollectionReturn[] _finishedEvents)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://" + _url))
        {
            yield return request.SendWebRequest();

            //See if theres an error
            ErrorResult error = GetErrorFromWWWText(request);

            bool success = error == null;

            NameValueCollection collection = null;

            if (!success)
            {
                MasterManager.instance.PopupManager.QueuePopup(EPopupTypes.SingleTextSingleOption, new PopupSingleTextSingleOption.Data("Network Error!", error.ErrorMessage, "Okay"));
            }
            else
            {
                collection = ParseQueryString(request.downloadHandler.text);
            }

            //Call the callbacks
            foreach (CollectionReturn i in _finishedEvents)
            {
                i(success, error, collection);
            }
        }
    }

    private ErrorResult GetErrorFromWWWText(UnityWebRequest _website)
    {
        if(_website.error != null)
        {
            return new ErrorResult(-1, _website.error);
        }

        string[] split = _website.downloadHandler.text.Split('\n');

        foreach(string i in split)
        {
            if(i.Contains("Error:"))
            {
                int errorNum = -1;
                string message = "";

                string[] splitString = i.Split('[', ']');

                errorNum = System.Convert.ToInt32(splitString[1]);
                message = splitString[2];

                return new ErrorResult(errorNum, message);
            }
        }

        return null;
    }

    private void AddHashToURL(ref string _url, string _hashData)
    {
        _url += "&Hash=" + Md5Sum(_hashData + PrivateKey);
    }

    public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

    public static NameValueCollection ParseQueryString(string s)
    {
        NameValueCollection nvc = new NameValueCollection();
        // remove anything other than query string from url
        if (s.Contains("?"))
        {
            s = s.Substring(s.IndexOf('?') + 1);
        }
        foreach (string vp in Regex.Split(s, "&"))
        {
            string[] singlePair = Regex.Split(vp, "=");
            if (singlePair.Length == 2)
            {
                nvc.Add(singlePair[0], singlePair[1]);
            }
            else
            {
                // only one key with no value specified in query string
                nvc.Add(singlePair[0], string.Empty);
            }
        }
        return nvc;
    }
}
