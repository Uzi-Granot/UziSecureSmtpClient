/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpAttachment
//	The SecureSmtpAttachment class is a child class of the
//	SecureSmtpMessage class. It holds an email attachement.
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
/// Email attachment type
/// </summary>
public enum AttachmentType
	{
	/// <summary>
	/// Attachment file
	/// </summary>
	Attachment,
	/// <summary>
	/// Inline attachment. i.e. image for html content
	/// </summary>
	Inline,
	}

/// <summary>
///	Email attachement
/// </summary>
/// <remarks>
///	The SecureSmtpAttachment class is a child class of the
///	SecureSmtpMessage class. It holds an email attachement.
/// </remarks>
public class SecureSmtpAttachment : SecureSmtpPart
	{
	/// <summary>
	/// Content ID for linking attachment to html element
	/// </summary>
	public string ContentID;
	/// <summary>
	/// Attachment name
	/// </summary>
	public string Name;
	/// <summary>
	/// Attachement file name
	/// </summary>
	public string FileName;
	/// <summary>
	/// Medial type string i.e. text/html
	/// </summary>
	public string MediaType;
	/// <summary>
	/// Transfer encoding i.e. Base64 or QuotedPrintable
	/// </summary>
	public TransferEncoding ContentTransferEncoding;

	private AttachmentType Type;
	private Stream AttachmentStream;

	/// <summary>
	/// Attachment from disk file
	/// </summary>
	/// <param name="Type">Type of attachment</param>
	/// <param name="AttachmentFileName">Attachment file name</param>
	/// <param name="ContentID">Html content id</param>
	public SecureSmtpAttachment
			(
			AttachmentType Type,
			string AttachmentFileName,
			string ContentID = null
			)
		{
		// load contents from stream
		AttachmentStream = new FileStream(AttachmentFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		int Ptr = AttachmentFileName.LastIndexOf('\\');
		ConstructorHelper(Type, AttachmentFileName.Substring(Ptr + 1), ContentID);
		return;
		}

	/// <summary>
	/// Attachment from byte array
	/// </summary>
	/// <param name="Type">Attachment type</param>
	/// <param name="ByteArray">Byte array</param>
	/// <param name="Name">Attachment name</param>
	/// <param name="ContentID">Html content id</param>
	public SecureSmtpAttachment
			(
			AttachmentType Type,
			byte[] ByteArray,
			string Name,
			string ContentID = null
			)
		{
		AttachmentStream = new MemoryStream(ByteArray);
		ConstructorHelper(Type, Name, ContentID);
		return;
		}

	/// <summary>
	/// Attachment from stream
	/// </summary>
	/// <param name="Type">Attachment type</param>
	/// <param name="AttachmentStream">Attachment stream</param>
	/// <param name="Name">Attachment name</param>
	/// <param name="ContentID">Html content id</param>
	public SecureSmtpAttachment
			(
			AttachmentType Type,
			Stream AttachmentStream,
			string Name,
			string ContentID = null
			)
		{
		this.AttachmentStream = AttachmentStream;
		ConstructorHelper(Type, Name, ContentID);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor helper
	////////////////////////////////////////////////////////////////////
	private void ConstructorHelper
			(
			AttachmentType Type,
			string Name,
			string ContentID
			)
		{
		if(string.IsNullOrWhiteSpace(Name)) throw new ApplicationException("Attachment name is empty.");
		if(Type == AttachmentType.Inline && string.IsNullOrWhiteSpace(ContentID)) throw new ApplicationException("Content ID is empty.");
		this.Type = Type;
		this.Name = Name;
		FileName = Name;
		this.ContentID = ContentID;
		MediaType = "application/octet";
		ContentTransferEncoding = TransferEncoding.Base64;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send data to server
	////////////////////////////////////////////////////////////////////
	internal override void SendData
			(
			StreamWriter Writer
			)
		{
		this.Writer = Writer;

		// all valid application subtypes as per IANA organization
		// http://www.iana.org/assignments/media-types/media-types.xhtml

		if(string.IsNullOrWhiteSpace(Name))
			{
			if(string.IsNullOrWhiteSpace(FileName)) throw new ApplicationException("Both Name and FileName are empty.");
			Name = FileName;
			}
		else if(string.IsNullOrWhiteSpace(FileName)) FileName = Name;

		// send content-type header
		SendDataLine(string.Format("content-type: {0}; name={1}", MediaType, Name));

		// send content-transfer-encoding header
		SendDataLine(string.Format("content-transfer-encoding: {0}", TransferEncodingText(ContentTransferEncoding)));

		// send content-disposition header
		SendDataLine(string.Format("content-disposition: {0}; filename={1}", Type == AttachmentType.Inline ? "inline" : "attachment", FileName));

		// send content-transfer-encoding header
		if(Type == AttachmentType.Inline)
			{
			SendDataLine(string.Format("content-id: <{0}>", ContentID));
			}

		// send extra CRLF
		SendEndOfLine();

		// content stream to base64
		const int BufLen = 57 * 1024;
		byte[] Buf = new byte[BufLen];
		using (BinaryReader Reader = new BinaryReader(AttachmentStream))
			{
			for(;;)
				{
				// read first buffer
				Buf = Reader.ReadBytes(BufLen);
				if(Buf.Length == 0) break;
				string StrBuf = Convert.ToBase64String(Buf, Base64FormattingOptions.InsertLineBreaks);
				SendDataBlock(StrBuf);
				}
			}

		// send plain text view
		return;
		}
	}
}
