using System;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models
{


    public class SimulationGuiViewModel : Observable, ISimulationStateParameters
    {
        private int _generation;
        private int _moves;
        private int _points;
        private MutationTechnique _mutationTechnique;
        private double _mutationChance = .03;
        private double _mutationRate = .005;
        private int _updateDelay = 25;
        private int _numberOfPairs = 1000;
        private int _mapSize = 11;
        private bool _runInBackground = true;
        private int _currentIndividual;

        public RelayCommand Run { get; set; }

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
                if(SetField(ref _numberOfPairs, value))
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

        public static MutationTechnique[] MutationTechniques => Enum.GetValues<MutationTechnique>();

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

        public SimulationGuiViewModel(Action startSim)
        {
            Run = new RelayCommand(startSim);
        }

    }
}
