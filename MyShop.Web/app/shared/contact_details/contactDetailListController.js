(function (app) {
    'use strict';

    app.controller('contactDetailListController', contactDetailListController);

    contactDetailListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function contactDetailListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.contactDetails = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getContactDetails = getContactDetails;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteContactDetail = deleteContactDetail;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedContactDetails: JSON.stringify(listId)
                }
            }
            apiService.del('api/contactdetail/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.contactDetails, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.contactDetails, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("contactDetails", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteContactDetail(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/contactdetail/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getContactDetails();
        }

        function getContactDetails(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/contactdetail/getall', config, dataLoadCompleted, dataLoadFailed);           
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.contactDetails = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        $scope.getContactDetails();
    }
})(angular.module('myshop.contact_details'));