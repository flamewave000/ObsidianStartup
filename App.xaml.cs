using System;
using System.Threading;
using System.Windows;

namespace Obsidian_Startup
{
	public partial class App : Application
	{
		private static Mutex mutex = new Mutex(false, "Obsidian_Startup");
		private static bool locked = false;
		protected override void OnStartup(StartupEventArgs e)
		{
			if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
				Shutdown();
			else
				locked = true;
			base.OnStartup(e);
		}
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			if (locked)
				mutex.ReleaseMutex();
		}
	}
}
