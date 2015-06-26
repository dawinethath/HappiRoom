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
    public class DemosController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Demos        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Demos
                //Entity Framework can page only sorted data
                .OrderBy(demo => demo.id)
                .Select(demo => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    demo.id,
                    demo.block_id,
                    demo.name
                }).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }
        
        // PUT api/Demos/5
        public HttpResponseMessage Put(int id, Demo demo)
        {
            if (ModelState.IsValid && id == demo.id)
            {
                db.Demos.Attach(demo);
                db.Entry(demo).State = EntityState.Modified;

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

        // POST api/Blocks
        public HttpResponseMessage Post(Demo demo)
        {            
            if (ModelState.IsValid)
            {
                db.Demos.Add(demo);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, demo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = demo.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Blocks/5
        public HttpResponseMessage Delete(int id)
        {
            Demo demo = db.Demos.Single(r => r.id == id);
            if (demo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Demos.Remove(demo);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, demo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DemoExists(int id)
        {
            return db.Demos.Count(e => e.id == id) > 0;
        }
    }
}