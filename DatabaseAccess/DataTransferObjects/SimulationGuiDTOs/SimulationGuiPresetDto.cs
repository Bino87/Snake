using System;
using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects.SimulationGuiDTOs
{
    public class SimulationGuiPresetDto : MongoDbDataTransferObject
    {
        protected override int ParametersCount => 8;

        public string Name { get; set; }
        public string Technique { get; set; }
        public double MutationChance { get; set; }
        public double MutationRate { get; set; }
        public int NumberOfPairs { get; set; }
        public int MapSize { get; set; }
        public bool RunInBackGround { get; set; }
        public int NumberOfIterations { get; set; }

        public override MongoDbCallParameters CreateParameters()
        {
            return base.CreateParameters()
                    .Add(ParameterNames.MongoDb.cName, Name)
                    .Add(ParameterNames.MongoDb.cMutationTechnique, Technique)
                    .Add(ParameterNames.MongoDb.cMutationChance, MutationChance)
                    .Add(ParameterNames.MongoDb.cMutationRate, MutationRate)
                    .Add(ParameterNames.MongoDb.cNumberOfPairs, NumberOfPairs)
                    .Add(ParameterNames.MongoDb.cNumberOfIterations, NumberOfIterations)
                    .Add(ParameterNames.MongoDb.cMapSize, MapSize)
                    .Add(ParameterNames.MongoDb.cRunInBackground, RunInBackGround);
        }
    }
}
