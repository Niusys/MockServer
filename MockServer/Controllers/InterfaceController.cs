using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MockServer.Entities;
using MockServer.Models;
using MockServer.MongoStorage;
using MongoDB.Driver;
using Niusys.Extensions.ComponentModels;
using System;
using System.Threading.Tasks;

namespace MockServer.Controllers
{
    public class InterfaceController : Controller
    {
        private readonly MockServerNoSqlRepository<ApiInterface> _mockServerNoSqlRepository;
        private readonly IMapper _mapper;

        public InterfaceController(MockServerNoSqlRepository<ApiInterface> mockServerNoSqlRepository, IMapper mapper)
        {
            _mockServerNoSqlRepository = mockServerNoSqlRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> List(string category = "", int pageIndex = 1, int pageSize = 100)
        {
            var filters = Builders<ApiInterface>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(category))
            {
                filters &= Builders<ApiInterface>.Filter.Eq(x => x.Category, category);
            }

            var sort = Builders<ApiInterface>.Sort.Ascending(x => x.RequestPath);
            var list = await _mockServerNoSqlRepository.PaginationSearchAsync(filters, sort, pageIndex, pageSize, ignoreCount: false);

            var viewModels = _mapper.Map<Page<ApiInterfaceListItem>>(list);

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ApiInterfaceCreateModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApiInterfaceCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var apiInterfaceEntity = _mapper.Map<ApiInterface>(model);
            await _mockServerNoSqlRepository.AddAsync(apiInterfaceEntity);

            var filter = Builders<ApiInterface>.Filter.Where(x => x.Category == model.Category && x.RequestPath == model.RequestPath);
            var addedApiInterface = await _mockServerNoSqlRepository.SearchOneAsync(filter);

            return RedirectToAction(nameof(Test), new { id = addedApiInterface.Sysid.ToString() });
        }

        [HttpGet]
        public async Task<IActionResult> Modify(string id)
        {
            var entity = await _mockServerNoSqlRepository.GetByIdAsync(id);
            var modifyModel = _mapper.Map<ApiInterfaceModifyModel>(entity);
            return View(modifyModel);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(ApiInterfaceModifyModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = _mapper.Map<ApiInterface>(model);
            await _mockServerNoSqlRepository.ReplaceOneAsync(entity);
            return RedirectToAction(nameof(Test), new { id = model.InterfaceId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _mockServerNoSqlRepository.GetByIdAsync(id);
            var modifyModel = _mapper.Map<ApiInterfaceListItem>(entity);
            return View(modifyModel);
        }

        [HttpPost, ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteSubmit(string id)
        {
            var entity = await _mockServerNoSqlRepository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            await _mockServerNoSqlRepository.Delete(entity);
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Test(string id)
        {
            var entity = await _mockServerNoSqlRepository.GetByIdAsync(id);
            var testModel = _mapper.Map<ApiInterfaceTestModel>(entity);
            return View(testModel);
        }
    }
}
