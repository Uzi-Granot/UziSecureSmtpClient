/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpPart
//	The SecureSmtpPart class is a base class for message parts
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
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace UziSecureSmtpClient
{
/// <summary>
/// Secure SMTP Part base class for message parts
/// </summary>
public class SecureSmtpPart
	{
	/// <summary>
	/// Stream writer
	/// </summary>
	protected StreamWriter Writer;

	/// <summary>
	/// Hex conversion
	/// </summary>
	protected static char[] Hex = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

	/// <summary>
	/// Default constructor
	/// </summary>
	protected SecureSmtpPart() {}

	////////////////////////////////////////////////////////////////////
	// Send data
	////////////////////////////////////////////////////////////////////
	internal virtual void SendData
			(
			StreamWriter Writer
			)
		{
		throw new ApplicationException("Unimplemented SendData");
		}

	/// <summary>
	/// Send data line
	/// </summary>
	/// <param name="Text">Text to send</param>
	protected void SendDataLine
			(
			string Text
			)
		{
		#if DEBUG
		Debug.WriteLine(Text);
		#endif

		Writer.Write(Text);
		Writer.Write("\r\n");
		return;
		}

	/// <summary>
	/// Send end of line
	/// </summary>
	protected void SendEndOfLine()
		{
		Writer.Write("\r\n");
		return;
		}

	/// <summary>
	/// Send data block
	/// </summary>
	/// <param name="Text">Text block</param>
	protected void SendDataBlock
			(
			string Text
			)
		{
		Writer.Write(Text);
		Writer.Write("\r\n");
		return;
		}

	/// <summary>
	/// Transfer encoding to text
	/// </summary>
	/// <param name="TransEncoding">Transfer encoding code</param>
	/// <returns>Transfer encoding text</returns>
	protected string TransferEncodingText
			(
			TransferEncoding TransEncoding
			)
		{
		switch(TransEncoding)
			{
			case TransferEncoding.QuotedPrintable: return "quoted-printable";
			case TransferEncoding.Base64: return "base64";
			case TransferEncoding.SevenBit: return "7bit";
			case TransferEncoding.EightBit: return "8bit";
			}
		#if DEBUG
		Debug.WriteLine("Unknown transfer encoding.");
		#endif
		throw new ApplicationException("Unknown transfer encoding.");
		}

	/// <summary>
	/// Quoted Printable Encoding
	/// </summary>
	/// <param name="Input">Byte array input</param>
	/// <returns>String output</returns>
	internal static string QuotedPrintableEncode
			(
			byte[] Input,
			bool FinalEol
			)
		{
		// input is empty
		if(Input == null || Input.Length == 0) return null;

		// create output string
		StringBuilder Str = new StringBuilder();

		// maximum line length
		const int MaxLineLen = 76;

		// convert string to quoted printable
		int InputLen = Input.Length;
		int Start = 0;
		for(int Index = 0; Index < InputLen; Index++)
			{
			// get next character from input line
			int Chr = Input[Index];

			// printable ascii character except = and space or tab
			if(Chr >= ' ' && Chr < '=' || Chr > '=' && Chr <= '~' || Chr == '\t')
				{
				// line length is less than max allowed 76
				if(Str.Length - Start < MaxLineLen - 1)
					{
					// append character
					Str.Append((char) Chr);
					continue;
					}

				// line length is at max-1 (75) and there is no end of line at the input string
				// add soft end of line and append character
				if(InputLen - Index < 3 || Input[Index + 1] != '\r' || Input[Index + 2] != '\n')
					{
					// add soft end of line
					Str.Append("=\r\n");
					Start = Str.Length;
					Str.Append((char) Chr);
					continue;
					}

				// line length is at max-1 (75) and there is hard end of line at the input string
				// last character is not space or tab
				if(Chr != ' ' && Chr != '\t')
					{
					// append character and hard end of line
					Str.Append((char) Chr);
					Str.Append("\r\n");
					}

				// append soft end of line followed by space or tab and hard end of line
				else
					{
					Str.Append(Chr == ' ' ? "=\r\n=20\r\n" : "=\r\n=09\r\n");
					}
				// skip the hard end of line in input stream
				Index += 2;
				Start = Str.Length;
				continue;
				}

			// test for end of line
			if(Chr == '\r' && InputLen - Index > 1 && Input[Index + 1] == '\n')
				{
				// get last character in output string
				int LastChar = Str.Length > 0 ? Str[Str.Length - 1] : 0;

				// last character is not space or tab
				if(LastChar != ' ' && LastChar != '\t')
					{
					Str.Append("\r\n");
					}

				// last character is space or tab and there is enough space for =20 or =09
				else if(Str.Length - Start <= MaxLineLen - 2)
					{
					// remove last space or tab and append soft end of line followed by space and hard end of line
					Str.Length--;
					Str.Append(LastChar == ' ' ? "=20\r\n" : "=09\r\n");
					}

				// not enough room for escaped space or tab
				else
					{
					// remove last space or tab and append soft end of line followed by space and hard end of line
					Str.Length--;
					Str.Append(LastChar == ' ' ? "=\r\n=20\r\n" : "=\r\n=09\r\n");
					}
				Index++;
				Start = Str.Length;
				continue;
				}

			// all other 8 bit characters 
			// not enough room for three characters plus a spare one
			if(Str.Length - Start > MaxLineLen - 4)
				{
				// add soft return
				Str.Append("=\r\n");
				Start = Str.Length;
				}

			// convert to hex formal =XX
			Str.Append('=');
			Str.Append(Hex[Chr >> 4]);
			Str.Append(Hex[Chr & 0x0F]);
			}

		// if last char is not end of line, add soft end of line
		if(FinalEol && Str[Str.Length - 1] != '\n')
			{
			// add soft return
			Str.Append("=\r\n");
			}

		// exit
		return Str.ToString();
		}
	}
}
