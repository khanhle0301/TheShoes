(function (app) {
    app.controller('heightListController', heightListController);

    heightListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function heightListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.heights = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getHeights = getHeights;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteHeight = deleteHeight;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedHeights: JSON.stringify(listId)
                }
            }
            apiService.del('api/height/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.heights, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.heights, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("heights", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteHeight(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/height/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getHeights();
        }

        function getHeights(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageHeight: 20
                }
            }
            apiService.get('api/height/getall', config, dataLoadCompleted, dataLoadFailed);            
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.heights = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data);
        }


        $scope.getHeights();
    }
})(angular.module('myshop.heights'));