namespace DataAccessLibrary.Internal.ParameterNames
{
    internal static class ParameterNames
    {
        internal static class SQL
        {
            internal const string cId = "ID";
            internal const string cValue = "VALUE";
            internal const string cLayerID = "LAYER_ID";
            internal const string cInternalIndex = "INTERNAL_INDEX";
            internal const string cActivationFunctionId = "ACTIVATION_FUNCTION_ID";
            internal const string cNetworkId = "NETWORK_ID";
            internal const string cNumberOfNodes = "NUMBER_OF_NODES";
            internal const string cDataTable = "DATA_TABLE";
        }

        internal static class MongoDb
        {
            internal const string cId = "Id";
            internal const string cName = "Name";
            internal const string cMutationTechnique = "MutationTechnique";
            internal const string cMutationChance = "MutationChance";
            internal const string cMutationRate = "MutationRate";
            internal const string cNumberOfPairs = "NumberOfPairs";
            internal const string cNumberOfIterations = "NumberOfIterations";
            internal const string cMapSize = "MapSize";
            internal const string cRunInBackground = "RunInBackground";
        }
    }
}
