using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkTemplateDto : MongoDbDataTransferObject
    {
        protected override int ParametersCount => 7;

        public int[] BiasCount { get; set; }

        public int[] WeightsCount { get; set; }

        public string[] ActivationFunctions { get; set; }

        public int Layers { get; set; }

        public int OutputCount { get; set; }

        public int InputCount { get; set; }

        public int[] LayerSetup { get; set; }
        public string Name { get; set; }

        public override MongoDbCallParameters CreateParameters()
        {
            return base.CreateParameters()
                       .Add(ParameterNames.MongoDb.cName, Name)
                       .Add(ParameterNames.MongoDb.cBiasCount, BiasCount)
                       .Add(ParameterNames.MongoDb.cWeightsCount, WeightsCount)
                       .Add(ParameterNames.MongoDb.cActivationFunctions, ActivationFunctions)
                       .Add(ParameterNames.MongoDb.cLayers, Layers)
                       .Add(ParameterNames.MongoDb.cOutputCount, OutputCount)
                       .Add(ParameterNames.MongoDb.cInputCount, InputCount)
                       .Add(ParameterNames.MongoDb.cLayerSetup, LayerSetup);
        }
    }
}