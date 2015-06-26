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
    public class InvoicesController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Invoices        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Invoices
                //Entity Framework can page only sorted data
                .OrderBy(invoice => invoice.id)
                .Select(invoice => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    invoice.id,
                    invoice.room_id,
                    invoice.customer_id,
                    invoice.block_id,
                    invoice.number,
                    invoice.room_number,
                    invoice.customer_name,
                    invoice.wfrom,
                    invoice.wto,
                    invoice.wmax,
                    invoice.wround,
                    invoice.wqty,
                    invoice.wadd_on,
                    invoice.wprice,
                    invoice.wamt,
                    invoice.efrom,
                    invoice.eto,
                    invoice.emax,
                    invoice.eround,
                    invoice.eqty,
                    invoice.eadd_on,
                    invoice.eprice,
                    invoice.eamt,
                    invoice.date_in,
                    invoice.month_of,
                    invoice.from_date,
                    invoice.to_date,
                    invoice.rental,
                    invoice.fine,
                    invoice.discount,
                    invoice.deposit,
                    invoice.debt,
                    invoice.service,
                    invoice.trash,
                    invoice.total,
                    invoice.rate,
                    invoice.rate_id,
                    invoice.billing_date,
                    invoice.due_date,
                    invoice.paid_usd,
                    invoice.paid_khr,
                    invoice.paid_date,
                    invoice.memo,
                    invoice.memo2,
                    invoice.status,
                    invoice.biller,
                    invoice.cashier,
                    invoice.printed
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Invoices/5
        public HttpResponseMessage Put(int id, Invoice invoice)
        {
            if (ModelState.IsValid && id == invoice.id)
            {
                db.Invoices.Attach(invoice);
                db.Entry(invoice).State = EntityState.Modified;

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

        // POST api/Invoices
        public HttpResponseMessage Post(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, invoice);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = invoice.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Invoices/5
        public HttpResponseMessage Delete(int id)
        {
            Invoice invoice = db.Invoices.Single(r => r.id == id);
            if (invoice == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Invoices.Remove(invoice);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, invoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.id == id) > 0;
        }       
    }
}