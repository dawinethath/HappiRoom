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
using System.Data.Entity.Core.Objects;

namespace HappiRoom.Controllers
{
    public class RoomsController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();
        
        // GET api/Rooms        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Rooms
                //Entity Framework can page only sorted data                
                .OrderBy(room => room.id)
                .Select(room => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    room.id,
                    room.number,
                    room.description,
                    room.block_id,
                    room.customer_id,
                    room.rental_id,
                    room.electricity_fee_id,
                    room.water_fee_id,
                    room.wadd_on,
                    room.eadd_on,
                    room.status
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }
        
        // PUT api/Rooms/5
        public HttpResponseMessage Put(int id, Room room)
        {
            if (ModelState.IsValid && id == room.id)
            {
                db.Rooms.Attach(room);               
                db.Entry(room).State = EntityState.Modified;

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
       
        // POST api/Rooms
        public HttpResponseMessage Post(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, room);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = room.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        
        // DELETE api/Rooms/5
        public HttpResponseMessage Delete(int id)
        {
            Room room = db.Rooms.Single(r => r.id == id);
            if (room == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Rooms.Remove(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, room);
        }
                
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.id == id) > 0;
        }

        private Customer GetCustomer(int room_id)
        {
            return db.Customers.Single(c => c.id == 1);
        }
    }
}