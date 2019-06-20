/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	TestSecureSmtpClient
//	The TestSecureSmtpClient application is a demo and
//	test program for the SecureSmtpClient library.
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
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;
using UziSecureSmtpClient;

namespace TestSecureSmtpClient
{
/////////////////////////////////////////////////////////////////////
//	TestSecureSmtpClient
//	The TestSecureSmtpClient application is a demo and
//	test program for the SecureSmtpClient library.
/////////////////////////////////////////////////////////////////////
public partial class TestSecureSmtpClient : Form
	{
	private string Subject = "Testing of UziSecureSmtpClient .Net Class Library";
	//private string Subject = "[BC] Tiến Độ";
	//private string Subject = "voué à diriger référendum « Boris »";

	private string PlainTextView =
		"Alternative\r\n\r\n" +
		"The multipart/alternative subtype indicates that each part is an \"alternative\" version of the same (or similar) content, " +
		"each in a different format denoted by its \"Content-Type\" header. The order of the parts is significant. RFC1341 states that: " +
		"In general, user agents that compose multipart/alternative entities should place the body parts in increasing order of preference, " +
		"that is, with the preferred format last.\r\n\r\n" +
		"Systems can then choose the \"best\" representation they are capable of processing; in general, this will be the last part that the " +
		"system can understand, although other factors may affect this.\r\n\r\n" +
		"Since a client is unlikely to want to send a version that is less faithful than the plain text version, this structure places the " +
		"plain text version (if present) first. This makes life easier for users of clients that do not understand multipart messages.\r\n\r\n" +
		"Most commonly, multipart/alternative is used for email with two parts, one plain text (text/plain) and one HTML (text/html). " + 
		"The plain text part provides backwards compatibility while the HTML part allows use of formatting and hyperlinks. Most email clients " +
		"offer a user option to prefer plain text over HTML; this is an example of how local factors may affect how an application chooses " +
		"which \"best\" part of the message to display.\r\n\r\n" +
		"While it is intended that each part of the message represent the same content, the standard does not require this to be enforced in " +
		"any way. At one time, anti-spam filters would only examine the text/plain part of a message, because it is easier to parse than the " +
		"text/html part. But spammers eventually took advantage of this, creating messages with an innocuous-looking text/plain part and " +
		"advertising in the text/html part. Anti-spam software eventually caught up on this trick, penalizing messages with very different " +
		"text in a multipart/alternative message.\r\n\r\n" +
		"Defined in RFC 2046, Section 5.1.4\r\n";

	private string HtmlView =
		"<html>\r\n" +
		"<head>\r\n" +
		"<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">\r\n" +
		"<style type=\"text/css\">\r\n" +
		"h4 {margin-top: 6pt; margin-bottom: 6pt; font-size: 120%; font-weight: bold; color: red}\r\n" +
		"p {margin-top: 6pt; margin-bottom: 6pt;}\r\n" +
		"q {quotes: \"\\00ab\" \"\\00bb\"; font-weight: bold; color: red}\r\n" +
		"img {float:left; border: 1px dotted black; margin: 0px 20px 15px 0px;}\r\n" +
		"</style>\r\n" +
		"</head>\r\n" +
		"<body>\r\n" +
		"<h4>Alternative</h4>\r\n" +
		"<p><img width=\"160\" height=\"160\" style=\"width:1.0in;height:1.0in\" src=\"cid:IMAGE001\">\r\n" +
		"The multipart/alternative subtype indicates that each part is an <q>alternative</q> version of the same (or similar) content, " +
		"each in a different format denoted by its <q>Content-Type</q> header. The order of the parts is significant. RFC1341 states that: " +
		"In general, user agents that compose multipart/alternative entities should place the body parts in increasing order of preference, " +
		"that is, with the preferred format last.</p>\r\n" +
		"<p>Systems can then choose the <q>best</q> representation they are capable of processing; in general, this will be the last part that the " +
		"system can understand, although other factors may affect this.</p>\r\n" +
		"<p>Since a client is unlikely to want to send a version that is less faithful than the plain text version, this structure places the " +
		"plain text version (if present) first. This makes life easier for users of clients that do not understand multipart messages.</p>\r\n" +
		"<p>Most commonly, multipart/alternative is used for email with two parts, one plain text (text/plain) and one HTML (text/html). " + 
		"The plain text part provides backwards compatibility while the HTML part allows use of formatting and hyperlinks. Most email clients " +
		"offer a user option to prefer plain text over HTML; this is an example of how local factors may affect how an application chooses " +
		"which <q>best</q> part of the message to display.</p>\r\n" +
		"<p>While it is intended that each part of the message represent the same content, the standard does not require this to be enforced in " +
		"any way. At one time, anti-spam filters would only examine the text/plain part of a message, because it is easier to parse than the " +
		"text/html part. But spammers eventually took advantage of this, creating messages with an innocuous-looking text/plain part and " +
		"advertising in the text/html part. Anti-spam software eventually caught up on this trick, penalizing messages with very different " +
		"text in a multipart/alternative message.</p>\r\n" +
		"<p>Defined <a href=\"http://tools.ietf.org/html/rfc2046#section-5.1.4\">in RFC 2046, Section 5.1.4</a></p>\r\n" +
		"</body>\r\n" +
		"</html>\r\n";

	private SecureSmtpClient Connection;
	private TestParam LastParam;
	private TestParam[] TestParamArray;
	private string TestParamFileName = "TestSecureSmtpClientParama.xml";
	private int ScrollIndex;

	private SecureSmtpClient ConnectionUnsecurePlain;
	private SecureSmtpClient ConnectionSecureHtml;
	private SecureSmtpClient ConnectionGMail;

	/////////////////////////////////////////////////////////////////////
	// constructor
	/////////////////////////////////////////////////////////////////////
	public TestSecureSmtpClient()
		{
		InitializeComponent();
		}

	/////////////////////////////////////////////////////////////////////
	// initialization
	/////////////////////////////////////////////////////////////////////
	private void OnLoad(object sender, EventArgs e)
		{
		#if DEBUG
		// current directory
		string CurDir = Environment.CurrentDirectory;
		string WorkDir = CurDir.Replace("bin\\Debug", "Work");
		if(WorkDir != CurDir && Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;
		#endif

		Text = "Test Uzi Secure Smtp Client Ver 2.0 2019/06/20";
		TypeComboBox.Items.Add("Secure (SSL/TLS) OAuth2");
		TypeComboBox.Items.Add("Secure (SSL/TLS) Plain Text");
		TypeComboBox.Items.Add("Unsecure Plain Text");
		TestParamArray = TestParam.LoadTestParam(TestParamFileName);
		if(TestParamArray == null)
			{
			NextButton.Enabled = false;
			PreviousButton.Enabled = false;
			SetScreen(new TestParam());
			}
		else
			{
			NextButton.Enabled = true;
			PreviousButton.Enabled = true;
			SetScreen(TestParamArray[0]);
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Send Mail prototype.
	// This method should be used as a prototype for your email
	// sending method.
	/////////////////////////////////////////////////////////////////////
	private void OnSendEmail(object sender, EventArgs e)
		{
		try
			{
			// read screen parameters
			TestParam Param = ReadScreen();

			// create one of three possible connection classes
			// the SecureSmtpClient is re-useable. If you send more than one email,
			// you can reuse the class. It is of real benefit for gmail.
			if(Connection != null && Param.CompareTo(LastParam) != 0) Connection = null;
			if(Connection == null)
				{
				switch(Param.Type)
					{
					case 0:
						Connection = new SecureSmtpClient(Param.Host, Param.UserName, new SecureSmtpOAuth2(Param.RefreshToken));
						break;

					case 1:
						Connection = new SecureSmtpClient(ConnectMethod.Secure, Param.Host, Param.UserName, Param.UserPassword);
						break;

					case 2:
						Connection = new SecureSmtpClient(ConnectMethod.Unsecure, Param.Host, Param.UserName, Param.UserPassword);
						break;
					}
				Connection.PortNo = Param.Port;
				Connection.Timeout = Param.Timeout;
				LastParam = Param;
				}

			// create mail message object
			SecureSmtpMessage Message = new SecureSmtpMessage();

			// Set subject
			Message.Subject = Subject;

			// Set mail from address and display name.
			Message.From = new MailAddress(Param.FromAddress, Param.FromName);

			// Add minimum one or more recipients.
			Message.To.Add(new MailAddress(Param.ToAddress, Param.ToName));

			// create mixed multipart boundary
			SecureSmtpMultipart Mixed = new SecureSmtpMultipart(MultipartType.Mixed);
			Message.RootPart = Mixed;

			// create alternative boundary
			SecureSmtpMultipart Alternative = new SecureSmtpMultipart(MultipartType.Alternative);
			Mixed.AddPart(Alternative);

			// Add plain text mail body contents.
			SecureSmtpContent PlainTextContent = new SecureSmtpContent(ContentType.Plain, PlainTextView);
			Alternative.AddPart(PlainTextContent);

			// create related boundary
			SecureSmtpMultipart Related = new SecureSmtpMultipart(MultipartType.Related);
			Alternative.AddPart(Related);

			// add html mail body content
			SecureSmtpContent HtmlContent = new SecureSmtpContent(ContentType.Html, HtmlView);
			Related.AddPart(HtmlContent);

			// add inline image attachment.
			// NOTE image id is set to IMAGE001 this id must match the html image id in HtmlView text.
			SecureSmtpAttachment ImageAttachment = new SecureSmtpAttachment(AttachmentType.Inline, "EmailImage.png", "IMAGE001");
			ImageAttachment.MediaType = "image/png";
			Related.AddPart(ImageAttachment);
		
			// add file attachment to the email.
			// The recipient of the email will be able to save it as a file.
			SecureSmtpAttachment PdfAttachment = new SecureSmtpAttachment(AttachmentType.Attachment, "rfc2045.pdf");
			Mixed.AddPart(PdfAttachment);

			// send mail
			Connection.SendMail(Message);
			MessageBox.Show("Email was successfully sent");
			}

		// catch exceptions
		catch (Exception Exp)
			{
			MessageBox.Show(Exp.Message);
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// On test (Unused)
	/////////////////////////////////////////////////////////////////////
	private void OnSendEmailAlternate(object sender, EventArgs e)
		{
		TestParam Param = ReadScreen();
		//ExampleUnsecurePlainText(Param.Host, Param.UserName, Param.UserPassword, Param.FromName, Param.FromAddress, Param.ToName, Param.ToAddress, Subject, PlainTextView);
		//ExampleSecureHtml(Param.Host, Param.UserName, Param.UserPassword, Param.FromName, Param.FromAddress, Param.ToName, Param.ToAddress, Subject, HtmlView);
		ExampleGMail(Param.Host, Param.UserName, Param.RefreshToken, Param.FromName, Param.FromAddress, Param.ToName, Param.ToAddress, Subject, HtmlView);
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Example One: Unsecure transmission and plain text password.
	// Message content is plain text.
	// The message is a single part message with no boundaries.
	/////////////////////////////////////////////////////////////////////
	private void ExampleUnsecurePlainText
			(
			string Host,
			string UserName,
			string UserPassword,
			string FromName,
			string FromAddress,
			string ToName,
			string ToAddress,
			string MessageSubject,
			string MessageText
			)
		{
		try
			{
			// create smtp client
			if(ConnectionUnsecurePlain == null) ConnectionUnsecurePlain = new SecureSmtpClient(ConnectMethod.Secure, Host, UserName, UserPassword);

			// create mail message object
			SecureSmtpMessage Message = new SecureSmtpMessage();

			// Set subject
			Message.Subject = MessageSubject;

			// Set mail from address and display name.
			Message.From = new MailAddress(FromAddress, FromName);

			// Add minimum one or more recipients.
			// in addition you can add CC or BCC recipients.
			Message.To.Add(new MailAddress(ToAddress, ToName));

			// Add mail body contents.
			// The plain text is the only content part.
			// Load it to the root part of the message.
			SecureSmtpContent PlainTextContent = new SecureSmtpContent(ContentType.Plain, MessageText);
			Message.RootPart = PlainTextContent;

			// send mail
			ConnectionUnsecurePlain.SendMail(Message);
			MessageBox.Show("Email was successfully sent");
			}

		// catch exceptions
		catch (Exception Exp)
			{
			MessageBox.Show(Exp.Message);
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Example Two: Secure transmission and no password authentication.
	// Message content is HTML.
	// The HTML has an embedded image. The image is sent as an inline
	// attachment. This message is a two parts message. It has two related 
	// parts, the html part and the inline image attachment part.
	// The two parts are separated by SecureSmtpMultipart boundary.
	/////////////////////////////////////////////////////////////////////
	private void ExampleSecureHtml
			(
			string Host,
			string UserName,
			string UserPassword,
			string FromName,
			string FromAddress,
			string ToName,
			string ToAddress,
			string MessageSubject,
			string MessageHtml
			)
		{
		try
			{
			// create smtp client
			if(ConnectionSecureHtml == null) ConnectionSecureHtml = new SecureSmtpClient(ConnectMethod.Secure, Host, UserName, UserPassword);

			// create mail message object
			SecureSmtpMessage Message = new SecureSmtpMessage();

			// Set subject
			Message.Subject = MessageSubject;

			// Set mail from address and display name.
			Message.From = new MailAddress(FromAddress, FromName);

			// Add minimum one or more recipients.
			// in addition you can add CC or BCC recipients.
			Message.To.Add(new MailAddress(ToAddress, ToName));

			// This message has two related parts, the html part and the inline image attachment part.
			// The two parts are separated by SecureSmtpMultipart boundary.
			// the multipart boundary is the root part.
			SecureSmtpMultipart Related = new SecureSmtpMultipart(MultipartType.Related);
			Message.RootPart = Related;

			// The first part is the html content.
			// It is added to array of part content children of the multipart parent.
			SecureSmtpContent HtmlContent = new SecureSmtpContent(ContentType.Html, MessageHtml);
			Related.AddPart(HtmlContent);

			// The second part is the inline image attachement.
			SecureSmtpAttachment ImageAttachment = new SecureSmtpAttachment(AttachmentType.Inline, "EmailImage.png", "IMAGE001");
			ImageAttachment.MediaType = "image/png";
			Related.AddPart(ImageAttachment);

			// send mail
			ConnectionSecureHtml.SendMail(Message);
			MessageBox.Show("Email was successfully sent");
			}

		// catch exceptions
		catch (Exception Exp)
			{
			MessageBox.Show(Exp.Message);
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Example Three: Secure transmission and password authentication.
	// This example is specific to GMail.
	// The message is a multi-part message. At the root level it is a
	// mixed part message with two mixed parts: content and file 
	// attachment.
	// The content part is divided into two alternate parts: HTML content 
	// and plain text content.
	// The HTML part is divided into two related parts: the HTML content
	// and an embedded image. The image is sent as an inline attachment.
	/////////////////////////////////////////////////////////////////////
	private void ExampleGMail
			(
			string Host,
			string UserName,
			string RefreshToken,
			string FromName,
			string FromAddress,
			string ToName,
			string ToAddress,
			string MessageSubject,
			string MessageHtml
			)
		{
		try
			{
			// create smtp client
			if(ConnectionGMail == null) ConnectionGMail = new SecureSmtpClient(Host, UserName, new SecureSmtpOAuth2(RefreshToken));

			// create mail message object
			SecureSmtpMessage Message = new SecureSmtpMessage();

			// Set subject
			Message.Subject = MessageSubject;

			// Set mail from address and display name.
			Message.From = new MailAddress(FromAddress, FromName);

			// Add minimum one or more recipients.
			// in addition you can add CC or BCC recipients.
			Message.To.Add(new MailAddress(ToAddress, ToName));

			// This message is made of two alternative methods for content plus a attachment file.
			// create mixed multipart boundary
			SecureSmtpMultipart Mixed = new SecureSmtpMultipart(MultipartType.Mixed);
			Message.RootPart = Mixed;

			// create alternative boundary
			// there are two alternatives to the content: plain text and HTML.
			// NOTE: The recipient of the message will display the last part it knows how to display.
			// For email program that can handle both HTML and text make sure the HTML part is last.
			SecureSmtpMultipart Alternative = new SecureSmtpMultipart(MultipartType.Alternative);
			Mixed.AddPart(Alternative);

			// Add plain text mail body contents.
			SecureSmtpContent PlainTextContent = new SecureSmtpContent(ContentType.Plain, PlainTextView);
			Alternative.AddPart(PlainTextContent);

			// The HTML alternative has two related parts. The HTML content and an inline image attachement
			// create related boundary
			SecureSmtpMultipart Related = new SecureSmtpMultipart(MultipartType.Related);
			Alternative.AddPart(Related);

			// Add html mail body content.
			SecureSmtpContent HtmlContent = new SecureSmtpContent(ContentType.Html, HtmlView);
			Related.AddPart(HtmlContent);

			// Add inline image attachment.
			// NOTE image id is set to IMAGE001 this id must match the html image id in HtmlView text.
			SecureSmtpAttachment ImageAttachment = new SecureSmtpAttachment(AttachmentType.Inline, "EmailImage.png", "IMAGE001");
			ImageAttachment.MediaType = "image/png";
			Related.AddPart(ImageAttachment);
		
			// Add file attachment to the email.
			// The recipient of the email will be able to save it as a file.
			SecureSmtpAttachment PdfAttachment = new SecureSmtpAttachment(AttachmentType.Attachment, "rfc2045.pdf");
			Mixed.AddPart(PdfAttachment);

			// send mail
			ConnectionGMail.SendMail(Message);
			MessageBox.Show("Email was successfully sent");
			}

		// catch exceptions
		catch (Exception Exp)
			{
			MessageBox.Show(Exp.Message);
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// set screen
	/////////////////////////////////////////////////////////////////////
	private void SetScreen
			(
			TestParam Param
			)
		{
		if(TypeComboBox.SelectedIndex != Param.Type) TypeComboBox.SelectedIndex = Param.Type;
		HostTextBox.Text = Param.Host;
		PortTextBox.Text = Param.Port.ToString();
		UserNameTextBox.Text = Param.UserName;
		if(Param.Type != 0)
			{
			UserPasswordTextBox.Text = Param.UserPassword;
			UserPasswordTextBox.Enabled = true;
			RefreshTokenTextBox.Text = string.Empty;
			RefreshTokenTextBox.Enabled = false;
			BrowseButton.Enabled = false;
			}
		else
			{
			UserPasswordTextBox.Text = string.Empty;
			UserPasswordTextBox.Enabled = false;
			RefreshTokenTextBox.Text = Param.RefreshToken;
			RefreshTokenTextBox.Enabled = true;
			BrowseButton.Enabled = true;
			}
		TimeoutTextBox.Text = Param.Timeout.ToString();
		FromNameTextBox.Text = Param.FromName;
		FromAddressTextBox.Text = Param.FromAddress;
		ToNameTextBox.Text = Param.ToName;
		ToAddressTextBox.Text = Param.ToAddress;
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Type combo box changed
	/////////////////////////////////////////////////////////////////////
	private void OnTypeChanged(object sender, EventArgs e)
		{
		PortTextBox.Text = TypeComboBox.SelectedIndex == 2 ? "587" : "465";
		if(TypeComboBox.SelectedIndex != 0)
			{
			UserPasswordTextBox.Enabled = true;
			RefreshTokenTextBox.Text = string.Empty;
			RefreshTokenTextBox.Enabled = false;
			BrowseButton.Enabled = false;
			}
		else
			{
			UserPasswordTextBox.Text = string.Empty;
			UserPasswordTextBox.Enabled = false;
			RefreshTokenTextBox.Enabled = true;
			BrowseButton.Enabled = true;
			}
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Read user parameters
	/////////////////////////////////////////////////////////////////////
	private TestParam ReadScreen()
		{
		TestParam Param = new TestParam();
		Param.Type = TypeComboBox.SelectedIndex;
		Param.Host = HostTextBox.Text.Trim();
		int.TryParse(PortTextBox.Text.Trim(), out Param.Port);
		Param.UserName = UserNameTextBox.Text.Trim();
		Param.UserPassword = UserPasswordTextBox.Text.Trim();
		Param.RefreshToken = RefreshTokenTextBox.Text.Trim();
		int.TryParse(TimeoutTextBox.Text.Trim(), out Param.Timeout);
		Param.FromName = FromNameTextBox.Text.Trim();
		Param.FromAddress = FromAddressTextBox.Text.Trim();
		Param.ToName = ToNameTextBox.Text.Trim();
		Param.ToAddress = ToAddressTextBox.Text.Trim();

		if(TestParamArray == null)
			{
			TestParamArray = new TestParam[1];
			TestParamArray[0] = Param;
			}
		else
			{
			int Index;
			for(Index = 0; Index < TestParamArray.Length && Param.CompareTo(TestParamArray[Index]) != 0; Index++);
			if(Index == 0) return Param;

			List<TestParam> TestParamList = new List<TestParam>(TestParamArray);
			if(Index < TestParamArray.Length) TestParamList.RemoveAt(Index);
			TestParamList.Insert(0, Param);
			if(TestParamList.Count > 20) TestParamList.RemoveRange(20, TestParamList.Count - 20);
			TestParamArray = TestParamList.ToArray();
			}
		TestParam.SaveTestParam(TestParamArray, TestParamFileName);
		NextButton.Enabled = true;
		PreviousButton.Enabled = true;
		ScrollIndex = 0;
		return Param;
		}

	/////////////////////////////////////////////////////////////////////
	// browse for refresh token file
	/////////////////////////////////////////////////////////////////////
	private void OnBrowse(object sender, EventArgs e)
		{
		OpenFileDialog Dialog = new OpenFileDialog();
		Dialog.CheckFileExists = true;
		Dialog.DefaultExt = ".xml";
		Dialog.Filter = "Refresh token file (*.xml)|*.xml";
		Dialog.InitialDirectory = Environment.CurrentDirectory;
		Dialog.RestoreDirectory = true;
		Dialog.Title = "Select refresh token file";
		if(Dialog.ShowDialog(this) == DialogResult.OK) RefreshTokenTextBox.Text = Dialog.FileName;
		Dialog.Dispose();
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// scroll through recent screens
	/////////////////////////////////////////////////////////////////////
	private void OnNext(object sender, EventArgs e)
		{
		ScrollIndex--;
		if(ScrollIndex < 0) ScrollIndex = 0;
		SetScreen(TestParamArray[ScrollIndex]);
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// scroll through recent screens
	/////////////////////////////////////////////////////////////////////
	private void OnPrevoius(object sender, EventArgs e)
		{
		ScrollIndex++;
		if(ScrollIndex >= TestParamArray.Length) ScrollIndex = TestParamArray.Length - 1;
		SetScreen(TestParamArray[ScrollIndex]);
		return;
		}
	}
}
