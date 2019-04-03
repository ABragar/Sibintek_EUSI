using Base;
using CorpProp.Model;

using CorpProp.Model.Import;


namespace CorpProp
{
    /// <summary>
    /// Представляет модель объектов CorpProp.
    /// </summary>
    public static class ModelInitializer
    {
        /// <summary>
        /// Инициализация моделей объектов приложения.
        /// </summary>
        /// <param name="context"></param>
        public static void Init(IInitializerContext context)
        {
            EstateModel.Init(context);
            ImportModel.Init(context);
            SeparLandModel.CreateModelConfig(context);
            CorporateGovernanceModel.Init(context);
            LawModel.Init(context);
            DocumentFlowModel.Init(context);
            NSIModel.Init(context);
            FileModel.Init(context);
            ProjectActivityModel.Init(context);
            SettingsModel.Init(context);
        }        
    }
}
