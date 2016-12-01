﻿(function (app) {
    app.controller('footerListController', footerListController);

    footerListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function footerListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.footers = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getFooters = getFooters;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteFooter = deleteFooter;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedFooters: JSON.stringify(listId)
                }
            }
            apiService.del('api/footer/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.footers, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.footers, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("footers", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteFooter(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/footer/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getFooters();
        }

        function getFooters(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/footer/getall', config, dataLoadCompleted, dataLoadFailed);            
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.footers = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.getFooters();
    }
})(angular.module('myshop.footers'));