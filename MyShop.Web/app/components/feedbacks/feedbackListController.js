(function (app) {
    'use strict';

    app.controller('feedbackListController', feedbackListController);

    feedbackListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function feedbackListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.feedbacks = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getFeedbacks = getFeedbacks;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteFeedback = deleteFeedback;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.changeStatus = changeStatus;

        function changeStatus(id) {
            var config = {
                params: {
                    id: id
                }
            }
            apiService.del('api/feedback/changestatus', config, function () {
                notificationService.displaySuccess('Thay đổi trạng thái thành công');
                search();
            }, function () {
                notificationService.displayError('Thay đổi trạng thái không thành công');
            });
        }

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedFeedbacks: JSON.stringify(listId)
                }
            }
            apiService.del('api/feedback/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.feedbacks, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.feedbacks, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("feedbacks", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteFeedback(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/feedback/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getFeedbacks();
        }

        function getFeedbacks(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/feedback/getall', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.feedbacks = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.getFeedbacks();
    }
})(angular.module('myshop.feedbacks'));