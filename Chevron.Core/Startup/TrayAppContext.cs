using Microsoft.Extensions.DependencyInjection;
using Chevron.Core.Forms;

namespace Chevron.Core.Startup {
  public class TrayAppContext : ApplicationContext {
    private NotifyIcon trayIcon;
    private Form1 form1;
    private readonly IServiceProvider _services;
    public TrayAppContext(IServiceProvider services, Form1 form1Instance) {
      _services = services;
      form1 = form1Instance;

      trayIcon = new NotifyIcon {
        Icon = form1.Icon,
        Visible = true,
        ContextMenuStrip = new ContextMenuStrip()
      };

      trayIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
      {
            new ToolStripMenuItem("Show Settings", null, ShowSettingsMenuClick),
            new ToolStripMenuItem("Exit", null, ExitMenuClick)
      });

      trayIcon.Click += (s, e) => {
        if (e is MouseEventArgs mouseEvent && mouseEvent.Button == MouseButtons.Left) {
          ShowForm();
        }
      };

      // Ensure cleanup on app exit
      Application.ApplicationExit += (s, e) => {
        trayIcon.Visible = false;
        trayIcon.Dispose();
        form1?.Dispose();
      };

      ShowForm();
    }

    private void ShowSettingsMenuClick(object sender, EventArgs e) {
      ShowForm();
    }

    private void ExitMenuClick(object sender, EventArgs e) {
      trayIcon.Visible = false;
      Application.Exit();
    }

    private void ShowForm() {
      if (form1 == null || form1.IsDisposed) {
        form1 = _services.GetRequiredService<Form1>();
      }
      if (!form1.Visible) {
        form1.Show();
      } else {
        form1.Activate(); // Bring to front if already visible
      }
    }
  }
}
