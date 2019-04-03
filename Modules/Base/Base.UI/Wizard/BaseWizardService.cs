using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.ViewModal;

namespace Base.UI.Wizard
{
    public abstract class BaseWizardService<TWizardObject, TObject> : IWizardService<TWizardObject>
        where TWizardObject : DecoratedWizardObject<TObject> where
        TObject : BaseObject, new()
    {
        private readonly IBaseObjectService<TObject> _base_object_service;

        protected BaseWizardService(IBaseObjectService<TObject> baseObjectService, IAccessService accessService)
        {
            _base_object_service = baseObjectService;
            AccessService = accessService;
        }

        #region Private and Protected
        protected IAccessService AccessService { get; }

        protected TWizardObject GetNextState(TWizardObject obj, string step)
        {
            if (obj.PreviousSteps == null) obj.PreviousSteps = new List<string>();

            if (!string.IsNullOrEmpty(obj.Step) && !string.Equals(step, obj.Step, StringComparison.CurrentCultureIgnoreCase))
                obj.PreviousSteps.Add(obj.Step);

            obj.Step = step;

            return obj;
        }

        protected TWizardObject GetPrevState(TWizardObject obj)
        {
            if (obj.PreviousSteps == null || !obj.PreviousSteps.Any()) return obj;

            obj.Step = obj.PreviousSteps.Last();
            obj.PreviousSteps.Remove(obj.PreviousSteps.Last());

            return obj;
        }

        protected int GetStepIndexByName(string stepname, ViewModelConfig config)
        {
            var wDetailView = (WizardDetailView)config.DetailView;

            //TODO: Возможно есть смысл вернуть Exception
            if (wDetailView?.Steps == null || wDetailView.Steps.Count == 0) return -1;

            int index;
            for (index = 0; index < wDetailView.Steps.Count; index++)
            {
                if (string.Equals(wDetailView.Steps[index].Name, stepname, StringComparison.CurrentCultureIgnoreCase))
                    break;
            }

            return index;
        }

        protected string GetStepNameByIndex(int index, ViewModelConfig config)
        {
            var wDetailView = (WizardDetailView)config.DetailView;

            //TODO: Возможно есть смысл вернуть Exception
            if (wDetailView?.Steps == null || wDetailView.Steps.Count == 0 || index >= wDetailView.Steps.Count) return "";

            return wDetailView.Steps[index].Name;
        }

        protected bool StepIsLast(string stepname, ViewModelConfig config)
        {
            var wDetailView = (WizardDetailView)config.DetailView;

            //TODO: Возможно есть смысл вернуть Exception
            if (wDetailView?.Steps == null || wDetailView.Steps.Count == 0) return true;

            return string.Equals(wDetailView.Steps[wDetailView.Steps.Count - 1].Name, stepname,
                StringComparison.CurrentCultureIgnoreCase);
        }

        protected bool StepIsFirst(string stepname, ViewModelConfig config)
        {
            var wDetailView = (WizardDetailView)config.DetailView;

            //TODO: Возможно есть смысл вернуть Exception
            if (wDetailView?.Steps == null || wDetailView.Steps.Count == 0) return true;

            return string.Equals(wDetailView.Steps[0].Name, stepname,
                StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion


        public async Task<TWizardObject> NextStepAsync(IUnitOfWork unitOfWork, TWizardObject obj, ViewModelConfig config)
        {
            var currentStepName = obj.Step;

            await OnBeforeStepChangeAsync(unitOfWork, config, obj);

            var wDetailView = (WizardDetailView)config.DetailView;

            if (wDetailView == null) throw new Exception("Ошибка конфигурации мастера");

            var currentIndex = GetStepIndexByName(currentStepName, config);
            obj.Index = currentIndex + 1;

            if (currentIndex < wDetailView.Steps.Count - 1)
            {
                var nextStepName = GetStepNameByIndex(currentIndex + 1, config);
                InitStep(unitOfWork, config, nextStepName, obj);
            }
            else
            {
                GetNextState(obj, WizardConfig.WIZARD_COMPLETE_KEY);
            }


            await OnAfterStepChangeAsync(unitOfWork, config, currentStepName, obj);

            SetIsComplete(obj, wDetailView);
            return obj;
        }


        private void SetIsComplete(TWizardObject obj, WizardDetailView config)
        {
            var requieredFields = config.Editors.Where(x => x.IsRequired).ToDictionary(x => x.PropertyName);

            obj.IsCompleted = true;
            var steps = config.Steps.Where(x => obj.PreviousSteps.All(a => a != x.Name) && x.Name != obj.Step);
            foreach (var wizzardStep in steps)
            {
                if (wizzardStep.CheckStepRule == null &&
                    wizzardStep.StepProperties.Any(x => requieredFields.ContainsKey(x.Name)))
                {
                    obj.IsCompleted = false;
                    break;
                }
                if (wizzardStep.CheckStepRule?.Invoke(obj) == StepRuleResult.Ok &&
                    wizzardStep.StepProperties.Any(x => requieredFields.ContainsKey(x.Name)))
                {
                    obj.IsCompleted = false;
                    break;
                }
            }
        }

        public async Task<TWizardObject> PrevStepAsync(IUnitOfWork unitOfWork, TWizardObject obj, ViewModelConfig config)
        {
            if (!StepIsFirst(obj.Step, config))
            {
                obj = GetPrevState(obj);
                SetIsComplete(obj, (WizardDetailView)config.DetailView);
            }

            return obj;
        }

        public async Task<TWizardObject> StartAsync(IUnitOfWork unitOfWork, TWizardObject wizardObj, ViewModelConfig config)
        {
            var obj = wizardObj;
            if (wizardObj == null)
            {
                obj = (TWizardObject)Activator.CreateInstance(config.TypeEntity);
            }

            obj.PreviousSteps = new List<string>();

            var wDetailView = config.DetailView as WizardDetailView;

            obj.HasSummary = wDetailView.HasSummary;

            if (wDetailView?.Steps == null || wDetailView.Steps.Count == 0) return obj;

            var firstStepName = "";

            if (string.IsNullOrEmpty(wDetailView.FirstStep))
            {
                foreach (var step in wDetailView.Steps)
                {
                    if ((step.CheckStepRule == null) || (step.CheckStepRule(wizardObj) != StepRuleResult.Forward))
                    {
                        firstStepName = step.Name;
                        break;
                    }
                }
            }
            else
            {
                firstStepName = wDetailView.FirstStep;
            }

            obj.Step = firstStepName;
            obj.StepCount = wDetailView.Steps.Count;
            obj.Index = 0;

            await OnBeforeStartAsync(unitOfWork, config, obj);

            //NOTE: Concept (Добавляет в историю все шаги с первого до явно назначенного)
            if (!string.Equals(obj.Step, firstStepName, StringComparison.InvariantCultureIgnoreCase))
                foreach (var wizzardStep in wDetailView.Steps)
                    if (!string.Equals(wizzardStep.Name, obj.Step, StringComparison.InvariantCultureIgnoreCase)) obj.PreviousSteps.Add(wizzardStep.Name); else break;

            InitStep(unitOfWork, config, obj.Step, obj);

            await OnAfterStartAsync(unitOfWork, config, obj);

            SetIsComplete(obj, wDetailView);
            return obj;
        }

        public virtual Task OnBeforeStartAsync(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {
            OnBeforeStart(unitOfWork, config, obj);
            return Task.CompletedTask;
        }

        [Obsolete("use async")]
        public virtual void OnBeforeStart(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {

        }

        public virtual Task OnAfterStartAsync(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {
            OnAfterStart(unitOfWork, config, obj);
            return Task.CompletedTask;
        }

        [Obsolete("use async")]
        public virtual void OnAfterStart(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {

        }

        public virtual Task<TObject> CompleteAsync(IUnitOfWork unitOfWork, TWizardObject obj)
        {
            return Task.FromResult(Complete(unitOfWork, obj));

        }

        [Obsolete("use async")]
        public virtual TObject Complete(IUnitOfWork unitOfWork, TWizardObject obj)
        {
            return _base_object_service.Create(unitOfWork, obj.GetObject());
        }


        async Task<IBaseObject> IWizardService<TWizardObject>.CompleteAsync(IUnitOfWork unitOfWork, TWizardObject obj)
        {
            return await CompleteAsync(unitOfWork, obj);
        }

        public virtual Task OnBeforeStepChangeAsync(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {
            OnBeforeStepChange(unitOfWork, config, obj);
            return Task.CompletedTask;
        }

        [Obsolete("use async")]
        public virtual void OnBeforeStepChange(IUnitOfWork unitOfWork, ViewModelConfig config, TWizardObject obj)
        {

        }

        public virtual Task OnAfterStepChangeAsync(IUnitOfWork unitOfWork, ViewModelConfig config, string prevStepName, TWizardObject obj)
        {
            OnAfterStepChange(unitOfWork, config, prevStepName, obj);
            return Task.CompletedTask;
        }

        [Obsolete("use async")]
        public virtual void OnAfterStepChange(IUnitOfWork unitOfWork, ViewModelConfig config, string prevStepName, TWizardObject obj)
        {

        }

        public void InitStep(IUnitOfWork unitOfWork, ViewModelConfig config, string nextStepName, TWizardObject obj, bool writeHistory = false)
        {
            var wDetailView = (WizardDetailView)config.DetailView;
            if (wDetailView == null) throw new Exception("Ошибка конфигурации мастера");

            var step = wDetailView.Steps.FirstOrDefault(x => x.Name == nextStepName);

            if (step?.CheckStepRule != null)
            {
                switch (step.CheckStepRule(obj))
                {
                    case StepRuleResult.Backward:
                        //go back
                        //GetPrevState(obj);
                        //Taк как мы фактически проверяем правила следующих, еще не назначенных шагов, то Backward
                        //это команда оставаться на месте
                        break;
                    case StepRuleResult.Forward:
                        //get next step name
                        //init step
                        var currentIndex = GetStepIndexByName(nextStepName, config);

                        if (writeHistory)
                            obj.PreviousSteps.Add(nextStepName);

                        if (currentIndex < wDetailView.Steps.Count - 1)
                        {
                            nextStepName = GetStepNameByIndex(currentIndex + 1, config);
                            InitStep(unitOfWork, config, nextStepName, obj, writeHistory);
                        }
                        else
                        {
                            GetNextState(obj, WizardConfig.WIZARD_COMPLETE_KEY);
                        }

                        break;
                    case StepRuleResult.End:
                        GetNextState(obj, WizardConfig.WIZARD_COMPLETE_KEY);
                        break;
                    case StepRuleResult.Ok:
                    default:
                        //get state
                        GetNextState(obj, nextStepName);
                        break;
                }
            }
            else
            {
                //get state
                GetNextState(obj, nextStepName);
            }
        }


    }
}
