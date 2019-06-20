/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpRefreshToken
//	The SecureSmtpRefreshToken class holds the refresh token info.
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

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace UziSecureSmtpClient
{
/// <summary>
///	The SecureSmtpRefreshToken class holds the refresh token info.
/// </summary>
public class SecureSmtpRefreshToken
	{
	/// <summary>
	/// client ID
	/// </summary>
	public string ClientID;

	/// <summary>
	/// client secret
	/// </summary>
	public string ClientSecret;

	/// <summary>
	/// refresh token
	/// </summary>
	public string RefreshToken;

	/// <summary>
	/// Save Program State
	/// </summary>
	/// <param name="FileName">Refresh token file name.</param>
	public void SaveState
			(
			string FileName
			)
		{
		// create a new program state file
		using(XmlTextWriter TextFile = new XmlTextWriter(FileName, null))
			{ 
			// create xml serializing object
			XmlSerializer XmlFile = new XmlSerializer(typeof(SecureSmtpRefreshToken));

			// serialize the program state
			XmlFile.Serialize(TextFile, this);
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Load Program State
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Load Program State
	/// </summary>
	/// <param name="FileName">Refresh token file name</param>
	/// <returns>Refresh token class</returns>
	public static SecureSmtpRefreshToken LoadState
			(
			string FileName
			)
		{
		// program state file does not exist
		if(!File.Exists(FileName)) return(null);

		XmlTextReader TextFile = null;
		SecureSmtpRefreshToken State = null;
		try
			{
			// read program state file
			TextFile = new XmlTextReader(FileName);

			// create xml serializing object
			XmlSerializer XmlFile = new XmlSerializer(typeof(SecureSmtpRefreshToken));

			// deserialize the program state
			State = (SecureSmtpRefreshToken) XmlFile.Deserialize(TextFile);
			}
		catch
			{
			State = null;
			}

		// close the file
		if(TextFile != null) TextFile.Close();

		// exit
		return State;
		}
	}
}
