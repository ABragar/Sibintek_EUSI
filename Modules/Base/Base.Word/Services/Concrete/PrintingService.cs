using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.UI;
using Base.UI.ViewModal;
using Base.Word.Services.Abstract;
using WordTemplates;
using Base.Utils.Common.Caching;

namespace Base.Word.Services.Concrete
{
    public struct ObjectWithName<T>
    {
        public ObjectWithName(T obj, string name)
        {
            Object = obj;
            Name = name;
        }

        public T Object { get; }
        public string Name { get; }
    }

    public class PrintResult
    {
        public PrintResult(byte[] result, string result_key)
        {
            Result = result;
            ResultKey = result_key;
        }

        public byte[] Result { get; }
        public string Name { get; internal set; }
        public string ResultKey { get; }
    }

    public class PrintingService : IPrintingService
    {
        private readonly IPrintingSettingsService _printing_settings_service;
        private readonly IViewModelConfigService _view_model_config_service;
        private readonly IFileSystemService _file_system_service;
        private readonly IWordService _word_service;
        private readonly ISimpleCacheWrapper _cache;

        public PrintingService(IPrintingSettingsService printing_settings_service,
            IViewModelConfigService view_model_config_service,
            IFileSystemService file_system_service,
            IWordService word_service, ISimpleCacheWrapper cache)
        {
            _printing_settings_service = printing_settings_service;
            _view_model_config_service = view_model_config_service;
            _file_system_service = file_system_service;
            _word_service = word_service;
            _cache = cache;
        }


        private static readonly CacheAccessor<byte[]> TemplateAccessor = new CacheAccessor<byte[]>();
        private async Task<MemoryStream> GetTemplateAsync(Guid template_id)
        {

            var buffer = await _cache.GetOrAddAsync(TemplateAccessor, template_id.ToString(), async () =>
            {
                var path = _file_system_service.GetFilePath(template_id);

                return await Helpers.ReadBytesAsync(path);
            });

            return await Helpers.CopyToMemoryAsync(buffer);
        }


        private async Task<ObjectWithName<Guid>> GetTemplateIdAsync(IUnitOfWork uow, string mnemonic, int? template_settings_id)
        {
            if (mnemonic == null)
                throw new ArgumentNullException(nameof(mnemonic));

            var settings = _printing_settings_service.GetAll(uow).Where(x => x.Mnemonic == mnemonic);

            if (template_settings_id != null)
                settings = settings.Where(x => x.ID == template_settings_id);

            var template = await settings.Select(x => new { x.Template.FileID, x.TemplateName }).FirstOrDefaultAsync();

            if (template == null)
                throw new InvalidOperationException("Template not found for mnemonic {mnemonic}");

            return new ObjectWithName<Guid>(template.FileID, template.TemplateName);
        }



        private Task<TemplateContent> GetContentAsync(IUnitOfWork uow, ViewModelConfig config, int object_id)
        {
            return (Task<TemplateContent>)(GetContentInternalAsyncmMethodInfo.MakeGenericMethod(config.TypeEntity)
                .Invoke(this, new object[] { uow, config, object_id }));

        }

        private static readonly MethodInfo GetContentInternalAsyncmMethodInfo =
            typeof(PrintingService).GetMethod(nameof(GetContentInternalAsync), BindingFlags.NonPublic | BindingFlags.Instance);





        private async Task<TemplateContent> GetContentInternalAsync<T>(IUnitOfWork uow, ViewModelConfig config, int object_id)
            where T : BaseObject
        {
            var service = config.GetService<IBaseObjectService<T>>();

            var obj = await service.GetAll(uow).Where(x => x.ID == object_id).FirstOrDefaultAsync();

            var template_config = config.GetPrintConfig();

            return template_config.GetContent(new TemplateConfigContext(uow), obj);
        }

        private string GetResultKey(TemplateContent content, string mnemonic, int object_id, Guid template_id)
        {
            return mnemonic + object_id + template_id + content.GenerateHash();
        }

        private static readonly CacheAccessor<PrintResult> PrintResultAccessor = new CacheAccessor<PrintResult>();
        public async Task<PrintResult> PrintAsync(IUnitOfWork uow, string mnemonic, int id, int? template_settings_id)
        {
            var config = _view_model_config_service.Get(mnemonic);

            var content = await GetContentAsync(uow, config, id);

            var template_id_with_name = await GetTemplateIdAsync(uow, mnemonic, template_settings_id);

            var result_key = GetResultKey(content, mnemonic, id, template_id_with_name.Object);

            var result = await _cache.GetOrAddAsync(PrintResultAccessor, result_key, async () =>
            {
                using (var template = await GetTemplateAsync(template_id_with_name.Object))
                {

                    
                    _word_service.ProcessDocument(template, content);
                    return new PrintResult(template.ToArray(), result_key);


                }

            });

            TemplateValue title_value;

            var title = content.Values.TryGetValue("title", out title_value) ? title_value.Title : id.ToString();

            var name = $"{title}({template_id_with_name.Name}).docx";

            result.Name = name;

            return result;
        }

        public Template GetTemplateConfig(string mnemonic)
        {
            var config = _view_model_config_service.Get(mnemonic);

            var template_config = config.GetPrintConfig();

            return template_config?.Template ?? new Template();
        }
    }
}