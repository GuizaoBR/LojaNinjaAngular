using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using LojaNinja.Data;
using LojaNinja.Models;

namespace LojaNinja.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [RoutePrefix("api/Pedidos")]
    public class PedidosController : ApiController
    {
        private LojaNinjaContext db = new LojaNinjaContext();

        // GET: api/Pedidos
        public IQueryable<PedidoView> GetPedidos()
        {
            var pedidos = db.Pedidos
                .Include(x => x.Produto)
                .Include(x => x.Cliente)
                .GroupBy(x => new { x.Numero, x.Cliente, x.Data, x.ValorTotal })
                .AsEnumerable()
                .Select(x => new PedidoView
                {
                    Cliente = x.Key.Cliente.Nome,
                    Numero = x.Key.Numero,
                    Produtos = string.Join(",", x.Select(y => y.Produto.Descricao).ToList()),
                    Data = x.Key.Data,
                    ValorTotal = x.Key.ValorTotal
                }).AsQueryable();
           // var lista = pedidos.ToList();
            return pedidos;
        }

        // GET: api/Pedidos/5
        [Route("ByNum/{numero}")]
        [ResponseType(typeof(PedidoView))]
        public IHttpActionResult GetPedidoByNum(int numero)
        {
            var pedidos = db.Pedidos.Where(x => x.Numero == numero)
                .Include(x => x.Produto)
                .Include(x => x.Cliente)
                .GroupBy(x => new { x.Numero, x.Cliente, x.Data, x.ValorTotal })
                .AsEnumerable()
                .Select(x => new PedidoView
                {
                    Cliente = x.Key.Cliente.Nome,
                    Numero = x.Key.Numero,
                    Produtos = string.Join(",", x.Select(y => y.Produto).ToList()),
                    Data = x.Key.Data,
                    ValorTotal = x.Key.ValorTotal
                })
                .AsQueryable();

            if (pedidos == null)
            {
                return NotFound();
            }

            return Ok(pedidos);
        }
        [Route("Item/{id}")]
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult GetPedidoItem(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        // PUT: api/Pedidos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedido(int id, Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedido.Id)
            {
                return BadRequest();
            }

            db.Entry(pedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pedidos
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult PostPedido(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pedidos.Add(pedido);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pedido.Id }, pedido);
        }

        // DELETE: api/Pedidos/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult DeletePedido(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            db.Pedidos.Remove(pedido);
            db.SaveChanges();

            return Ok(pedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidoExists(int id)
        {
            return db.Pedidos.Count(e => e.Id == id) > 0;
        }
    }
}