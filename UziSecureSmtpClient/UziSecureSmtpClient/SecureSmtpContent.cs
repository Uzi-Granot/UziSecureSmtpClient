/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpContent
//	The SecureSmtpCcontent class is a child class of the
//	SecureSmtpMessage class. It holds the content plain text part,
//	or the html part.
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
using System.Net.Mime;
using System.Text;

namespace UziSecureSmtpClient
{
/// <summary>
/// Content type (plain or html)
/// </summary>
public enum ContentType
	{
	/// <summary>
	/// Plain text content
	/// </summary>
	Plain,
	/// <summary>
	/// HTML content
	/// </summary>
	Html,
	}

/// <summary>
/// Content part
/// </summary>
/// <remarks>
///	The SecureSmtpCcontent class is a child class of the
///	SecureSmtpMessage class. It holds the content plain text part,
///	or the html part.
/// </remarks>
public class SecureSmtpContent : SecureSmtpPart
	{
	/// <summary>
	/// Character set (override default utf-7)
	/// </summary>
	public string CharSet;
	/// <summary>
	/// Media type (override default  "text/html" or "text/plain")
	/// </summary>
	public string MediaType;
	/// <summary>
	/// Content transfer encoding (override QuotedPrintable)
	/// </summary>
	public TransferEncoding ContentTransferEncoding;

	private ContentType Type;
	private string ContentString;

	/// <summary>
	/// Email content as a string
	/// </summary>
	/// <param name="Type">Content media type</param>
	/// <param name="ContentString">Content string</param>
	public SecureSmtpContent
			(
			ContentType Type,
			string ContentString
			)
		{
		this.ContentString = ContentString;
		ConstructorHelper(Type);
		return;
		}

	/// <summary>
	/// Email content as a stream
	/// </summary>
	/// <param name="Type">Content media type</param>
	/// <param name="ContentStream">Content stream</param>
	public SecureSmtpContent
			(
			ContentType Type,
			Stream ContentStream
			)
		{
		// read content string from the stream
		using (StreamReader Reader = new StreamReader(ContentStream))
			{
			ContentString = Reader.ReadToEnd();
			}
		ConstructorHelper(Type);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor helper
	////////////////////////////////////////////////////////////////////
	private void ConstructorHelper
			(
			ContentType Type
			)
		{
		if(string.IsNullOrWhiteSpace(ContentString)) throw new ApplicationException("Content string is empty.");
		this.Type = Type;
		MediaType = Type == ContentType.Html ? "text/html" : "text/plain";
		CharSet = "utf-7";
		ContentTransferEncoding = TransferEncoding.QuotedPrintable;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send content to output stream
	////////////////////////////////////////////////////////////////////
	internal override void SendData
			(
			StreamWriter Writer
			)
		{
		// save output stream
		this.Writer = Writer;

		// send content-type header
		SendDataLine(string.Format("content-type: {0}; charset={1}{2}", MediaType, CharSet, Type == ContentType.Html ? "" : "; format=flowed"));

		// send content-transfer-encoding header
		SendDataLine(string.Format("content-transfer-encoding: {0}", TransferEncodingText(ContentTransferEncoding)));

		// send extra CRLF
		SendEndOfLine();

		// convert content string unicode to byte array
		byte[] ContentBytes = Encoding.UTF8.GetBytes(ContentString);
		char[] CharBuffer = null;
		string EncodedText = null;
		switch(ContentTransferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
				EncodedText = QuotedPrintableEncode(ContentBytes, true);
				break;

			case TransferEncoding.Base64:
				EncodedText = Convert.ToBase64String(ContentBytes, Base64FormattingOptions.InsertLineBreaks);
				break;

			case TransferEncoding.SevenBit:
				CharBuffer = new char[ContentBytes.Length];
				for(int Index = 0; Index < ContentBytes.Length; Index++)
					{
					if((ContentBytes[Index] & 0x80) != 0 || ContentBytes[Index] == 0) throw new ApplicationException("Seven bit transfer encoding failed.");
					CharBuffer[Index] = (char) ContentBytes[Index];
					}
				EncodedText = new string(CharBuffer);
				break;

			case TransferEncoding.EightBit:
				CharBuffer = new char[ContentBytes.Length];
				for(int Index = 0; Index < ContentBytes.Length; Index++)
					{
					if(ContentBytes[Index] == 0) throw new ApplicationException("Eight bit transfer encoding failed.");
					CharBuffer[Index] = (char) ContentBytes[Index];
					}
				EncodedText = new string(CharBuffer);
				break;
			}

		// send plain text view
		SendDataBlock(EncodedText);
		return;
		}
	}
}
