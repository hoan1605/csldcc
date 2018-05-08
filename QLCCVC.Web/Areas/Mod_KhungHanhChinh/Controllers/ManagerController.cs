using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLCCVC.DAL;
using QLCCVC.DAL.DomainModels;

namespace QLCCVC.Web.Areas.Mod_khungHanhChinh
{
    public class ManagerController : Controller
    {
        #region Private

        private readonly log4net.ILog _logService = log4net.LogManager.GetLogger(typeof(ManagerController));
        private readonly IKhungRepository _khungRepository;
        private readonly ICapDonViHanhChinhRepository _donViHanhChinhRepository;
        private readonly ILoaiToChucRepository _loaiToChucRepository;
        private readonly ILinhVucRepository _linhVucRepository;

        public ManagerController(IKhungRepository khungRepository, ICapDonViHanhChinhRepository donViHanhChinhRepository,
            ILoaiToChucRepository loaiToChucRepository, ILinhVucRepository linhVucRepository)
        {
            _khungRepository = khungRepository;
            _donViHanhChinhRepository = donViHanhChinhRepository;
            _loaiToChucRepository = loaiToChucRepository;
            _linhVucRepository = linhVucRepository;
        }

        #endregion

        #region Main Method

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(KhungModel modelKhung)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _khungRepository.Insert(modelKhung);
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                _logService.Error($"Add - Message: {ex.Message}", ex);
            }
            return View(modelKhung);
        }

        public ActionResult Edit(long id)
        {
            try
            {
                var khung = _khungRepository.FindById(id);
                if (khung != null && khung.Id > 0)
                    return View(khung);
            }
            catch (Exception ex)
            {
                _logService.Error($"Add - Message: {ex.Message}", ex);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(long Id, KhungModel modelKhung)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentKhung = _khungRepository.FindById(Id);
                    if (currentKhung != null && currentKhung.Id > 0)
                    {
                        _khungRepository.Update(modelKhung);
                        return RedirectToAction("List");
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error($"Add - Message: {ex.Message}", ex);
            }
            return View(modelKhung);
        }

        public JsonResult GetDataForPageList(int? pi, int? pz, string searchString, string filterDonvi, string filterToChuc, string filterLinhVuc)
        {
            try
            {
                var pageIndex = pi ?? 1;
                var pageSize = pz ?? 10;
                var totalRecords = 0;

                const string orderBy = " STT ";
                var fieldList = "*";
                var filter = "";
                //filter by name
                if (!string.IsNullOrEmpty(searchString))
                {
                    var filterWithSearch = " lower(tenKhung) LIKE N'%" + searchString.ToLower() + "%'";
                    //set up filter
                    filter += !string.IsNullOrEmpty(filter) ? " AND " + filterWithSearch : filterWithSearch;
                }

                if (!string.IsNullOrEmpty(filterDonvi))
                {
                    var filterWithDonVi = " DM_capDonViHanhChinh_ID = N'" + filterDonvi.ToLower() + "'";
                    //set up filter
                    filter += !string.IsNullOrEmpty(filter) ? " AND " + filterWithDonVi : filterWithDonVi;
                }

                if (!string.IsNullOrEmpty(filterToChuc))
                {
                    var filterWithToChuc = " DM_loaiToChuc_ID = N'" + filterToChuc.ToLower() + "'";
                    //set up filter
                    filter += !string.IsNullOrEmpty(filter) ? " AND " + filterWithToChuc : filterWithToChuc;
                }

                if (!string.IsNullOrEmpty(filterLinhVuc))
                {
                    var filterWithLinhVuc = " DM_linhVuc_ID = N'" + filterLinhVuc.ToLower() + "'";
                    //set up filter
                    filter += !string.IsNullOrEmpty(filter) ? " AND " + filterWithLinhVuc : filterWithLinhVuc;
                }

                //get list data in DB
                var listKhungHanhChinhs = _khungRepository.Pagination(orderBy, fieldList, filter, pageIndex, pageSize, out totalRecords).ToList();

                if (listKhungHanhChinhs != null && listKhungHanhChinhs.Any())
                {
                    //map model in DB to model view 

                    return Json(new
                    {
                        Result = listKhungHanhChinhs,
                        Pagination = new
                        {
                            NumberOfPage = pageSize,
                            CurrentPage = pageIndex,
                            TotalRecords = totalRecords
                        },
                        Message = "Lấy dữ liệu thành công.",
                        Status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logService.Error($"GetListForPageList => Exception: {ex.Message}", ex);
            }
            return Json(new { Message = "Không có dữ liệu.", Status = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDataCteForPageList(string searchString, string filterDonvi, string filterToChuc, string filterLinhVuc)
        {
            try
            {
                //get list data in DB
                var listKhungHanhChinhs = _khungRepository.FindAllCte(0, searchString, filterDonvi, filterToChuc, filterLinhVuc);

                if (listKhungHanhChinhs != null && listKhungHanhChinhs.Any())
                {
                    //map model in DB to model view 
                    return Json(new
                    {
                        Result = listKhungHanhChinhs,
                        Message = "Lấy dữ liệu thành công.",
                        Status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logService.Error($"GetListForPageList => Exception: {ex.Message}", ex);
            }
            return Json(new { Message = "Không có dữ liệu.", Status = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Public Method

        public ActionResult _KhungChaComboboxForm(long khungId, long parentId)
        {
            try
            {
                var khungs = _khungRepository.FindAll().ToList();
                if (khungId > 0)
                {
                    khungs = khungs.Where(p => !p.Id.Equals(khungId)).ToList();
                }
                ViewBag.ParentId = parentId;
                return PartialView(khungs);
            }
            catch (Exception ex)
            {
                _logService.Error($"KhungChaComboboxForm => Exception: {ex.Message}", ex);
            }
            return PartialView();
        }

        public ActionResult _DonViComboboxForm(string donviId)
        {
            try
            {
                var donViHanhChinhs = _donViHanhChinhRepository.FindAll().ToList();
                ViewBag.DonViId = donviId;
                return PartialView(donViHanhChinhs);
            }
            catch (Exception ex)
            {
                _logService.Error($"DonViComboboxForm => Exception: {ex.Message}", ex);
            }
            return PartialView();
        }

        public ActionResult _ToChucComboboxForm(string tochucId)
        {
            try
            {
                var toChucs = _loaiToChucRepository.FindAll().ToList();
                ViewBag.ToChucId = tochucId;
                return PartialView(toChucs);
            }
            catch (Exception ex)
            {
                _logService.Error($"ToChucComboboxForm => Exception: {ex.Message}", ex);
            }
            return PartialView();
        }

        public ActionResult _LinhVucComboboxForm(string linhVucId)
        {
            try
            {
                var linhVucs = _linhVucRepository.FindAll().ToList();
                ViewBag.LinhVucId = linhVucId;
                return PartialView(linhVucs);
            }
            catch (Exception ex)
            {
                _logService.Error($"LinhVucComboboxForm => Exception: {ex.Message}", ex);
            }
            return PartialView();
        }

        #endregion

        #region Menu

        public ActionResult DonViHanhChinhMenu(string viewName)
        {
            try
            {
                var donViHanhChinhs = _donViHanhChinhRepository.FindAll().ToList();
                if (!string.IsNullOrEmpty(viewName))
                    return PartialView(viewName, donViHanhChinhs);
                return PartialView(donViHanhChinhs);
            }
            catch (Exception ex)
            {
                _logService.Error($"DonViComboboxForm => Exception: {ex.Message}", ex);
            }
            if (!string.IsNullOrEmpty(viewName))
                return PartialView(viewName);
            return PartialView();
        }

        public ActionResult ToChucMenu(string viewName)
        {
            try
            {
                var toChucs = _loaiToChucRepository.FindAll().ToList();
                if (!string.IsNullOrEmpty(viewName))
                    return PartialView(viewName, toChucs);
                return PartialView(toChucs);
            }
            catch (Exception ex)
            {
                _logService.Error($"ToChucComboboxForm => Exception: {ex.Message}", ex);
            }
            if (!string.IsNullOrEmpty(viewName))
                return PartialView(viewName);
            return PartialView();
        }

        public ActionResult LinhVucMenu(string viewName)
        {
            try
            {
                var linhVucs = _linhVucRepository.FindAll().ToList();
                if (!string.IsNullOrEmpty(viewName))
                    return PartialView(viewName, linhVucs);
                return PartialView(linhVucs);
            }
            catch (Exception ex)
            {
                _logService.Error($"LinhVucComboboxForm => Exception: {ex.Message}", ex);
            }
            if (!string.IsNullOrEmpty(viewName))
                return PartialView(viewName);
            return PartialView();
        }

        #endregion
    }
}