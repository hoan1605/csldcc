'use strict';

angular.module('cmsAngular').controller('khungListCtrl', ['$scope', '$http', 'notificationService', '$location',
    function ($scope, $http, notificationService, $location) {
        //page 
        console.log("khungListCtrl load");

        $scope.defaultPage = 1;
        $scope.currentPage = 1;
        $scope.numPerPage = 10;
        $scope.dataList = [];
        $scope.pagination = {
            NumberOfPage: 0,
            CurrentPage: 0,
            TotalRecords: 0
        };
        $scope.totalRecords = 0;
        $scope.currentRecords = $scope.numPerPage;
        $scope.searchType = 1; //1: Tất cả từ khóa, 2: Từ khóa đã đặt quảng cáo theo cms
        $scope.search = '';
        $scope.deleteId = 0;
        $scope.setNumPerPage = 10;
        $scope.selectedTypeCategory = 0;

        $scope.filterDonVi = '';
        $scope.filterToChuc = '';
        $scope.filterLinhVuc = '';

        $scope.getListDataView = function (page) {

            var keywordSearch = $scope.search.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, ' ');
            var urlService = "/Mod_khungHanhChinh/manager/GetDataForPageList" +
                                "?pi=" + page +
                                "&pz=" + $scope.numPerPage +
                                "&searchString=" + keywordSearch +
                                "&filterDonVi=" + $scope.filterDonVi +
                                "&filterToChuc=" + $scope.filterToChuc +
                                "&filterLinhVuc=" + $scope.filterLinhVuc;
            $scope.dataList = [];
            $http.get(urlService)
                .then(function (response) {
                    if (response.data.Result != null && response.data.Status === true) {
                        var dataRespone = response.data.Result;
                        var paginaionRespone = response.data.Pagination;
                        $scope.pagination = paginaionRespone;
                        $.map(dataRespone,
                            function (item, index) {
                                var stt = (paginaionRespone.NumberOfPage * (paginaionRespone.CurrentPage - 1)) + (index + 1);

                                item = $.extend(item, { STT: stt });

                                $scope.dataList.push(item);
                            });
                        $scope.totalRecords = paginaionRespone.TotalRecords;
                        $scope.currentRecords =
                            ((($scope.currentPage - 1) * $scope.numPerPage) + $scope.numPerPage) < $scope.totalRecords
                            ? ((($scope.currentPage - 1) * $scope.numPerPage) + $scope.numPerPage)
                            : $scope.totalRecords;
                    } else {
                        $scope.dataList = [];
                        $scope.totalRecords = 0;
                        $scope.currentRecords = 0;
                    }
                });
        };

        //$scope.getListDataView($scope.defaultPage);

        $scope.getListKhungCte = function () {

            var keywordSearch = $scope.search.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, ' ');
            var urlService = "/Mod_khungHanhChinh/manager/GetAllDataCteForPageList" +
                                "?searchString=" + keywordSearch +
                                "&filterDonVi=" + $scope.filterDonVi +
                                "&filterToChuc=" + $scope.filterToChuc +
                                "&filterLinhVuc=" + $scope.filterLinhVuc;
            $scope.dataList = [];
            $http.get(urlService)
                .then(function (response) {
                    if (response.data.Result != null && response.data.Status === true) {
                        var dataRespone = response.data.Result;
                        $.map(dataRespone,
                            function (item, index) {
                                var stt = (index + 1);

                                item = $.extend(item, { STT: stt });

                                $scope.dataList.push(item);
                            });

                    } else {
                        $scope.dataList = [];
                    }
                });
        };

        $scope.getListKhungCte();

        //input search change
        $("#KhungSearch").on("change", function () {
            var _thisString = $(this).val();
            $scope.search = _thisString;
        });
        //bind enter click
        $('#KhungSearch').bind("enterKey", function (e) {
            var _thisString = $(this).val();
            $scope.search = _thisString;
            //$scope.getListDataView($scope.defaultPage);
            $scope.getListKhungCte();
        });

        $('#KhungSearch').keyup(function (e) {
            if (e.keyCode == 13) {
                $(this).trigger("enterKey");
            }
        });

        $("#KhungSearchBtn").on("click", function () {
            //$scope.getListDataView($scope.defaultPage);
            $scope.getListKhungCte();
        });

        $('input[name="filterDonVi"]').on('change', function () {
            var _thisVal = $(this).val();
            console.log('filterDonVi', _thisVal);
            $scope.filterDonVi = _thisVal;
            $scope.$apply();
            //$scope.getListDataView($scope.defaultPage);
            $scope.getListKhungCte();
        });

        $('input[name="filterToChuc"]').on('change', function () {
            var _thisVal = $(this).val();
            console.log('filterToChuc', _thisVal);
            $scope.filterToChuc = _thisVal;
            $scope.$apply();
            //$scope.getListDataView($scope.defaultPage);
            $scope.getListKhungCte();
        });

        $('input[name="filterLinhVuc"]').on('change', function () {
            var _thisVal = $(this).val();
            console.log('filterLinhVuc', _thisVal);
            $scope.filterLinhVuc = _thisVal;
            $scope.$apply();
            //$scope.getListDataView($scope.defaultPage);
            $scope.getListKhungCte();
        });

        $scope.openModalDelete = function (id) {
            $('#ModalDelete').modal('show');
            $('#DeleteComfirmBtn').off();
            $('#DeleteComfirmBtn').on("click", function () {
                var urlService = "/admin/ProductCategoryManager/Delete?id=" + id;

                $http.get(urlService)
                    .then(function (response) {
                        notificationService.push(response.data.Message, response.data.Status);
                        $scope.getListDataView($scope.currentPage);
                        $('#ModalDelete').modal('hide');
                    });
            });
        };

    }]);