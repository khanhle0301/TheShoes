(function (app) {
    app.controller('providerListController', providerListController);

    providerListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function providerListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.providers = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProviders = getProviders;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteProvider = deleteProvider;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedProviders: JSON.stringify(listId)
                }
            }
            apiService.del('api/provider/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.providers, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.providers, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("providers", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProvider(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/provider/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getProviders();
        }

        function getProviders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/provider/getall', config, dataLoadCompleted, dataLoadFailed);           
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.providers = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        $scope.getProviders();
    }
})(angular.module('myshop.providers'));