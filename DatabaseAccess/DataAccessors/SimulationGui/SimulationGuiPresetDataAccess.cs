using DataAccessLibrary.DataTransferObjects.SimulationGuiDTOs;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;
using MongoDB.Driver;

namespace DataAccessLibrary.DataAccessors.SimulationGui
{
    public class SimulationGuiPresetDataAccess : MongoDbDatabaseAccessAbstract<SimulationGuiPresetDto>
    {
        protected override Table Table => Table.SIMULATION_GUI_PRESET;
        public override FilterDefinition<SimulationGuiPresetDto> GetUpsertFilterDefinition(SimulationGuiPresetDto item)
        {
            return Builders<SimulationGuiPresetDto>.Filter.Eq(ParameterNames.MongoDb.cName, item.Name);
        }
    }
}
