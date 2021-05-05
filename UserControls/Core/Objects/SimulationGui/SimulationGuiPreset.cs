using System;
using DataAccessLibrary.DataAccessors.SimulationGui;
using DataAccessLibrary.DataTransferObjects.SimulationGuiDTOs;
using Simulation.Enums;

namespace UserControls.Core.Objects.SimulationGui
{
    public record SimulationGuiPreset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MutationTechnique Technique { get; set; }
        public double MutationChance { get; set; }
        public double MutationRate { get; set; }
        public int NumberOfPairs { get; set; }
        public int MapSize { get; set; }
        public bool RunInBackGround { get; set; }
        public int NumberOfIterations { get; set; }

        public Guid Save()
        {
            SimulationGuiPresetDataAccess dataAccess = new();
            return dataAccess.Insert(GetDto());
        }

        public void Delete()
        {
            SimulationGuiPresetDataAccess dataAccess = new();
            dataAccess.DeleteById(Id);
        }

        private SimulationGuiPresetDto GetDto()
        {
            return new()
            {
                MutationChance = MutationChance,
                MutationRate = MutationRate,
                Technique = Technique.ToString(),
                RunInBackGround = RunInBackGround,
                MapSize = MapSize,
                Name = Name,
                NumberOfIterations = NumberOfIterations,
                NumberOfPairs = NumberOfPairs
            };
        }
    }
}
