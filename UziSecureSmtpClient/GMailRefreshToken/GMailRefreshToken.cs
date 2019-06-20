/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	GMailRefreshToken
//	The GMailRefreshToken application creates a refresh token file
//	from the JSON file created by GMail API certification.
//
//	Author: Uzi Granot
//	Version: 1.0
//	Date: February 1, 2017
//	Copyright (C) 2016-2019 Uzi Granot. All Rights Reserved
//
//	The UziSecureSmtpClient .NET class library, the TestSecureSmtpClient
//	test and demo application and the GMailRefreshToken application are
//	free software. They are distributed under the Code Project Open
//	License (CPOL 1.02) agreement. The full text of the CPOL is given in:
//	https://www.codeproject.com/info/cpol10.aspx
//	
//	The main points of CPOL 1.02 subject to the terms of the License are:
//
//	Source Code and Executable Files can be used in commercial applications;
//	Source Code and Executable Files can be redistributed; and
//	Source Code can be modified to create derivative works.
//	No claim of suitability, guarantee, or any warranty whatsoever is
//	provided. The software is provided "as-is".
//	The Article accompanying the Work may not be distributed or republished
//	without the Author's consent
//
//	Version History:
//
//	Version 1.0 2017/03/01
//		Original revision
//
//	For full history please view SecureSmtpClient.cs
/////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GMailRefreshToken
{
/////////////////////////////////////////////////////////////////////
//	The GMailRefreshToken application creates a refresh token file
//	from the JSON file created by GMail API certification.
/////////////////////////////////////////////////////////////////////
public partial class GMailRefreshToken : Form
	{
	// get autorization URI
	const string AuthrizationUri = "https://accounts.google.com/o/oauth2/auth";

	// get token from authorization code URI
	const string TokenUri = "https://accounts.google.com/o/oauth2/token";

	// authorization scope send email using gmail
	const string Scope = "https://mail.google.com/";

	// token request content type
	const string TokenRequestMethod = "POST";
	const string TokenRequestContentType = "application/x-www-form-urlencoded";
	const string TokenRequestAccept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

	// html telling user to close browser after authorization
	const string ResponseString =
		"<html>\r\n" +
		"<head>\r\n" +
		"<style type=\"text/css\">\r\n" +
		"body {background:#88DDFF;font-family: \"arial\",\"sans-serif\"}\r\n" +
		"p {color:red;font-weight:bold;font-size:120%}\r\n" +
		"</style>\r\n" +
		"</head>\r\n" +
		"<body>\r\n" +
		"<p>Close this browser tab and</p>\r\n" +
		"<p>return to the Email Authorization program.</p>\r\n" +
		"</body>\r\n" +
		"</html>\r\n";

	/////////////////////////////////////////////////////////////////////
	// Constructor
	/////////////////////////////////////////////////////////////////////
	public GMailRefreshToken()
		{
		InitializeComponent();
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Initialization
	/////////////////////////////////////////////////////////////////////
	private void OnLoad(object sender, EventArgs e)
		{
		Text = "GMailRefreshToken Ver 1.0 2017/03/01";
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Browse for GMail API JSON file name
	/////////////////////////////////////////////////////////////////////
	private void OnBrowse(object sender, EventArgs e)
		{
		OpenFileDialog Dialog = new OpenFileDialog();
		Dialog.CheckFileExists = true;
		Dialog.DefaultExt = ".json";
		Dialog.Filter = "Gmail api json files (*.json)|*.json";
		Dialog.InitialDirectory = Environment.CurrentDirectory;
		Dialog.RestoreDirectory = true;
		Dialog.Title = "Select Gmail API client json file";
		if(Dialog.ShowDialog(this) == DialogResult.OK) JsonFileTextBox.Text = Dialog.FileName;
		Dialog.Dispose();
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Request user concent to access gmail account
	/////////////////////////////////////////////////////////////////////
	private void OnCreateToken(object sender, EventArgs e)
		{
		try
			{
			// client ID
			string ClientID;

			// client secret
			string ClientSecret;

			// read gmail api json file
			if(!ReadJsonFile(out ClientID, out ClientSecret)) return;

			// get email address of gmail account
			string EmailAddress = EmailAddressTextBox.Text.Trim();
			if(string.IsNullOrWhiteSpace(EmailAddress) || EmailAddress.IndexOf('@') < 0)
				{
				MessageBox.Show("Email address is blank or invalid");
				return;
				}

			// tell user what will happen next
			if(MessageBox.Show("A request to use: " + EmailAddress + "\r\nfor sending emails will be displayed\r\nin your default internet browser.",
				"EMail Autorization", MessageBoxButtons.OKCancel) != DialogResult.OK) return;

			// anti forgery state random string 48 char long
			string AntiForgeryState = RandomTextString(48);

			// Creates a redirect URI using an available port on the loopback address.
			string RedirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());
			string EscRedirectUri = System.Uri.EscapeDataString(RedirectURI);

			// Creates an HttpListener to listen for requests on that redirect URI.
			HttpListener RedirectHttp = new HttpListener();
			RedirectHttp.Prefixes.Add(RedirectURI);
			RedirectHttp.Start();

			// Creates the OAuth 2.0 authorization request.
			string AuthorizationRequest = string.Format(
				"{0}" +
				"?client_id={1}" +
				"&response_type=code" +
				"&scope={2}" +
				"&redirect_uri={3}" +
				"&state={4}" +
				"&login_hint={5}",
				AuthrizationUri,
				ClientID,
				Scope,
				EscRedirectUri,
				AntiForgeryState,
				EmailAddress);

			// Opens request in the browser.
			Process.Start(AuthorizationRequest);

			// Waits for the OAuth authorization response.
			HttpListenerContext Context = RedirectHttp.GetContext();

			// Sends an HTTP response to the browser telling the user to close the browser tab
			HttpListenerResponse Response = Context.Response;
			byte[] ResponseBuffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
			Response.ContentLength64 = ResponseBuffer.Length;
			Stream ResponseOutput = Response.OutputStream;
			ResponseOutput.Write(ResponseBuffer, 0, ResponseBuffer.Length);
			ResponseOutput.Close();

			// Checks for errors.
			if(Context.Request.QueryString.Get("error") != null)
				{
				MessageBox.Show(String.Format("Authorization response error: {0}.", Context.Request.QueryString.Get("error")));
				return;
				}

			// make sure we have code and state
			if(Context.Request.QueryString.Get("code") == null || Context.Request.QueryString.Get("state") == null)
				{
				MessageBox.Show("Authorization response must have code and state.\r\n" + Context.Request.QueryString);
				return;
				}

			// Compares the receieved state to the expected value, to ensure that
			// this app made the request which resulted in authorization.
			string StateReceived = Context.Request.QueryString.Get("state");
			if(StateReceived != AntiForgeryState)
				{
				MessageBox.Show(String.Format("Authorization response invalid forgery state.\r\nState sent: {0}\r\nState received: {1}", AntiForgeryState, StateReceived));
				return;
				}

			// extracts the code
			string AuthorizationCode = Context.Request.QueryString.Get("code");

			// builds a request string to get refresh token from access code
			string TokenRequestStr = string.Format(
				"code={0}" +
				"&redirect_uri={1}" +
				"&client_id={2}" +
				"&client_secret={3}" + 
				"&scope={4}" +
				"&grant_type=authorization_code",
				AuthorizationCode,
				EscRedirectUri,
				ClientID,
				ClientSecret,
				Scope
				);
			// convert token request string to byte array
			byte[] TokenRequestBin = Encoding.ASCII.GetBytes(TokenRequestStr);
		
			// get refresh token from code
			// sends the request
			HttpWebRequest tokenRequest = (HttpWebRequest) WebRequest.Create(TokenUri);
			tokenRequest.Method = TokenRequestMethod;
			tokenRequest.ContentType = TokenRequestContentType;
			tokenRequest.Accept = TokenRequestAccept;
			tokenRequest.ContentLength = TokenRequestBin.Length;
			Stream stream = tokenRequest.GetRequestStream();
			stream.Write(TokenRequestBin, 0, TokenRequestBin.Length);
			stream.Close();

			// refresh token
			string RefreshToken;

			// gets the response
			WebResponse tokenResponse = tokenRequest.GetResponse();
			using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
				{
				// reads response body
				string ResponseText = reader.ReadToEnd();

				// extract refresh token from response
				RefreshToken = GetResponseValue(ResponseText, "refresh_token");
				}

			// tell user to save token;
			SaveFileDialog Dialog = new SaveFileDialog();
			Dialog.DefaultExt = ".xml";
			Dialog.FileName = "SecureSmtpRefreshToken.xml";
			Dialog.Filter = "GMail token state file (*.xml)|*.xml";
			Dialog.InitialDirectory = Environment.CurrentDirectory;
			Dialog.RestoreDirectory = true;
			Dialog.Title = "Save refresh token file";
			Dialog.AddExtension = true;
			Dialog.CheckPathExists = true;
			Dialog.CreatePrompt = false;
			Dialog.OverwritePrompt = true;
			string SaveFileName = null;
			if(Dialog.ShowDialog(this) == DialogResult.OK) SaveFileName = Dialog.FileName;
			Dialog.Dispose();
			if(SaveFileName == null) return;

			// save results
			UziSecureSmtpClient.SecureSmtpRefreshToken State = new UziSecureSmtpClient.SecureSmtpRefreshToken();
			State.ClientID = ClientID;
			State.ClientSecret = ClientSecret;
			State.RefreshToken = RefreshToken;
			State.SaveState(SaveFileName);
			return;
			}

		catch (WebException ex)
			{
			MessageBox.Show("Token Response exception message:\r\n" + ex.Message);
			return;
			}
		}

		////////////////////////////////////////////////////////////////////
		// Get available port number
		////////////////////////////////////////////////////////////////////

		private static int GetRandomUnusedPort()
		{
		// port number is set to zero
		// the system will set available port number between 1024 and 5000
		// See TcpListener Constructor (IPAddress, Int32) remarks section
		// ref https://msdn.microsoft.com/en-us/library/c6z86e63(v=vs.110).aspx
		TcpListener Listener = new TcpListener(IPAddress.Loopback, 0);
		Listener.Start();
		int port = ((IPEndPoint) Listener.LocalEndpoint).Port;
		Listener.Stop();
		return port;
		}

	////////////////////////////////////////////////////////////////////
	// Byte array to base64 url and no padding
	////////////////////////////////////////////////////////////////////

	private static string RandomTextString
			(
			int Length
			)
		{
		const string Xlate = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

		// fill random byte array
		byte[] ByteArray = RandomByteArray(Length);

		// output character array
		char[] CharArray = new char[Length];
		for(int Index = 0; Index < Length; Index++) CharArray[Index] = Xlate[ByteArray[Index] & 0x3f];
		return new string(CharArray);
		}

	////////////////////////////////////////////////////////////////////
	// Create random byte array
	////////////////////////////////////////////////////////////////////

	private static Byte[] RandomByteArray
			(
			int	Length
			)
		{
		Byte[] ByteArray = new Byte[Length];
		using(RNGCryptoServiceProvider RandNumGen = new RNGCryptoServiceProvider())
			{
			RandNumGen.GetBytes(ByteArray);
			}
		return(ByteArray);
		}

	////////////////////////////////////////////////////////////////////
	// Read client json file
	////////////////////////////////////////////////////////////////////

	private bool ReadJsonFile
			(
			out string ClientID,
			out string ClientSecret
			)
		{
		ClientID = null;
		ClientSecret = null;
		string FileName = JsonFileTextBox.Text.Trim();
		if(string.IsNullOrWhiteSpace(FileName))
			{
			MessageBox.Show("Client ID file name is empty");
			return false;
			}
		if(!File.Exists(FileName))
			{
			MessageBox.Show("Client ID file does not exist");
			return false;
			}

		string JsonText;
		using(StreamReader reader = new StreamReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
			{
			// reads response body
			JsonText = reader.ReadToEnd();
			}

		// exit
		ClientID = GetResponseValue(JsonText, "client_id");
		ClientSecret = GetResponseValue(JsonText, "client_secret");
		return true;
		}

	////////////////////////////////////////////////////////////////////
	// get response value.
	////////////////////////////////////////////////////////////////////

	private static string GetResponseValue
			(
			string Response,
			string Key
			)
		{
		int Ptr = Response.IndexOf(Key);
		if(Ptr < 0) throw new ApplicationException("Response value not found (1): " + Key);

		// skip to colon
		Ptr = Response.IndexOf(':', Ptr);
		if(Ptr < 0) throw new ApplicationException("Response value not found (2): " + Key);

		// find start of value
		int Start = Response.IndexOf('"', Ptr + 1);
		if(Start < 0) throw new ApplicationException("Response value not found (3): " + Key);
		Start++;

		// find end of value
		int End = Response.IndexOf('"', Start);
		if(End < 0 || End <= Start) throw new ApplicationException("Response value not found (4): " + Key);

		return Response.Substring(Start, End - Start);
		}
	}
}
