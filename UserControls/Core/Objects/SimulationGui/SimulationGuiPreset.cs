using System;
using DataAccessLibrary.DataAccessors.SimulationGui;
using DataAccessLibrary.DataTransferObjects.SimulationGuiDTOs;
using DataAccessLibrary.Interfaces;
using Simulation.Enums;

namespace UserControls.Core.Objects.SimulationGui
{
    public record SimulationGuiPreset : IToMongoDbDataTransferObject<SimulationGuiPresetDto>
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
            return dataAccess.Insert(ToDataTransferObject());
        }

        public void Delete()
        {
            SimulationGuiPresetDataAccess dataAccess = new();
            dataAccess.DeleteById(Id);
        }


        public SimulationGuiPresetDto ToDataTransferObject()
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
