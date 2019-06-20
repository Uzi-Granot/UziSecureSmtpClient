/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	SecureSmtpClient
//	The SecureSmtpClient class handles the communication
//	between your application and the mail server.
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
//	Version 2.0 2019/06/20
//		Original revision
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UziSecureSmtpClient
{
/// <summary>
/// OAuth2 interface
/// </summary>
public interface IOAuth2
	{
	/// <summary>
	/// Get OAuth2 password
	/// </summary>
	/// <returns>OAuth2 password</returns>
	string OAuth2Password();
	}

/// <summary>
/// Connection method
/// </summary>
public enum ConnectMethod
	{
	/// <summary>
	/// Unsecure (not using SSL stream)
	/// </summary>
	Unsecure,
	/// <summary>
	/// Secure (using SSL stream)
	/// </summary>
	Secure,
	}

/// <summary>
/// Class for handling an SMTP connection.
/// </summary>
/// <remarks>
///	The SecureSmtpClient class handles the communication
///	between your application and the mail server.
/// </remarks>
public class SecureSmtpClient : IDisposable
	{
	// Authentication method
	private enum Authentication
		{
		PlainText,
		OAuth2,
		} 

	// List of SMTP reply codes used by this program
	private enum SmtpReplyCode
		{
		IgnoreReply = 0,
		Ready = 220,
		ClosingChannel = 221,
		AuthenticationSuccessful = 235,
		RequestCompleted = 250,
		SendUserAndPassword = 334,
		StartInput = 354,
		}
	private enum AuthenticationMethod
		{
		None = 0,
		PlainText = 1,
		XOAuth2 = 2,
		}

	// constructor properties
	private Authentication AuthMethod;
	private ConnectMethod ConnMethod;
	private string Host;
	private int Port = 465;
	private string UserName;
	private string UserPassword;
	private int TimeoutMS = 20000;
	private IOAuth2 OAuth2;

	// properties
	private List<string> ReplyStrings;
	private string Certificate;
	private string PolicyErrors;
	private TcpClient Socket;
	private StreamReader Reader;
	private StreamWriter Writer;
	private AuthenticationMethod SupportedAuth;

	/// <summary>
	/// Secure SMTP Client constructor for plain text authorization
	/// </summary>
	/// <param name="ConnMethod">Connect method (secure/Unsecure)</param>
	/// <param name="Host">Email server host name</param>
	/// <param name="UserName">User name (host login id)</param>
	/// <param name="UserPassword">User password (host login password)</param>
	public SecureSmtpClient
			(
			ConnectMethod ConnMethod,
			string Host,
			string UserName,
			string UserPassword
			)
		{
		// test all required values
		if(string.IsNullOrWhiteSpace(Host)) throw new ApplicationException("Host name is missing");
		if(string.IsNullOrWhiteSpace(UserName)) throw new ApplicationException("User name is missing");
		if(string.IsNullOrWhiteSpace(UserPassword)) throw new ApplicationException("User password is missing");

		// save arguments
		this.ConnMethod = ConnMethod;
		this.Host = Host;
		this.UserName = UserName;
		this.UserPassword = UserPassword;
		AuthMethod = Authentication.PlainText;
		if(ConnMethod == ConnectMethod.Unsecure) Port = 587;
		return;
		}

	/// <summary>
	/// Secure SMTP Client constructor for OAuth2 authorization
	/// </summary>
	/// <param name="Host">Email server host name</param>
	/// <param name="UserName">User name (host login id)</param>
	/// <param name="OAuth2">Class implementing IOAuth2 interface</param>
	public SecureSmtpClient
			(
			string Host,
			string UserName,
			IOAuth2 OAuth2 
			)
		{
		// test all required values
		if(string.IsNullOrWhiteSpace(Host)) throw new ApplicationException("Host name is missing");
		if(string.IsNullOrWhiteSpace(UserName)) throw new ApplicationException("User name is missing");
		if(OAuth2 == null) throw new ApplicationException("Authorization value is missing");

		// save arguments
		this.Host = Host;
		this.UserName = UserName;
		this.OAuth2 = OAuth2;
		AuthMethod = Authentication.OAuth2;
		ConnMethod = ConnectMethod.Secure;
		return;
		}

	/// <summary>
	/// Host port number (override default value)
	/// </summary>
	public int PortNo
		{
		set
			{
			if(value <= 0 || value >= 65536) throw new ApplicationException("Invalid port number. Normally SSL=465, NonSSL=587");
			Port = value;
			return;
			}
		}

	/// <summary>
	/// Connection timeout (override default value)
	/// </summary>
	public int Timeout
		{
		set
			{
			if(value < 5 || value > 600) throw new ApplicationException("Invalid timeout (5-600 seconds)");
			TimeoutMS = 1000 * value;
			return;
			}
		}

	/// <summary>
	/// Send the email message to the mail server
	/// </summary>
	/// <param name="Message">Email message</param>
	public void SendMail
			(
			SecureSmtpMessage Message
			)
		{
		try
			{
			if(string.IsNullOrWhiteSpace(Message.Subject)) throw new ApplicationException("Email subject is missing");
			if(Message.From == null || string.IsNullOrWhiteSpace(Message.From.Address)) throw new ApplicationException("From address is missing or in error");
			if(Message.To.Count == 0) throw new ApplicationException("At least one mail to address is required");
			foreach(MailAddress MA in Message.To)
				{
				if(string.IsNullOrWhiteSpace(MA.Address)) throw new ApplicationException("To mail address address is missing");
				}
			foreach(MailAddress MA in Message.Cc)
				{
				if(string.IsNullOrWhiteSpace(MA.Address)) throw new ApplicationException("CC mail address address is missing");
				}
			foreach(MailAddress MA in Message.Bcc)
				{
				if(string.IsNullOrWhiteSpace(MA.Address)) throw new ApplicationException("To mail address address is missing");
				}

			// open connection
			OpenConnection();

			// send host name and expect request completed 250
			SendCommand("EHLO " + Host, SmtpReplyCode.RequestCompleted);

			// get supported authorization from last reply
			SupportedAuthorizations();

			// plain text authentication method
			if(AuthMethod == Authentication.PlainText)
				{
				if((SupportedAuth & AuthenticationMethod.PlainText) == 0) throw new ApplicationException("Email server does not support plain text authorization");
				SendCommand("AUTH LOGIN", SmtpReplyCode.SendUserAndPassword);
				SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName)), SmtpReplyCode.SendUserAndPassword);
				SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(UserPassword)), SmtpReplyCode.AuthenticationSuccessful);
				}

			// OAuth2 authentication method
			else
				{
				if((SupportedAuth & AuthenticationMethod.XOAuth2) == 0) throw new ApplicationException("Email server does not support XOAUTH2 authorization");
				string OAuth2Password = OAuth2.OAuth2Password();
				string AuthStr = string.Format("user={0}\u0001auth=Bearer {1}\u0001\u0001", UserName, OAuth2Password);
				byte[] AuthBin = new byte[AuthStr.Length];
				for(int x = 0; x < AuthStr.Length; x++) AuthBin[x] = (byte) AuthStr[x];
				SendCommand("AUTH XOAUTH2 " + Convert.ToBase64String(AuthBin), SmtpReplyCode.AuthenticationSuccessful);
				}

			// from address
			SendCommand(string.Format("MAIL FROM: <{0}>", Message.From.Address), SmtpReplyCode.RequestCompleted);
                 
			// send the addresses of all recipients
			SendRecipientAddress(Message.To);
			SendRecipientAddress(Message.Cc);
			SendRecipientAddress(Message.Bcc);

			// start data block
			SendCommand("DATA", SmtpReplyCode.StartInput);

			// send from
			SendDataLine(string.Format("From: {0} <{1}>", Message.From.DisplayName, Message.From.Address));

			// send to list
			SendAddressTo("To", Message.To);
			SendAddressTo("Cc", Message.Cc);

			// send subject
			SendSubjectLine(Message.Subject);

			// send date and time
			SendDataLine(string.Format("Date: {0:r}", DateTime.UtcNow));

			// send mime vertion
			SendDataLine("MIME-Version: 1.0");

			// send all data parts
			Message.RootPart.SendData(Writer);

			// data terminating dot
			SendCommand("\r\n.", SmtpReplyCode.RequestCompleted);

			// terminate connection
			SendCommand("QUIT", SmtpReplyCode.ClosingChannel);

			// dispose connection
			Dispose();
			return;
			}

		catch (Exception Exp)
			{
			try
				{
				SendCommand("QUIT", SmtpReplyCode.IgnoreReply);
				}
			catch {}

			// dispose connection
			Dispose();
			throw new Exception(Exp.Message, Exp);
			}
		}

	// Open connection to host on port.
	private void OpenConnection()
		{
		ReplyStrings = new List<string>();
		Socket = new TcpClient();

		// connect to mail server
		Socket.SendTimeout = TimeoutMS;
		Socket.ReceiveTimeout = TimeoutMS;
		Socket.Connect(Host, Port);

		if(ConnMethod == ConnectMethod.Secure)
			{
			// create SSL stream
			SslStream sslStream = new SslStream(Socket.GetStream(), true, ServerValidationCallback, ClientCertificateSelectionCallback, EncryptionPolicy.AllowNoEncryption);

			// autheticate client
			sslStream.AuthenticateAsClient(Host);

			// create read and write streams
			Writer = new StreamWriter(sslStream, Encoding.ASCII);
			Reader = new StreamReader(sslStream, Encoding.ASCII);
			}
		else
			{
			NetworkStream NetStream = Socket.GetStream();
			Writer = new StreamWriter(NetStream, Encoding.ASCII);
			Reader = new StreamReader(NetStream, Encoding.ASCII);
			}

		// Code 220 means that service is up and working
		SmtpReplyCode ReplyCode = GetReply();
		if(ReplyCode != SmtpReplyCode.Ready)
			{
			#if DEBUG
			Debug.WriteLine(string.Format("Open connection to {0} failed. Reply code {1}", Host, (int) ReplyCode));
			#endif

			throw new ApplicationException("Connect to mail server failed");
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send Recipient  Address
	////////////////////////////////////////////////////////////////////
	private void SendRecipientAddress
			(
			List<MailAddress> AddressList
			)
		{
		// send one address at a time
		foreach(MailAddress MA in AddressList)
			{
			SendCommand(string.Format("RCPT TO: <{0}>", MA.Address), SmtpReplyCode.RequestCompleted);
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send Address To
	////////////////////////////////////////////////////////////////////
	private void SendAddressTo
			(
			string Type,
			List<MailAddress> AddressList
			)
		{
		// nothing to do
		if(AddressList.Count == 0) return;

		// build list
		StringBuilder List = new StringBuilder();

		// send one address at a time
		foreach(MailAddress MA in AddressList)
			{
			if(List.Length == 0)
				{
				// send first item of the list
				List.AppendFormat("{0}: {1} <{2}>", Type, MA.DisplayName, MA.Address);
				}
			else
				{
				// append all others
				List.AppendFormat(",\r\n {0} <{1}>", MA.DisplayName, MA.Address);
				}
			}

		// send the list
		SendDataLine(List.ToString());
		return;
		}

	////////////////////////////////////////////////////////////////////
	// read supported authorization available from reply
	////////////////////////////////////////////////////////////////////
	internal void SupportedAuthorizations()
		{
		SupportedAuth = AuthenticationMethod.None;
		for(int Index = 0; Index < ReplyStrings.Count; Index++)
			{
			string ReplyString = ReplyStrings[Index];

			// reply string starts with AUTH
			if(string.Compare(ReplyString, 4, "AUTH ", 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
				{
				// remove the AUTH text including the following character 
				// to ensure that split only gets the modules supported
				string[] AuthTypes = ReplyString.Substring(9).Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				foreach(string AuthType in AuthTypes)
					{
					if(string.Compare(AuthType, "PLAIN", StringComparison.OrdinalIgnoreCase) == 0) SupportedAuth |= AuthenticationMethod.PlainText;
					else if (string.Compare(AuthType, "XOAUTH2", StringComparison.OrdinalIgnoreCase) == 0) SupportedAuth |= AuthenticationMethod.XOAuth2;
					}
				}
			}
		return;
		}

	private bool ServerValidationCallback
			(
			object Sender,
			X509Certificate Certificate,
			X509Chain Chain,
			SslPolicyErrors PolicyErrors
			)
		{
		// save for debugging
		this.Certificate = Certificate.ToString();
		this.PolicyErrors = PolicyErrors.ToString();

		// certificate is accepted
		return true;
		}

	private X509Certificate ClientCertificateSelectionCallback
			(
			object sender,
			string targethost,
			X509CertificateCollection localcertificates,
			X509Certificate remotecertificate,
			string[] acceptableissuers
			)
		{
		// ignore
		return null;
		}

	////////////////////////////////////////////////////////////////////
	// Send command to the server.
	////////////////////////////////////////////////////////////////////
	private void SendCommand
			(
			string Command,
			SmtpReplyCode ExpectedReply
			)
		{
		#if DEBUG
		Debug.WriteLine(Command);
		#endif

		// send command
		Writer.Write(Command);
		Writer.Write("\r\n");
		Writer.Flush();

		// test reply
		if(ExpectedReply != SmtpReplyCode.IgnoreReply)
			{
			SmtpReplyCode ReplyCode = GetReply();
			if(ReplyCode != ExpectedReply)
				{
				#if DEBUG
				Debug.WriteLine(string.Format("Send command {0} failed. Reply code {1}", Command, (int) ReplyCode));
				#endif
				throw new ApplicationException("Command error: " + Command);
				}
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send a line of text to the server.
	////////////////////////////////////////////////////////////////////
	private void SendDataLine
			(
			string Text
			)
		{
		#if DEBUG
		Debug.WriteLine(Text);
		#endif

		// send command
		Writer.Write(Text);
		Writer.Write("\r\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send subject line to the server.
	////////////////////////////////////////////////////////////////////
	/*
		To encode a header using this technique you must use this format:
		=?charset?encoding?encoded-text?=
		And this is an example of its use:
		=?utf-8?Q?hello?=
		The encoding must be either B or Q, these mean base 64 and quoted-printable respectively.
		You can read the RFC 1342 document for more information about how they work.

		// convert content string unicode to byte array
		byte[] ContentBytes = Encoding.UTF8.GetBytes(ContentString);
		string EncodedText = "=?utf-8?Q?" + QuotedPrintableEncode(ContentBytes) + "?=";
	*/

	private void SendSubjectLine
			(
			string Text
			)
		{
		#if DEBUG
		Debug.WriteLine("Subject: " + Text);
		#endif

		// test for unicode characters (not ascii)
		bool Unicode = false;
		foreach(char Chr in Text)
			{
			if(Chr > '~')
				{
				Unicode = true;
				break;
				}
			}

		// we need special encoding
		if(Unicode)
			{
			// convert content string unicode to byte array
			byte[] SubjectBytes = Encoding.UTF8.GetBytes(Text);
			string SubjectText = SecureSmtpPart.QuotedPrintableEncode(SubjectBytes, false);

			// send subject
			Writer.Write("Subject: =?utf-8?Q?");
			Writer.Write(SubjectText);
			Writer.Write("?=\r\n");
			}
		else
			{
			// send subject
			Writer.Write("Subject: ");
			Writer.Write(Text);
			Writer.Write("\r\n");
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Send end of line to the server.
	////////////////////////////////////////////////////////////////////
	private void SendEndOfLine()
		{
		// note: WriteLine is not used to make sure eol is CRLF
		Writer.Write("\r\n");

		// flush
		Writer.Flush();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Get the reply message from the server.
	////////////////////////////////////////////////////////////////////
	private SmtpReplyCode GetReply()
		{
		// reset reply
		ReplyStrings.Clear();

		// get reply
		string Str = Reader.ReadLine();

		// end of file
		if(Str == null) throw new ApplicationException("Response from server: timeout");

		// save reply
		ReplyStrings.Add(Str);

		// reply must be at least 4 char long
		if(Str.Length < 4) throw new ApplicationException("Response from server is too short");

		// read more lines if required
		while(Str[3] == '-')
			{
			Str = Reader.ReadLine();
			if(Str == null) break;
			ReplyStrings.Add(Str);
			}

		int Code;
		if(!int.TryParse(ReplyStrings[0].Substring(0, 3), out Code) || Code <= 0) Code = -1;
		return (SmtpReplyCode) Code;
		}

	/// <summary>
	/// Close SecureSmtpClient object
	/// </summary>
	public void Dispose()
		{
		if(Reader != null)
			{
			Reader.Close();
			Reader = null;
			}
		if(Writer != null)
			{
			Writer.Flush();
			Writer.Close();
			Writer = null;
			}
		if(Socket != null)
			{
			Socket.Close();
			Socket = null;
			}
		return;
		}
	}
}
