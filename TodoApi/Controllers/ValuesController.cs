using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase 
  {
    private readonly ILogger<ValuesController> _logger;
    public ValuesController(ILogger<ValuesController> logger)
    {
      _logger = logger;
    }

    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      _logger.LogInformation("at get all method ... ");
      return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
      _logger.LogInformation("at get by Id method {id} ", id);
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      HttpContext.Session.SetString("", "");
      HttpContext.Session.SetInt32("", 3);
      HttpContext.Session.GetString("");
      HttpContext.Session.GetInt32("");

      HttpContext.Session.Set<DateTime>("key", DateTime.Now);
      
      
    }
  }
}
