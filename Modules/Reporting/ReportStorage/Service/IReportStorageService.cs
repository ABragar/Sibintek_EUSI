using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using DAL.EF;
using DAL.Entities;
using ReportStorage.Exceptions;

namespace ReportStorage.Service
{
    public interface IReportStorageService
    {
        void Create(ReportDbContext context, Report obj);

        void Delete(ReportDbContext context, int id);

        void Update(ReportDbContext context, Report obj);

        IQueryable<Report> GetAll(ReportDbContext context);
        Report Get(ReportDbContext context, int id);

        Report Get(ReportDbContext context, Guid guid);
    }

    public class ReportStorageService : IReportStorageService
    {
        protected readonly IPathHelper _pathHelper;

        public ReportStorageService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }

        public void Create(ReportDbContext context, Report obj)
        {
            var created = context.Reports.Add(new Report()
            {
                Name = obj.Name,
                GuidId = obj.GuidId,
                Extension = obj.Extension,
                Code = obj.Code,
                RelativePath = obj.RelativePath,
                Module = obj.Module,
                Params = obj.Params,
                Number = obj.Number,
                ReportType = obj.ReportType,
                Description = obj.Description,
                ReportVersion = obj.ReportVersion,
                ReportPublishCode = obj.ReportPublishCode
            });
            context.SaveChanges();
        }

        public virtual void Delete(ReportDbContext context, int id)
        {
            var report = context.Reports.SingleOrDefault(r => r.ID == id);

            if (report == null)
                throw new ObjectValidationException("Отчет не найден или уже удален.");

            var filePath = Path.Combine(_pathHelper.GetFilesDirectory(), report.GuidId.ToString("N") + report.Extension);

            if (File.Exists(filePath))
                File.Delete(filePath);

            context.Reports.Remove(report);
            context.SaveChanges();
        }

        public virtual void Update(ReportDbContext context, Report obj)
        {
            if (obj == null)
                throw new ObjectValidationException("Отчет не найден.");

            //Report findedReport = context.Reports.Find(obj);
            context.Reports.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IQueryable<Report> GetAll(ReportDbContext context)
        {
            return context.Reports.OrderBy(x => x.Number);
        }

        public Report Get(ReportDbContext context, int id)
        {
            return context.Reports.SingleOrDefault(r => r.ID == id);
        }

        public Report Get(ReportDbContext context, Guid guid)
        {
            return context.Reports.SingleOrDefault(r => r.GuidId == guid);
        }
    }

    public class ReportStorageServiceWithHistory : ReportStorageService
    {
        public ReportStorageServiceWithHistory(IPathHelper pathHelper) : base(pathHelper)
        {
        }
        
        /// <summary>
        /// Пометить запись как удалённую и записать в историю
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        public override void Delete(ReportDbContext context, int id)
        {
            var report = context.Reports.SingleOrDefault(r => r.ID == id);

            if (report == null)
                throw new ObjectValidationException("Отчет не найден или уже удален.");

            ToHistory(context, report);
        }

        protected void ToHistory(ReportDbContext context, Report obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Невозможно переменстить в истрию. Объект не должен быть NULL");

            ReportHistory historyItem = context.ReportHistory.Add(new ReportHistory()
            {
                CreatedDate = DateTime.Now,
                Report = obj
            });

            obj.Hidden = true;

            context.SaveChanges();
        }


//        public override void Update(ReportContext context, Report obj)
//        {
//            if (obj == null)
//                throw new ObjectValidationException("Отчет не найден.");
//
//            Report findedReport = context.Reports.Find(obj);
//            Delete(context, obj.ID);
//            Create(context, obj);
//            context.SaveChanges();
//        }
    }
}