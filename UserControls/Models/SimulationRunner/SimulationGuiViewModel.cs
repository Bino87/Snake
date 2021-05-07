using System;
using System.Collections.ObjectModel;
using Commons.Extensions;
using DataAccessLibrary.DataAccessors.SimulationGui;
using DataAccessLibrary.DataTransferObjects.SimulationGuiDTOs;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Core.Objects.SimulationGui;

namespace UserControls.Models.SimulationRunner
{
    public class SimulationGuiViewModel : Observable, ISimulationStateParameters
    {
        private SimulationGuiPreset _preset;
        private int _generation;
        private int _moves;
        private int _points;
        private MutationTechnique _mutationTechnique;
        private double _mutationChance = .03;
        private double _mutationRate = .005;
        public string _name;
        private int _updateDelay = 25;
        private int _numberOfPairs = 1000;
        private int _mapSize = 11;
        private bool _runInBackground = true;
        private int _currentIndividual;
        private int _numberOfIterations = 10;

        public RelayCommand Run { get; set; }
        public RelayCommand Save { get; set; }
        public RelayCommand Delete { get; set; }

        public int NumberOfIterations
        {
            get => _numberOfIterations;
            set => SetField(ref _numberOfIterations, value.Clamp(1, 20));
        }

        public int MaxMoves => MapSize * MapSize;

        public int MapSize
        {
            get => _mapSize;
            set => SetField(ref _mapSize, value);
        }

        public int CurrentIndividual
        {
            get => _currentIndividual;
            set
            {
                if (SetField(ref _currentIndividual, value))
                {
                    OnPropertyChanged(nameof(Individual));
                }
            }
        }

        public int NumberOfPairs
        {
            get => _numberOfPairs;
            set
            {
                if (SetField(ref _numberOfPairs, value))
                    OnPropertyChanged(nameof(Individual));
            }
        }

        public int UpdateDelay
        {
            get => _updateDelay;
            set => SetField(ref _updateDelay, value);
        }

        public bool RunInBackground
        {
            get => _runInBackground;
            set => SetField(ref _runInBackground, value);
        }

        public int Generation
        {
            get => _generation;
            set => SetField(ref _generation, value);
        }

        public (int Current, int Total) Individual => (_currentIndividual, _numberOfPairs * 2);

        public int Moves
        {
            get => _moves;
            set => SetField(ref _moves, value);
        }

        public int Points
        {
            get => _points;
            set => SetField(ref _points, value);
        }


        public ObservableCollection<SimulationGuiPreset> Presets
        {
            get;
            set;
        }

        public SimulationGuiPreset Preset
        {
            get => _preset;
            set
            {

                if (SetField(ref _preset, value) && value.IsNotNull())
                {
                    Name = value.Name;
                    MapSize = value.MapSize;
                    MutationTechnique = value.Technique;
                    MutationChance = value.MutationChance;
                    MutationRate = value.MutationRate;
                    NumberOfPairs = value.NumberOfPairs;
                    NumberOfIterations = value.NumberOfIterations;
                    RunInBackground = value.RunInBackGround;
                }
            }
        }
        public MutationTechnique[] MutationTechniques => Enum.GetValues<MutationTechnique>();

        public MutationTechnique MutationTechnique
        {
            get => _mutationTechnique;
            set => SetField(ref _mutationTechnique, value);
        }

        public double MutationChance
        {
            get => _mutationChance;
            set => SetField(ref _mutationChance, value);
        }

        public double MutationRate
        {
            get => _mutationRate;
            set => SetField(ref _mutationRate, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private void SavePreset()
        {
            SimulationGuiPreset preset = new()
            {
                Name = Name,
                MapSize = MapSize,
                MutationChance = MutationChance,
                MutationRate = MutationRate,
                NumberOfPairs = NumberOfPairs,
                NumberOfIterations = NumberOfIterations,
                RunInBackGround = RunInBackground,
                Technique = MutationTechnique,
            };

            preset.Id = preset.Save();

            Presets.Add(preset);
        }

        private void DeletePreset()
        {
            SimulationGuiPreset preset = new()
            {
                Id = Preset.Id,
            };

            preset.Delete();
            Presets.Remove(Preset);
        }

        public SimulationGuiViewModel(Action startSim)
        {

            SimulationGuiPresetDataAccess presetDataAccess = new();
            SimulationGuiPresetDto[] all = presetDataAccess.GetAll();

            Presets = new ObservableCollection<SimulationGuiPreset>();

            foreach (SimulationGuiPresetDto presetDto in all)
            {
                Presets.Add(new SimulationGuiPreset()
                {
                    Technique = presetDto.Technique.Parse<MutationTechnique>(),
                    MutationRate = presetDto.MutationRate,
                    RunInBackGround = presetDto.RunInBackGround,
                    Name = presetDto.Name,
                    MutationChance = presetDto.MutationChance,
                    NumberOfPairs = presetDto.NumberOfPairs,
                    NumberOfIterations = presetDto.NumberOfIterations,
                    MapSize = presetDto.MapSize,
                    Id = presetDto.Id
                });
            }

            Run = new RelayCommand(startSim);
            Save = new RelayCommand(SavePreset, () => !Name.IsNullOrWhiteSpace());
            Delete = new RelayCommand(DeletePreset, () => Preset.IsNotNull());
        }

    }
}
