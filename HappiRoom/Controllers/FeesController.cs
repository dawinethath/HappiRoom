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
    public class FeesController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Fees        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Fees
                //Entity Framework can page only sorted data
                .OrderBy(fee => fee.id)
                .Select(fee => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    fee.id,
                    fee.name,
                    fee.fee,
                    fee.type,
                    fee.status
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Fees/5
        public HttpResponseMessage Put(int id, Fee fee)
        {
            if (ModelState.IsValid && id == fee.id)
            {
                db.Fees.Attach(fee);
                db.Entry(fee).State = EntityState.Modified;

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

        // POST api/Fees
        public HttpResponseMessage Post(Fee fee)
        {
            if (ModelState.IsValid)
            {
                db.Fees.Add(fee);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, fee);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = fee.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Fees/5
        public HttpResponseMessage Delete(int id)
        {
            Fee fee = db.Fees.Single(r => r.id == id);
            if (fee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Fees.Remove(fee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, fee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeeExists(int id)
        {
            return db.Fees.Count(e => e.id == id) > 0;
        }
    }
}