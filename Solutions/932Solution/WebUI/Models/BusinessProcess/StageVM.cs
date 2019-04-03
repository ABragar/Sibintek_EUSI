using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Concrete;
using Base.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Base;
using Base.DAL;

namespace WebUI.Models.BusinessProcess
{
    public class StageVM
    {
        public string Name { get; set; }
        public int StageID { get; set; }
        public int StagePerformID { get; set; }
        public IEnumerable<StageAction> Actions { get; set; }
        public User Performer { get; set; }
        public User FromUser { get; set; }
        public ISecurityUser CurrentUser { get; set; }
        public IEnumerable<User> PermittedUsers { get; set; }
        public PerformerType PerformerType { get; set; }
        public int ObjectID { get; set; }
        public string ObjectType { get; set; }
        public string Color { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDateExpected { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public string ElapsedString { get; set; }
        public double ElapsedPercentage { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        private ElapsedStatus? _status;
        public ElapsedStatus? Status
        {
            get
            {
                return _status ??
                       (_status =
                           (ElapsedStatus)
                               Enumerable.Range(0, _ranges.Length).FirstOrDefault(x => _ranges[x] > ElapsedPercentage));
            }
        }



        private static readonly int[] _ranges = { 50, 75, 100, int.MaxValue };


        public StageVM(IUnitOfWork uow, StagePerform stagePerform)
        {
            Actions = stagePerform.Stage.Outputs.Where(x=>!x.Hidden);
            StageID = stagePerform.Stage.ID;
            StagePerformID = stagePerform.ID;
            Name = stagePerform.Stage.Title;
            Color = stagePerform.Stage.Color;
            BeginDate = stagePerform.BeginDate;
            EndDateExpected = stagePerform.BeginDate + TimeSpan.FromMinutes(stagePerform.Stage.PerformancePeriod);
            ElapsedTime = DateTime.Now - stagePerform.BeginDate;
            TimeLeft = TimeSpan.FromMinutes(stagePerform.Stage.PerformancePeriod) - (DateTime.Now - stagePerform.BeginDate);
            ElapsedPercentage = 100 * (DateTime.Now - stagePerform.BeginDate).TotalSeconds / TimeSpan.FromMinutes(stagePerform.Stage.PerformancePeriod).TotalSeconds;
            ElapsedString = ElapsedPercentage.ToString(CultureInfo.GetCultureInfo("en-US"));
            Performer = stagePerform.PerformUser?.Copy(uow);
            FromUser = stagePerform.FromUser?.Copy(uow);
        }

    }

    public enum ElapsedStatus
    {
        Good = 0, Info, Warning, Danger
    }
}