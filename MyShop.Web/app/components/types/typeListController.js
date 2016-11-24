(function (app) {
    'use strict';

    app.controller('typeListController', typeListController);

    typeListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function typeListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.types = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getTypes = getTypes;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteType = deleteType;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedTypes: JSON.stringify(listId)
                }
            }
            apiService.del('api/type/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.types, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.types, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("types", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteType(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/type/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getTypes();
        }

        function getTypes(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageType: 20
                }
            }
            apiService.get('api/type/getall', config, dataLoadCompleted, dataLoadFailed);            
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.types = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.getTypes();
    }
})(angular.module('myshop.types'));