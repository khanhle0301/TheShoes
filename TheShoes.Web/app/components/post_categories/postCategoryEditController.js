(function (app) {
    app.controller('postCategoryEditController', postCategoryEditController);

    postCategoryEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function postCategoryEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.postCategory = {
            UpdatedDate: new Date()
        }

        $scope.UpdatePostCategory = UpdatePostCategory;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.postCategory.Alias = commonService.getSeoTitle($scope.postCategory.Name);
        }

        function loadpostCategoryDetail() {
            apiService.get('api/postCategory/getbyid/' + $stateParams.id, null, function (result) {
                $scope.postCategory = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdatePostCategory() {
            apiService.put('api/postCategory/update', $scope.postCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('post_categories');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        function loadParentCategory() {
            apiService.get('api/postCategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        loadParentCategory();
        loadpostCategoryDetail();
    }

})(angular.module('theshoes.post_categories'));