(function (app) {
    app.controller('sizeEditController', sizeEditController);

    sizeEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function sizeEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.size = {};
        $scope.UpdateSize = UpdateSize;

        function loadSizeDetail() {
            apiService.get('api/size/getbyid/' + $stateParams.id, null, function (result) {
                $scope.size = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateSize() {
            apiService.put('api/size/update', $scope.size, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.size.Name + ' đã được cập nhật.');
            $state.go('sizes');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadSizeDetail();
    }

})(angular.module('myshop.sizes'));