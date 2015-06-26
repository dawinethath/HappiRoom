using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HappiRoom.Models;
using Newtonsoft.Json;

namespace HappiRoom.Controllers
{
    public class LotteryRecordsController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/LotteryRecords        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.LotteryRecords
                //Entity Framework can page only sorted data
                .OrderBy(LotteryRecord => LotteryRecord.id)
                .Select(LotteryRecord => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    LotteryRecord.id,
                    LotteryRecord.types,
                    LotteryRecord.dates,
                    LotteryRecord.a1,
                    LotteryRecord.a2,
                    LotteryRecord.a3,
                    LotteryRecord.a4,
                    LotteryRecord.b,
                    LotteryRecord.c,
                    LotteryRecord.d
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/LotteryRecords/5
        public HttpResponseMessage Put(int id, LotteryRecord tbl)
        {
            if (ModelState.IsValid && id == tbl.id)
            {
                db.LotteryRecords.Attach(tbl);
                db.Entry(tbl).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/LotteryRecords
        public HttpResponseMessage Post(LotteryRecord tbl)
        {
            if (ModelState.IsValid)
            {
                db.LotteryRecords.Add(tbl);               
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbl);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbl.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/LotteryRecords/5
        public HttpResponseMessage Delete(int id)
        {
            LotteryRecord tbl = db.LotteryRecords.Single(r => r.id == id);
            if (tbl == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.LotteryRecords.Remove(tbl);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool isExists(int id)
        {
            return db.LotteryRecords.Count(e => e.id == id) > 0;
        }
    }
}