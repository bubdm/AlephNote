﻿using AlephNote.PluginInterface;
using MSHC.Math.Encryption;
using MSHC.Util.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AlephNote.Plugins.SimpleNote
{
	public class SimpleNoteConfig : IRemoteStorageConfiguration
	{
		private const string ENCRYPTION_KEY = @"rLPDWseNePtqLjXuYRdAAWjQnJoSjxjp";
		
		private const int ID_USERNAME = 6151;
		private const int ID_PASSWORD = 6152;

		public string Username = string.Empty;
		public string Password = string.Empty;

		public XElement Serialize()
		{
			var data = new object[]
			{
				new XElement("Username", Username),
				new XElement("Password", Encrypt(Password)),
			};

			var r = new XElement("config", data);
			r.SetAttributeValue("plugin", SimpleNotePlugin.Name);
			r.SetAttributeValue("pluginversion", SimpleNotePlugin.Version.ToString());
			return r;
		}

		public void Deserialize(XElement input)
		{
			if (input.Name.LocalName != "config") throw new Exception("LocalName != 'config'");

			Username = XHelper.GetChildValue(input, "Username", string.Empty);
			Password = Decrypt(XHelper.GetChildValue(input, "Password", string.Empty));
		}

		public IEnumerable<DynamicSettingValue> ListProperties()
		{
			yield return DynamicSettingValue.CreateText(ID_USERNAME, "Username", Username);
			yield return DynamicSettingValue.CreatePassword(ID_PASSWORD, "Password", Password);
			yield return DynamicSettingValue.CreateHyperlink("Create Simplenote account", "https://simplenote.com/");
		}

		public void SetProperty(int id, string value)
		{
			if (id == ID_USERNAME) Username = value;
			if (id == ID_PASSWORD) Password = value;
		}

		public bool IsEqual(IRemoteStorageConfiguration iother)
		{
			var other = iother as SimpleNoteConfig;
			if (other == null) return false;

			if (this.Username != other.Username) return false;
			if (this.Password != other.Password) return false;

			return true;
		}

		public IRemoteStorageConfiguration Clone()
		{
			return new SimpleNoteConfig
			{
				Username = this.Username,
				Password = this.Password,
			};
		}

		private string Encrypt(string data)
		{
			if (string.IsNullOrWhiteSpace(data)) return string.Empty;
			return Convert.ToBase64String(AESThenHMAC.SimpleEncryptWithPassword(Encoding.UTF32.GetBytes(data), ENCRYPTION_KEY));
		}

		private string Decrypt(string data)
		{
			if (string.IsNullOrWhiteSpace(data)) return string.Empty;
			return Encoding.UTF32.GetString(AESThenHMAC.SimpleDecryptWithPassword(Convert.FromBase64String(data), ENCRYPTION_KEY));
		}

		public string GetUniqueName()
		{
			return Username;
		}
	}
}