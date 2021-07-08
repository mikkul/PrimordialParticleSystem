using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;
using Myra.Graphics2D.UI.Properties;
using PPSMonoGame.PPS;
using System;
using System.IO;

namespace PPSMonoGame.UI
{
	internal class Sidebar : VerticalStackPanel
	{
		private readonly MonoGamePrimordialParticleSystem _pps;
		private readonly WindowManager _windowManager;
		private readonly string _mainContentDirectory;

		private PropertyGrid _settingsPropertyGrid;

		public Sidebar(MonoGamePrimordialParticleSystem pps, WindowManager windowManager, string mainContentDirectory)
		{
			_pps = pps;
			_windowManager = windowManager;
			_mainContentDirectory = mainContentDirectory;

			Spacing = 10;
			Padding = new Thickness(5, 10);

			ConstructGUI();
		}

		public Label FPSCounterLabel { get; set; }
		public Label ParticleCountLabel { get; set; }

		private void ConstructGUI()
		{
			AddCounters();
			this.Widgets.Add(new HorizontalSeparator());
			AddSimulationOptions();
			this.Widgets.Add(new HorizontalSeparator());
			AddWindowSizeCustomization();
			this.Widgets.Add(new HorizontalSeparator());
			AddParticleSpawningOptions();
			this.Widgets.Add(new HorizontalSeparator());
			AddSettingsPropertyGrid();
			this.Widgets.Add(new HorizontalSeparator());
			AddPresets();
		}

		private void AddCounters()
		{
			FPSCounterLabel = new Label();
			ParticleCountLabel = new Label();

			this.Widgets.Add(FPSCounterLabel);
			this.Widgets.Add(ParticleCountLabel);
		}

		private void AddSimulationOptions()
		{
			var pauseResumeSimulationButton = new TextButton
			{
				Text = "Pause",
			};
			pauseResumeSimulationButton.Click += (s, a) =>
			{
				_pps.Settings.IsPaused ^= true;
				pauseResumeSimulationButton.Text = _pps.Settings.IsPaused ? "Resume" : "Pause";
			};

			this.Widgets.Add(pauseResumeSimulationButton);
		}

		private void AddPresets()
		{
			string presetsPath = Path.Combine(_mainContentDirectory, "Presets");

			var saveSettingsPresetNameInput = new LabelledInput
			{
				Text = "Preset name:",
			};

			var saveSettingsButton = new TextButton
			{
				Text = "Save preset",
			};
			saveSettingsButton.Click += (s, a) =>
			{
				string name = saveSettingsPresetNameInput.Value;
				if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
				{
					return;
				}

				_pps.Settings.SaveToFile(Path.Combine(presetsPath, Path.ChangeExtension(name, ".preset")));
			};

			var loadSettingsButton = new TextButton
			{
				Text = "Load settings from file",
			};
			var loadSettingsFileDialog = new FileDialog(FileDialogMode.OpenFile)
			{
				Filter = "*.preset",
				Folder = presetsPath,
			};
			loadSettingsFileDialog.Closed += (s, a) =>
			{
				if (!loadSettingsFileDialog.Result)
				{
					return;
				}

				try
				{
					_pps.Settings = PrimordialParticleSystems.Settings.FromFile<PPSSettings>(loadSettingsFileDialog.FilePath);
					if (_settingsPropertyGrid != null)
					{
						_settingsPropertyGrid.Object = _pps.Settings;
					}
				}
				catch (Exception e)
				{
					System.Diagnostics.Debugger.Break();
				}
			};
			loadSettingsButton.Click += (s, a) =>
			{
				loadSettingsFileDialog.ShowModal(this.Desktop);
			};

			this.Widgets.Add(saveSettingsPresetNameInput);
			this.Widgets.Add(saveSettingsButton);
			this.Widgets.Add(loadSettingsButton);
		}

		private void AddSettingsPropertyGrid()
		{
			_settingsPropertyGrid = new PropertyGrid
			{
				Object = _pps.Settings,
			};

			this.Widgets.Add(_settingsPropertyGrid);
		}

		private void AddParticleSpawningOptions()
		{
			var particleCountInput = new LabelledInput
			{
				Text = "Number of particles:",
			};

			var spawnParticlesButton = new TextButton
			{
				Text = "Spawn",
			};
			spawnParticlesButton.Click += (s, a) =>
			{
				bool isANumber = int.TryParse(particleCountInput.Value, out int amount);
				if (isANumber)
				{
					_pps.Spawn(amount);
				}
			};

			var clearParticlesButton = new TextButton
			{
				Text = "Clear",
			};
			clearParticlesButton.Click += (s, a) =>
			{
				_pps.Clear();
			};

			this.Widgets.Add(particleCountInput);
			this.Widgets.Add(spawnParticlesButton);
			this.Widgets.Add(clearParticlesButton);
		}

		private void AddWindowSizeCustomization()
		{
			var windowWidthInput = new LabelledInput
			{
				Text = "Window width:",
			};

			var windowHeightInput = new LabelledInput
			{
				Text = "Window height:",
			};

			var applyWindowSizeButton = new TextButton
			{
				Text = "Apply",
			};
			applyWindowSizeButton.Click += (s, a) =>
			{
				bool isValidWidth = int.TryParse(windowWidthInput.Value, out int newWidth);
				bool isValidHeight = int.TryParse(windowHeightInput.Value, out int newHeight);

				if (!isValidWidth || !isValidHeight)
				{
					return;
				}

				_windowManager.WindowWidth = newWidth;
				_windowManager.WindowHeight = newHeight;
				_windowManager.ApplyChanges();
			};

			this.Widgets.Add(windowWidthInput);
			this.Widgets.Add(windowHeightInput);
			this.Widgets.Add(applyWindowSizeButton);
		}
	}
}
