using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using Microsoft.Msagl.Drawing;
using NLog;
using RestService.Helpers.NetworkGraphSociety;

namespace RestService.Helpers
{
    internal class ZipObject
    {
//        private static SemaphoreSlim semaphore=new SemaphoreSlim(0,1);
        
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        
        private Object lockObject=new object();

        public ZipObject()
        {
            
        }
        public ZipObject(ZipArchive zipArchive)
        {
            
        }
        
        public void CreateEntry(Task<GraphImage> o,ZipArchive zip)
        {
            lock (lockObject)
            {
//                Console.WriteLine($"Create Entry start.Task {o.Id}");
                GraphImage image = o.Result;
                ZipArchiveEntry entry = zip.CreateEntry(image.FileName);
                using (var entryStream = entry.Open())
                {
                    entryStream.Write(image.Image, 0, image.Image.Length);
                }
//                Console.WriteLine($"Create Entry end.Task {o.Id}");
            }
            
        }

        public async Task<MemoryStream> CreateAsync(List<int> ids)
        {
            return await Task<MemoryStream>.Run(() =>
            {
                Task<GraphBuilderOptions> task = Task<GraphBuilderOptions>.Factory.StartNew(() =>
                {
                    SocietyInfo rootSociety = GetSocietyInfo("1");
                    GraphBuilderOptions options = new GraphBuilderOptions()
                    {
                        Name = "test",
                        Height = 700,
                        Width = 500,
                        RootColor = new Color(247, 198, 0),
                        SheetColor = new Color(255, 251, 0),
                        OtherColor = new Color(251, 203, 170),
                        Root = rootSociety.ShortName
                    };
                    return options;
                });

                MemoryStream stream = new MemoryStream();
                using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {

                    Task[] t = new Task[ids.Count];
                    int i = 0;

                    GraphBuilderOptions optionsBase = task.Result;
                    foreach (int id in ids)
                    {
                        t[i] = Task<GraphImage>.Factory.StartNew(o =>
                            {
                                GraphBuilderOptions options = o as GraphBuilderOptions;
                                SocietyInfo sheetSociety = GetSocietyInfo(id);
                                options.Sheet = sheetSociety.ShortName;
                                options.Name = $"{sheetSociety.IDEUP}_{DateTime.Now:yyyy.MM.dd}.jpg";
                                GraphBuilder builder = new GraphBuilder() {Options = options};
                                GraphImageProvider imageProvider = new GraphImageProvider(builder);
                                return imageProvider.GetGraphImageBySocietyId(id,false);
                            }, optionsBase.Clone())
                            .ContinueWith((task1, o) => CreateEntry(task1, (ZipArchive) o), zip);
                        i++;
                    }

                    Task.WaitAll(t);
                }

                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            });
        }
        
        
        private SocietyInfo GetSocietyInfo(int id)
        {
            _log.Debug("Input {{id: {0} }}", id);
            using (var context = new ReportDbContext())
            {
                DbRawSqlQuery<SocietyInfo> rawSqlQuery = context.pGetSocietyInfo(id);
                SocietyInfo first = rawSqlQuery.FirstOrDefault();
                if(first==null)throw new DataException($"Society with id {id} not found");
                return first;
            }
        }
        private SocietyInfo GetSocietyInfo(string ideup)
        {
            _log.Debug("Input {{ideup: {0} }}", ideup);
            using (var context = new ReportDbContext())
            {
                DbRawSqlQuery<SocietyInfo> rawSqlQuery = context.pGetSocietyInfoByIdEUP(ideup);
                SocietyInfo first = rawSqlQuery.FirstOrDefault();
                if(first==null)throw new DataException($"Society with ideup {ideup} not found");
                return first;
            }
        }
    }
}