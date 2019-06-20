/////////////////////////////////////////////////////////////////////
//
//	UziSecureSmtpClient
//	Secure SMTP Client .NET Class Library developed in c#.
//
//	TestParam
//	Save recently used test parameters.
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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TestSecureSmtpClient
{
//	TestParam
//	Save recently used test parameters.
public class TestParam : IComparable<TestParam>
	{
	public int Type;
	public string Host;
	public int Port;
	public string UserName;
	public string UserPassword;
	public string RefreshToken;
	public int Timeout;
	public string FromName;
	public string FromAddress;
	public string ToName;
	public string ToAddress;

	////////////////////////////////////////////////////////////////////
	// constructor
	////////////////////////////////////////////////////////////////////
	public TestParam()
		{
		Type = 0;
		Host = string.Empty;
		Port = 465;
		UserName = string.Empty;
		UserPassword = string.Empty;
		RefreshToken = string.Empty;
		Timeout = 20;
		FromName = string.Empty;
		FromAddress = string.Empty;
		ToName = string.Empty;
		ToAddress = string.Empty;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// CompareTo implemetation of IComparable
	////////////////////////////////////////////////////////////////////
	public int CompareTo
			(
			TestParam Other
			)
		{
		if(this.Type != Other.Type) return this.Type - Other.Type;
		int Cmp = string.CompareOrdinal(this.Host, Other.Host);
		if(Cmp != 0) return Cmp;
		if(this.Port != Other.Port) return this.Port - Other.Port;
		Cmp = string.CompareOrdinal(this.UserName, Other.UserName);
		if(Cmp != 0) return Cmp;
		Cmp = string.CompareOrdinal(this.UserPassword, Other.UserPassword);
		if(Cmp != 0) return Cmp;
		Cmp = string.CompareOrdinal(this.RefreshToken, Other.RefreshToken);
		if(Cmp != 0) return Cmp;
		if(this.Timeout != Other.Timeout) return this.Timeout - Other.Timeout;
		Cmp = string.CompareOrdinal(this.FromName, Other.FromName);
		if(Cmp != 0) return Cmp;
		Cmp = string.CompareOrdinal(this.FromAddress, Other.FromAddress);
		if(Cmp != 0) return Cmp;
		Cmp = string.CompareOrdinal(this.ToName, Other.ToName);
		if(Cmp != 0) return Cmp;
		return string.CompareOrdinal(this.ToAddress, Other.ToAddress);
		}

	////////////////////////////////////////////////////////////////////
	// Save Program State
	////////////////////////////////////////////////////////////////////
	public static void SaveTestParam
			(
			TestParam[] TestParamArray,
			string FileName
			)
		{
		// create a new program state file
		using(XmlTextWriter TextFile = new XmlTextWriter(FileName, null))
			{ 
			// create xml serializing object
			XmlSerializer XmlFile = new XmlSerializer(typeof(TestParam[]));

			// serialize the program state
			XmlFile.Serialize(TextFile, TestParamArray);
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Load Program State
	////////////////////////////////////////////////////////////////////

	public static TestParam[] LoadTestParam
			(
			string FileName
			)
		{
		// program state file does not exist
		if(!File.Exists(FileName)) return(null);

		XmlTextReader TextFile = null;
		TestParam[] TestParamArray = null;
		try
			{
			// read program state file
			TextFile = new XmlTextReader(FileName);

			// create xml serializing object
			XmlSerializer XmlFile = new XmlSerializer(typeof(TestParam[]));

			// deserialize the program state
			TestParamArray = (TestParam[]) XmlFile.Deserialize(TextFile);
			}
		catch
			{
			TestParamArray = null;
			}

		// close the file
		if(TextFile != null) TextFile.Close();

		// exit
		return TestParamArray;
		}
	}
}
