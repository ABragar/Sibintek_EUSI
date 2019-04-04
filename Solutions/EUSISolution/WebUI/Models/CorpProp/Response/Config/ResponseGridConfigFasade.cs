using System;
using System.Linq;
using Base.DAL;
using CorpProp;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using TResponse = CorpProp.Entities.Request.Response;

namespace WebUI.Models.CorpProp.Response.Config
{
    public static class ResponseGridConfigFasade
    {

        public static IQueryable<TReturn> ResponseCells<TReturn, TItem>(
            IUnitOfWork uofw,
            int? requestId)
            where TReturn : ResponseCellBase<TItem>
        {
            var responses = uofw.GetRepository<TResponse>().All();
            IQueryable<TReturn> cells = uofw.GetRepository<TReturn>().All();
            var strCellData = from cell in cells
                              join response in responses on cell.LinkedResponseID equals response.ID
                              where response.Hidden == false
                              where response.RequestID == requestId
                              where
                              response.ResponseStatus != null &&
                              (response.ResponseStatus.Code ==
                               ((int)RequestInitializer.ResponseStates.Ready).ToString() ||
                               response.ResponseStatus.Code ==
                               ((int)RequestInitializer.ResponseStates.NoInfo).ToString())
                              select cell;
            return strCellData.Distinct();
        }

        public static IQueryable<RequestRow> RequestRows(IUnitOfWork unitOfWork, int srcRequestId)
        {
            var rows = unitOfWork.GetRepository<RequestRow>().All();
            var contents = unitOfWork.GetRepository<RequestContent>().All();
            var requests = unitOfWork.GetRepository<Request>().All();
            var responses = unitOfWork.GetRepository<TResponse>().All();
            var requestRowsJoin = from row in rows
                                  join content in contents on row.RequestContentID equals content.ID
                                  join request in requests on content.ID equals request.RequestContentID
                                  join response in responses on request.ID equals response.RequestID
                                  where request.ID == srcRequestId
                                  select row;
            var requestRows = requestRowsJoin.Distinct();
            return requestRows;
        }

        public static IQueryable<RequestRow> ResponseRows(IUnitOfWork unitOfWork, int srcResponseId)
        {
            var rows = unitOfWork.GetRepository<RequestRow>().All();
            var contents = unitOfWork.GetRepository<RequestContent>().All();
            var requests = unitOfWork.GetRepository<Request>().All();
            var responses = unitOfWork.GetRepository<TResponse>().All();
            var requestRowsJoin = from row in rows
                                  join content in contents on row.RequestContentID equals content.ID
                                  join request in requests on content.ID equals request.RequestContentID
                                  join response in responses on request.ID equals response.RequestID
                                  where response.ID == srcResponseId
                                  select row;
            var requestRows = requestRowsJoin.Distinct();
            return requestRows;
        }

        public static IQueryable<RequestColumn> GetColumnsByResponse(IUnitOfWork uofw, int? responseId)
        {
            if (responseId == null)
                throw new ArgumentException(nameof(responseId));
            var requests = uofw.GetRepository<Request>().All();
            var responses = uofw.GetRepository<TResponse>().All();
            var requestColumnQueryable = uofw.GetRepository<RequestColumn>().All();
            var requestContentQuery = uofw.GetRepository<RequestContent>().All();

            var columns = from column in requestColumnQueryable
                          join content in requestContentQuery on column.RequestContentID equals content.ID
                          join request in requests on content.ID equals request.RequestContentID
                          join response in responses on request.ID equals response.RequestID
                          where response.ID == responseId
                          select column;

            return columns.Distinct();
        }

        public static IQueryable<RequestColumn> GetColumnsByRequest(int? requestId, IUnitOfWork uofw)
        {
            if (requestId == null)
                throw new ArgumentException(nameof(requestId));
            var requests = uofw.GetRepository<Request>().All();
            var responses = uofw.GetRepository<TResponse>().All();
            var requestColumnQueryable = uofw.GetRepository<RequestColumn>().All();
            var requestContentQuery = uofw.GetRepository<RequestContent>().All();

            var columns = from column in requestColumnQueryable
                          join content in requestContentQuery on column.RequestContentID equals content.ID
                          join request in requests on content.ID equals request.RequestContentID
                          join response in responses on request.ID equals response.RequestID
                          where request.ID == requestId
                          select column;

            return columns.Distinct();
        }


    }
}