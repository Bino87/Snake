using System;
using UserControls.Core.Commands.Base;
using UserControls.Models.SimulationRunner;

namespace UserControls.Core.Commands.MainViewCommands.SimulationRunnerCommands
{
    public class StopSimulationCommand : RelayCommand
    {
        
        public StopSimulationCommand(Action execute, SimulationRunnerViewModel simRunner) : base(execute, () => simRunner.IsBusy)
        {
        }
    }
}