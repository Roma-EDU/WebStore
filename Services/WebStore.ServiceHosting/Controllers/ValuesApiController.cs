using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(ServiceAddress.Values)]
    [ApiController]
    public class ValuesApiController : ControllerBase
    {
        private static readonly List<string> _values = Enumerable.Range(1, 10).Select(i => $"Value {i:00}").ToList();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (!validateId(id, out var errorResult))
                return errorResult;

            return _values[id];
        }

        // POST /api/values/add
        [HttpPost]
        [HttpPut("add")]
        public ActionResult Post([FromBody] string value)
        {
            _values.Add(value);
            return Ok();
        }

        // PUT api/values/edit/5
        [HttpPut("{id}")]
        [HttpPut("edit/{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (!validateId(id, out var errorResult))
                return errorResult;

            _values[id] = value;
            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!validateId(id, out var errorResult))
                return errorResult;

            _values.RemoveAt(id);
            return Ok();
        }

        private bool validateId(int id, out ActionResult errorResult)
        {
            errorResult = null;
            if (id < 0)
            {
                errorResult = BadRequest();
                return false;
            }

            if (id >= _values.Count)
            {
                errorResult = NotFound();
                return false;
            }

            return true;
        }
    }
}
