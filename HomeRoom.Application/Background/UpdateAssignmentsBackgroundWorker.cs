using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using HomeRoom.Enumerations;
using HomeRoom.Gradebook;
using HomeRoom.GradeBook;

namespace HomeRoom.Background
{
    public class UpdateAssignmentsBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Assignment> _assignmentRepo;
        public UpdateAssignmentsBackgroundWorker(AbpTimer timer, IRepository<Assignment> assignmentRepo) : base(timer)
        {
            _assignmentRepo = assignmentRepo;
            Timer.Period = 3600000;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var now = Clock.Now;

                var openAssignmens = _assignmentRepo.GetAllList(x => x.StartDate <= now);

                foreach (var assignment in openAssignmens)
                {
                    assignment.Status = AssignmentStatus.Open;
                }

                var closeAssignments = _assignmentRepo.GetAllList(x => x.DueDate <= now);

                foreach (var assignment in closeAssignments)
                {
                    assignment.Status = AssignmentStatus.Closed;
                }

                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
