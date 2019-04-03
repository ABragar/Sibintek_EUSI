using System;
using System.Linq;
using System.Linq.Expressions;
using Base;
using Base.DAL;
using Base.UI;
using CorpProp.Entities;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Request;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Services.Response;
using CorpProp.Services.Response.Fasade;
using CorpProp.Services.Subject;
using Base.UI.ViewModal;

namespace CorpProp
{
    /// <summary>
    /// Представляет инициализатор функционала Вопросы, Ответы (CorpProp).
    /// </summary>
    public static class RequestInitializer
    {
        /// <summary>
        ///  Инициализация.
        /// </summary>
        /// <param name="context"></param>
        public static void Init(IInitializerContext context)
        {
            context.CreateVmConfig<RequestDynamicType>("RequestDynamicType").Service<IRequestDynamicQueryService>();
            context.CreateVmConfig<ResponseDynamicType>("ResponseDynamicType")
                .ListView(lv => lv.HiddenActions(new[] { LvAction.Settings }))
                .Service<IResponseDynamicQueryService>();

            context.CreateVmConfigOnBase<Society>("Society", "SocietyOnRequest")
                   .Service<ISocietyOnRequestService>()
                   .DetailView(builder => builder.Title("ОГ адресаты").Editors(
                   factory => factory
                   .Add(society => society.ResponsableForResponse, editorBuilder => editorBuilder.Visible(true))
                   .Add(society => society.SubdivisionResponsableForResponse, columnBuilder => columnBuilder.Visible(true))
                   .Add(society => society.ContactResponsableForResponse, columnBuilder => columnBuilder.Visible(true))
                   )
                   .Editors(factory => factory.AddOneToManyAssociation<SibUser>("SocietyAndSibUserSysName", y => y
                       .Mnemonic("SocietyAndSibUser")
                       .TabName("[0]Пользователи ОГ").IsLabelVisible(false)
                       .Create((uofw, entity, id) =>
                       {
                           entity.Society = uofw.GetRepository<Society>().Find(id);
                           entity.SocietyID = id;
                       })
                       .Delete((uofw, entity, id) =>
                       {
                           entity.Society = null;
                       })
                       .Filter((uofw, q, id, oid) => q.Where(w => w.SocietyID == id)))))
                   .ListView(builder => builder.Title("ОГ адресаты")
                       .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                       .Columns(factory => factory.Clear()
                                                  .Add(society => society.Name, columnBuilder => columnBuilder.Visible(true).Order(1))
                                                  .Add(society => society.ResponsableForResponse, columnBuilder => columnBuilder.Visible(true).Order(2))
                                                  .Add(society => society.ContactResponsableForResponse, columnBuilder => columnBuilder.Visible(true).Order(3))
                                                  .Add(society => society.SubdivisionResponsableForResponse, columnBuilder => columnBuilder.Visible(true).Order(4))
                                                  .Add(society => society.FederalDistrict, columnBuilder => columnBuilder.Visible(true).Order(5))
                                                  .Add(society => society.Region, columnBuilder => columnBuilder.Visible(true).Order(6))
                                                  .Add(society => society.IsShareControl, columnBuilder => columnBuilder.Visible(true).Order(7))
                                                  .Add(society => society.ShareInEquity, columnBuilder => columnBuilder.Visible(true).Order(8))));

            context.CreateVmConfig<Response>("Response")
                .Service<Services.Response.ResponseService>()
                .Title("Запрос в ОГ")
                .ListView(x => x.Title("Запрос в ОГ")
                .Columns(factory => factory
                .Add(response => response.Name)
                .Add(response => response.Date)
                .Add(response => response.ResponseStatus)
                .Add(response => response.Executor)))
                .DetailView(x => x.Title("Запрос в ОГ")
                    .DefaultSettings(
                                     (work, response, arg3) =>
                                     {
                                         void DisableAll()
                                         {
                                             arg3.ReadOnly(response1 => response1.Name);
                                             arg3.ReadOnly(response1 => response1.Description);
                                             arg3.ReadOnly(response1 => response1.Text);
                                             arg3.ReadOnly(response1 => response1.Term);
                                             arg3.ReadOnly(response1 => response1.Date);
                                             arg3.ReadOnly(response1 => response1.Executor);
                                             arg3.ReadOnly(response1 => response1.ExecutorSociety);
                                             arg3.ReadOnly(response1 => response1.ResponseStatus);
                                             arg3.ReadOnly(response1 => response1.Request);
                                         }
                                         DisableAll();

                                         var userRepo = work.GetRepository<SibUser>();
                                         switch (Base.Ambient.AppContext.SecurityUser.ID)
                                         {
                                             case var currentUser when (response.Request?.Responsible?.UserID == currentUser): //автор запроса
                                             if (response.ResponseStatus.Code ==
                                                 ((int)ResponseStates.Draft).ToString())
                                             {
                                                 //arg3.ReadOnly(response1 => response1.Term, false);
                                             }
                                             //должна быть возможность изменять “Статус" для ответов в любом статусе.
                                             arg3.ReadOnly(response1 => response1.ResponseStatus, false);
                                             break;
                                             case var currentUser when (response.ExecutorSociety != null && userRepo.Find(user =>
                                                 user.SocietyID != null                                               
                                                 && user.SocietyID == response.ExecutorSociety.ID
                                                 && user.ExecutorOnRequest
                                                 && user.UserID == currentUser) != null
                                                 )://Соисполнители

                                             if (response.ResponseStatus.Code ==
                                                 ((int)ResponseStates.OnCompletion).ToString()
                                                 ||
                                                 response.ResponseStatus.Code ==
                                                 ((int)ResponseStates.Draft).ToString())
                                             {
                                                 //TODO? AssociationEditor -> ReadOnly(false)
                                             }
                                             break;
                                             case var currentUser when (response.ExecutorSociety != null && userRepo.Find(user =>
                                                 user.SocietyID != null
                                                 && user.SocietyID == response.ExecutorSociety.ID
                                                 && user.ResponsibleOnRequest
                                                 && user.UserID == Base.Ambient.AppContext.SecurityUser.ID) != null
                                                 )://Ответственный за ответы
                                             if (response.ResponseStatus.Code ==
                                                   ((int)ResponseStates.OnCompletion).ToString()
                                                   ||
                                                   response.ResponseStatus.Code ==
                                                   ((int)ResponseStates.Draft).ToString())
                                             {
                                                 //TODO? AssociationEditor -> ReadOnly(false)
                                                 arg3.ReadOnly(response1 => response1.ResponseStatus, false);
                                                 arg3.ReadOnly(response1 => response1.Text, false);
                                             }
                                             break;
                                         }
                                     })
                    .Toolbar(t =>
                    {
                        t.Add("GetResponseInstructionToolbar", "Response", p => p.ListParams(l => l.Add("id", "[ID]")));
                    })
                    .Editors(edts => edts
                        .Add<ResponseGridEditor>("ResponseGrid", e => e
                        .Title("Таблица ответа")
                        .TabName("[5]Таблица ответа")                        
                        .IsLabelVisible(false)
                        .Mnemonic("ResponseDynamicType")
                        .AddParam("Response", "true"))
                        .AddManyToManyRigthAssociation<FileCardAndResponse>("Response_Filecards", y => y.TabName("[2]Документы"))
                        .AddManyToManyLeftAssociation<ResponseAndSibUser>("Response_CoexecutorsSibUserSysName",
                                                                            y => y.Mnemonic("Response_CoexecutorsSibUser")
                                                                                  .TabName(Helpers.CaptionHelper.DefaultTabName)
                                                                                  .Title("Соисполнители ответа")
                                                                                  .Order(20)                                                                                  
                                                                                  .IsLabelVisible(true)
                                                                                 )
                        ))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<Response>(baseMnemonic: "Response", createMnemonic: "IncomingResponse")
                .ListView(
                    builder =>
                        builder.DataSource(
                            sourceBuilder => sourceBuilder.Filter(IncomingResponseFilter())));

            context.CreateVmConfigOnBase<Response>(baseMnemonic: "Response", createMnemonic: "OutgoingResponse")
                .ListView(
                    builder =>
                        builder.DataSource(
                            sourceBuilder => sourceBuilder.Filter(OutgoingResponseFilter())));

            context.CreateVmConfigOnBase<Response>(baseMnemonic: "Response", createMnemonic: "DraftResponse")
                .ListView(
                    builder =>
                        builder.DataSource(
                            sourceBuilder => sourceBuilder.Filter(DraftResponseFilter())));

            context.CreateVmConfigOnBase<Response>("Response", "Request_Response")
                   .DetailView(builder => builder.Editors(factory => factory
                        .Add(response => response.ExecutorSociety, editorBuilder => editorBuilder.Visible(true).Order(1))
                        .Add(response => response.Responsable, editorBuilder => editorBuilder.Visible(true).Order(2))
                        .Add(response => response.Date, editorBuilder => editorBuilder.Visible(true).Order(3))
                        .Add(response => response.Term, editorBuilder => editorBuilder.Visible(true).Order(4))
                        .Add(response => response.ResponseStatus, editorBuilder => editorBuilder.Visible(true).Order(5))
                        .Add(response => response.Request, editorBuilder => editorBuilder.Visible(false))
                        .Add(response => response.Description, editorBuilder => editorBuilder.Visible(false))
                        .Add(response => response.Name, editorBuilder => editorBuilder.Visible(false))
                        .Add(response => response.Text, editorBuilder => editorBuilder.Visible(false))));

            context.CreateVmConfig<Request>("Request")
                .Service<Services.Request.RequestService>()
                .Title("Запрос")
                .ListView(x => x.Title("Запросы"))
                .DetailView(x => x.Title("Запрос")
                    .DefaultSettings((work, request, arg3) =>
                    {
                        if (request.RequestStatus != null && request.RequestStatus.Code == RequestDirectedStatusCode)
                        {
                            arg3.ReadOnly(request1 => request1.RequestContent);
                        }
                        //При формировании нового запроса автоматически присваивать ему статус #2559
                        Func<IUnitOfWork, string, RequestStatus> getStatusIdByCode = (unitOfWork, stateCode) =>
                        {
                            var requestState = unitOfWork.GetRepository<RequestStatus>().Find(state => state != null && state.Code == stateCode);
                            return requestState;
                        };
                        if (request.ID <= 0)
                        {
                            var draftState = getStatusIdByCode(work, RequestDraftStatusCode);
                            request.RequestStatusID = draftState.ID;
                            request.RequestStatus = draftState;
                            var currentUserId = Base.Ambient.AppContext.SecurityUser.ID;
                            var currentSibUser =
                                work.GetRepository<SibUser>().Find(sUser => sUser.UserID == currentUserId);
                            if (currentSibUser != null)
                            {
                                request.Responsible = currentSibUser;
                                request.ResponsibleID = currentSibUser.ID;
                            }
                        }
                    })
                    .Editors(edts => edts
                        .Add<ResponseGridEditor>("ResponseGrid", e => e
                        .Title("Сводная таблица ответов")
                        .TabName("[5]Сводная таблица ответов")
                        .IsLabelVisible(false)
                        .Mnemonic("RequestDynamicType")
                        .AddParam("Request", "true"))
                        .AddManyToManyLeftAssociation<RequestAndSociety>
                        ("RequestExecutors", y => y.Mnemonic("SocietyOnRequest").TabName("[3]Адресаты").IsLabelVisible(false))
                        .AddOneToManyAssociation<Response>("Request_ResponseSysName",
                            y => y.Mnemonic("Request_Response")
                                  .TabName("[4]Ответы")
                                  .IsLabelVisible(false)
                            .Create((uofw, entity, id) =>
                            {
                                entity.Request = uofw.GetRepository<Request>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Request = null;
                            })
                      .Filter((uofw, q, id, oid) => q.Where(w => w.RequestID == id && !w.Hidden)))
                      .AddManyToManyRigthAssociation<FileCardAndRequest>("Request_FileCards", y => y.TabName("[2]Документы").IsLabelVisible(false))
                      ))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfig<RequestColumnItems>()
                   .Title("Список значений")
                   .LookupProperty(builder => builder.Text(items => items.Item));

            context.CreateVmConfig<RequestColumn>()
                .Service<Services.Request.RequestColumnService>()
                .Title("Столбец запроса")
                .ListView(x => x.Title("Столбцы запроса"))
                .DetailView(x => x
                    .DefaultSettings(
                                     (work, column, arg3) =>
                                     {
                                         if (!string.IsNullOrWhiteSpace(column.TypeData?.Code))
                                         {
                                             var type = ResponseTypeDataFacade.GetSimpleTypeOrNull(column.TypeData.Code);
                                             bool isNumeric;
                                             ResponseTypeDataFacade.IsNumericDict.TryGetValue(type, out isNumeric);
                                             if (isNumeric)
                                             {
                                                 arg3.Visible(requestColumn => requestColumn.MaxLength, false);
                                                 arg3.Visible(requestColumn => requestColumn.MinLength, false);
                                                 arg3.Visible(requestColumn => requestColumn.MaxDate, false);
                                                 arg3.Visible(requestColumn => requestColumn.MinDate, false);
                                             }
                                             else
                                             {
                                                 arg3.Visible(requestColumn => requestColumn.MaxValue, false);
                                                 arg3.Visible(requestColumn => requestColumn.MinValue, false);
                                                 if (type != typeof(string))
                                                 {
                                                     arg3.Visible(requestColumn => requestColumn.MaxLength, false);
                                                     arg3.Visible(requestColumn => requestColumn.MinLength, false);
                                                 }
                                                 if (type != typeof(DateTime))
                                                 {
                                                     arg3.Visible(requestColumn => requestColumn.MaxDate, false);
                                                     arg3.Visible(requestColumn => requestColumn.MinDate, false);
                                                 }
                                             }
                                         }
                                     })
                    .Title("Столбец запроса")
                    .Editors(factory => factory
                        // Получает или задает список значений.
                        .AddOneToManyAssociation<RequestColumnItems>("RequestColumnItems_RequestColumn",
                                y => y.IsLabelVisible(false)
                                      .TabName("Список значений")
                                      .Create((uofw, entity, id) =>
                                      {
                                          //entity.RequestColumnID = id;
                                          entity.RequestColumn = uofw.GetRepository<RequestColumn>().Find(id);
                                      })
                                      .Filter((uofw, q, id, oid) => q.Where(w => w.RequestColumnID == id)))))
                .LookupProperty(z => z.Text(t => t.Name));


            context.CreateVmConfig<RequestContent>()
            .Service<Services.RequestContent.RequestContentService>()
                .Title("Содержание запроса")
                .ListView(x => x.Title("Содержания запросов"))
                .DetailView(x => x.Title("Содержание запроса").Editors(edt => edt
                      .AddOneToManyAssociation<RequestColumn>("RequestContent_RequestColumn",
                            y => y.TabName("[1]Столбцы запроса")
                                  .IsLabelVisible(false)
                                  .Create((uofw, entity, id) =>
                                  {
                                      entity.RequestContent = uofw.GetRepository<RequestContent>().Find(id);
                                  })
                                  .Delete((uofw, entity, id) =>
                                  {
                                      entity.RequestContent = null;
                                  })
                      .Filter((uofw, q, id, oid) => q.Where(w => w.RequestContentID == id && !w.Hidden)))
                      .AddOneToManyAssociation<RequestRow>("RequestContent_RequestRow",
                            y => y.TabName("[2]Строки запроса")
                                  .IsLabelVisible(false)
                                  .Create((uofw, entity, id) =>
                                  {
                                      entity.RequestContent = uofw.GetRepository<RequestContent>().Find(id);
                                  })
                                  .Delete((uofw, entity, id) =>
                                  {
                                      entity.RequestContent = null;
                                  })
                      .Filter((uofw, q, id, oid) => q.Where(w => w.RequestContentID == id && !w.Hidden)))))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<RequestRow>()
                .Title("Строка запроса")
                .ListView(x => x.Title("Строки запроса"))
                .DetailView(x => x.Title("Строка запроса"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<RequestTemplate>()
                .Title("Шаблон запроса")
                .ListView(x => x.Title("Шаблоны запросов"))
                .DetailView(x => x.Title("Шаблон запроса"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfigOnBase<SibUser>("SibUser", "Response_CoexecutorsSibUser")
                   .ListView(builder => builder.DataSource(sourceBuilder => sourceBuilder.Filter(user => user.ExecutorOnRequest)));
            

            #region ManyToManyConfigs
            context.CreateVmConfig<FileCardAndResponse>("Response_Filecards");
            context.CreateVmConfig<ResponseAndSibUser>("RequestExecutors");
            #endregion



            //ResponseServiceAddRow;

            //context.CreateVmConfig<ResponseCell>()
            //    .Title("Ячейка ответа")
            //    .ListView(x => x.Title("Ячейки ответов"))
            //    .DetailView(x => x.Title("Ячейка ответа"))
            //    .LookupProperty(x => x.Text(t=>t.ID));

            //context.CreateVmConfig<ResponseRow>()
            //    .Title("Строка ответа")
            //    .ListView(x => x.Title("Строки ответов"))
            //    .DetailView(x => x.Title("Строка ответа"))
            //    .LookupProperty(x => x.Text(t=>t.ID));


        }

        public enum ResponseStates
        {
            ///Черновик
            Draft = 101,
            ///Готов
            Ready = 102,
            ///Нет информации
            NoInfo = 103,
            /// <summary>
            /// На доработке
            /// </summary>
            OnCompletion = 106,
            /// <summary>
            /// В работе
            /// </summary>
            OnWork = 107
        }

        public enum RequestStates
        {
            /// <summary>
            /// Черновик
            /// </summary>
            Draft = 101,
            /// <summary>
            /// Направлен
            /// </summary>
            Directed = 102,
            /// <summary>
            /// Готов
            /// </summary>
            Ready = 104,
        }

        public static string RequestDraftStatusCode => ((int)RequestStates.Draft).ToString();
        public static string RequestDirectedStatusCode => ((int)RequestStates.Directed).ToString();

        //TODO Перейти на expression, не копипастить
        //public static Expression<Func<Response, bool>> XXX()
        //{

        //    Expression<Func<Response, bool>> x1 = response =>
        //        response.Request != null && response.Request.RequestStatus != null &&
        //        response.Request.RequestStatus.Code != RequestDraftStatusCode;

        //    Expression<Func<Response, bool>> x2 =
        //        response => (response.ResponseStatus.Code == ((int)ResponseStates.Ready).ToString() ||
        //                     response.ResponseStatus.Code == ((int)ResponseStates.NoInfo).ToString() ||
        //                     response.ResponseStatus.Code != ((int)ResponseStates.Draft).ToString());
        //    var dd = Expression.AndAlso(x1.Body, x2.Body);
        //    var lambda = Expression.Lambda<Func<Response, bool>>(dd, x1.Parameters[0]);
        //    return lambda;
        //}


        //public static Expression<Func<Response, bool>> OutgoingResponseFilter()
        //{
        //    return XXX();
        //}

        public static Expression<Func<Response, bool>> OutgoingResponseFilter()
        {
            return
                response =>
                    response.Request != null && response.Request.RequestStatus!= null &&
                    response.Request.RequestStatus.Code != RequestDraftStatusCode &&
                    (response.ResponseStatus != null && 
                        (response.ResponseStatus.Code == ((int)ResponseStates.Ready).ToString() ||
                        response.ResponseStatus.Code == ((int)ResponseStates.NoInfo).ToString()));
        }

        public static Expression<Func<Response, bool>> IncomingResponseFilter()
        {
            return
                response =>

                    (response.Request != null && response.Request.RequestStatus != null &&
                     response.Request.RequestStatus.Code != RequestDraftStatusCode &&
                     (response.ResponseStatus == null ||
                     (response.ResponseStatus.Code != ((int) ResponseStates.Ready).ToString() &&
                      response.ResponseStatus.Code != ((int) ResponseStates.NoInfo).ToString())));
        }

        public static Expression<Func<Response, bool>> DraftResponseFilter()
        {
            return
                response =>
                    response.Request != null && response.Request.RequestStatus != null &&
                    response.Request.RequestStatus.Code != RequestDraftStatusCode &&
                    (response.ResponseStatus != null &&
                    response.ResponseStatus.Code == ((int)ResponseStates.Draft).ToString());
        }

    }
}
