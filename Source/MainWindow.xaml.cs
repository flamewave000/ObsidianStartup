using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;

namespace Obsidian_Startup
{
	public partial class MainWindow : Window
	{
		private ObsidianConfig? config;
		public MainWindow()
		{
			config = ObsidianUtil.LoadVaults();
			if (config != null)
			{
				UpdateJumpList(config);
				var args = Environment.GetCommandLineArgs();
				if (args != null && args.Length > 1 && config.vaults.TryGetValue(args[1].Substring(2), out ObsidianVault? jump))
				{
					ObsidianUtil.OpenVault(jump.path);
					Close();
					return;
				}
				else
				{
					var defaultVault = config.vaults.Values.FirstOrDefault(it => it?.open == true, null);
					if (defaultVault != null)
					{
						InitializeComponent();
						OpenVaultButton.Content = GetString("OpenVaultButton", Path.GetFileName(defaultVault.path));
						return;
					}
				}
			}
			ChooseVault(this, new RoutedEventArgs());
		}

		private void CloseDialog(object sender, RoutedEventArgs e) => Close();
		private void ChooseVault(object sender, RoutedEventArgs e)
		{
			if (config != null)
			{
				foreach (var vault in config.vaults)
					vault.Value.open = false;
				ObsidianUtil.SaveVaults(config);
			}
			ObsidianUtil.Launch();
			Close();
		}
		private void OpenVault(object sender, RoutedEventArgs e)
		{
			ObsidianUtil.Launch();
			Close();
		}
		private void DragWindow(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void UpdateJumpList(ObsidianConfig config)
		{
			var jumpList = new JumpList();
			foreach (var vault in config.vaults)
			{
				string vaultName = Path.GetFileName(vault.Value.path);
				jumpList.JumpItems.Add(new JumpTask()
				{
					Title = vaultName,
					Description = GetString("OpenVaultJumpTask", vaultName),
					CustomCategory = GetString("VaultsJumpTaskCategory"),
					Arguments = "--" + vault.Key
				});
			}
			jumpList.JumpItems.Sort((left, right) => (left as JumpTask)!.Title.CompareTo((right as JumpTask)!.Title));
			JumpList.SetJumpList(Application.Current, jumpList);
		}

		private string GetString(string name) => (string)Application.Current.FindResource(name);
		private string GetString(string name, params object?[] args) => string.Format((string)Application.Current.FindResource(name), args);
	}
}
