(function (app) {
    app.controller('postListController', postListController);

    postListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function postListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.posts = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getPosts = getPosts;
        $scope.deletePost = deletePost;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;      

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedPosts: JSON.stringify(listId)
                }
            }
            apiService.del('api/post/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                getPosts();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.posts, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.posts, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("posts", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deletePost(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/post/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    getPosts();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function getPosts(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                params: {
                    filter: $scope.filterExpression,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/post/getall', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.posts = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
            //if ($scope.filterExpression && $scope.filterExpression.length) {
            //    notificationService.displayInfo(result.data.Items.length + ' items found');
            //}
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        $scope.getPosts();
    }
})(angular.module('myshop.posts'));