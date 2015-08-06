using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.Core.Services.Impl;
using Xunit;
using Rhino.Mocks;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Services
{

    public class WorkflowFacilitatorTester
    {
        [Fact]
        public void ShouldGetNoValidStateCommandsForWrongUser()
        {
            var facilitator = new WorkflowFacilitator();
            var report = new ExpenseReport();
            var employee = new Employee();
            IStateCommand[] commands = facilitator.GetValidStateCommands(new ExecuteTransitionCommand{Report = report, CurrentUser = employee});

            Assert.Equal(commands.Length, 0);
            
        }


        [Fact]
        public void ShouldReturnAllStateCommandsInCorrectOrder()
        {
            var facilitator = new WorkflowFacilitator();
            IStateCommand[] commands = facilitator.GetAllStateCommands();

            Assert.Equal(commands.Length, 7);

            Assert.IsType<DraftingCommand>(commands[0]);
            Assert.IsType<DraftToSubmittedCommand>(commands[1]);
            Assert.IsType<ApprovedToSubmittedCommand>(commands[2]);
            Assert.IsType<DraftToCancelledCommand>(commands[3]);
            Assert.IsType<ApprovedToCancelledCommand>(commands[4]);
            Assert.IsType<SubmittedToDraftCommand>(commands[5]);
            Assert.IsType<SubmittedToApprovedCommand>(commands[6]);
        }

        [Fact]
        public void ShouldFilterFullListToReturnValidCommands()
        {
            var mocks = new MockRepository();
            var facilitator = mocks.PartialMock<WorkflowFacilitator>();
            var commandsToReturn = new IStateCommand[]
                                       {
                                           new StubbedStateCommand(true), new StubbedStateCommand(true),
                                           new StubbedStateCommand(false)
                                       };
            
            Expect.On(facilitator).Call(facilitator.GetAllStateCommands()).IgnoreArguments().Return(commandsToReturn);
            mocks.ReplayAll();

            IStateCommand[] commands = facilitator.GetValidStateCommands(null);

            mocks.VerifyAll();
            Assert.Equal(commands.Length, 2);
        }

        public class StubbedStateCommand : IStateCommand
        {
            private bool _isValid;

            public StubbedStateCommand(bool isValid)
            {
                _isValid = isValid;
            }

            public bool IsValid(ExecuteTransitionCommand transitionCommand)
            {
                return _isValid;
            }

            public ExecuteTransitionResult Execute(ExecuteTransitionCommand transitionCommand)
            {
                throw new NotImplementedException();
            }

            public string TransitionVerbPresentTense
            {
                get { throw new NotImplementedException(); }
            }

            public bool Matches(string commandName)
            {
                throw new NotImplementedException();
            }

            public ExpenseReportStatus GetBeginStatus()
            {
                throw new NotImplementedException();
            }
        }
    }
}