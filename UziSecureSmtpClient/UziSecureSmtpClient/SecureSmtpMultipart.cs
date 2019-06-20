/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpMultipart
//	The SecureSmtpMultipart class is a child class of SecureSmtpMessage
//	class. It holds multipart boundary information.
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

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UziSecureSmtpClient
{
// must be in that order
/// <summary>
/// Multipart type
/// </summary>
public enum MultipartType
	{
	/// <summary>
	/// Mixed
	/// </summary>
	Mixed,
	/// <summary>
	/// Alternative
	/// </summary>
	Alternative,
	/// <summary>
	/// Related
	/// </summary>
	Related,
	}

/// <summary>
/// Multipart constructor
/// </summary>
/// <remarks>
///	The SecureSmtpMultipart class is a child class of SecureSmtpMessage
///	class. It holds multipart boundary information.
/// </remarks>
public class SecureSmtpMultipart : SecureSmtpPart
	{
	private MultipartType Type;
	private string Boundary;
	private List<SecureSmtpPart> Children;

	private static string[] MultipartText = {"mixed", "alternative", "related"};
	private static string[] MultipartCode = {"MIX", "ALT", "REL"};

	/// <summary>
	/// Multipart constructor
	/// </summary>
	/// <param name="Type">Multipart type: Mixed, Alternative, Related</param>
	public SecureSmtpMultipart
			(
			MultipartType Type
			)
		{
		this.Type = Type;
		Children = new List<SecureSmtpPart>();
		return;
		}

	/// <summary>
	/// Add child part to list of parts
	/// </summary>
	/// <param name="Part">Derived class from SecureSmtpPart</param>
	public void AddPart
			(
			SecureSmtpPart Part
			)
		{
		Children.Add(Part);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create string with random hex digits
	////////////////////////////////////////////////////////////////////
	private static string RandomString
			(
			int Length
			)
		{
		byte[] ByteArray = new byte[Length];
		using(RNGCryptoServiceProvider RandNumGen = new RNGCryptoServiceProvider())
			{
			RandNumGen.GetBytes(ByteArray);
			}
		StringBuilder Str = new StringBuilder();
		foreach(byte OneByte in ByteArray)
			{
			Str.Append(Hex[OneByte >> 4]);
			Str.Append(Hex[OneByte & 0x0F]);
			}
		return(Str.ToString());
		}

	////////////////////////////////////////////////////////////////////
	// Send multipart headers to output stream
	////////////////////////////////////////////////////////////////////
	internal override void SendData
			(
			StreamWriter Writer
			)
		{
		// save output writer stream
		this.Writer = Writer;

		// no children
		if(Children.Count == 0) return;

		// create boundary
		Boundary = string.Format("--------{0}-{1}", MultipartCode[(int) Type], RandomString(12));

		// send multipart header
		SendDataLine(string.Format("Content-Type: multipart/{0};\r\n boundary=\"{1}\"\r\n\r\n--{1}", MultipartText[(int) Type], Boundary));

		// send children data
		for(int Index = 0; Index < Children.Count; Index++)
			{
			// send data of next child
			Children[Index].SendData(Writer);
			
			// send boundry line or last boundry line
			string BoundaryLine = "\r\n--" + Boundary;
			if(Index == Children.Count - 1) BoundaryLine = BoundaryLine + "--";
			SendDataLine(BoundaryLine);
			}		
		return;
		}
	}
}
