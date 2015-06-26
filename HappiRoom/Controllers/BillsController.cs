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
    public class BillsController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Bills        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Bills
                //Entity Framework can page only sorted data
                .OrderBy(bill => bill.id)
                .Select(bill => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    bill.id,
                    bill.type,
                    bill.number,
                    bill.amount,
                    bill.discount,
                    bill.rate,
                    bill.sub_code,
                    bill.due_date,
                    bill.status,
                    bill.biller,
                    bill.cashier,
                    bill.paid_usd,
                    bill.paid_khr,
                    bill.paid_date,
                    bill.memo
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Bills/5
        public HttpResponseMessage Put(int id, Bill bill)
        {
            if (ModelState.IsValid && id == bill.id)
            {
                db.Bills.Attach(bill);
                db.Entry(bill).State = EntityState.Modified;

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

        // POST api/Bills
        public HttpResponseMessage Post(Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bills.Add(bill);               
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, bill);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = bill.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Bills/5
        public HttpResponseMessage Delete(int id)
        {
            Bill bill = db.Bills.Single(r => r.id == id);
            if (bill == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Bills.Remove(bill);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, bill);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool billExists(int id)
        {
            return db.Bills.Count(e => e.id == id) > 0;
        }
    }
}