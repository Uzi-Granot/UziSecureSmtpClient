/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpOAuth2
//	The SecureSmtpOAuth2 class calculates the host login password
//	from the GMail refresh token file.
//
//	Author: Uzi Granot
//	Version: 1.0
//	Date: February 1, 2017
//	Copyright (C) 2016-2019 Uzi Granot. All Rights Reserved
//
//	The UziSecureSmtpClient .NET class library and the TestSecureSmtpClient
//	test and demo application are free software.
//	They are distributed under the Code Project Open License (CPOL 1.02)
//	agreement. The full text of the CPOL is given
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UziSecureSmtpClient
{
/// <summary>
/// Expose OAuth2Password member
/// </summary>
/// <remarks>
///	The SecureSmtpOAuth2 class calculates the host login password
///	from the GMail refresh token file.
/// </remarks>
public class SecureSmtpOAuth2 : IOAuth2
	{
	private SecureSmtpRefreshToken RefreshToken;
	private string AccessToken;
	private DateTime AccessTokenExpireTime;

	// get new token from refreash authorization code
	private const string RefreshTokenUrl = "https://www.googleapis.com/oauth2/v4/token";

	// get token from authorization code URI
	private const string TokenUri = "https://accounts.google.com/o/oauth2/token";

	// authorization scope send email using gmail
	private const string Scope = "https://mail.google.com/";

	/// <summary>
	/// Secure SMTP OAuth2 constructor
	/// </summary>
	/// <param name="RefreshTokenFileName">Refresh token file name</param>
	public SecureSmtpOAuth2
			(
			string RefreshTokenFileName
			)
		{
		// no file name 
		if(string.IsNullOrWhiteSpace(RefreshTokenFileName))
			{
			throw new ApplicationException("Refresh token file name is missing or empty");
			}

		// look for refresh token
		RefreshToken = SecureSmtpRefreshToken.LoadState(RefreshTokenFileName);
		if(RefreshToken == null)
			{
			throw new ApplicationException("SecureSmtpRefreshToken.xml file is missing or invalid");
			}
		return;
		}

	/// <summary>
	/// get OAuth2 password
	/// </summary>
	/// <returns>OAuth2 password</returns>
	public string OAuth2Password()
		{
		// not the first time
		if(AccessToken != null)
			{
			// token did not expire
			if(DateTime.UtcNow < AccessTokenExpireTime) return AccessToken;
			}

		string TokenRequestStr = string.Empty;
		string ResponseText = string.Empty;
		try
			{
			// Creates a redirect URI using an available port on the loopback address.
			string RedirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());
			string EscRedirectUri = Uri.EscapeDataString(RedirectURI);

			// builds the request string
			TokenRequestStr = string.Format(
				"redirect_uri={0}" +
				"&client_id={1}" +
				"&client_secret={2}" + 
				"&refresh_token={3}" +
				"&grant_type=refresh_token",
				EscRedirectUri,
				RefreshToken.ClientID,
				RefreshToken.ClientSecret,
				RefreshToken.RefreshToken
				);

			// convert string to byte array
			byte[] TokenRequestBin = Encoding.ASCII.GetBytes(TokenRequestStr);
		
			// sends the request
			HttpWebRequest tokenRequest = (HttpWebRequest) WebRequest.Create(RefreshTokenUrl);
			tokenRequest.Method = "POST";
			tokenRequest.ContentType = "application/x-www-form-urlencoded";
			tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			tokenRequest.ContentLength = TokenRequestBin.Length;
			Stream stream = tokenRequest.GetRequestStream();
			stream.Write(TokenRequestBin, 0, TokenRequestBin.Length);
			stream.Close();

			// gets the response
			WebResponse tokenResponse = tokenRequest.GetResponse();
			using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
				{
				// reads response body
				ResponseText = reader.ReadToEnd();

				// get access token
				AccessToken = GetResponseValue(ResponseText, "access_token");

				// get expires in value (3600)
				string ExpiresInStr = GetResponseValue(ResponseText, "expires_in");
				int ExpiersIn = int.Parse(ExpiresInStr);

				// set expire time
				AccessTokenExpireTime = DateTime.UtcNow.AddSeconds(ExpiersIn - 300);
				}

			// return password
			return AccessToken;
			}

		catch (Exception exp)
			{
			throw new ApplicationException("Get password exception:\r\nToken request:\r\n" + TokenRequestStr + "\r\nResponse:\r\n" + ResponseText + "Exception message:\r\n" + exp.Message);
			}
		}

	////////////////////////////////////////////////////////////////////
	// Get available port number
	////////////////////////////////////////////////////////////////////
	private static int GetRandomUnusedPort()
		{
		// port number is set to zero
		// the system will set available port number between 1024 and 5000
		// See TcpListener Constructor(IPAddress, Int32) remarks section
		// ref https://msdn.microsoft.com/en-us/library/c6z86e63(v=vs.110).aspx
		TcpListener Listener = new TcpListener(IPAddress.Loopback, 0);
		Listener.Start();
		int port = ((IPEndPoint) Listener.LocalEndpoint).Port;
		Listener.Stop();
		return port;
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
		// look for key
		int Ptr = Response.IndexOf(Key);
		if(Ptr < 0) throw new ApplicationException("Response value not found (1): " + Key);

		// skip to colon
		Ptr = Response.IndexOf(':', Ptr);
		if(Ptr < 0) throw new ApplicationException("Response value not found (2): " + Key);

		// skip space
		for(Ptr++; Ptr < Response.Length && Response[Ptr] == ' '; Ptr++);
		if(Ptr == Response.Length) throw new ApplicationException("Response value not found (3): " + Key);

		// start and end of value
		int Start;
		int End;

		// value is quoted text
		if(Response[Ptr] == '"')
			{
			// find end of value
			Start = Ptr + 1;
			End = Response.IndexOf('"', Start);
			if(End < 0 || End <= Start) throw new ApplicationException("Response value not found (4): " + Key);
			}

		// value is a number
		else
			{
			Start = Ptr;
			for(End = Ptr; End < Response.Length && char.IsDigit(Response[End]); End++);
			}

		// make sure value is not empty
		if(End < 0 || End <= Start) throw new ApplicationException("Response value not found (5): " + Key);

		// exit
		return Response.Substring(Start, End - Start);
		}
	}
}
