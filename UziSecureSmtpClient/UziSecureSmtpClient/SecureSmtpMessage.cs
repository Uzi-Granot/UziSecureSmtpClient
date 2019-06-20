/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpMessage
//	The SecureSmtpMessage class represent the email message.
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
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace UziSecureSmtpClient
{
/// <summary>
/// Secure Smtp Message body
/// </summary>
/// <remarks>
///	The SecureSmtpMessage class represent the email message.
/// </remarks>
public class SecureSmtpMessage
	{
	/// <summary>
	/// Message subject line
	/// </summary>
	public string Subject;
	/// <summary>
	/// Message from address
	/// </summary>
	public MailAddress From;
	/// <summary>
	/// Message list of to addresses
	/// </summary>
	public List<MailAddress> To;
	/// <summary>
	/// Message list of CC addresses
	/// </summary>
	public List<MailAddress> Cc;
	/// <summary>
	/// Message list of BCC addresses
	/// </summary>
	public List<MailAddress> Bcc;
	/// <summary>
	/// Message root part. It can be content, attachment or multipart boundary.
	/// </summary>
	public SecureSmtpPart RootPart;

	/// <summary>
	/// Message default constructor
	/// </summary>
	public SecureSmtpMessage()
		{
		To = new List<MailAddress>();
		Cc = new List<MailAddress>();
		Bcc = new List<MailAddress>();
		return;
		}
	}
}
