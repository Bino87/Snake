using System;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models
{
    

    public class SimulationGuiViewModel : Observable, ISimulationInitParameters
    {
        private int _generation;
        private (int current, int total) _individual = (1,20);
        private int _moves;
        private int _points;
        private MutationTechnique _mutationTechnique;
        private double _mutationChance = .2;
        private double _mutationRate = .05;
        private int _updateDelay = 100;
        private int _numberOfPairs;
        private int _mapSize;
        private bool _runInBackground = true;
        private int _maxMoves = 100;

        public RelayCommand Run { get; set; }

        public int MaxMoves
        {
            get => _maxMoves;
            set => SetField(ref _maxMoves, value);
        }

        public int MapSize
        {
            get => _mapSize;
            set => SetField(ref _mapSize, value);
        }

        public int NumberOfPairs
        {
            get => _numberOfPairs;
            set => SetField(ref _numberOfPairs, value);
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

        public (int Current, int Total) Individual
        {
            get => _individual;
            set => SetField(ref _individual, value);
        }

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

        public SimulationGuiViewModel(Action startSim)
        {
            Run = new RelayCommand(startSim);
        }

        public void SetParameters(ISimulationState state)
        {
            Generation = state.Generation;
            //Individual = state.Individual;
            Moves = state.Moves;
            Points = state.Points;
        }

    }
}
