﻿using AlephNote.Settings;
using System.Windows;

namespace AlephNote.WPF.Windows
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow
	{
		private readonly SettingsWindowViewmodel viewmodel;
		private readonly MainWindowViewmodel ownerVM;

		public SettingsWindow(MainWindowViewmodel owner, AppSettings data)
		{
			InitializeComponent();

			ownerVM = owner;
			viewmodel = new SettingsWindowViewmodel(owner.Owner, data.Clone());
			DataContext = viewmodel;
		}

		private void OnOKClicked(object sender, RoutedEventArgs e)
		{
			if (!viewmodel.Settings.IsEqual(ownerVM.Settings)) ownerVM.ChangeSettings(viewmodel.Settings);
			Close();
		}

		private void OnCancelClicked(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}