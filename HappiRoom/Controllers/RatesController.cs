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
    public class RatesController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Rates        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Rates
                //Entity Framework can page only sorted data
                .OrderBy(rate => rate.id)
                .Select(rate => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    rate.id,
                    rate.rate,
                    rate.date,
                    rate.status
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Rates/5
        public HttpResponseMessage Put(int id, Rate rate)
        {
            if (ModelState.IsValid && id == rate.id)
            {
                db.Rates.Attach(rate);
                db.Entry(rate).State = EntityState.Modified;

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

        // POST api/Rates
        public HttpResponseMessage Post(Rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Rates.Add(rate);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, rate);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = rate.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Rates/5
        public HttpResponseMessage Delete(int id)
        {
            Rate rate = db.Rates.Single(r => r.id == id);
            if (rate == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Rates.Remove(rate);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, rate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RateExists(int id)
        {
            return db.Rates.Count(e => e.id == id) > 0;
        }
    }
}