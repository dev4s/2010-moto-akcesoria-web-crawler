using System;
using System.Threading;
using System.Windows.Forms;

namespace MotoAkcesoriaWebCrawler
{
	public partial class MotoAkcesoriaWebCrawler : Form
	{
		private Thread _programThread;

		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new MotoAkcesoriaWebCrawler());
		}

		private MotoAkcesoriaWebCrawler()
		{
			InitializeComponent();
		}

		private void MotoAkcesoriaWebCrawler_Load(object sender, EventArgs e)
		{
			informationLabel.Text = "";
			stopBtn.Enabled = false;
			pauseBtn.Enabled = false;

			listBox.Items.Clear();
		}

		private void startBtn_Click(object sender, EventArgs e)
		{
			if (!CheckTextboxes())
			{
				EnabledAllTextBoxes(false);

				if (Program.ThreadStop)
				{
					Program.ThreadStop = false;
					Program.CloseMySqlConnection();

					informationLabel.Text = "Uruchomiono ponownie: " + DateTime.Now;
				}

				if(_programThread != null && Program.ThreadPause)
				{
					Program.ThreadPause = false;

					startBtn.Enabled = false;
					pauseBtn.Enabled = true;
					stopBtn.Enabled = true;

					informationLabel.Text = "Wznowiono: " + DateTime.Now;
				}
				else if (Program.OpenMySqlConnection(serverTextBox.Text, databaseTextBox.Text, userTextBox.Text, passwordTextBox.Text))
				{
					Program.SetStartAndStopId(startId.Text, stopId.Text);
					progressBar.Minimum = Convert.ToInt32(startId.Text);
					progressBar.Maximum = Convert.ToInt32(stopId.Text);

					_programThread = new Thread(Program.Start);
					_programThread.Start();

					startBtn.Enabled = false;
					pauseBtn.Enabled = true;
					stopBtn.Enabled = true;
				}
				else
				{
					EnabledAllTextBoxes(true);
					Error("Nastąpił błąd przy połączeniu do bazy danych, sprawdź czy wszystkie dane zostały dobrze wprowadzone");
				}
			}
		}

		private void EnabledAllTextBoxes(bool value)
		{
			serverTextBox.Enabled = value;
			databaseTextBox.Enabled = value;
			userTextBox.Enabled = value;
			passwordTextBox.Enabled = value;
			startId.Enabled = value;
			stopId.Enabled = value;
		}

		private bool CheckTextboxes()
		{
			var error = false;

			if (String.IsNullOrEmpty(serverTextBox.Text))
			{
				Error("Pole serwer nie może być puste");
				error = true;
			}
			if(String.IsNullOrEmpty(databaseTextBox.Text))
			{
				Error("Pole bazy danych nie może być puste");
				error = true;
			}
			if(String.IsNullOrEmpty(userTextBox.Text))
			{
				Error("Pole z nazwą użytkownika nie może być puste");
				error = true;
			}
			if (String.IsNullOrEmpty(passwordTextBox.Text))
			{
				Error("Pole z hasłem nie może być puste");
				error = true;
			}

			if (String.IsNullOrEmpty(startId.Text))
			{
				Error("Pole z id startowym nie może być puste");
				error = true;
			}
			try
			{
				if (!String.IsNullOrEmpty(startId.Text))
					Convert.ToInt32(startId.Text);
			}
			catch (Exception)
			{
				Error("Pole z id startowym nie może zawierać innych znaków niż litery");
				error = true;
			}

			if (String.IsNullOrEmpty(stopId.Text))
			{
				Error("Pole z id zakończeniowym nie może być puste");
				error = true;
			}
			try
			{
				if (!String.IsNullOrEmpty(stopId.Text))
					Convert.ToInt32(stopId.Text);
			}
			catch (Exception)
			{
				Error("Pole z id zakończeniowym nie może zawierać innych znaków niż litery");
				error = true;
			}
			return error;
		}

		private static void Error(string errorText)
		{
			MessageBox.Show(errorText, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void stopBtn_Click(object sender, EventArgs e)
		{
			Program.ThreadStop = true;

			EnabledAllTextBoxes(true);

			informationLabel.Text = "Zatrzymano";
			startBtn.Enabled = true;
			pauseBtn.Enabled = false;
			stopBtn.Enabled = false;
		}

		private void pauseBtn_Click(object sender, EventArgs e)
		{
			Program.ThreadPause = true;

			informationLabel.Text = "Wstrzymano";
			startBtn.Enabled = true;
			pauseBtn.Enabled = false;
			stopBtn.Enabled = false;
		}

		private void MotoAkcesoriaWebCrawler_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.ThreadStop = true;

			if (_programThread == null) return;
			while (_programThread.IsAlive)
			{
				_programThread.Abort();
			}
		}

		//delegaty
		private delegate void UpdateProgressBarCallback(int value);
		public void UpdateProgressBar(int value)
		{
			if (InvokeRequired)
			{
				UpdateProgressBarCallback upbc = (UpdateProgressBar);
				Invoke(upbc, new object[] {value});
			}
			else
			{
				progressBar.Value = value;
			}
		}

		private delegate void UpdateListBoxAddCallback(string value);
		public void UpdateListBoxAdd(string value)
		{
			if (InvokeRequired)
			{
				UpdateListBoxAddCallback upbc = (UpdateListBoxAdd);
				Invoke(upbc, new object[] { value });
			}
			else
			{
				listBox.Items.Add(value);
			}
		}

		private delegate void UpdateListBoxSelectedValueCallback();
		public void UpdateListBoxSelectedValue()
		{
			if (InvokeRequired)
			{
				UpdateListBoxSelectedValueCallback upbc = (UpdateListBoxSelectedValue);
				Invoke(upbc);
			}
			else
			{
				listBox.SelectedIndex = listBox.Items.Count - 1;
				listBox.SelectedIndex = -1;
			}
		}
	}
}
