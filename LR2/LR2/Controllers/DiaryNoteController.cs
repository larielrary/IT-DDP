using ClassLibrary;
using ClassLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MeteorologicalDiaryServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryNoteController : ControllerBase
    {
        private readonly IRepository<DiaryNote> _repository;

        public DiaryNoteController()
        {
            _repository = new Repository<DiaryNote>("diary.json");
        }

        [HttpGet]
        public ActionResult<IEnumerable<DiaryNote>> Get()
        {
            return _repository.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<DiaryNote> Get(int id)
        {
            return _repository.GetById(id);
        }

        [HttpPut]
        public ActionResult Put(DiaryNote note)
        {
            _repository.Update(note);
            return Ok(note);
        }

        [HttpPost]
        public ActionResult Post(DiaryNote note)
        {
            _repository.Create(note);
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);

            return Ok();
        }
    }
}